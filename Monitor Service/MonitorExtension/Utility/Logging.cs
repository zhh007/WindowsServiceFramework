using System;
using Service.Core.ExceptionHandler;
using Service.Core.Log;
using SC_BaseClasses = Service.Core.Utility.BaseClasses;

namespace SampleExtension.Utility
{
	internal class Logging : SC_BaseClasses.LoggingBase
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