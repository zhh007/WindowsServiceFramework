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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;
using Service.Core.Log;
using Service.Core.Log.Configuration;
using Service.Core.StatusDatabase;
using Service.Core.Utility.FileWatch;
using Service.Core.Utility.Utility;
using SU_Tmr = Service.Core.Utility.Timer;

namespace Service.Core.Utility.BaseClasses {
	public abstract class SettingsBase {
		#region	Abstract properties

		public virtual int ServiceId { get; set; }

		#endregion

		private const string serviceNameResourceName = "SERVICE_NAME";
		private const string serviceDescriptionResourceName = "SERVICE_DESCRIPTION";
		private const string serviceDisplayNameResourceName = "SERVICE_DISPLAY_NAME";
		private const string serviceStatusDbSetupPrefix = "SERVICE_STATUS_DB_SETUP_";
		private const string serviceCommandPrefix = "SERVICE_COMMAND_";

		private const int defaultInterval = 60; // 1 minute.
		private const string extensionPathResourceName = "EXTENSION_PATH";
		private const char notifyFiltersDelimiter = ',';

		protected const string AppSettingsSectionName = "appSettings";
		protected const string ConnectionStringsSectionName = "connectionStrings";
		protected const string ServiceStatusDatabasePathKey = "ServiceStatusDatabasePath";
		protected const string ServiceIdKey = "ServiceId";
		protected const string LogFileKey = "LogFileName";
		protected const string EnableDebugLoggingKey = "EnableDebugLogging";
		protected const string EnableInfoLoggingKey = "EnableInfoLogging";
		protected const string EnableWarnLoggingKey = "EnableWarnLogging";
		protected const string ApplicationIdKey = "ApplicationId";
		protected const string LocationIdKey = "LocationId";
		protected const string SystemIdKey = "SystemId";

		public const string DefaultServiceStatusDbFileName = @"ServiceStatus.db";

		public const int DefaultScheduleTimerInterval = 60;

		#region LINQ to XML utility methods

		/// <summary>
		/// Gets the element value.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns></returns>
		internal static string GetElementValue(XElement element, string defaultValue) {
			return element == null ? defaultValue : element.Value;
		}

		#endregion

		#region Status

		public bool IsRunning { get; set; }

		#endregion

		#region Service Descriptors

		public StatusDatabase.Service Service {
			get {
				return new StatusDatabase.Service {
					ServiceName = ServiceName,
					ServiceDisplayName = ServiceDisplayName,
					ServiceDescription = ServiceDescription,
					LocationId = LoggerConfiguration.LocationId,
					ApplicationId = LoggerConfiguration.ApplicationId,
					SystemId = LoggerConfiguration.SystemId,
					InstallPath = InstallPath,
					IsHub = IsHub
				};
			}
		}

		public string ServiceName {
			get {
				return GetResourceString(serviceNameResourceName);
			}
		}

		public string ServiceDescription {
			get {
				return GetResourceString(serviceDescriptionResourceName);
			}
		}

