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
using System.Diagnostics;
using System.Threading;
using Service.Core.Utility.Utility;

namespace Service.Core.Utility.BaseClasses {
	public class Timers {
		public enum TimerIntervalSpanEnum {
			Days,
			Hours,
			Minutes,
			Seconds
		}

		private static System.Threading.Timer Timer;
		private static Stopwatch Stopwatch;
		private static AutoResetEvent AutoResetEvent;

		/// <summary>
		/// Gets or sets the timer callback.
		/// </summary>
		/// <value>The timer callback.</value>
		public static TimerCallback TimerCallback { get; set; }

		/// <summary>
		/// Starts the timer.
		/// </summary>
		/// <param name="timerCallback">The timer callback.</param>
		public static void StartTimer(TimerCallback timerCallback) {
			TimerCallback = timerCallback;
			StartTimer(Settings.Instance.UseSchedule ? Settings.DefaultScheduleTimerInterval : Settings.Instance.TimerInterval);
			Settings.Instance.IsRunning = false;
		}

		/// <summary>
		/// Starts the timer.
		/// </summary>
		/// <param name="timerCallback">The timer callback.</param>
		/// <param name="interval">The interval.</param>
		public static void StartTimer(TimerCallback timerCallback, int interval) {
			TimerCallback = timerCallback;
			StartTimer(interval);
		}

		/// <summary>
		/// Starts the timer.
		/// </summary>
		/// <param name="interval">The interval.</param>
		public static void StartTimer(int interval) {
			Logging.Log(Log.LogLevelEnum.Debug, string.Format("Settings.Instance.IsHub = {0}", Settings.Instance.IsHub));
			Logging.Log(Log.LogLevelEnum.Debug, string.Format("Settings.Instance.IsHubWithExtensionLoaded = {0}", Settings.Instance.IsHubWithExtensionLoaded));
			Logging.Log(Log.LogLevelEnum.Debug, string.Format("!Settings.Instance.IsHub || Settings.Instance.IsHubWithExtensionLoaded = {0}", !Settings.Instance.IsHub || Settings.Instance.IsHubWithExtensionLoaded));
			if (!Settings.Instance.IsHub || Settings.Instance.IsHubWithExtensionLoaded) {
				if (Timer == null) {
					Logging.Log(Log.LogLevelEnum.Info, "Starting timer");
					if (TimerCallback == null) {
						Exception ex = new Exception("TimerCallback not defined.");
						Logging.Log(Log.LogLevelEnum.Fatal, ex.Message);
						Logging.HandleException(ex);
						throw ex;
					}
					AutoResetEvent = new AutoResetEvent(false);

					Logging.Log(Log.LogLevelEnum.Debug, string.Format("interval = {0}\n\tIntervalSpan = {1}", interval, Settings.Instance.TimerIntervalSpan));

					switch (Settings.Instance.TimerIntervalSpan) {
						case TimerIntervalSpanEnum.Days:
							interval = (interval * 86400) * 1000;	// (interval * seconds in a day) * milliseconds in a second
							break;
						case TimerIntervalSpanEnum.Hours:
							interval = (interval * 3600) * 1000;	// (interval * seconds in an hour) * milliseconds in a second
							break;
						case TimerIntervalSpanEnum.Minutes:
							interval = interval * 60000;			// interval * milliseconds in a minute
							break;
						case TimerIntervalSpanEnum.Seconds:
							interval = interval * 1000;				// interval * milliseconds in a second
							break;
					}

					Logging.Log(Log.LogLevelEnum.Debug, string.Format("Timer = \n\tTimerCallback = {0}\n\tAutoResetEvent = {1}\n\tinterval = {2}\n\tIntervalSpan = {3}", TimerCallback.Method, AutoResetEvent, interval, Settings.Instance.TimerIntervalSpan));
					Timer = new System.Threading.Timer(TimerCallback, AutoResetEvent, interval, interval);
					Logging.Log(Log.LogLevelEnum.Info, "Timer started");
				}
			}
			else {
				Logging.Log(Log.LogLevelEnum.Info, "Service is hub with no extensions: No timer started.");
			}
		}

		/// <summary>
		/// Stops the timer.
		/// </summary>
		public static void StopTimer() {
			Logging.Log(Log.LogLevelEnum.Info, "Stopping timer");
			Timer.Dispose();
			Logging.Log(Log.LogLevelEnum.Info, "Timer stopped");
		}

		/// <summary>
		/// Starts the stop watch.
		/// </summary>
		public static void StartStopWatch() {
			Logging.Log(Log.LogLevelEnum.Info, "Starting stopwatch");
			Stopwatch = new Stopwatch();
			Stopwatch.Start();
			Logging.Log(Log.LogLevelEnum.Info, "Stopwatch started");
		}

		/// <summary>
		/// Stops the stopwatch.
		/// </summary>
		/// <returns>TimeSpan</returns>
		public static TimeSpan StopStopwatch() {
			Logging.Log(Log.LogLevelEnum.Info, "Stopping stopwatch");
			Stopwatch.Stop();
			Logging.Log(Log.LogLevelEnum.Info, "Stopwatch stopped");
			return Stopwatch.Elapsed;
		}

		/// <summary>
		/// Formats the time span.
		/// </summary>
		/// <param name="timeSpan">The time span.</param>
		/// <returns>string</returns>
		public static string FormatTimeSpan(TimeSpan timeSpan) {
			return string.Format("{0:00}:{1:00}:{2:00}.{3:00}:{4:00}", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
		}
	}
}