using System;
using System.ServiceProcess;
using LoggingEngine;
using ServiceStatusDatabase;
using SelfInstallingWindowsService.Utility;
using SelfInstallingWindowsService.WCF;

namespace SelfInstallingWindowsService.Service
{
	internal class ServiceControl
	{
		public static void ExecuteCommand(string serviceName, int customCommandId)
		{
			try
			{
				Logging.Log(LogLevelEnum.Info, "Executing custom command");
				Logging.Log(LogLevelEnum.Debug, string.Format("\t\tService name: {0}\n\t\tCommand ID: {1}", serviceName, customCommandId));
				using (ServiceController sc = new ServiceController(serviceName))
				{
					sc.ExecuteCommand(customCommandId);
				}
				Logging.Log(LogLevelEnum.Info, "Custom command executed (may not complete before this message)");
			}
			catch (Exception ex)
			{
				Logging.Log(LogLevelEnum.Fatal, "Custom command failed: " + FileLogger.GetInnerException(ex).Message);
			}
		}

		public static void ExecuteCommand(int serviceId, int customCommandId)
		{
			ExecuteCommand(DatabaseController.GetService(serviceId).ServiceName, customCommandId);
		}	

		public static void SetServiceStatus(int serviceId, ServiceStatusEnum serviceStatus)
		{
			if (!string.IsNullOrEmpty(Settings.Instance.ServiceStatusDatabasePath))
			{
				ServiceStatusDatabase.Service service = DatabaseController.GetService(serviceId);
				service.ServiceStatus = (ServiceStatusDatabase.Service.ServiceStatusEnum)serviceStatus;
				DatabaseController.SetStatus(service);
			}
		}

		public static void SetServiceStatus(int serviceId, ServiceStatusEnum serviceStatus, bool success)
		{
			if (!string.IsNullOrEmpty(Settings.Instance.ServiceStatusDatabasePath))
			{
				SetServiceStatus(serviceId, serviceStatus);
				DatabaseController.SetExtensionSuccessStatus(serviceId, success, Settings.Instance.ServiceStatusDatabasePath);
			}
		}
	}
}