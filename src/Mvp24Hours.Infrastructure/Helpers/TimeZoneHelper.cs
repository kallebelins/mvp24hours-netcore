//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mvp24Hours.Helpers
{
    /// <summary>
    /// Contains timezone functions
    /// </summary>
    public static class TimeZoneHelper
    {
        /// <summary>
        /// Get current date and time based on South America time zone
        /// </summary>
        public static DateTime GetTimeZoneNow()
        {
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, GetTimeZoneInfo());
        }

        /// <summary>
        /// Get date and time based on South America time zone
        /// </summary>
        public static DateTime GetTimeZone(DateTime utcDateTime, DateTimeKind? kind)
        {
            var dUtc = (kind ?? utcDateTime.Kind) switch
            {
                DateTimeKind.Utc => utcDateTime,
                DateTimeKind.Local => utcDateTime.ToUniversalTime(),
                _ => DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc),
            };
            return TimeZoneInfo.ConvertTime(dUtc, GetTimeZoneInfo());
        }

        private static TimeZoneInfo _timeZoneInfo;

        private static TimeZoneInfo GetTimeZoneInfo()
        {
            if (_timeZoneInfo != null)
            {
                return _timeZoneInfo;
            }

            string timeZoneIds = ConfigurationHelper.GetSettings("Mvp24Hours:General:TimeZoneIds");
            var timeZoneIdsList = new List<string>();

            if (!string.IsNullOrEmpty(timeZoneIds))
            {
                timeZoneIdsList.AddRange(timeZoneIds.Split(","));
            }
            else
            {
                timeZoneIdsList.Add("E. South America Standard Time");
                timeZoneIdsList.Add("Brazil/East");
                timeZoneIdsList.Add("America/Sao_Paulo");
            }

            if (_timeZoneInfo == null)
            {
                _timeZoneInfo = TimeZoneInfo.GetSystemTimeZones()
                    .FirstOrDefault(x => timeZoneIdsList.Contains(x.Id));

                if (_timeZoneInfo == null)
                {
                    _timeZoneInfo = TimeZoneInfo.Local;
                }
            }

            return _timeZoneInfo;
        }
    }
}
