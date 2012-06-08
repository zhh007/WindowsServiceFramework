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
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using Service.Core.Log;
using Service.Core.ServiceExtensibility;
using Service.Core.WindowsService.Utility;
using Service.Core.WindowsService.WCF;
using SC_Utility = Service.Core.Utility;

//using SelfInstallingWindowsServiceExtensibility;

namespace Service.Core.WindowsService.Service {
	public partial class WindowsService : ServiceBase {
		private WcfServiceHost<WcfService, IWcfContract> serviceConnection = null;
		private bool wcfStopStart = false;

		[Import(typeof(IExtensibility))]
		public IExtensibility Extension { get; set; }

		[Import("PerformAction")]
		public Func<ActionParameters, ActionResult> PerformAction { get; set; }

		protected static CompositionContainer container = new CompositionContainer();

		protected override void OnStart(string[] args) {
			try {
				Logging.Log(LogLevelEnum.Info, "Starting service");

				InitializeExtension();

				if (Settings.Instance.UseSchedule && container.Catalog != null) {
					Timers.StartTimer(ScheduleTimerCallback);
				}
				else if (Settings.Instance.UseTimer && container.Catalog != null) {
					Timers.StartTimer(TimerCallback);
				}
				else if (Settings.Instance.UseFileWatcher && container.Catalog != null) {
					foreach (KeyValuePair<string, FileSystemWatcher> fileWatcher in SC_Utility.FileWatch.FileWatchers.Instance.Watchers) {
						StartFileWatcher(fileWatcher.Value);
					}
				}

				StartWcfService();

				Logging.Log(LogLevelEnum.Info, "Service started");
			}
			catch (Exception ex) {
				Logging.Log(LogLevelEnum.Fatal, "Start failed: " + FileLogger.GetInnerException(ex).Message);
				Logging.HandleException(ex);
			}
		}

		protected override void OnStop() {
			try {
				Logging.Log(LogLevelEnum.Info, "Stopping service");

				StopWcfService();

				if ((Settings.Instance.UseSchedule && container.Catalog != null) ||
					(Settings.Instance.UseTimer && container.Catalog != null)) {
					Timers.StopTimer();
				}
				else if (Settings.Instance.UseFileWatcher && container.Catalog != null) {
					foreach (KeyValuePair<string, FileSystemWatcher> fileWatcher in SC_Utility.FileWatch.FileWatchers.Instance.Watchers) {
						StopFileWatcher(fileWatcher.Value);
					}
				}

				ReleaseExtension();

				Logging.Log(LogLevelEnum.Info, "Service stopped");
			}
			catch (Exception ex) {
				Logging.Log(LogLevelEnum.Fatal, "Stop failed: " + FileLogger.GetInnerException(ex).Message);
				Logging.HandleException(ex);
			}
		}

		protected override void OnCustomCommand(int command) {
			ServiceStatusEnum serviceStatus = ServiceStatusEnum.Nothing;
			try {
				base.OnCustomCommand(command);
				switch (command) {
					case ServiceCommands.ServiceControl:
						serviceStatus = ServiceStatusEnum.PerformingAction;
						Timers.StartStopWatch();

						ServiceControl.SetServiceStatus(Settings.Instance.ServiceId, StatusDatabase.Service.ServiceStatusEnum.PerformingAction);

						ActionResult result = PerformAction(new ActionParameters {
							ActionType = ActionParameters.ActionTypes.Manual
						});

						Logging.Log(LogLevelEnum.Debug, string.Format("ActionResult = \n\tSuccess = {0}\n\tMessage = {1}", result.Success, result.Message));

						ServiceControl.SetServiceStatus(Settings.Instance.ServiceId, StatusDatabase.Service.ServiceStatusEnum.Running, result.Success);

						TimeSpan executeTime = Timers.StopStopwatch();
						Logging.Log(LogLevelEnum.Debug, string.Format("Time to execute: {0}", Timers.FormatTimeSpan(executeTime)));
						serviceStatus = ServiceStatusEnum.Running;
						break;

					case ServiceCommands.WcfRestart:
						serviceStatus = ServiceStatusEnum.Running;
						break;

					case ServiceCommands.WcfStop:
						wcfStopStart = true;
						OnStop();
						wcfStopStart = false;
						serviceStatus = ServiceStatusEnum.Stopped;
						break;

					case ServiceCommands.WcfStart:
						wcfStopStart = true;
						OnStart(null);
						wcfStopStart = false;
						serviceStatus = ServiceStatusEnum.Running;
						break;

					case ServiceCommands.Uninstall: // TODO: This uninstall option does not work... Why?
						Logging.Log(LogLevelEnum.Info, "Uninstall started");
						OnStop();
						// TODO: Remove from status database....
						ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
						Logging.Log(LogLevelEnum.Info, "Uninstall complete");
						break;
				}
			}
			catch (Exception ex) {
				Logging.Log(LogLevelEnum.Fatal, string.Format("Command failed: {0}: {1}", command, FileLogger.GetInnerException(ex).Message));
				Logging.HandleException(ex);
			}

			ServiceControl.SetServiceStatus(Settings.Instance.ServiceId, (StatusDatabase.Service.ServiceStatusEnum)serviceStatus);
		}

