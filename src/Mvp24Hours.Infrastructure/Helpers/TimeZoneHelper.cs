//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using System;

namespace Mvp24Hours.Infrastructure.Helpers
{
    public class TimeZoneHelper
    {
        public static DateTime GetTimeZoneNow()
        {
            return GetTimeZone(DateTime.UtcNow);
        }

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
