#region

// -----------------------------------------------------
// MIT License
// Copyright (C) 2012 John M. Baughman (jbaughmanphoto.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial
// portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// -----------------------------------------------------

#endregion

using System;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using Service.Core.Log;
using Service.Core.StatusDatabase;
using Service.Core.WindowsService.Utility;

namespace Service.Core.WindowsService.Service {
	public abstract class ServiceControl {
		public enum ServiceCommandEnum {
			Start = 0,
			Stop = 1,
			Restart = 2,
			Uninstall = 255
		}

		/// <summary>
		/// Executes the command.
		/// </summary>
		/// <param name="serviceCommand">The service command.</param>
		public static void ExecuteCommand(ServiceCommandEnum serviceCommand) {
			ExecuteCommand(Settings.Instance.ServiceName, serviceCommand);
		}

		/// <summary>
		/// Executes the command.
		/// </summary>
		/// <param name="serviceName">Name of the service.</param>
		/// <param name="serviceCommand">The service command.</param>
		public static void ExecuteCommand(string serviceName, ServiceCommandEnum serviceCommand) {
			System.ServiceProcess.ServiceController serviceController = new System.ServiceProcess.ServiceController(serviceName);

			switch (serviceCommand) {
				case ServiceCommandEnum.Start:
					try {
						Logging.Log(LogLevelEnum.Info, "Starting service");
						serviceController.Start();
						serviceController.WaitForStatus(ServiceControllerStatus.Running);
						Logging.Log(LogLevelEnum.Info, "Service started");
					}
					catch (Exception ex) {
						Logging.Log(LogLevelEnum.Fatal, "Start failed: " + FileLogger.GetInnerException(ex).Message);
						MessageBox.Show("Could not start " + Settings.Instance.ServiceDisplayName);
					}
					break;

				case ServiceCommandEnum.Stop:
					try {
						Logging.Log(LogLevelEnum.Info, "Stopping service");
						serviceController.Stop();
						serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
						Logging.Log(LogLevelEnum.Info, "Service stopped");
					}
					catch (Exception ex) {
						Logging.Log(LogLevelEnum.Fatal, "Stop failed: " + FileLogger.GetInnerException(ex).Message);
						MessageBox.Show("Could not stop " + Settings.Instance.ServiceName);
					}
					break;

				case ServiceCommandEnum.Restart:
					try {
						Logging.Log(LogLevelEnum.Info, "Restarting service");
						serviceController.Stop();
						serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
						serviceController.Start();
						serviceController.WaitForStatus(ServiceControllerStatus.Running);
						Logging.Log(LogLevelEnum.Info, "Service restarted");
					}
					catch (Exception ex) {
						Logging.Log(LogLevelEnum.Fatal, "Restart failed: " + FileLogger.GetInnerException(ex).Message);
						MessageBox.Show("Could not restart " + Settings.Instance.ServiceName);
					}
					break;

				case ServiceCommandEnum.Uninstall:
					try {
						Logging.Log(LogLevelEnum.Info, "Uninstalling service");

						if (ServiceControl.ServiceStatus != ServiceControllerStatus.Running) {
							serviceController.Start();
							serviceController.WaitForStatus(ServiceControllerStatus.Running);
						}

						serviceController.ExecuteCommand((int)ServiceCommandEnum.Uninstall);
						serviceController.Close();

						Thread.Sleep(250);
						//File.Delete(Settings.Instance.ServiceFile);

						Logging.Log(LogLevelEnum.Info, "Service uninstalled");
						return;
					}
					catch (Exception ex) {
						Logging.Log(LogLevelEnum.Fatal, "Uninstall failed: " + FileLogger.GetInnerException(ex).Message);
						MessageBox.Show("Could not uninstall " + Settings.Instance.ServiceName);
					}
					break;
			}

			serviceController.Close();
		}