		#region Private service control methods

		#region WCF service control methods

		private void StartWcfService() {
			if ((Settings.Instance.IsHub || Settings.Instance.UseWcf) && !wcfStopStart) {
				Logging.Log(LogLevelEnum.Info, "Initialize WCF service start");
				serviceConnection = new WcfServiceHost<WcfService, IWcfContract>();
				serviceConnection.Start();
				Logging.Log(LogLevelEnum.Info, "Initialize WCF service end");
			}
		}

		private void StopWcfService() {
			if ((Settings.Instance.IsHub || Settings.Instance.UseWcf) && !wcfStopStart) {
				Logging.Log(LogLevelEnum.Info, "Stopping WCF service");
				serviceConnection.Stop();
				serviceConnection = null;
				Logging.Log(LogLevelEnum.Info, "WCF service stopped");
			}
		}

		#endregion WCF service control methods

		#region Timer

		private void TimerCallback(object stateInfo) {
			Logging.Log(LogLevelEnum.Info, "TimerCallback started");
			Timers.StartStopWatch();
			Settings.Instance.IsRunning = true;

			ServiceControl.SetServiceStatus(Settings.Instance.ServiceId, StatusDatabase.Service.ServiceStatusEnum.PerformingAction);

			// TODO: (Timer) Insert execute process code here.

			ActionResult result = PerformAction(new ActionParameters {
				ActionType = ActionParameters.ActionTypes.Timer
			});

			Logging.Log(LogLevelEnum.Debug, string.Format("ActionResult = \n\tSuccess = {0}\n\tMessage = {1}", result.Success, result.Message));

			ServiceControl.SetServiceStatus(Settings.Instance.ServiceId, StatusDatabase.Service.ServiceStatusEnum.Running, result.Success);

			Settings.Instance.IsRunning = false;

			TimeSpan executeTime = Timers.StopStopwatch();
			Logging.Log(LogLevelEnum.Debug, string.Format("Time to execute: {0}", Timers.FormatTimeSpan(executeTime)));

			AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
			autoEvent.Set();

			Logging.Log(LogLevelEnum.Info, "TimerCallback ended");
		}

		#endregion Timer

		#region FileSystemWatcher

		private void InitializeFileWatcher(bool autoStartFilewatcher) {
			Logging.Log(LogLevelEnum.Info, "Start InitializeFileWatcher");
			Logging.Log(LogLevelEnum.Debug, string.Format("autoStartFilewatcher: {0}", autoStartFilewatcher));

			foreach (KeyValuePair<string, FileSystemWatcher> fileWatcher in SC_Utility.FileWatch.FileWatchers.Instance.Watchers) {
				fileWatcher.Value.Changed += new FileSystemEventHandler(fileSystemWatcher_OnChanged);
				fileWatcher.Value.Created += new FileSystemEventHandler(fileSystemWatcher_OnChanged);
				fileWatcher.Value.Deleted += new FileSystemEventHandler(fileSystemWatcher_OnChanged);
				fileWatcher.Value.Renamed += new RenamedEventHandler(fileSystemWatcher_OnRenamed);
				fileWatcher.Value.Error += new ErrorEventHandler(fileSystemWatcher_OnError);

				if (autoStartFilewatcher) {
					StartFileWatcher(fileWatcher.Value);
				}
			}
		}

