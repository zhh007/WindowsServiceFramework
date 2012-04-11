using System.Collections.Generic;
using Service.Core.Log;
using SC_BaseClasses = Service.Core.Utility.BaseClasses;

namespace MonitorServiceAdmin.Utility
{
    internal class Logging : SC_BaseClasses.LoggingBase
    {
        public static void Log(LogLevelEnum logLevel, string message)
        {
            Log(logLevel, message, Settings.Instance.LoggerConfiguration);
        }

        public static List<LogRecord> Parse(LogEnum log, LogLevelEnum logLevel)
        {
            switch (log)
            {
                case LogEnum.Admin: return Parse(logLevel, Settings.Instance.LoggerConfiguration);
                //case LogEnum.Service: return Parse(logLevel, Settings.Instance.ServiceLoggerConfiguration);
                default: return null;
            }
        }

        public static List<LogRecord> Parse(LogEnum log)
        {
            switch (log)
            {
                case LogEnum.Admin: return Parse(Settings.Instance.LoggerConfiguration);
                //case LogEnum.Service: return Parse(Settings.Instance.ServiceLoggerConfiguration);
                default: return null;
            }
        }
    }
}