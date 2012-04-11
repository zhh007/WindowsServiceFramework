using System;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using LoggingEngine;
using SelfInstallingWindowsServiceAdmin.Utility;
using ServiceStatusDatabase;

namespace SelfInstallingWindowsServiceAdmin.Service
{
	public class ServiceControl
	{
		public enum ServiceCommandEnum
		{
			Start = 0,
			Stop = 1,
			Restart = 2,
			Uninstall = 255			
		}

		public static void ExecuteCommand(ServiceCommandEnum serviceCommand)
		{
			ExecuteCommand(Settings.Instance.ServiceName, serviceCommand);
		}

		public static void ExecuteCommand(string serviceName, ServiceCommandEnum serviceCommand)
		{
			System.ServiceProcess.ServiceController serviceController = new System.ServiceProcess.ServiceController(serviceName);

			switch (serviceCommand)
			{
				case ServiceCommandEnum.Start:
					try
					{
						Logging.Log(LogLevelEnum.Info, "Starting service");
						serviceController.Start();
						serviceController.WaitForStatus(ServiceControllerStatus.Running);
						Logging.Log(LogLevelEnum.Info, "Service started");
					}
					catch (Exception ex)
					{
						Logging.Log(LogLevelEnum.Fatal, "Start failed: " + FileLogger.GetInnerException(ex).Message);
						MessageBox.Show("Could not start " + Settings.Instance.ServiceDisplayName);
					}
					break;

				case ServiceCommandEnum.Stop:
					try
					{
						Logging.Log(LogLevelEnum.Info, "Stopping service");
						serviceController.Stop();
						serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
						Logging.Log(LogLevelEnum.Info, "Service stopped");
					}
					catch (Exception ex)
					{
						Logging.Log(LogLevelEnum.Fatal, "Stop failed: " + FileLogger.GetInnerException(ex).Message);
						MessageBox.Show("Could not stop " + Settings.Instance.ServiceName);
					}
					break;

				case ServiceCommandEnum.Restart:
					try
					{
						Logging.Log(LogLevelEnum.Info, "Restarting service");
						serviceController.Stop();
						serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
						serviceController.Start();
						serviceController.WaitForStatus(ServiceControllerStatus.Running);
						Logging.Log(LogLevelEnum.Info, "Service restarted");
					}
					catch (Exception ex)
					{
						Logging.Log(LogLevelEnum.Fatal, "Restart failed: " + FileLogger.GetInnerException(ex).Message);
						MessageBox.Show("Could not restart " + Settings.Instance.ServiceName);
					}
					break;

				case ServiceCommandEnum.Uninstall:
					try
					{
						Logging.Log(LogLevelEnum.Info, "Uninstalling service");

						if (ServiceControl.ServiceStatus != ServiceControllerStatus.Running)
						{
							serviceController.Start();
							serviceController.WaitForStatus(ServiceControllerStatus.Running);
						}

						serviceController.ExecuteCommand((int)ServiceCommandEnum.Uninstall);
						serviceController.Close();

						Thread.Sleep(250);
						File.Delete(Settings.Instance.ServiceFile);

						Logging.Log(LogLevelEnum.Info, "Service uninstalled");
						return;
					}
					catch (Exception ex)
					{
						Logging.Log(LogLevelEnum.Fatal, "Uninstall failed: " + FileLogger.GetInnerException(ex).Message);
						MessageBox.Show("Could not uninstall " + Settings.Instance.ServiceName);
					}
					break;				
			}

			serviceController.Close();
		}

		public static void ExecuteCommand(int serviceId, ServiceCommandEnum serviceCommand)
		{
			ExecuteCommand(DatabaseController.GetService(serviceId).ServiceName, serviceCommand);
		}

		public static void ExecuteCommand(string serviceName, int customCommandId)
		{
			try
			{
				Logging.Log(LogLevelEnum.Info, "Executing custom command");
				Logging.Log(LogLevelEnum.Debug, string.Format("\t\tService name: {0}\n\t\tCommand ID: {1}", serviceName, customCommandId));
				ServiceController sc = new ServiceController(serviceName);
				sc.ExecuteCommand(customCommandId);
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

		public static ServiceControllerStatus GetServiceStatus(string serviceName)
		{
			System.ServiceProcess.ServiceController sc = new System.ServiceProcess.ServiceController(serviceName);
			ServiceControllerStatus status = sc.Status;
			sc.Close();

			return status;
		}

		public static ServiceControllerStatus ServiceStatus
		{
			get
			{
				System.ServiceProcess.ServiceController sc = new System.ServiceProcess.ServiceController(Settings.Instance.ServiceName);
				ServiceControllerStatus status = sc.Status;
				sc.Close();

				return status;
			}
		}
	}
}