		public string ServiceDisplayName {
			get {
				return GetResourceString(serviceDisplayNameResourceName);
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

		public List<ServiceCommandStruct> ServiceCommands {
			get {
				List<string> commandList = GetResourceList(serviceCommandPrefix);
				List<ServiceCommandStruct> returnList = new List<ServiceCommandStruct>();

				foreach (string command in commandList) {
					string[] splitCommand = command.Split('|');
					returnList.Add(new ServiceCommandStruct {
						CommandId = int.Parse(splitCommand[0]),
						CommandDescription = splitCommand[0]
					});
				}

				return returnList;
			}
		}

		public string InstallPath {
			get {
				return AssemblyDirectory;
			}
		}

		#endregion

		#region Hub settings

		public bool UseWcf {
			get {
				bool useWcf = false; // By default, we don't want to run as a WCF service.
				try {
					if (bool.TryParse(GetConfigItem(ConfigType.Service, AppSettingsSectionName, "UseWcf").ToString().ToLower(), out useWcf))
						return useWcf;
					else
						return useWcf;
				}
				catch {
					return useWcf;
				}
			}
		}

		public bool IsHub {
			get {
				bool isHub = false; // By default, we don't want to run as the hub with a WCF service.
				try {
					if (bool.TryParse(GetConfigItem(ConfigType.Service, AppSettingsSectionName, "IsHub").ToString().ToLower(), out isHub))
						return isHub;
					else
						return isHub;
				}
				catch {
					return isHub;
				}
			}
		}

		public bool IsHubWithExtensionLoaded {
			get {
				return IsHub && ExtensionLoaded;
			}
		}

		public bool ExtensionLoaded { get; set; }

		public string WcfPort {
			get {
				return string.IsNullOrEmpty(GetConfigItem(ConfigType.Service, AppSettingsSectionName, "WcfPort").ToString()) ? "8001" : GetConfigItem(ConfigType.Service, AppSettingsSectionName, "WcfPort").ToString();
			}
		}

		public string WcfURLFormat {
			get {
				return GetResourceManager().GetString("ServiceURLFormat");
			}
		}

		public string WcfMexPort {
			get {
				return string.IsNullOrEmpty(GetConfigItem(ConfigType.Service, AppSettingsSectionName, "WcfMexPort").ToString()) ? "8002" : GetConfigItem(ConfigType.Service, AppSettingsSectionName, "WcfMexPort").ToString();
			}
		}

		public string WcfWSDLFormat {
			get {
				return GetResourceManager().GetString("ServiceWSDLFormat");
			}
		}

		public bool UseLoopback {
			get {
				// Default to operable production IP.
				return string.IsNullOrEmpty(GetConfigItem(ConfigType.Service, AppSettingsSectionName, "UseLoopback").ToString()) ? false : bool.Parse(GetConfigItem(ConfigType.Service, AppSettingsSectionName, "UseLoopback").ToString());
			}
		}

		#endregion

		#region Service status database

		public virtual void SetServiceStatusDatabasePath(string databasePath) { }

		public string ServiceStatusDatabasePath {
			get {
				if (IsHub) {
					return Path.Combine(InstallPath, DefaultServiceStatusDbFileName);
				}
				return GetConfigItem(ConfigType.Service, AppSettingsSectionName, ServiceStatusDatabasePathKey).ToString();
			}
		}

		public List<string> ServiceStatusDbSetupSql {
			get {
				return GetResourceList(serviceStatusDbSetupPrefix);
			}
		}

		public ConnectionStringSettings ConnectionStringSettings {
			get {
				ConnectionStringSettings tempSettings = (ConnectionStringSettings)GetConfigItem(ConfigType.Service, ConnectionStringsSectionName, "statusDb");
				tempSettings.ConnectionString = string.Format(tempSettings.ConnectionString, ServiceStatusDatabasePath);
				return tempSettings;
			}
		}

		#endregion

		#region Logging configuration

		public FileLoggerConfiguration LoggerConfiguration {
			get {
				FileLoggerConfiguration retConfig = new FileLoggerConfiguration();

				ConfigType configType = IsAdmin ? ConfigType.Admin : ConfigType.Service;

				retConfig.LogFileName = string.IsNullOrEmpty(GetConfigItem(configType, AppSettingsSectionName, LogFileKey).ToString()) ? FileLoggerConfiguration.DefaultLogFileName : GetConfigItem(configType, AppSettingsSectionName, LogFileKey).ToString();
				retConfig.EnableDebug = string.IsNullOrEmpty(GetConfigItem(configType, AppSettingsSectionName, EnableDebugLoggingKey).ToString()) ? false : bool.Parse(GetConfigItem(configType, AppSettingsSectionName, EnableDebugLoggingKey).ToString());
				retConfig.EnableInfo = string.IsNullOrEmpty(GetConfigItem(configType, AppSettingsSectionName, EnableInfoLoggingKey).ToString()) ? false : bool.Parse(GetConfigItem(configType, AppSettingsSectionName, EnableInfoLoggingKey).ToString());
				retConfig.EnableWarn = string.IsNullOrEmpty(GetConfigItem(configType, AppSettingsSectionName, EnableWarnLoggingKey).ToString()) ? false : bool.Parse(GetConfigItem(configType, AppSettingsSectionName, EnableWarnLoggingKey).ToString());

				retConfig.ApplicationId = string.IsNullOrEmpty(GetConfigItem(ConfigType.Service, AppSettingsSectionName, ApplicationIdKey).ToString()) ? -1 : int.Parse(GetConfigItem(ConfigType.Service, AppSettingsSectionName, ApplicationIdKey).ToString());
				retConfig.LocationId = string.IsNullOrEmpty(GetConfigItem(ConfigType.Service, AppSettingsSectionName, LocationIdKey).ToString()) ? -1 : int.Parse(GetConfigItem(ConfigType.Service, AppSettingsSectionName, LocationIdKey).ToString());
				retConfig.ReportedBy = string.Empty;
				retConfig.SystemId = string.IsNullOrEmpty(GetConfigItem(ConfigType.Service, AppSettingsSectionName, SystemIdKey).ToString()) ? -1 : int.Parse(GetConfigItem(ConfigType.Service, AppSettingsSectionName, SystemIdKey).ToString());

				return retConfig;
			}
		}

		#endregion

		#region Protected resource methods

		protected string ResourceName {
			get {
				return Assembly.GetEntryAssembly().GetName().Name + ".Properties.Resources";
			}
		}

		protected ResourceManager GetResourceManager() {
			// Everything is stored in the main EXE, so let's go there...
			return new ResourceManager(ResourceName, Assembly.GetEntryAssembly());
		}

		protected List<string> GetResourceList(string resourcePrefix) {
			List<string> returnCommands = new List<string>();

			ResourceManager rm = GetResourceManager();

			int count = 1;

			while (!string.IsNullOrEmpty(rm.GetString(resourcePrefix + count.ToString()))) {
				returnCommands.Add(rm.GetString(resourcePrefix + count.ToString()));
				count++;
			}

			return returnCommands;
		}

		protected List<string[]> GetResourceList(string resourcePrefix, char delimiter) {
			List<string[]> returnCommands = new List<string[]>();

			ResourceManager rm = GetResourceManager();

			int count = 1;

			while (!string.IsNullOrEmpty(rm.GetString(resourcePrefix + count.ToString()))) {
				returnCommands.Add(rm.GetString(resourcePrefix + count.ToString()).Split(delimiter));
				count++;
			}
			return returnCommands;
		}

		protected Dictionary<string, string> GetResourceList(string resourcePrefix, List<string> names) {
			Dictionary<string, string> returnCommands = new Dictionary<string, string>();

			ResourceManager rm = GetResourceManager();

			foreach (var name in names) {
				if (!string.IsNullOrEmpty(rm.GetString(resourcePrefix + name))) {
					returnCommands.Add(name, rm.GetString(resourcePrefix + name));
				}
			}

			return returnCommands;
		}

		protected string GetResourceString(string resourceId) {
			ResourceManager rm = GetResourceManager();
			if (!string.IsNullOrEmpty(rm.GetString(resourceId))) {
				return rm.GetString(resourceId);
			}
			return string.Empty;
		}

		#endregion

		#region Protected INI methods

		// Writes the key + value to the specified section
		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section, string key, string defaultValue, string filePath);

		// Reads the value from the specified section + key
		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder result, int size, string filePath);

