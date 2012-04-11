using System;
using Service.Core.ExceptionHandler;
using Service.Core.Log;
using SC_Utility = Service.Core.Utility;

namespace Service.Core.WindowsService.Utility
{
	internal class Logging : SC_Utility.BaseClasses.LoggingBase
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