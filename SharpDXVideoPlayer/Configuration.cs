using System;
using System.Configuration;
using System.Globalization;

namespace SharpDXVideoPlayer
{
    public static class Configuration
    {
        /// <summary>
        /// The PreferredBackBufferWidth
        /// </summary>
        public static int PreferredBackBufferWidth
        {
            get
            {
                var val = ConfigurationManager.AppSettings["PreferredBackBufferWidth"];
                if (String.IsNullOrEmpty(val))
                {
                    return 1920;
                }

                return Math.Max(640, Convert.ToInt32(val, CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// The PreferredBackBufferHeight
        /// </summary>
        public static int PreferredBackBufferHeight
        {
            get
            {
                var val = ConfigurationManager.AppSettings["PreferredBackBufferHeight"];
                if (String.IsNullOrEmpty(val))
                {
                    return 1080;
                }

                return Math.Max(480, Convert.ToInt32(val, CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// The PreferredBackBufferHeight
        /// </summary>
        public static int PreferredRefreshRate
        {
            get
            {
                var val = ConfigurationManager.AppSettings["PreferredRefreshRate"];
                if (String.IsNullOrEmpty(val))
                {
                    return 60;
                }

                return Math.Max(5, Convert.ToInt32(val, CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Flag indicating if we want to use anti aliasing 
        /// </summary>
        public static bool AntiAliasing
        {
            get
            {
                var val = ConfigurationManager.AppSettings["AntiAliasing"];
                if (String.IsNullOrEmpty(val))
                {
                    return true;
                }

                return Convert.ToBoolean(val);
            }
        }

        /// <summary>
        /// DirectX9 only workaround for crash of TransferVideoFrame
        /// </summary>
        public static bool DirectX9Only
        {
            get
            {
                var val = ConfigurationManager.AppSettings["DirectX9Only"];
                if (String.IsNullOrEmpty(val))
                {
                    return true;
                }

                return Convert.ToBoolean(val);
            }
        }
    }
}
