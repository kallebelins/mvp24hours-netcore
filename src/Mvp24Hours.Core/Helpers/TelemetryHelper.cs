//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Logging;
using Mvp24Hours.Core.Enums.Infrastructure;
using Mvp24Hours.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mvp24Hours.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class TelemetryHelper
    {
        #region [ Properties / Fields ]
        private static readonly Dictionary<TelemetryLevels, List<ITelemetryService>> services = new();
        private static readonly Dictionary<TelemetryLevels, List<Action<string>>> servicesAction1 = new();
        private static readonly Dictionary<TelemetryLevels, List<Action<string, object[]>>> servicesAction2 = new();

        private static readonly Dictionary<string, List<ITelemetryService>> serviceFilters = new();
        private static readonly Dictionary<string, List<Action<string>>> serviceActionFilters1 = new();
        private static readonly Dictionary<string, List<Action<string, object[]>>> serviceActionFilters2 = new();

        private static readonly List<string> ignoreNames = new();

        private static bool servicesAction1Started = false;
        private static bool servicesAction2Started = false;
        private static bool servicesStarted = false;
        private static bool serviceActionFilters1Started = false;
        private static bool serviceActionFilters2Started = false;
        private static bool serviceFiltersStarted = false;
        #endregion

        #region [ Ignore Services ]
        public static void AddIgnoreService(params string[] serviceNames)
        {
            if (serviceNames.AnySafe())
            {
                ignoreNames.AddRange(serviceNames);
            }
        }

        public static void RemoveIgnoreService(string serviceName)
        {
            if (serviceName.HasValue() && ignoreNames.AnySafe(x => x == serviceName))
            {
                ignoreNames.Remove(serviceName);
            }
        }
        #endregion

        #region [ Add Services ]
        public static void Add(TelemetryLevels level, params Action<string>[] actions)
        {
            if (!actions.AnySafe())
            {
                throw new ArgumentNullException(nameof(actions));
            }
            if (!servicesAction1.ContainsKey(level))
            {
                servicesAction1.Add(level, new List<Action<string>>());
            }
            servicesAction1[level].AddRange(actions);
            servicesAction1Started = true;
        }

        public static void Add(TelemetryLevels level, params Action<string, object[]>[] actions)
        {
            if (!actions.AnySafe())
            {
                throw new ArgumentNullException(nameof(actions));
            }
            if (!servicesAction2.ContainsKey(level))
            {
                servicesAction2.Add(level, new List<Action<string, object[]>>());
            }
            servicesAction2[level].AddRange(actions);
            servicesAction2Started = true;
        }

        public static void Add(TelemetryLevels level, params ITelemetryService[] telemetryServices)
        {
            if (!telemetryServices.AnySafe())
            {
                throw new ArgumentNullException(nameof(telemetryServices));
            }
            if (!services.ContainsKey(level))
            {
                services.Add(level, new List<ITelemetryService>());
            }
            services[level].AddRange(telemetryServices);
            servicesStarted = true;
        }

        public static void AddFilter(string serviceName, params Action<string>[] actions)
        {
            if (!serviceName.HasValue())
            {
                throw new ArgumentNullException(nameof(serviceName));
            }
            if (!actions.AnySafe())
            {
                throw new ArgumentNullException(nameof(actions));
            }
            if (!serviceActionFilters1.ContainsKey(serviceName))
            {
                serviceActionFilters1.Add(serviceName, new List<Action<string>>());
            }
            serviceActionFilters1[serviceName].AddRange(actions);
            serviceActionFilters1Started = true;
        }

        public static void AddFilter(string serviceName, params Action<string, object[]>[] actions)
        {
            if (!serviceName.HasValue())
            {
                throw new ArgumentNullException(nameof(serviceName));
            }
            if (!actions.AnySafe())
            {
                throw new ArgumentNullException(nameof(actions));
            }
            if (!serviceActionFilters2.ContainsKey(serviceName))
            {
                serviceActionFilters2.Add(serviceName, new List<Action<string, object[]>>());
            }
            serviceActionFilters2[serviceName].AddRange(actions);
            serviceActionFilters2Started = true;
        }

        public static void AddFilter(string serviceName, params ITelemetryService[] telemetryServices)
        {
            if (!serviceName.HasValue())
            {
                throw new ArgumentNullException(nameof(serviceName));
            }
            if (!telemetryServices.AnySafe())
            {
                throw new ArgumentNullException(nameof(telemetryServices));
            }
            if (!serviceFilters.ContainsKey(serviceName))
            {
                serviceFilters.Add(serviceName, new List<ITelemetryService>());
            }
            serviceFilters[serviceName].AddRange(telemetryServices);
            serviceFiltersStarted = true;
        }
        #endregion

        #region [ Get Services ]
        public static IList<ITelemetryService> GetServices(TelemetryLevels level)
        {
            if (services.ContainsKey(level))
                return services[level];
            return default;
        }

        public static IList<Action<string>> GetActions1(TelemetryLevels level)
        {
            if (servicesAction1.ContainsKey(level))
                return servicesAction1[level];
            return default;
        }

        public static IList<Action<string, object[]>> GetActions2(TelemetryLevels level)
        {
            if (servicesAction2.ContainsKey(level))
                return servicesAction2[level];
            return default;
        }

        public static IList<ITelemetryService> GetFilters(string serviceName)
        {
            if (serviceFilters.ContainsKey(serviceName))
                return serviceFilters[serviceName];
            return default;
        }

        public static IList<Action<string>> GetActionFilters1(string serviceName)
        {
            if (serviceActionFilters1.ContainsKey(serviceName))
                return serviceActionFilters1[serviceName];
            return default;
        }

        public static IList<Action<string, object[]>> GetActionFilters2(string serviceName)
        {
            if (serviceActionFilters2.ContainsKey(serviceName))
                return serviceActionFilters2[serviceName];
            return default;
        }
        #endregion

        #region [ Remove Services ]
        public static void Remove(TelemetryLevels level)
        {
            if (servicesAction1.ContainsKey(level))
            {
                servicesAction1.Remove(level);
            }
            if (servicesAction2.ContainsKey(level))
            {
                servicesAction2.Remove(level);
            }
            if (services.ContainsKey(level))
            {
                services.Remove(level);
            }
        }

        public static void Remove(string serviceName)
        {
            if (serviceName.HasValue())
            {
                if (serviceActionFilters1.ContainsKey(serviceName))
                {
                    serviceActionFilters1.Remove(serviceName);
                }
                if (serviceActionFilters2.ContainsKey(serviceName))
                {
                    serviceActionFilters2.Remove(serviceName);
                }
                if (serviceFilters.ContainsKey(serviceName))
                {
                    serviceFilters.Remove(serviceName);
                }
            }
        }

        public static void Clear()
        {
            services.Clear();
            servicesAction1.Clear();
            servicesAction2.Clear();
        }
        #endregion

        #region [ Execute Services ]
        public static void Execute(TelemetryLevels level, params object[] args)
        {
            Execute(level, "unknown", args);
        }

        public static void Execute(TelemetryLevels level, string eventName, params object[] args)
        {
            if (ignoreNames.AnySafe(x => x == eventName)) return;

            // filtered
            if (serviceFiltersStarted && serviceFilters.ContainsKey(eventName))
            {
                foreach (var item in serviceFilters[eventName])
                {
                    item.Execute(eventName, args);
                }
            }
            if (serviceActionFilters1Started && serviceActionFilters1.ContainsKey(eventName))
            {
                foreach (var item in serviceActionFilters1[eventName])
                {
                    item.Invoke(eventName);
                }
            }
            if (serviceActionFilters2Started && serviceActionFilters2.ContainsKey(eventName))
            {
                foreach (var item in serviceActionFilters2[eventName])
                {
                    item.Invoke(eventName, args);
                }
            }

            // services
            if (servicesStarted && services.Any(x => x.Key.HasFlag(level)))
            {
                foreach (var key in services.Keys.Where(x => x.HasFlag(level)).ToList())
                {
                    foreach (var item in services[key])
                    {
                        item.Execute(eventName, args);
                    }
                }
            }
            if (servicesAction1Started && servicesAction1.Any(x => x.Key.HasFlag(level)))
            {
                foreach (var key in servicesAction1.Keys.Where(x => x.HasFlag(level)).ToList())
                {
                    foreach (var item in servicesAction1[key])
                    {
                        item.Invoke(eventName);
                    }
                }
            }
            if (servicesAction2Started && servicesAction2.Any(x => x.Key.HasFlag(level)))
            {
                foreach (var key in servicesAction2.Keys.Where(x => x.HasFlag(level)).ToList())
                {
                    foreach (var item in servicesAction2[key])
                    {
                        item.Invoke(eventName, args);
                    }
                }
            }
        }
        #endregion
    }
}