		private void StartFileWatcher(FileSystemWatcher fileSystemWatcher) {
			Logging.Log(LogLevelEnum.Info, "Start FileSystemWatcher start");
			fileSystemWatcher.EnableRaisingEvents = true;
			Logging.Log(LogLevelEnum.Info, "End FileSystemWatcher start");
		}

		private void StopFileWatcher(FileSystemWatcher fileSystemWatcher) {
			Logging.Log(LogLevelEnum.Info, "Start FileSystemWatcher start");
			fileSystemWatcher.EnableRaisingEvents = false;
			Logging.Log(LogLevelEnum.Info, "End FileSystemWatcher start");
		}

		private void fileSystemWatcher_OnChanged(object source, FileSystemEventArgs e) {
			fileSystemWatcherChanged(new SC_Utility.FileWatch.FileWatcherEventAttributes {
				ChangeType = e.ChangeType,
				FullPath = e.FullPath,
				Name = e.Name
			});
		}

		private void fileSystemWatcher_OnRenamed(object source, RenamedEventArgs e) {
			fileSystemWatcherChanged(new SC_Utility.FileWatch.FileWatcherEventAttributes {
				ChangeType = e.ChangeType,
				FullPath = e.FullPath,
				Name = e.Name,
				OldFullPath = e.OldFullPath,
				OldName = e.OldName
			});
		}

		private void fileSystemWatcher_OnError(object source, ErrorEventArgs e) {
			Logging.Log(LogLevelEnum.Error, string.Format("Error: {0}", Log.FileLogger.GetInnerException(e.GetException())));
			Logging.HandleException(e.GetException());
		}

		private void fileSystemWatcherChanged(SC_Utility.FileWatch.FileWatcherEventAttributes eventAttributes) {
			Logging.Log(LogLevelEnum.Info, "Start FileSystemWatcher changed event");
			Logging.Log(LogLevelEnum.Debug, string.Format("ChangeType: {0}", eventAttributes.ChangeType));
			Logging.Log(LogLevelEnum.Debug, string.Format("FullPath: {0}", eventAttributes.FullPath));
			Logging.Log(LogLevelEnum.Debug, string.Format("Name: {0}", eventAttributes.Name));
			if (eventAttributes.ChangeType.Equals(WatcherChangeTypes.Renamed)) {
				Logging.Log(LogLevelEnum.Debug, string.Format("OldFullPath: {0}", eventAttributes.OldFullPath));
				Logging.Log(LogLevelEnum.Debug, string.Format("OldName: {0}", eventAttributes.OldName));
			}

			Timers.StartStopWatch();

			Settings.Instance.IsRunning = true;

			// TODO: (FileSystemWatcher) Insert execute process code here.

			Settings.Instance.IsRunning = false;

			TimeSpan executeTime = Timers.StopStopwatch();
			Logging.Log(LogLevelEnum.Info, string.Format("Time to execute: {0}", Timers.FormatTimeSpan(executeTime)));

			Logging.Log(LogLevelEnum.Info, "End FileSystemWatcher changed event");
		}

		#endregion FileSystemWatcher

		#region Schedule

		private void ScheduleTimerCallback(object stateInfo) {
			if (IsScheduledTime() && !Settings.Instance.IsRunning) {
				Logging.Log(LogLevelEnum.Info, "ScheduleTimerCallback started");
				Timers.StartStopWatch();

				Settings.Instance.IsRunning = true;

				ServiceControl.SetServiceStatus(Settings.Instance.ServiceId, StatusDatabase.Service.ServiceStatusEnum.PerformingAction);

				// TODO: (Timer) Insert execute process code here.

				ActionResult result = PerformAction(new ActionParameters {
					ActionType = ActionParameters.ActionTypes.Schedule
				});

				Logging.Log(LogLevelEnum.Debug, string.Format("ActionResult = \n\tSuccess = {0}\n\tMessage = {1}", result.Success, result.Message));

				ServiceControl.SetServiceStatus(Settings.Instance.ServiceId, StatusDatabase.Service.ServiceStatusEnum.Running, result.Success);

				Settings.Instance.IsRunning = false;

				TimeSpan executeTime = Timers.StopStopwatch();
				Logging.Log(LogLevelEnum.Debug, string.Format("Time to execute: {0}", Timers.FormatTimeSpan(executeTime)));

				AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
				autoEvent.Set();

				Logging.Log(LogLevelEnum.Info, "ScheduleTimerCallback ended");
			}
		}