		/// <summary>
		/// Executes the command.
		/// </summary>
		/// <param name="serviceId">The service id.</param>
		/// <param name="serviceCommand">The service command.</param>
		public static void ExecuteCommand(int serviceId, ServiceCommandEnum serviceCommand) {
			ExecuteCommand(DatabaseController.GetService(serviceId, Settings.Instance.LoggerConfiguration).ServiceName, serviceCommand);
		}

		/// <summary>
		/// Executes the command.
		/// </summary>
		/// <param name="serviceName">Name of the service.</param>
		/// <param name="customCommandId">The custom command id.</param>
		public static void ExecuteCommand(string serviceName, int customCommandId) {
			try {
				Logging.Log(LogLevelEnum.Info, "Executing custom command");
				Logging.Log(LogLevelEnum.Debug, string.Format("\t\tService name: {0}\n\t\tCommand ID: {1}", serviceName, customCommandId));
				using (ServiceController sc = new ServiceController(serviceName)) {
					sc.ExecuteCommand(customCommandId);
				}
				Logging.Log(LogLevelEnum.Info, "Custom command executed (may not complete before this message)");
			}
			catch (Exception ex) {
				Logging.Log(LogLevelEnum.Fatal, "Custom command failed: " + FileLogger.GetInnerException(ex).Message);
			}
		}

		/// <summary>
		/// Executes the command.
		/// </summary>
		/// <param name="serviceId">The service id.</param>
		/// <param name="customCommandId">The custom command id.</param>
		public static void ExecuteCommand(int serviceId, int customCommandId) {
			ExecuteCommand(DatabaseController.GetService(serviceId, Settings.Instance.LoggerConfiguration).ServiceName, customCommandId);
		}

		/// <summary>
		/// Gets the service status.
		/// </summary>
		/// <param name="serviceName">Name of the service.</param>
		/// <returns></returns>
		public static ServiceControllerStatus GetServiceStatus(string serviceName) {
			System.ServiceProcess.ServiceController sc = new System.ServiceProcess.ServiceController(serviceName);
			ServiceControllerStatus status = sc.Status;
			sc.Close();

			return status;
		}

		/// <summary>
		/// Gets the service status.
		/// </summary>
		/// <value>The service status.</value>
		public static ServiceControllerStatus ServiceStatus {
			get {
				System.ServiceProcess.ServiceController sc = new System.ServiceProcess.ServiceController(Settings.Instance.ServiceName);
				ServiceControllerStatus status = sc.Status;
				sc.Close();

				return status;
			}
		}

		/// <summary>
		/// Sets the service status.
		/// </summary>
		/// <param name="serviceId">The service id.</param>
		/// <param name="serviceStatus">The service status.</param>
		public static void SetServiceStatus(int serviceId, StatusDatabase.Service.ServiceStatusEnum serviceStatus) {
			if (!string.IsNullOrEmpty(Settings.Instance.ServiceStatusDatabasePath)) {
				StatusDatabase.Service service = DatabaseController.GetService(serviceId, Settings.Instance.LoggerConfiguration);
				service.ServiceStatus = (StatusDatabase.Service.ServiceStatusEnum)serviceStatus;
				DatabaseController.SetStatus(service, Settings.Instance.LoggerConfiguration);
			}
		}

		/// <summary>
		/// Sets the service status.
		/// </summary>
		/// <param name="serviceId">The service id.</param>
		/// <param name="serviceStatus">The service status.</param>
		/// <param name="success">if set to <c>true</c> [success].</param>
		public static void SetServiceStatus(int serviceId, StatusDatabase.Service.ServiceStatusEnum serviceStatus, bool success) {
			if (!string.IsNullOrEmpty(Settings.Instance.ServiceStatusDatabasePath)) {
				SetServiceStatus(serviceId, serviceStatus);
				DatabaseController.SetExtensionSuccessStatus(serviceId, success, Settings.Instance.ServiceStatusDatabasePath, Settings.Instance.LoggerConfiguration);
			}
		}
	}
}