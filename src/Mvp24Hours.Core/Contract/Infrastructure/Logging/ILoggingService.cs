//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using System;

namespace Mvp24Hours.Core.Contract.Infrastructure.Logging
{
    /// <summary>
    /// Provides logging interface and utility functions.
    /// </summary>
    public interface ILoggingService
    {
        /// <summary>
        /// 
        /// </summary>
        bool IsDebugEnabled { get; }
        /// <summary>
        /// 
        /// </summary>
        bool IsErrorEnabled { get; }
        /// <summary>
        /// 
        /// </summary>
        bool IsFatalEnabled { get; }
        /// <summary>
        /// 
        /// </summary>
        bool IsInfoEnabled { get; }
        /// <summary>
        /// 
        /// </summary>
        bool IsTraceEnabled { get; }
        /// <summary>
        /// 
        /// </summary>
        bool IsWarnEnabled { get; }

        /// <summary>
        /// 
        /// </summary>
        void Debug(Exception exception);
        /// <summary>
        /// 
        /// </summary>
        void Debug(string format, params object[] args);
        /// <summary>
        /// 
        /// </summary>
        void Debug(Exception exception, string format, params object[] args);
        /// <summary>
        /// 
        /// </summary>
        void Error(Exception exception);
        /// <summary>
        /// 
        /// </summary>
        void Error(string format, params object[] args);
        /// <summary>
        /// 
        /// </summary>
        void Error(Exception exception, string format, params object[] args);
        /// <summary>
        /// 
        /// </summary>
        void Fatal(Exception exception);
        /// <summary>
        /// 
        /// </summary>
        void Fatal(string format, params object[] args);
        /// <summary>
        /// 
        /// </summary>
        void Fatal(Exception exception, string format, params object[] args);
        /// <summary>
        /// 
        /// </summary>
        void Info(Exception exception);
        /// <summary>
        /// 
        /// </summary>
        void Info(string format, params object[] args);
        /// <summary>
        /// 
        /// </summary>
        void Info(Exception exception, string format, params object[] args);
        /// <summary>
        /// 
        /// </summary>
        void Trace(Exception exception);
        /// <summary>
        /// 
        /// </summary>
        void Trace(string format, params object[] args);
        /// <summary>
        /// 
        /// </summary>
        void Trace(Exception exception, string format, params object[] args);
        /// <summary>
        /// 
        /// </summary>
        void Warn(Exception exception);
        /// <summary>
        /// 
        /// </summary>
        void Warn(string format, params object[] args);
        /// <summary>
        /// 
        /// </summary>
        void Warn(Exception exception, string format, params object[] args);
    }
}
