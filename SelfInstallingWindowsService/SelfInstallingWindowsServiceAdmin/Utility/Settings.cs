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
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Xml;
using Service.Core.Log;
using Service.Core.Log.Configuration;
using SC_BaseClasses = Service.Core.Utility.BaseClasses;

namespace SelfInstallingWindowsServiceAdmin.Utility {
	public class Settings : SC_BaseClasses.SettingsBase {
		private static readonly Lazy<Settings> instance = new Lazy<Settings>(() => new Settings());

		public static DataTable ServiceConfiguration { get; private set; }

		private const string sectionName = "appSettings";
		private const string serviceConfigColumnKey = "Key";
		private const string serviceConfigColumnValue = "Value";
		private const string serviceConfigColumnOriginalValue = "OriginalValue";

		#region Logging configuration

		public FileLoggerConfiguration ServiceLoggerConfiguration {
			get {
				LoadConfiguration(ServiceConfigFile);
				FileLoggerConfiguration retConfig = new FileLoggerConfiguration();

				foreach (DataRow row in ServiceConfiguration.Rows) {
					switch (row["Key"].ToString()) {
						case LogFileKey:
							retConfig.LogFileName = row["Value"].ToString();
							break;
						case EnableDebugLoggingKey:
							retConfig.EnableDebug = bool.Parse(row["Value"].ToString());
							break;
						case EnableInfoLoggingKey:
							retConfig.EnableInfo = bool.Parse(row["Value"].ToString());
							break;
						case EnableWarnLoggingKey:
							retConfig.EnableWarn = bool.Parse(row["Value"].ToString());
							break;
					}
				}

				retConfig.ApplicationId = 0;
				retConfig.LocationId = 0;
				retConfig.ReportedBy = string.Empty;
				retConfig.SystemId = 0;

				return retConfig;
			}
		}

		#endregion Logging configuration

		public override int ServiceId {
			get {
				LoadConfiguration(ServiceConfigFile);
				int serviceId = -1;

				foreach (DataRow row in ServiceConfiguration.Rows) {
					if (row["Key"].ToString() == ServiceIdKey && !string.IsNullOrEmpty(ServiceStatusDatabasePath)) {
						int.TryParse(row["Value"].ToString(), out serviceId);
						break;
					}
				}

				UnloadConfiguration();
				return serviceId;
			}
			set {
				LoadConfiguration(ServiceConfigFile);
				UpdateConfiguration(ServiceIdKey, value.ToString());
				SaveConfiguration(ServiceConfigFile, null);
				UnloadConfiguration();
			}
		}

		public override void SetServiceStatusDatabasePath(string databasePath) {
			Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			config.AppSettings.Settings.Remove(ServiceStatusDatabasePathKey);
			config.AppSettings.Settings.Add(ServiceStatusDatabasePathKey, databasePath);
			config.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection(sectionName);

			LoadConfiguration(ServiceConfigFile);
			UpdateConfiguration(ServiceStatusDatabasePathKey, databasePath);
			SaveConfiguration(ServiceConfigFile, null);
			UnloadConfiguration();
		}

		private Settings() { }

		public static Settings Instance {
			get {
				return instance.Value;
			}
		}

		public void LoadConfiguration(string configurationFile) {
			Logging.Log(LogLevelEnum.Debug, "Loading service configuration");
			ServiceConfiguration = new DataTable("config");
			ServiceConfiguration.Columns.Add(new DataColumn(serviceConfigColumnKey, typeof(string)));
			ServiceConfiguration.Columns.Add(new DataColumn(serviceConfigColumnValue, typeof(string)));
			ServiceConfiguration.Columns.Add(new DataColumn(serviceConfigColumnOriginalValue, typeof(string)));
			DataColumn[] key = { ServiceConfiguration.Columns[0] };
			ServiceConfiguration.PrimaryKey = key;

			try {
				// Load the XML configuration
				XmlDocument xmlDoc = Utilities.LoadXmlDocument(configurationFile);
				XmlNode configuration = xmlDoc.SelectSingleNode("configuration");

				//Build the node list
				//XmlNodeList sectionList = configuration.ChildNodes;
				//for (int y = 0; y < sectionList.Count; y++)
				//{
				XmlNodeList settingsList = xmlDoc.SelectNodes("configuration/" + sectionName + "/add");
				if (settingsList.Count != 0 && settingsList != null) {
					//Add a property to customClass for each node found.
					for (int i = 0; i < settingsList.Count; i++) {
						XmlAttribute attribKey = settingsList[i].Attributes["key"];
						XmlAttribute attribValue = settingsList[i].Attributes["value"];

						DataRow dr = ServiceConfiguration.NewRow();
						dr[serviceConfigColumnKey] = attribKey.Value;
						dr[serviceConfigColumnValue] = attribValue.Value;
						dr[serviceConfigColumnOriginalValue] = attribValue.Value;
						ServiceConfiguration.Rows.Add(dr);
					}
				}
				//}
				xmlDoc = null;
				Logging.Log(LogLevelEnum.Debug, string.Format("Service configuration loaded: {0} keys", ServiceConfiguration.Rows.Count));
			}
			catch {
				throw;
			}
		}

