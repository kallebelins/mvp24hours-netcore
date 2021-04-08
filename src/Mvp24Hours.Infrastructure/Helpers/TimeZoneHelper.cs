//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using System;

namespace Mvp24Hours.Infrastructure.Helpers
{
    /// <summary>
    /// Contains timezone functions
    /// </summary>
    public class TimeZoneHelper
    {
        /// <summary>
        /// Get current date and time based on South America time zone
        /// </summary>
        public static DateTime GetTimeZoneNow()
        {
            return GetTimeZone(DateTime.UtcNow);
        }

        /// <summary>
        /// Get date and time based on South America time zone
        /// </summary>
        public static DateTime GetTimeZone(DateTime utcDateTime)
        {
            DateTime dUtc;
            switch (utcDateTime.Kind)
            {
                case DateTimeKind.Utc:
                    dUtc = utcDateTime;
                    break;
                case DateTimeKind.Local:
                    dUtc = utcDateTime.ToUniversalTime();
                    break;
                default: //DateTimeKind.Unspecified
                    dUtc = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
                    break;
            }
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
        }
    }
}
