//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using NLog;
using NLog.Config;
using NLog.LayoutRenderers;
using System.Globalization;
using System.Text;

namespace Mvp24Hours.Infrastructure.Logging.Renderer
{
    /// <summary>
    /// 
    /// </summary>
    [LayoutRenderer("utc_date")]
    public class UtcDateRenderer : LayoutRenderer
    {
        public UtcDateRenderer()
        {
            Format = "G";
            Culture = CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// 
        /// </summary>
        protected int GetEstimatedBufferSize(LogEventInfo ev)
        {
            return 10;
        }

        /// <summary>
        /// 
        /// </summary>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultParameter]
        public string Format { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(logEvent.TimeStamp.ToUniversalTime().ToString(Format, Culture));
        }

    }
}
