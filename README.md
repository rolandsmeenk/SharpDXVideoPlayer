SharpDXVideoPlayer
==================

This application uses the SharpDX Toolkit, a XNA/Monogame like framework that is part of SharpDX (but is now deprecated).

The main component is the VideoPlaneRenderer that uses a SpriteBatch for rendering a full screen textured quad and 
therefore can also be given a custom shader for applying real-time effects. A simple grayscale effect is provided as example.
The internal MediaPlayer transfers it's video frames to the texture that is used by the SpriteBatch.

For testing I used the infamous "Big Buck Bunny" movie at 4K resolution, both the 30Hz and 60Hz version. 
Download link: http://bbb3d.renderfarming.net/download.html

Screen resolution, target frame rate and video to use can be specified in the application config file (App.config).

Known issues
============
* TransferVideoFrame may throw exceptions on some NVidia cards. A workaround for this is using DirectX9 only (see configuration file).
* On Windows 8.1 the videoplayer may drop frames when decoding high bitrate video frames. This issue does not occur on Windows 8.  


