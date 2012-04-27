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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Service.Core.Log;
using SC_BaseClasses = Service.Core.Utility.BaseClasses;

namespace SampleExtension.Utility {
	public class Settings : SC_BaseClasses.SettingsBase {
		#region	Singleton constructor

		private static readonly Lazy<Settings> instance = new Lazy<Settings>(() => new Settings());

		public static DataTable Configuration { get; private set; }

		private const string sectionName = "appSettings";

		private Settings() { }

		public static Settings Instance {
			get {
				return ParseConfiguration();
			}
		}

		private static Settings ParseConfiguration() {
			try {
				// LINQ to XML is your friend.
				Settings configuration = instance.Value;
				if (!configuration.AppSettingsLoaded) {
					TextReader reader = new StreamReader(configuration.XmlFilePath);
					XDocument xdoc = XDocument.Load(reader);

					// Insert configuration parsing here using LINQ to XML.

					configuration.AppSettingsLoaded = true;
				}

				return configuration;
			}
			catch (Exception ex) {
				StackTrace stackTrace = new StackTrace();
				Logging.Log(LogLevelEnum.Error, string.Format("{0}: {1}", stackTrace.GetFrame(1).GetMethod().Name, FileLogger.GetInnerException(ex).Message));
				Logging.HandleException(ex);
				throw;
			}
		}

		#endregion

		public void LoadConfiguration(string configurationFile) {
			Configuration = new DataTable("config");
			Configuration.Columns.Add(new DataColumn("Key", typeof(string)));
			Configuration.Columns.Add(new DataColumn("Value", typeof(string)));
			Configuration.Columns.Add(new DataColumn("OriginalValue", typeof(string)));
			DataColumn[] key = { Configuration.Columns[0] };
			Configuration.PrimaryKey = key;

			try {
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(configurationFile);
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

						DataRow dr = Configuration.NewRow();
						dr["Key"] = attribKey.Value;
						dr["Value"] = attribValue.Value;
						dr["OriginalValue"] = attribValue.Value;
						Configuration.Rows.Add(dr);
					}
				}
				//}
				xmlDoc = null;
			}
			catch {
				throw;
			}
		}

		public void UnloadConfiguration() {
			Configuration.Rows.Clear();
			Configuration = null;
		}

		public void SaveConfiguration(string configurationFile, DataTable configTable) {
			try {
				//Reload the configuration file
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(configurationFile);
				//Save a backup version
				xmlDoc.Save(configurationFile + "_bak" + DateTime.Now.ToString("yyyyMMddhhmmss", CultureInfo.InvariantCulture));
				//Repolulate the three supported sections
				RepopulateXmlSection(xmlDoc);
				xmlDoc.Save(configurationFile);
			}
			catch {
				throw;
			}
		}

		private void RepopulateXmlSection(XmlDocument xmlDoc) {
			XmlNodeList nodes = xmlDoc.SelectNodes("configuration/" + sectionName + "/add");
			for (int i = 0; i < nodes.Count; i++) {
				//Find the property in the property collection with the same name as the current node in the Xml document
				DataRow dr = Configuration.Rows.Find(nodes[i].Attributes["key"].Value);

				if (!dr["Value"].ToString().Equals(dr["OriginalValue"].ToString())) {
					//Set the node value to the property value (which will have been set in the Property grid.
					nodes[i].Attributes["value"].Value = dr["Value"].ToString();
				}
			}
		}

		public void UpdateConfiguration(string key, string value) {
			DataRow dr = Configuration.Rows.Find(key);
			dr["Value"] = value;
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

		public string ServiceFile {
			get {
				return GetResourceManager().GetString("ServiceFile");
			}
		}

		public string ServiceConfigFile {
			get {
				return GetResourceManager().GetString("ServiceConfigFile");
			}
		}

		public string DependencyPrefix {
			get {
				return GetResourceManager().GetString("DependencyPrefix");
			}
		}

		#region App settings

		private bool AppSettingsLoaded { get; set; }

		#endregion
	}
}