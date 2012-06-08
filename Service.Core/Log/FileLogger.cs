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
using System.IO;
using log4net;
using log4net.Config;
using Service.Core.Log.Configuration;

namespace Service.Core.Log {
	public sealed class FileLogger : ILogger {
		private static FileLogger instance = null;
		private readonly ILog logger;
		private FileLoggerConfiguration Settings = null;

		private FileLogger(FileLoggerConfiguration settings) {
			Settings = settings;
			log4net.GlobalContext.Properties["LogName"] = Settings.LogFileName;
			logger = LogManager.GetLogger(typeof(FileLogger));
			XmlConfigurator.Configure();
		}

		public static FileLogger Instance(FileLoggerConfiguration settings) {
			if (string.IsNullOrEmpty(settings.LogFileName)) {
				settings.LogFileName = FileLoggerConfiguration.DefaultLogFileName;
			}

			instance = new FileLogger(settings);

			return instance;
		}

		public void WriteLog(LogLevelEnum logLevel, string log) {
			Write(logLevel, log + LogRecord.LogEntryEndOfLineUniqueMarker, null);
		}

		public void WriteLog(LogLevelEnum logLevel, Exception exception) {
			//Write(logLevel, exception);
		}

		public static Exception GetInnerException(Exception ex) {
			Exception returnEx = ex;
			while (returnEx.InnerException != null) {
				returnEx = returnEx.InnerException;
			}
			return returnEx;
		}

		private void Write(LogLevelEnum logLevel, string log, Exception exception) {
			switch (logLevel) {
				case LogLevelEnum.Debug:
					if (Settings.EnableDebug)
						logger.Debug(log);
					break;
				case LogLevelEnum.Error:
					logger.Error(log);
					// Handle exception here
					break;
				case LogLevelEnum.Fatal:
					logger.Fatal(log);
					// Handle exception here
					break;
				case LogLevelEnum.Info:
					if (Settings.EnableInfo)
						logger.Info(log);
					break;
				case LogLevelEnum.Warn:
					if (Settings.EnableWarn)
						logger.Warn(log);
					break;
			}
		}

		public List<LogRecord> Parse() {
			if (!File.Exists(Settings.LogFileName)) { return null; }

			List<LogRecord> logRecords = new List<LogRecord>();
			using (FileStream file = new FileStream(Settings.LogFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
				using (StreamReader stream = new StreamReader(file)) {
					string delimiterReplacement = "[[CR]]";
					string[] separator = new string[] { delimiterReplacement };
					string read = stream.ReadToEnd().Replace(LogRecord.LogEntryEndOfLine, delimiterReplacement);
					string[] lines = read.Replace("\n", LogRecord.LineFeedPlaceHolder).Split(separator, StringSplitOptions.RemoveEmptyEntries);

					foreach (string line in lines) {
						logRecords.Add(new LogRecord(line));
					}

					lines = null;

					stream.Close();
					file.Close();
				}
			}

			return logRecords;
		}

		public List<LogRecord> Parse(LogLevelEnum logLevel) {
			List<LogRecord> logRecords = Parse();
			List<LogRecord> returnRecords = new List<LogRecord>();
			logRecords.ForEach(
				delegate(LogRecord logRecord) {
					if (logRecord.LogLevel == logLevel) {
						returnRecords.Add(logRecord);
					}
				});

			return returnRecords;
		}
	}
}