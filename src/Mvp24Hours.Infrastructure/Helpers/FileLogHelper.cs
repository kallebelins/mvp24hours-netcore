//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Infrastructure.Log;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Mvp24Hours.Infrastructure.Helpers
{
    public class FileLogHelper
    {
        private static readonly ILoggingService _logger;

        static FileLogHelper()
        {
            _logger = LoggingService.GetLoggingService();
        }

        public static void WriteLog<T>(T dto, string suffixFilename = null, string header = null)
        {
            try
            {
                string logPath = ConfigurationHelper.GetSettings("Mvp24Hours:FileLog:Path");
                if (string.IsNullOrEmpty(logPath))
                    return;
                string filename = $"{DateTime.Today.ToString("yyyy_MM_dd")}_{Guid.NewGuid().ToString()}.log";
                if (!string.IsNullOrEmpty(suffixFilename))
                {
                    filename = $"{suffixFilename.ToLower()}_{filename}";
                }
                var folder = $"{logPath}\\{DateTime.Today.ToString("yyyy_MM_dd")}\\";
                WriteDisk(dto, folder, filename, $"{header} Hora : {DateTime.Now.ToString("HH:mm:ss.fff")}", true);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static void WriteLogToken<T>(string token, string fileName, T obj)
        {
            try
            {
                lock (obj)
                {
                    string logPath = ConfigurationHelper.GetSettings("Mvp24Hours:FileLog:TokenPath");
                    if (string.IsNullOrEmpty(logPath))
                        return;

                    var folder = $"{logPath}\\{token}\\";
                    Directory.CreateDirectory(folder);

                    var fullPath = $"{folder}{fileName}.json";
                    File.WriteAllText(fullPath, JsonConvert.SerializeObject(obj));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static T ReadLogToken<T>(string token, string fileName)
        {
            try
            {
                string logPath = ConfigurationHelper.GetSettings("Mvp24Hours:FileLog:TokenPath");
                var fullPath = $"{logPath}\\{token}\\{fileName}.json";
                if (!File.Exists(fullPath))
                    return default(T);
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(fullPath));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return default(T);
        }

        private static void WriteDisk<T>(T obj, string folder, string fileName, string header = null, bool append = false)
        {
            lock (obj)
            {
                Directory.CreateDirectory(folder);
                string fullpath = folder + fileName;
                using (var sw = new StreamWriter(fullpath, append))
                {
                    if (!string.IsNullOrEmpty(header))
                        sw.Write(header.PadLeft(5, '-').PadRight(5, '-') + "\r\n");
                    sw.Write(JsonConvert.SerializeObject(obj) + "\r\n");
                }
            }
        }
    }
}
