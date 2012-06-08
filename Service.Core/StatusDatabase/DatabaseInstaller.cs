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
using System.Data.SQLite;
using System.IO;
using Service.Core.Log;
using Service.Core.Log.Configuration;
using Service.Core.StatusDatabase.Utility;
using SC_StatusDbDt = Service.Core.StatusDatabase.DataTableClass;

namespace Service.Core.StatusDatabase {
	public class DatabaseInstaller {

		/// <summary>
		/// Creates the database. This is the method used by the hub set up.
		/// </summary>
		/// <returns>The service ID for the hub.</returns>
		public static int CreateDatabase(Service service, List<string> serviceStatusDbSetupSql, FileLoggerConfiguration loggerConfiguration) {
			int serviceId = -1;

			if (!File.Exists(service.ServiceStatusDatabasePath)) {
				Logging.Log(LogLevelEnum.Info, "Begin SQLite database initialization", loggerConfiguration);

				var db = new SC_StatusDbDt.Service();

				Logging.Log(LogLevelEnum.Debug, "Begin SQL statements", loggerConfiguration);
				foreach (var sqlStatement in serviceStatusDbSetupSql) {
					try {
						db.Execute(sqlStatement, new object[0]);
					}
					catch (Exception ex) {
						Logging.Log(LogLevelEnum.Fatal, FileLogger.GetInnerException(ex).Message, loggerConfiguration);
						Logging.HandleException(ex);
						throw;
					}
				}
				Logging.Log(LogLevelEnum.Debug, "End SQL statements", loggerConfiguration);

				serviceId = DatabaseInstaller.InitializeService(service, null, loggerConfiguration);

				Logging.Log(LogLevelEnum.Info, "End SQLite database initialization", loggerConfiguration);
			}

			return serviceId;
		}

		/// <summary>
		/// Initializes the service.
		/// </summary>
		/// <param name="service">The service.</param>
		/// <returns>The service ID</returns>
		public static int InitializeService(Service service, FileLoggerConfiguration loggerConfiguration) {
			Logging.Log(LogLevelEnum.Info, "Begin Service status initialization", loggerConfiguration);

			int serviceId = -1;

			Logging.Log(LogLevelEnum.Debug, "Begin SQLite database connection initialization", loggerConfiguration);
			using (SQLiteConnection sqliteConnection = SQLite.InitializeConnection(service.ServiceStatusDatabasePath, loggerConfiguration)) {
				sqliteConnection.Open();
				Logging.Log(LogLevelEnum.Debug, "SQLite Connection opened", loggerConfiguration);

				serviceId = InitializeService(service, sqliteConnection, loggerConfiguration);

				sqliteConnection.Close();
				Logging.Log(LogLevelEnum.Debug, "SQLite Connection closed", loggerConfiguration);
			}

			return serviceId;
		}

		/// <summary>
		/// Initializes the service.
		/// </summary>
		/// <param name="service">The service.</param>
		/// <param name="sqliteConnection">The sqlite connection.</param>
		/// <returns>The service ID</returns>
		internal static int InitializeService(Service service, SQLiteConnection sqliteConnection, FileLoggerConfiguration loggerConfiguration) {
			Logging.Log(LogLevelEnum.Info, string.Format("Beginning service status initialization:\n\t\tService name: {0}", service.ServiceName), loggerConfiguration);

			var svcTbl = new SC_StatusDbDt.Service();

			var newId = svcTbl.Insert(new {
				ServiceName = service.ServiceName,
				ServiceDescription = service.ServiceDescription,
				ServiceDisplayName = service.ServiceDisplayName,
				LocationId = service.LocationId,
				ApplicationId = service.ApplicationId,
				SystemId = service.SystemId,
				InstallPath = service.InstallPath,
				ServiceStatus = (int)Service.ServiceStatusEnum.Stopped,
				IsHub = service.IsHub
			});

			service = DatabaseController.GetService(service, loggerConfiguration);

			Logging.Log(LogLevelEnum.Info, string.Format("Service status initialization finished:\n\t\tService name: {0}\n\t\tService ID: {1}", service.ServiceName, service.ServiceId), loggerConfiguration);

			return service.ServiceId;
		}

		/// <summary>
		/// Removes the service.
		/// </summary>
		public static void RemoveService(Service service, FileLoggerConfiguration loggerConfiguration) {
			Logging.Log(LogLevelEnum.Info, "Begin Service status removal", loggerConfiguration);
			Logging.Log(LogLevelEnum.Debug, "Begin SQLite database connection initialization", loggerConfiguration);
			using (SQLiteConnection sqliteConnection = SQLite.InitializeConnection(service.ServiceStatusDatabasePath, loggerConfiguration)) {
				sqliteConnection.Open();
				Logging.Log(LogLevelEnum.Debug, "SQLite Connection opened", loggerConfiguration);

				Logging.Log(LogLevelEnum.Info, string.Format("Beginning service status removal:\t\tService name: {0}\t\tService ID: {1}", service.ServiceName, service.ServiceId), loggerConfiguration);
				string sqlStatement = string.Format("DELETE FROM ServiceCommand where ServiceId = {0}", service.ServiceId);

				try {
					SQLite.ExecuteNonQuery(sqlStatement, sqliteConnection, loggerConfiguration);
					sqlStatement = string.Format("DELETE FROM Service where ServiceId = {0}", service.ServiceId);
					SQLite.ExecuteNonQuery(sqlStatement, sqliteConnection, loggerConfiguration);
				}
				catch (Exception ex) {
					Logging.Log(LogLevelEnum.Fatal, FileLogger.GetInnerException(ex).Message, loggerConfiguration);
					Logging.HandleException(ex);
					throw;
				}

				sqliteConnection.Close();
				Logging.Log(LogLevelEnum.Debug, "SQLite Connection closed", loggerConfiguration);

				Logging.Log(LogLevelEnum.Info, string.Format("Service status removal finished:\t\tService name: {0}\t\tService ID: {1}", service.ServiceName, service.ServiceId), loggerConfiguration);
			}
			Logging.Log(LogLevelEnum.Info, "End Service status removal", loggerConfiguration);
		}
	}
}