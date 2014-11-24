using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.MediaFoundation;
using System.IO;
using SharpDX.Direct3D11;
using System;
using SharpDX.Toolkit.Diagnostics;
using System.Diagnostics;

namespace SharpDXVideoPlayer
{
    /// <summary>
    /// Renders the Kinect color stream to a full screen quad
    /// </summary>
    public class VideoPlaneRenderer : GameSystem
    {
        private Effect _customEffect;
        private SpriteBatch _spriteBatch;
        private MediaPlayer _mediaPlayer;
        private SharpDX.Toolkit.Graphics.SamplerState _samplerState;

        // Shader resource so we can use the video texture in a shader
        private ShaderResourceView _textureView;

        // Path to the video file to render to texture
        private string _filePath;

        // Custom effect to load
        private string _effectName;
        
        // Destination rectangle for the spritebatch that renders the video texture on screen
        public RectangleF DestinationRectangle { get; set; }
        public float Alpha { get; set;}

        private bool _videoExists = false;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game"></param>
        public VideoPlaneRenderer(Game game, string filePath, string effectName = "") :
            base(game)
        {
            // this game system has something to draw - enable drawing by default
            // this can be disabled to make objects drawn by this system disappear
            Visible = true;

            // this game system has logic that needs to be updated - enable update by default
            // this can be disabled to simulate a "pause" in logic update
            Enabled = true;

            // add the system itself to the systems list, so that it will get initialized and processed properly
            // this can be done after game initialization - the Game class supports adding and removing of game systems dynamically
            game.GameSystems.Add(this);

            _filePath = filePath;
            _effectName = effectName;
            Alpha = 1;
        }

        protected override void LoadContent()
        {
            if (!string.IsNullOrEmpty(_effectName))
            {
                _customEffect = Content.Load<Effect>(_effectName);
            }

            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.Looping = true;
            _mediaPlayer.Initialize(GraphicsDevice);

            _spriteBatch = ToDisposeContent(new SpriteBatch(GraphicsDevice));

            if (DestinationRectangle.Width == 0 || DestinationRectangle.Height == 0)
            {
                DestinationRectangle = new RectangleF(0,
                                                      0,
                                                      GraphicsDevice.BackBuffer.Width,
                                                      GraphicsDevice.BackBuffer.Height);
            }

            // Create texture to be used as rendertarget for our video frames. 
            // Make it the current backbuffer size
            var texture2D = new SharpDX.Direct3D11.Texture2D(this.GraphicsDevice, new SharpDX.Direct3D11.Texture2DDescription()
            {
                Width = (int)(DestinationRectangle.Width),
                Height = (int)DestinationRectangle.Height,
                ArraySize = 1,
                BindFlags = SharpDX.Direct3D11.BindFlags.ShaderResource | BindFlags.RenderTarget,
                Usage = SharpDX.Direct3D11.ResourceUsage.Default,
                CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.None,
                Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
                MipLevels = 1,
                OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None,
                SampleDescription = new SharpDX.DXGI.SampleDescription(count:1, quality:0),
            });
            _textureView = new ShaderResourceView(this.GraphicsDevice, texture2D);

            // Bind the media player with this output texture
            _mediaPlayer.OutputVideoTexture = texture2D;

            _samplerState = ToDisposeContent(SharpDX.Toolkit.Graphics.SamplerState.New(this.GraphicsDevice, new SamplerStateDescription()
            {
                Filter = Filter.MinMagMipLinear,
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                AddressW = TextureAddressMode.Clamp,
                BorderColor = Color.Black,
                ComparisonFunction = Comparison.Never,
                MaximumAnisotropy = 16,
                MipLodBias = 0,
                MinimumLod = -float.MaxValue,
                MaximumLod = float.MaxValue
            }));
  
            if (!string.IsNullOrEmpty(_filePath) && File.Exists(_filePath))
            {
                try
                {
                    var fileStream = File.Open(_filePath, FileMode.Open);

                    // Plays the video
                    _mediaPlayer.Url = _filePath;
                    _mediaPlayer.SetBytestream(fileStream);
                    _videoExists = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Failed to open video '{0}'", _filePath);
                }
            }

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            _mediaPlayer.Shutdown();

            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // Transfer the video to the texture
            _mediaPlayer.OnRender();

            base.Update(gameTime);
        }

        public void Restart()
        {
            if (_videoExists)
            {
                _mediaPlayer.PlaybackPosition = 0;
                _mediaPlayer.Play();
            }
        }

        public void Pause()
        {
            if (_videoExists)
            {
                _mediaPlayer.Pause();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, GraphicsDevice.BlendStates.AlphaBlend, _samplerState, null, null, _customEffect);
            _spriteBatch.Draw(_textureView, DestinationRectangle, new Color(1f, 1f, 1f, Alpha));
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