		private bool IsScheduledTime() {
			DateTime now = DateTime.Now;
			Logging.Log(LogLevelEnum.Debug, string.Format("Current time: Hour = {0}, Minute = {1}", now.Hour, now.Minute));

			foreach (SC_Utility.Timer.ScheduleTime scheduleTime in Settings.Instance.ScheduleTimes) {
				Logging.Log(LogLevelEnum.Debug, string.Format("ScheduleTime: SheduleType = {0}, MonthDay = {1}, Weekday = {2}, Hour = {3}, Minute = {4}", scheduleTime.ScheduleType, scheduleTime.MonthDay, scheduleTime.Weekday, scheduleTime.Hour, scheduleTime.Minute));

				bool isScheduledDay = false;
				switch (scheduleTime.ScheduleType) {
					case SC_Utility.Timer.ScheduleType.Weekday:
						if ((int)scheduleTime.Weekday == (int)DateTime.Now.DayOfWeek) {
							isScheduledDay = true;
						}
						break;
					case SC_Utility.Timer.ScheduleType.MonthDay:
						if (scheduleTime.MonthDay == DateTime.Now.Day) {
							isScheduledDay = true;
						}
						break;
				}

				if (isScheduledDay && (scheduleTime.Hour == DateTime.Now.Hour && scheduleTime.Minute == DateTime.Now.Minute)) {
					return true;
				}
			}
			return false;
		}

		#endregion Schedule

		#endregion Private service control methods

		#region MEF Extension Methods

		private void InitializeExtension() {
			try {
				Logging.Log(LogLevelEnum.Info, "Start initializing extension");

				Logging.Log(LogLevelEnum.Info, "Creating extension directory");
				string extensionPath = Utilities.CreateDirectory(Settings.Instance.ExtensionPath);
				Logging.Log(LogLevelEnum.Debug, string.Format("Extension path: {0}", extensionPath));
				Logging.Log(LogLevelEnum.Info, "Extension directory created");

				using (var catalog = new AggregateCatalog()) {
					catalog.Catalogs.Add(new DirectoryCatalog(Settings.Instance.ExtensionPath));
					Logging.Log(LogLevelEnum.Debug, string.Format("catalog.Parts.Count(): {0}", catalog.Parts.Count()));
					if (catalog.Parts.Count() > 0) {
						container = new CompositionContainer(catalog);
						container.ComposeParts(this);
						Settings.Instance.ExtensionLoaded = true;
					}
				}
				Logging.Log(LogLevelEnum.Info, "End initializing extension");
			}
			catch (Exception ex) {
				Logging.Log(LogLevelEnum.Fatal, string.Format("Extension initialization failed: {0}", FileLogger.GetInnerException(ex).Message));
				throw;
			}
		}

		private void ReleaseExtension() {
			try {
				if (container.Catalog != null) {
					Logging.Log(LogLevelEnum.Info, "Start releasing extension");
					Logging.Log(LogLevelEnum.Debug, string.Format("container.Catalog == null: {0}", container.Catalog == null));
					container.Dispose();
					Settings.Instance.ExtensionLoaded = false;
					Logging.Log(LogLevelEnum.Info, "End releasing extension");
				}
			}
			catch (Exception ex) {
				Logging.Log(LogLevelEnum.Fatal, string.Format("Extension release failed: {0}", FileLogger.GetInnerException(ex).Message));
				throw;
			}
		}

		#endregion MEF Extension Methods
	}
}