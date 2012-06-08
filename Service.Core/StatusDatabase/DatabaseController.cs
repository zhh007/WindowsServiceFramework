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
using System.Collections.Generic;
using Service.Core.Log.Configuration;
using SC_StatusDbDt = Service.Core.StatusDatabase.DataTableClass;

namespace Service.Core.StatusDatabase {
	public class DatabaseController {

		/// <summary>
		/// Sets the service status.
		/// </summary>
		/// <param name="service">The service.</param>
		public static void SetStatus(Service service, FileLoggerConfiguration loggerConfiguration) {
			SetStatus(service.ServiceStatus, service.ServiceId, loggerConfiguration);
		}

		/// <summary>
		/// Sets the service status.
		/// </summary>
		/// <param name="serviceStatus"></param>
		/// <param name="serviceId"></param>

		public static void SetStatus(Service.ServiceStatusEnum serviceStatus, int serviceId, FileLoggerConfiguration loggerConfiguration) {
			var svcTable = new SC_StatusDbDt.Service();
			svcTable.Update(new { ServiceStatus = (int)serviceStatus }, serviceId);
		}

		/// <summary>
		/// Gets the service status.
		/// </summary>
		/// <param name="serviceId"></param>
		/// <returns>The service status.</returns>
		public static Service.ServiceStatusEnum GetServiceStatus(int serviceId, FileLoggerConfiguration loggerConfiguration) {
			return GetService(serviceId, loggerConfiguration).ServiceStatus;
		}

		/// <summary>
		/// Gets the service status.
		/// </summary>
		/// <param name="service">The service.</param>
		/// <returns>The service status.</returns>
		public static Service.ServiceStatusEnum GetServiceStatus(Service service, FileLoggerConfiguration loggerConfiguration) {
			return GetServiceStatus(service.ServiceId, loggerConfiguration);
		}

		/// <summary>
		/// Gets the services.
		/// </summary>
		/// <param name="serviceStatusDatabasePath">The service status database path.</param>
		/// <param name="loggerConfiguration">The logger configuration.</param>
		/// <returns>
		/// A generic list of services administered by the hub service.
		/// </returns>
		public static List<Service> GetServices(FileLoggerConfiguration loggerConfiguration) {
			List<Service> services = new List<Service>();

			var svcTable = new SC_StatusDbDt.Service();
			var svcs = svcTable.All();

			foreach (var svc in svcs) {
				services.Add(new Service {
					ServiceId = (int)svc.ServiceId,
					ServiceName = svc.ServiceName,
					ServiceDescription = svc.ServiceDescription,
					ServiceStatus = (Service.ServiceStatusEnum)((int)svc.ServiceStatus),
					ServiceDisplayName = svc.ServiceDisplayName,
					LocationId = (int)svc.LocationId,
					SystemId = (int)svc.SystemId,
					ApplicationId = (int)svc.ApplicationId,
					InstallPath = svc.InstallPath,
					IsHub = svc.IsHub,
					ServiceCommands = GetServiceCommands((int)svc.ServiceId, loggerConfiguration)
				});
			}

			return services;
		}

		/// <summary>
		/// Gets the service.
		/// </summary>
		/// <param name="serviceId">The service id.</param>
		/// <param name="loggerConfiguration">The logger configuration.</param>
		/// <returns></returns>
		internal static Service GetService(int serviceId, FileLoggerConfiguration loggerConfiguration) {
			return GetService(new Service { ServiceId = serviceId }, loggerConfiguration);
		}

		/// <summary>
		/// Gets the service.
		/// </summary>
		/// <param name="service">The service.</param>
		/// <param name="loggerConfiguration">The logger configuration.</param>
		/// <returns>The complete service.</returns>
		internal static Service GetService(Service service, FileLoggerConfiguration loggerConfiguration) {
			var svcTbl = new SC_StatusDbDt.Service();
			string where = string.Empty;
			if (string.IsNullOrEmpty(service.ServiceName)) {
				where = string.Format("{0} = {1}", "ServiceId", service.ServiceId);
			}
			else {
				where = string.Format("{0} = '{1}'", "ServiceName", service.ServiceName);
			}
			var svc = svcTbl.Single(where: where);

			return new Service {
				ServiceId = (int)svc.ServiceId,
				ServiceName = svc.ServiceName,
				ServiceDescription = svc.ServiceDescription,
				ServiceStatus = (Service.ServiceStatusEnum)((int)svc.ServiceStatus),
				ServiceDisplayName = svc.ServiceDisplayName,
				LocationId = (int)svc.LocationId,
				SystemId = (int)svc.SystemId,
				ApplicationId = (int)svc.ApplicationId,
				InstallPath = svc.InstallPath,
				IsHub = svc.IsHub,
				ServiceCommands = GetServiceCommands(service, loggerConfiguration)
			};
		}

