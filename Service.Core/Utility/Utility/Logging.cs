using System;
using Service.Core.ExceptionHandler;
using Service.Core.Log;
using Service.Core.Utility.BaseClasses;

namespace Service.Core.Utility.Utility
{
	internal class Logging : LoggingBase
	{
		private static HandledExceptionHandler exceptionHandler = HandledExceptionHandler.Instance();

		public static void HandleException(Exception ex)
		{
			exceptionHandler.HandleException(ex);
		}

		public static void Log(LogLevelEnum logLevel, string message)
		{
			Log(logLevel, message, Settings.Instance.LoggerConfiguration);
		}
	}
}