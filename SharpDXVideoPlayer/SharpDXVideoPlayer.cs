using System;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;
using System.IO;
using SharpDX.Direct3D;
using System.Configuration;

namespace SharpDXVideoPlayer
{
    /// <summary>
    /// Simple SharpDXVideoPlayer game using SharpDX.Toolkit.
    /// </summary>
    public class SharpDXVideoPlayer : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        public VideoPlaneRenderer VideoRenderer { get; private set; }

        private KeyboardManager keyboard;
        private MouseManager mouse;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharpDXVideoPlayer" /> class.
        /// </summary>
        public SharpDXVideoPlayer()
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);

            graphicsDeviceManager.IsFullScreen = true;
            graphicsDeviceManager.PreferMultiSampling = Configuration.AntiAliasing;
            graphicsDeviceManager.SynchronizeWithVerticalRetrace = true;
            graphicsDeviceManager.PreferredBackBufferWidth = Configuration.PreferredBackBufferWidth;
            graphicsDeviceManager.PreferredBackBufferHeight = Configuration.PreferredBackBufferHeight;
            graphicsDeviceManager.PreferredRefreshRate = new SharpDX.DXGI.Rational(Configuration.PreferredRefreshRate, 1);
            // BUG WORKAROUND: Limit to DirectX feature level 9.1, 9.2 or 9.3 to work around crash in TransferVideoFrame on nVidia card used in this system
            if (Configuration.DirectX9Only)
            {
                graphicsDeviceManager.PreferredGraphicsProfile = new FeatureLevel[] { SharpDX.Direct3D.FeatureLevel.Level_9_3, SharpDX.Direct3D.FeatureLevel.Level_9_2, SharpDX.Direct3D.FeatureLevel.Level_9_1 };
            }

            // Add dynamic EffectCompilerSystem, only active on desktop and compiled in debug mode.
            // While the program is running, you can edit the shader and save a new version
            // The EffectCompilerSystem will recompile dynamically the effect without having to
            // reload/recompile the whole application.
            GameSystems.Add(new EffectCompilerSystem(this));

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";

            // Initialize input keyboard system
            keyboard = new KeyboardManager(this);
            Services.AddService(keyboard);

            // Initialize input mouse system
            mouse = new MouseManager(this);
            Services.AddService(mouse);

            string videosPath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            string backgroundVideoPath = Path.Combine(videosPath, ConfigurationManager.AppSettings["Video"]);

            // Note pass an empty effect if you don't want to use a shader to modify the video
            VideoRenderer = new VideoPlaneRenderer(this, backgroundVideoPath, "VideoShader");
        }

        protected override void Initialize()
        {
            // Modify the title of the window
            Window.Title = "SharpDXVideoPlayer";

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Get the current state of the keyboard
            var keyboardState = keyboard.GetState();

            if (keyboardState.IsKeyPressed(Keys.Escape))
            {
                Exit();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            // Use time in seconds directly
            var time = (float)gameTime.TotalGameTime.TotalSeconds;

            // Clears the screen with the Color.CornflowerBlue
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