		/// <summary>
		/// Gets the service commands.
		/// </summary>
		/// <param name="serviceId">The service id.</param>
		/// <param name="loggerConfiguration">The logger configuration.</param>
		/// <returns></returns>
		public static List<ServiceCommandStruct> GetServiceCommands(int serviceId, FileLoggerConfiguration loggerConfiguration) {
			return GetServiceCommands(new Service { ServiceId = serviceId }, loggerConfiguration);
		}

		/// <summary>
		/// Gets the service commands.
		/// </summary>
		/// <param name="service">The service.</param>
		/// <returns>A generic list of ServiceCommandStruct</returns>
		public static List<ServiceCommandStruct> GetServiceCommands(Service service, FileLoggerConfiguration loggerConfiguration) {
			var svcTable = new SC_StatusDbDt.ServiceCommand();
			var svcCommands = svcTable.All(where: string.Format("ServiceId = {0}", service.ServiceId));

			List<ServiceCommandStruct> tempCommands = new List<ServiceCommandStruct>();
			foreach (var command in svcCommands) {
				tempCommands.Add(new ServiceCommandStruct {
					CommandId = command.CommandId,
					CommandDescription = command.CommandDescription
				});
			}

			return tempCommands;
		}

		/// <summary>
		/// Updates the service monitor.
		/// </summary>
		/// <param name="service">The service.</param>
		/// <param name="serviceStatusDatabasePath">The service status database path.</param>
		public static void UpdateServiceMonitor(Service service, string serviceStatusDatabasePath, FileLoggerConfiguration loggerConfiguration) {
			var svcMon = new SC_StatusDbDt.ServiceMonitor();

			if (svcMon.Count(string.Format("ServiceId = {0}", service.ServiceId)) > 0) {
				svcMon.Update(new { MonitorTime = DateTime.Now }, new { ServiceId = service.ServiceId });
			}
			else {
				svcMon.Insert(new { MonitorTime = DateTime.Now, ServiceId = service.ServiceId });
			}
		}

		/// <summary>
		/// Gets the service monitor time.
		/// </summary>
		/// <param name="service">The service.</param>
		/// <param name="serviceStatusDatabasePath">The service status database path.</param>
		/// <returns></returns>
		public static DateTime GetServiceMonitorTime(Service service, string serviceStatusDatabasePath, FileLoggerConfiguration loggerConfiguration) {
			DateTime returnTime = new DateTime();

			var svcMon = new SC_StatusDbDt.ServiceMonitor();

			var monitorTime = svcMon.Single(new { ServiceId = service.ServiceId });

			returnTime = monitorTime.MonitorTime;

			return returnTime;
		}

		/// <summary>
		/// Sets the extension success status.
		/// </summary>
		/// <param name="serviceId">The service id.</param>
		/// <param name="success">if set to <c>true</c> [success].</param>
		/// <param name="serviceStatusDatabasePath">The service status database path.</param>
		public static void SetExtensionSuccessStatus(int serviceId, bool success, string serviceStatusDatabasePath, FileLoggerConfiguration loggerConfiguration) {
			var svcMon = new SC_StatusDbDt.ServiceMonitor();

			dynamic data = new { };

			if (svcMon.Count(string.Format("ServiceId = {0}", serviceId)) > 0) {
				if (success) {
					data = new { ServiceSuccessRunTime = DateTime.Now };
				}
				else {
					data = new { ServiceFailRunTime = DateTime.Now };
				}
				svcMon.Update(data, new { ServiceId = serviceId });
			}
			else {
				if (success) {
					data = new { ServiceSuccessRunTime = DateTime.Now, ServiceId = serviceId };
				}
				else {
					data = new { ServiceFailRunTime = DateTime.Now, ServiceId = serviceId };
				}
				svcMon.Insert(data);
			}
		}

		/// <summary>
		/// Gets the extension execute status time.
		/// </summary>
		/// <param name="service">The service.</param>
		/// <param name="success">if set to <c>true</c> [success].</param>
		/// <param name="serviceStatusDatabasePath">The service status database path.</param>
		/// <returns></returns>
		public static DateTime GetExtensionExecuteStatusTime(Service service, bool success, string serviceStatusDatabasePath, FileLoggerConfiguration loggerConfiguration) {
			DateTime returnTime = DateTime.MinValue;

			var svcMon = new SC_StatusDbDt.ServiceMonitor();

			var monitorTime = svcMon.Single(new { ServiceId = service.ServiceId });

			if (success) {
				returnTime = monitorTime.ServiceSuccessRunTime;
			}
			else {
				returnTime = monitorTime.ServiceFailRunTime;
			}

			return returnTime;
		}
	}
}