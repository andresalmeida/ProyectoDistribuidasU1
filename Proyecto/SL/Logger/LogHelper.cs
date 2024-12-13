using System;
using Serilog;

namespace SL.Logger
{
    public static class LogHelper
    {
        static LogHelper()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/application.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public static void LogInformation(string message)
        {
            Log.Information(message);
        }

        public static void LogWarning(string message)
        {
            Log.Warning(message);
        }

        public static void LogError(string message, Exception ex)
        {
            Log.Error(ex, message);
        }
    }
}
