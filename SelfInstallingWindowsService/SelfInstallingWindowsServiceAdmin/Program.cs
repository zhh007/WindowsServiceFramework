using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Resources;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using SelfInstallingWindowsServiceAdmin.UI;
using SelfInstallingWindowsServiceAdmin.Utility;
using Service.Core.Log;
using SC_StatusDb = Service.Core.StatusDatabase;
using SC_Svc = Service.Core.WindowsService.Service;

namespace SelfInstallingWindowsServiceAdmin {

	internal static class Program {

		/// <summary>
		/// Runs this instance.
		/// </summary>
		internal static void Run() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			InitializeAppDependencies();
			Settings.Instance.IsAdmin = true;

			if (!CheckAndInstallService()) return;
			if (!CheckAndInstallStatusDatabase()) return;

			Application.Run(new frmMain());
		}

		/// <summary>
		/// Checks and installs service.
		/// </summary>
		/// <returns></returns>
		private static bool CheckAndInstallService() {
			// if the service fails to start, chances are it's not installed.
			try {
				// Check if service is installed and/or running
				if (SC_Svc.ServiceControl.ServiceStatus != ServiceControllerStatus.Running) {
					SC_Svc.ServiceControl.ExecuteCommand(SC_Svc.ServiceControl.ServiceCommandEnum.Start);
				}
			}
			catch {
				// Prompt to install service
				if (MessageBox.Show(string.Format("Install {0}?", Settings.Instance.ServiceDisplayName), String.Empty, MessageBoxButtons.YesNo) == DialogResult.No) return false;

				Logging.Log(LogLevelEnum.Info, "Installing service");

				ExtractResources();

				// Pause 2 seconds to allow file writes.
				Thread.Sleep(2000);

				try {
					// Install the service
					ProcessStartInfo startInfo = new ProcessStartInfo(Settings.Instance.ServiceFile, "/install");
					startInfo.UseShellExecute = false;
					Process.Start(startInfo);
				}
				catch (Exception ex) {
					Logging.Log(LogLevelEnum.Fatal, "Install failed: " + FileLogger.GetInnerException(ex).Message);
					MessageBox.Show("Could not install " + Settings.Instance.ServiceName);
				}

				Logging.Log(LogLevelEnum.Info, "Service installed");

				// Pause 2 seconds to allow service to install.
				Thread.Sleep(2000);
				StartService();
			}

			return true;
		}

		/// <summary>
		/// Checks and install the status database.
		/// </summary>
		/// <returns></returns>
		private static bool CheckAndInstallStatusDatabase() {
			try {
				// If we are the hub, install our status database
				if (Settings.Instance.IsHub) {
					Logging.Log(LogLevelEnum.Info, "Installing service status database");
					int serviceId = SC_StatusDb.DatabaseInstaller.CreateDatabase(Settings.Instance.Service,
						Settings.Instance.ServiceStatusDbSetupSql, Settings.Instance.LoggerConfiguration);
					if (serviceId > -1) {
						Settings.Instance.ServiceId = serviceId;
					}
					Logging.Log(LogLevelEnum.Info, "Service status database installed");
				}
				else // Find it!!!
				{
					if (string.IsNullOrEmpty(Settings.Instance.ServiceStatusDatabasePath)) {
						bool adminFromHub = MessageBox.Show(string.Format("Set up hub administration of {0}?", Settings.Instance.ServiceDisplayName), String.Empty, MessageBoxButtons.YesNo) == DialogResult.Yes;

						if (adminFromHub) {
							Logging.Log(LogLevelEnum.Info, string.Format("Setting up hub administration of {0}", Settings.Instance.ServiceDisplayName));
							OpenFileDialog openFileDialog = new OpenFileDialog {
									FileName = Path.GetFileName(Settings.DefaultServiceStatusDbFileName),
									Filter = "Service Status Database (*.db)|*.db",
									Title = "Locate Service Status Database"
								};

							if (openFileDialog.ShowDialog() == DialogResult.OK) {
								Logging.Log(LogLevelEnum.Debug, string.Format("Service status database path: {0}", openFileDialog.FileName));
								Settings.Instance.SetServiceStatusDatabasePath(openFileDialog.FileName);
								int serviceId = SC_StatusDb.DatabaseInstaller.InitializeService(new SC_StatusDb.Service {
																										ServiceName = Settings.Instance.ServiceName,
																										ServiceDescription = Settings.Instance.ServiceDescription,
																										ServiceDisplayName = Settings.Instance.ServiceDisplayName,
																										ServiceStatusDatabasePath = Settings.Instance.ServiceStatusDatabasePath
																									},
																					Settings.Instance.LoggerConfiguration);
								if (serviceId > -1) {
									Settings.Instance.ServiceId = serviceId;
								}
							}
							else {
								// Prompt to install service
								if (MessageBox.Show(string.Format("Are you certain you do not want to have the hub monitor({0})?\n\nYou will not be able administer it through the hub application.", Settings.Instance.ServiceDisplayName), String.Empty, MessageBoxButtons.YesNo) == DialogResult.No) {
									CheckAndInstallStatusDatabase();
								}
								else {
									adminFromHub = false;
								}
							}
							Logging.Log(LogLevelEnum.Info, string.Format("Hub administration of {0} set up", Settings.Instance.ServiceName));
						}

						if (!adminFromHub) {
							MessageBox.Show(string.Format("{0} will not be administered via the hub application.\n\nNote: You will be prompted to administer {0} via the hub application on the next start up of this admin application.", Settings.Instance.ServiceDisplayName), String.Empty, MessageBoxButtons.OK);
							Logging.Log(LogLevelEnum.Info, string.Format("Hub administration of {0} bypassed", Settings.Instance.ServiceDisplayName));
						}
					}
				}
			}
			catch (Exception ex) {
				Logging.Log(LogLevelEnum.Fatal, "Status database install failed: " + FileLogger.GetInnerException(ex).Message);
				MessageBox.Show("Could not install status database for " + Settings.Instance.ServiceName);
			}

			return true;
		}

