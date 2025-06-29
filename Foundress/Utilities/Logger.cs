using Serilog;
using Serilog.Events;
using System;

namespace Foundress.Utilities
{
    public static class Logger
    {
        private static bool _initialized = false;
        private static LogEventLevel _level = LogEventLevel.Debug;
        private static string _logFilePath = "log/log.txt";

        public static LogEventLevel Level
        {
            get { return _level; }
        }

        // Event for log message callbacks
        public static event Action<string, string> OnLogMessage;

        public static void Init(string logFilePath = "log/log.txt", LogEventLevel level = LogEventLevel.Debug)
        {
            if (_initialized) return;
            _level = level;
            _logFilePath = logFilePath;
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(_level)
                .WriteTo.File(_logFilePath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            _initialized = true;
            Log.Verbose("Logger initialized");
        }

        public static void Write(LogEventLevel level, string message, Exception exception = null)
        {
            
            if (level >= _level)
            {
                if (exception != null) {
                    Log.Write(level, message, exception);
                    OnLogMessage?.Invoke(level.ToString(), $"{message} For more information, check the error log.");
                } else {
                    Log.Write(level, message);
                    OnLogMessage?.Invoke(level.ToString(), message);
                }
            }
        }

        public static void ChangeLevel(LogEventLevel level)
        {
            _level = level;
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(_level)
                .WriteTo.File(_logFilePath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            _initialized = true;
            Log.Verbose($"Logging level changed to {level.ToString()}");
        }

        // Convenience methods using the generic Log method
        public static void Verbose(string message) => Write(LogEventLevel.Verbose, message);
        public static void Debug(string message) => Write(LogEventLevel.Debug, message);
        public static void Info(string message) => Write(LogEventLevel.Information, message);
        public static void Warn(string message) => Write(LogEventLevel.Warning, message);
        public static void Error(string message) => Write(LogEventLevel.Error, message);
        public static void Fatal(string message) => Write(LogEventLevel.Fatal, message);

        // Exception overloads
        public static void Error(Exception exception, string message = "An exception occurred") 
            => Write(LogEventLevel.Error, message, exception);
        public static void Fatal(Exception exception, string message = "Fatal error occurred") 
            => Write(LogEventLevel.Fatal, message, exception);

        public static void Close() => Log.CloseAndFlush();
    }
} 