		public void UnloadConfiguration() {
			ServiceConfiguration.Rows.Clear();
			ServiceConfiguration = null;
		}

		public void SaveConfiguration(string configurationFile, DataTable configTable) {
			try {
				Logging.Log(LogLevelEnum.Debug, "Saving service configuration");
				//Reload the configuration file
				XmlDocument xmlDoc = Utilities.LoadXmlDocument(configurationFile);
				//Save a backup version
				Utilities.SaveXmlDocument(xmlDoc, configurationFile + "_bak" + DateTime.Now.ToString("yyyyMMddhhmmss", CultureInfo.InvariantCulture));
				//Repolulate the three supported sections
				RepopulateXmlSection(xmlDoc);
				Utilities.SaveXmlDocument(xmlDoc, configurationFile);
				Logging.Log(LogLevelEnum.Debug, string.Format("Service configuration saved: {0} keys", ServiceConfiguration.Rows.Count));
			}
			catch {
				throw;
			}
		}

		private void RepopulateXmlSection(XmlDocument xmlDoc) {
			XmlNodeList nodes = xmlDoc.SelectNodes("configuration/" + sectionName + "/add");
			for (int i = 0; i < nodes.Count; i++) {
				//Find the property in the property collection with the same name as the current node in the Xml document
				DataRow dr = ServiceConfiguration.Rows.Find(nodes[i].Attributes["key"].Value);

				if (!dr[serviceConfigColumnValue].ToString().Equals(dr[serviceConfigColumnOriginalValue].ToString())) {
					//Set the node value to the property value (which will have been set in the Property grid.
					nodes[i].Attributes["value"].Value = dr[serviceConfigColumnValue].ToString();
				}
			}
		}

		public void UpdateConfiguration(string key, string value) {
			DataRow dr = ServiceConfiguration.Rows.Find(key);

			// TODO: For future use...
			//if (dr == null)
			//{
			//    dr = Configuration.NewRow();
			//    dr["Key"] = key;
			//    dr["Value"] = value;
			//    Configuration.Rows.Add(dr);
			//    return;
			//}

			dr[serviceConfigColumnValue] = value;
		}

		public List<SC_BaseClasses.ConfigurationKey> GetServiceConfiguration() {
			LoadConfiguration(ServiceConfigFile);

			List<SC_BaseClasses.ConfigurationKey> configSettings = new List<SC_BaseClasses.ConfigurationKey>();

			foreach (DataRow row in ServiceConfiguration.Rows) {
				configSettings.Add(new SC_BaseClasses.ConfigurationKey {
					Section = "appSettings",
					Key = row[serviceConfigColumnKey].ToString(),
					Value = row[serviceConfigColumnValue].ToString(),
					OriginalValue = row[serviceConfigColumnOriginalValue].ToString(),
				});
			}

			return configSettings;
		}

		public List<string> Dependencies {
			get {
				return GetResourceList("Dependency_");
			}
		}

		public List<string> LoggingDependencies {
			get {
				return GetResourceList("Logging_");
			}
		}

		public string DependencyPrefix {
			get {
				return GetResourceManager().GetString("DependencyPrefix");
			}
		}

		public List<SC_BaseClasses.ConfigurationKey> GetAdminConfiguration() {
			List<SC_BaseClasses.ConfigurationKey> configSettings = new List<SC_BaseClasses.ConfigurationKey>();

			foreach (string configItem in ConfigurationManager.AppSettings) {
				configSettings.Add(new SC_BaseClasses.ConfigurationKey {
					Section = "appSettings",
					Key = configItem,
					Value = ConfigurationManager.AppSettings[configItem],
					OriginalValue = ConfigurationManager.AppSettings[configItem]
				});
			}

			return configSettings;
		}
	}
}