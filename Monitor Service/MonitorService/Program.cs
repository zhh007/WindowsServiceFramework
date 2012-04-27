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
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;
using MonitorService.Service;
using MonitorService.Utility;
using Service.Core.Log;
using SC_StatusDb = Service.Core.StatusDatabase;

namespace MonitorService {
	internal static class Program {

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		internal static void Run(string[] args) {
			string opt = args.Length > 0 ? args[0] : string.Empty;

			if (string.IsNullOrEmpty(opt)) {
				ServiceBase[] ServicesToRun;
				ServicesToRun = new ServiceBase[] { new WindowsService() };
				ServiceBase.Run(ServicesToRun);
			}
			else {
				try {
					switch (opt.ToLower()) {
						case "/install":
							Logging.Log(LogLevelEnum.Info, "Installing service");

							ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });

							Logging.Log(LogLevelEnum.Info, "Service installed");

							break;
						case "/uninstall":
							Logging.Log(LogLevelEnum.Info, "Uninstalling service");

							ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });

							SC_StatusDb.DatabaseInstaller.RemoveService(new SC_StatusDb.Service { ServiceId = Settings.Instance.ServiceId, ServiceStatusDatabasePath = Settings.Instance.ServiceStatusDatabasePath }, Settings.Instance.LoggerConfiguration);

							Logging.Log(LogLevelEnum.Info, "Service uninstalled");

							break;
					}
				}
				catch (Exception ex) {
					Logging.Log(LogLevelEnum.Fatal, "Service install failed: " + FileLogger.GetInnerException(ex).Message);
				}
			}
		}
	}
}