		/// <summary>
		/// Starts the service.
		/// </summary>
		private static void StartService() {
			if (MessageBox.Show(string.Format("Start {0}?", Settings.Instance.ServiceName), string.Empty, MessageBoxButtons.YesNo) == DialogResult.Yes) {
				SC_Svc.ServiceControl.ExecuteCommand(SC_Svc.ServiceControl.ServiceCommandEnum.Start);
			}
		}

		private static void InitializeAppDependencies() {
			List<string> appDependencies = new List<string>();

			ResourceManager rm = new ResourceManager(Assembly.GetEntryAssembly().GetName().Name + ".Properties.Resources", Assembly.GetEntryAssembly());

			int count = 1;

			string keyPrefix = "AppDependency_";

			while (!string.IsNullOrEmpty(rm.GetString(keyPrefix + count.ToString()))) {
				appDependencies.Add(rm.GetString(keyPrefix + count.ToString()));
				count++;
			}

			foreach (string dependency in appDependencies) {
				if (!File.Exists(dependency)) {
					Assembly assembly = Assembly.GetExecutingAssembly();
					using (Stream strm = assembly.GetManifestResourceStream(ProgramStarter.DependencyPrefix +
						(!ProgramStarter.DependencyPrefix.LastIndexOf(".").Equals(ProgramStarter.DependencyPrefix.Length - 1) ? "." : string.Empty)
						+ dependency)) {
						byte[] file = new byte[strm.Length];
						strm.Read(file, 0, (int)file.Length);
						strm.Close();

						FileInfo fi = new FileInfo(dependency);
						using (FileStream fs = fi.Create()) {
							fs.Write(file, 0, (int)file.Length);
							fs.Close();
						}
					}
				}
			}
		}

		/// <summary>
		/// Extracts the resources.
		/// </summary>
		private static void ExtractResources() {
			ExtractResource(Settings.Instance.ServiceFile);
			ExtractResource(Settings.Instance.ServiceConfigFile);
			List<string> dependencies = Settings.Instance.Dependencies;
			Logging.Log(LogLevelEnum.Info, "Begin extracting resources");
			foreach (string dependency in dependencies) {
				ExtractResource(dependency);
			}
			Logging.Log(LogLevelEnum.Info, "End extracting resources");
		}

		/// <summary>
		/// Extracts the resource.
		/// </summary>
		/// <param name="resourceName">Name of the resource.</param>
		private static void ExtractResource(string resourceName) {
			if (!File.Exists(resourceName)) {
				try {
					Logging.Log(LogLevelEnum.Debug, "Extracting resource: " + resourceName);

					Assembly assembly = Assembly.GetExecutingAssembly();
					using (Stream strm = assembly.GetManifestResourceStream(ProgramStarter.DependencyPrefix + (!ProgramStarter.DependencyPrefix.LastIndexOf(".").Equals(ProgramStarter.DependencyPrefix.Length - 1) ? "." : string.Empty) + resourceName)) {
						byte[] file = new byte[strm.Length];
						strm.Read(file, 0, (int)file.Length);
						strm.Close();

						FileInfo fi = new FileInfo(resourceName);
						using (FileStream fs = fi.Create()) {
							fs.Write(file, 0, (int)file.Length);
							fs.Close();
						}
					}

					Logging.Log(LogLevelEnum.Debug, "Extracted resource: " + resourceName);
				}
				catch (Exception ex) {
					Logging.Log(LogLevelEnum.Fatal, string.Format("Error extracting resource: {0}: {1})", resourceName, FileLogger.GetInnerException(ex).Message));
					MessageBox.Show("Error extracting resource: " + resourceName);
				}
			}
		}
	}
}