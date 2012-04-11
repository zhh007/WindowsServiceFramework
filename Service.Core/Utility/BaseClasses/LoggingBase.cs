using System.Collections.Generic;
using Service.Core.Log;
using Service.Core.Log.Configuration;

namespace Service.Core.Utility.BaseClasses
{
	public abstract class LoggingBase
	{
		protected static void Log(LogLevelEnum logLevel, string message, FileLoggerConfiguration configuration)
		{
			FileLogger fileLogger = FileLogger.Instance(configuration);
			fileLogger.WriteLog(logLevel, message);
		}

		protected static List<LogRecord> Parse(LogLevelEnum logLevel, FileLoggerConfiguration configuration)
		{
			FileLogger fileLogger = FileLogger.Instance(configuration);
			return fileLogger.Parse(logLevel);
		}

		protected static List<LogRecord> Parse(FileLoggerConfiguration configuration)
		{
			FileLogger fileLogger = FileLogger.Instance(configuration);
			return fileLogger.Parse();
		}
	}
}