		//Gathers a list of EntryKey for the known SectionHeader
		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, int key, string defaultValue, [MarshalAs(UnmanagedType.LPArray)] byte[] result, int size, string filePath);

		// Gathers a list of SectionHeaders.
		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(int section, string key, string defaultValue, [MarshalAs(UnmanagedType.LPArray)] byte[] result, int size, string filePath);

		private string IniFilePath {
			get { return Path.Combine(AssemblyDirectory, ServiceName + ".ini"); }
		}

		protected string XmlFilePath {
			get { return Path.Combine(AssemblyDirectory, ServiceName + ".xml"); }
		}

		private static string AssemblyDirectory {
			get {
				string codeBase = Assembly.GetExecutingAssembly().CodeBase;
				UriBuilder uri = new UriBuilder(codeBase);
				string path = Uri.UnescapeDataString(uri.Path);
				return Path.GetDirectoryName(path);
			}
		}

		/// <summary>
		/// Write value to the INI file.
		/// </summary>
		/// <param name="section">The section.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		protected void WriteIniValue(string section, string key, string value) {
			WritePrivateProfileString(section, key, value, IniFilePath);
		}

		protected string ReadIniValue(string section, string key, string defaultValue) {
			//    Sets the maxsize buffer to 250, if the more
			//    is required then doubles the size each time.
			for (int maxSize = 250; true; maxSize *= 2) {
				//    Obtains the EntryValue information and uses the StringBuilder
				//    Function to and stores them in the maxsize buffers (result).
				//    Note that the SectionHeader and EntryKey values has been passed.
				StringBuilder result = new StringBuilder(maxSize);
				int size = GetPrivateProfileString(section, key, defaultValue, result, maxSize, IniFilePath);
				if (size < maxSize - 1) {
					// Returns the value gathered from the EntryKey
					return result.ToString();
				}
			}
		}

		protected void WriteIniValue(string Section, string Key, int Value) {
			WriteIniValue(Section, Key, Value.ToString());
		}

		protected int ReadIniValue(string section, string key, int? defaultValue) {
			string s = ReadIniValue(section, key, (defaultValue != null ? defaultValue.ToString() : null));
			int i = 0;
			if (int.TryParse(s, out i))
				return i;
			else
				throw new IniFileException(string.Format("Tried to retrieve invalid value type from ini. Tried to convert \"{0}\" to int.", s));
		}

		/// <summary>
		/// Gets the .ini section names.
		/// </summary>
		/// <returns>List<string></returns>
		public List<string> GetIniSectionNames() {
			//    Sets the maxsize buffer to 500, if the more
			//    is required then doubles the size each time.
			for (int maxsize = 500; true; maxsize *= 2) {
				//    Obtains the information in bytes and stores
				//    them in the maxsize buffer (Bytes array)
				byte[] bytes = new byte[maxsize];
				int size = GetPrivateProfileString(0, "", "", bytes, maxsize, IniFilePath);

				// Check the information obtained is not bigger
				// than the allocated maxsize buffer - 2 bytes.
				// if it is, then skip over the next section
				// so that the maxsize buffer can be doubled.
				if (size < maxsize - 2) {
					// Converts the bytes value into an ASCII char. This is one long string.
					string Selected = Encoding.ASCII.GetString(bytes, 0,
											   size - (size > 0 ? 1 : 0));
					// Splits the Long string into an array based on the "\0"
					// or null (Newline) value and returns the value(s) in an array
					return Selected.Split(new char[] { '\0' }).ToList<string>();
				}
			}
		}

		/// <summary>
		/// Gets the .ini key names.
		/// </summary>
		/// <param name="section">The section.</param>
		/// <returns>List<string></returns>
		private List<string> GetIniKeyNames(string section) {
			//    Sets the maxsize buffer to 500, if the more
			//    is required then doubles the size each time.
			for (int maxsize = 500; true; maxsize *= 2) {
				//    Obtains the EntryKey information in bytes
				//    and stores them in the maxsize buffer (Bytes array).
				//    Note that the SectionHeader value has been passed.
				byte[] bytes = new byte[maxsize];
				int size = GetPrivateProfileString(section, 0, "", bytes, maxsize, IniFilePath);

				// Check the information obtained is not bigger
				// than the allocated maxsize buffer - 2 bytes.
				// if it is, then skip over the next section
				// so that the maxsize buffer can be doubled.
				if (size < maxsize - 2) {
					// Converts the bytes value into an ASCII char.
					// This is one long string.
					string entries = Encoding.ASCII.GetString(bytes, 0,
											  size - (size > 0 ? 1 : 0));
					// Splits the Long string into an array based on the "\0"
					// or null (Newline) value and returns the value(s) in an array
					return entries.Split(new char[] { '\0' }).ToList<string>();
				}
			}
		}

		public List<ConfigurationKey> GetIniSectionValues(string section) {
			List<ConfigurationKey> iniFileKeys = new List<ConfigurationKey>();

			foreach (string key in GetIniKeyNames(section)) {
				iniFileKeys.Add(new ConfigurationKey {
					Section = section,
					Key = key,
					Value = ReadIniValue(section, key, string.Empty),
					OriginalValue = ReadIniValue(section, key, string.Empty)
				});
			}

			return iniFileKeys;
		}

		/// <summary>
		/// Gets all .ini values.
		/// </summary>
		/// <returns>List<IniFileKey></returns>
		protected List<ConfigurationKey> GetAllIniValues() {
			List<ConfigurationKey> iniFileKeys = new List<ConfigurationKey>();

			foreach (string section in GetIniSectionNames()) {
				foreach (string key in GetIniKeyNames(section)) {
					iniFileKeys.Add(new ConfigurationKey {
						Section = section,
						Key = key,
						Value = ReadIniValue(section, key, string.Empty),
						OriginalValue = ReadIniValue(section, key, string.Empty)
					});
				}
			}

			iniFileKeys.Sort();

			return iniFileKeys;
		}

		protected string GetOrCreateIniValue(string section, string key, string defaultValue) {
			string returnString = ReadIniValue(section, key, string.Empty);
			if (string.IsNullOrEmpty(returnString)) {
				WriteIniValue(section, key, defaultValue);
				returnString = ReadIniValue(section, key, string.Empty);
			}
			return returnString;
		}

		#endregion

		#region Standard settings

		public bool IsAdmin { get; set; }

		public string ExtensionPath {
			get {
				return GetResourceString(extensionPathResourceName);
			}
		}

		#region Timer Settings

		public bool UseTimer {
			get {
				bool useTimer = false; // We aren't going to do anything by default.
				try {
					if (bool.TryParse(GetConfigItem(ConfigType.Service, AppSettingsSectionName, "UseTimer").ToString().ToLower(), out useTimer))
						return useTimer;
					else
						return useTimer;
				}
				catch {
					return useTimer;
				}
			}
		}

		public bool TimerAutoStart {
			get {
				bool timerAutoStart = false; // We aren't going to do anything by default.
				try {
					if (bool.TryParse(GetConfigItem(ConfigType.Service, AppSettingsSectionName, "TimerAutoStart").ToString().ToLower(), out timerAutoStart))
						return timerAutoStart;
					else
						return timerAutoStart;
				}
				catch {
					return timerAutoStart;
				}
			}
		}

		public int TimerInterval {
			get {
				int interval;
				try {
					if (int.TryParse(GetConfigItem(ConfigType.Service, AppSettingsSectionName, "TimerInterval").ToString(), out interval))
						return interval;
					else
						return defaultInterval;
				}
				catch {
					return defaultInterval;
				}
			}
		}

		public Timers.TimerIntervalSpanEnum TimerIntervalSpan {
			get {
				switch (GetConfigItem(ConfigType.Service, AppSettingsSectionName, "TimerIntervalSpan").ToString().ToUpper()) {
					case "D":
						return Timers.TimerIntervalSpanEnum.Days;
					case "H":
						return Timers.TimerIntervalSpanEnum.Hours;
					case "M":
						return Timers.TimerIntervalSpanEnum.Minutes;
					case "S":
						return Timers.TimerIntervalSpanEnum.Seconds;
				}
				return Timers.TimerIntervalSpanEnum.Seconds;
			}
		}

		#endregion Timer Settings

		#region FileSystemWatcher Settings

		public bool UseFileWatcher {
			get {
				bool useFileWatcher = false; // We aren't going to do anything by default.
				try {
					if (bool.TryParse(GetConfigItem(ConfigType.Service, AppSettingsSectionName, "UseFileWatcher").ToString().ToLower(), out useFileWatcher))
						return useFileWatcher;
					else
						return useFileWatcher;
				}
				catch {
					return useFileWatcher;
				}
			}
		}

		public bool FileWatcherAutoStart {
			get {
				bool fileWatcherAutoStart = false; // We aren't going to do anything by default.
				try {
					if (bool.TryParse(GetConfigItem(ConfigType.Service, AppSettingsSectionName, "FileWatcherAutoStart").ToString().ToLower(), out fileWatcherAutoStart))
						return fileWatcherAutoStart;
					else
						return fileWatcherAutoStart;
				}
				catch {
					return fileWatcherAutoStart;
				}
			}
		}

		public List<string> FileWatcherSystemIds {
			get {
				return GetConfigItem(ConfigType.Service, AppSettingsSectionName, "FileWatcherSystemIds").ToString().Split('|').ToList();
			}
		}

		public Dictionary<string, string> FileWatcherPath {
			get {
				Dictionary<string, string> returnList = new Dictionary<string, string>();
				List<string> configList = GetConfigItem(ConfigType.Service, AppSettingsSectionName, "FileWatcherPath").ToString().Split('|').ToList();

				configList.ForEach(
					l => {
						string[] config = l.Split(',');
						returnList.Add(config[0], config[1]);
					});

				return returnList;
			}
		}

		public Dictionary<string, string> FileWatcherFilter {
			get {
				Dictionary<string, string> returnList = new Dictionary<string, string>();
				List<string> configList = GetConfigItem(ConfigType.Service, AppSettingsSectionName, "FileWatcherFilter").ToString().Split('|').ToList();

				configList.ForEach(
					l => {
						string[] config = l.Split(',');
						returnList.Add(config[0], config[1]);
					});

				return returnList;
			}
		}

		//public Dictionary<string, NotifyFilters> FileWatcherNotifyFilter
		//{
		//    get
		//    {
		//        Dictionary<string, string> returnList = new Dictionary<string, string>();
		//        List<string> configList = ConfigurationManager.AppSettings["FileWatcherNotifyFilter"].Split('|').ToList();

		//        configList.ForEach(
		//            l =>
		//            {
		//                string[] config = l.Split(',');
		//                returnList.Add(config[0], config[1]);
		//            });

		//        return returnList;
		//    }
		//}

		public NotifyFilters FileWatcherNotifyFilter {
			get {
				return ParseFileWatcherNotifyFilter(GetConfigItem(ConfigType.Service, AppSettingsSectionName, "FileWatcherNotifyFilter").ToString());
			}
		}

		private NotifyFilters ParseFileWatcherNotifyFilter(string fileWatcherNotifyFilter) {
			FileSystemWatcher fileWatcher = new FileSystemWatcher();
			fileWatcher.NotifyFilter = (System.IO.NotifyFilters)FileWatcher.NotifyFilters.None;

			string[] notifyFilters = fileWatcherNotifyFilter.Split(notifyFiltersDelimiter);

			foreach (var notifyFilter in notifyFilters) {
				Logging.Log(LogLevelEnum.Debug, string.Format("Adding NotifyFilter: {0}", notifyFilter));
				switch (notifyFilter) {
					case "FN":
						SetNotifyFilter(NotifyFilters.FileName, fileWatcher);
						break;
					case "DN":
						SetNotifyFilter(NotifyFilters.DirectoryName, fileWatcher);
						break;
					case "A":
						SetNotifyFilter(NotifyFilters.Attributes, fileWatcher);
						break;
					case "SZ":
						SetNotifyFilter(NotifyFilters.Size, fileWatcher);
						break;
					case "LW":
						SetNotifyFilter(NotifyFilters.LastWrite, fileWatcher);
						break;
					case "LA":
						SetNotifyFilter(NotifyFilters.LastAccess, fileWatcher);
						break;
					case "CT":
						SetNotifyFilter(NotifyFilters.CreationTime, fileWatcher);
						break;
					case "S":
						SetNotifyFilter(NotifyFilters.Security, fileWatcher);
						break;
				}
			}

			NotifyFilters returnFilter = fileWatcher.NotifyFilter;
			Logging.Log(LogLevelEnum.Debug, string.Format("Returning returnFilter: {0}", returnFilter));

			fileWatcher.Dispose(); // MUST call this to release the FileWatcher created during filter parsing! Memory leak WILL be created otherwise!

			return returnFilter;
		}

		private void SetNotifyFilter(NotifyFilters filter, FileSystemWatcher fileWatcher) {
			if ((FileWatcher.NotifyFilters)fileWatcher.NotifyFilter == FileWatcher.NotifyFilters.None)
				fileWatcher.NotifyFilter = filter;
			else
				fileWatcher.NotifyFilter |= filter;
		}

		#endregion FileSystemWatcher Settings

		#region Schedule

		public bool UseSchedule {
			get {
				bool useSchedule = false; // We aren't going to do anything by default.
				try {
					if (bool.TryParse(GetConfigItem(ConfigType.Service, AppSettingsSectionName, "UseSchedule").ToString().ToLower(), out useSchedule))
						return useSchedule;
					else
						return useSchedule;
				}
				catch {
					return useSchedule;
				}
			}
		}

		//public List<DateTime> ScheduleTimesList
		//{
		//    get
		//    {
		//        List<DateTime> returnList = new List<DateTime>();
		//        string[] schedules = ConfigurationManager.AppSettings["Schedules"].Split(';');
		//        foreach (string schedule in schedules)
		//        {
		//            int hour = int.Parse(schedule.Substring(0, 2));
		//            int minute = int.Parse(schedule.Substring(2, 2));
		//            if ((schedule.Length == 4) &&
		//                (hour < 24) &&
		//                (minute < 60))
		//            {
		//                Logging.Log(LogLevelEnum.Debug, string.Format("Schedule time: Hour = {0}, Minute = {1}", hour, minute));
		//                returnList.Add(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minute, 0));
		//            }
		//        }
		//        return returnList;
		//    }
		//}

		public List<SU_Tmr.ScheduleTime> ScheduleTimes {
			get {
				List<SU_Tmr.ScheduleTime> returnList = new List<SU_Tmr.ScheduleTime>();

				string[] schedules = GetConfigItem(ConfigType.Service, AppSettingsSectionName, "Schedules").ToString().Split(';');
				foreach (string schedule in schedules) {
					string[] sched = schedule.Split('|');

					int hour;
					int minute;

					if (sched[1].Contains(':')) {
						string[] time = sched[1].Split(':');
						hour = int.Parse(time[0]);
						minute = int.Parse(time[1]);
					}
					else {
						hour = int.Parse(sched[1].Substring(0, 2));
						minute = int.Parse(sched[1].Substring(2, 2));
					}

					if (sched[0].Trim() == "E") {
						returnList.Add(new SU_Tmr.ScheduleTime { ScheduleType = SU_Tmr.ScheduleType.Weekday, Weekday = SU_Tmr.DayOfWeek.Sunday, Hour = hour, Minute = minute });
						returnList.Add(new SU_Tmr.ScheduleTime { ScheduleType = SU_Tmr.ScheduleType.Weekday, Weekday = SU_Tmr.DayOfWeek.Monday, Hour = hour, Minute = minute });
						returnList.Add(new SU_Tmr.ScheduleTime { ScheduleType = SU_Tmr.ScheduleType.Weekday, Weekday = SU_Tmr.DayOfWeek.Tuesday, Hour = hour, Minute = minute });
						returnList.Add(new SU_Tmr.ScheduleTime { ScheduleType = SU_Tmr.ScheduleType.Weekday, Weekday = SU_Tmr.DayOfWeek.Wednesday, Hour = hour, Minute = minute });
						returnList.Add(new SU_Tmr.ScheduleTime { ScheduleType = SU_Tmr.ScheduleType.Weekday, Weekday = SU_Tmr.DayOfWeek.Thursday, Hour = hour, Minute = minute });
						returnList.Add(new SU_Tmr.ScheduleTime { ScheduleType = SU_Tmr.ScheduleType.Weekday, Weekday = SU_Tmr.DayOfWeek.Friday, Hour = hour, Minute = minute });
						returnList.Add(new SU_Tmr.ScheduleTime { ScheduleType = SU_Tmr.ScheduleType.Weekday, Weekday = SU_Tmr.DayOfWeek.Saturday, Hour = hour, Minute = minute });
					}
					else {
						string[] days = sched[0].Split(',');
						foreach (string day in days) {
							if (day.Contains('-')) // Date range
							{
								string[] rangeDays = day.Split('-');
								int startDay = (int)SU_Tmr.ScheduleTime.ParseToDayOfWeek(rangeDays[0]);
								int endDay = (int)SU_Tmr.ScheduleTime.ParseToDayOfWeek(rangeDays[1]);
								for (int i = startDay; i <= endDay; i++) {
									returnList.Add(new SU_Tmr.ScheduleTime { ScheduleType = SU_Tmr.ScheduleType.Weekday, Weekday = (SU_Tmr.DayOfWeek)i, Hour = hour, Minute = minute });
								}
							}
							else if (Utilities.IsNumeric(day)) // Specific date
							{
								returnList.Add(new SU_Tmr.ScheduleTime { ScheduleType = SU_Tmr.ScheduleType.MonthDay, MonthDay = int.Parse(day), Weekday = SU_Tmr.DayOfWeek.None, Hour = hour, Minute = minute });
							}
							else // Day of the week.
							{
								returnList.Add(new SU_Tmr.ScheduleTime { ScheduleType = SU_Tmr.ScheduleType.Weekday, Weekday = SU_Tmr.ScheduleTime.ParseToDayOfWeek(day), Hour = hour, Minute = minute });
							}
						}
					}
				}

				return returnList;
			}
		}

		#endregion Schedule

		#endregion Standard settings

		#region Service Config

		#endregion

		#region Admin Config

		#endregion

		#region Config tools

		private enum ConfigType {
			Admin,
			Service
		}

		private object GetConfigItem(ConfigType configType, string section, string key) {
			try {
				Configuration config = null;
				switch (configType) {
					case ConfigType.Admin:
						config = ConfigurationManager.OpenExeConfiguration(Path.GetFileNameWithoutExtension(ServiceFile) + "Admin.exe");
						break;
					case ConfigType.Service:
						config = ConfigurationManager.OpenExeConfiguration(ServiceFile);
						break;
				}

				if (section == ConnectionStringsSectionName) {
					ConnectionStringsSection configSection = config.GetSection(section) as ConnectionStringsSection;
					return configSection.ConnectionStrings[key] == null ? null : configSection.ConnectionStrings[key];
				}
				else {
					AppSettingsSection configSection = config.GetSection(section) as AppSettingsSection;
					return string.IsNullOrEmpty(configSection.Settings[key].Value) ? string.Empty : configSection.Settings[key].Value;
				}
			}
			catch {
				return string.Empty;
			}
		}

		#endregion
	}

	public class IniFileException : Exception {

		public IniFileException(string Message) : base(Message) { }
	}

	public class ConfigurationKey : IComparable {

		public string Section { get; set; }

		public string Key { get; set; }

		public string Value { get; set; }

		public string OriginalValue { get; set; }

		public int CompareTo(object obj) {
			if (obj == null) return 1;

			ConfigurationKey otherIniFileKey = obj as ConfigurationKey;
			if (otherIniFileKey != null) {
				return this.Key.CompareTo(otherIniFileKey.Key);
			}
			else {
				throw new ArgumentException("Object is not a IniFileKey");
			}
		}
	}
}