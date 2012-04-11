using System;
using System.Collections.Generic;
using Service.Core.ExceptionHandler;
using Service.Core.Log;
using Service.Core.Log.Configuration;

namespace Service.Core.StatusDatabase.Utility
{
	internal class Logging
	{
		private static HandledExceptionHandler exceptionHandler = HandledExceptionHandler.Instance();

		public static void HandleException(Exception ex)
		{
			exceptionHandler.HandleException(ex);
		}

		public static void Log(LogLevelEnum logLevel, string message, FileLoggerConfiguration configuration)
		{
			FileLogger fileLogger = FileLogger.Instance(configuration);
			fileLogger.WriteLog(logLevel, message);
		}

		private static List<LogRecord> Parse(LogLevelEnum logLevel, FileLoggerConfiguration configuration)
		{
			FileLogger fileLogger = FileLogger.Instance(configuration);
			return fileLogger.Parse(logLevel);
		}

		private static List<LogRecord> Parse(FileLoggerConfiguration configuration)
		{
			FileLogger fileLogger = FileLogger.Instance(configuration);
			return fileLogger.Parse();
		}
	}
}