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

namespace Service.Core.Log {
	public class LogRecord {

		public DateTime LogTime { get; private set; }

		public LogLevelEnum LogLevel { get; private set; }

		public string LogMessage { get; private set; }

		public LogRecord() { }

		public LogRecord(string log4netEntry) {
			LogRecord logRecord = Parse(log4netEntry);
			this.LogLevel = logRecord.LogLevel;
			this.LogMessage = logRecord.LogMessage;
			this.LogTime = logRecord.LogTime;
		}

		/// <summary>
		/// Parses the specified log4net entry.
		/// </summary>
		/// <param name="log4netEntry">The log4net entry.</param>
		/// <returns></returns>
		public static LogRecord Parse(string log4netEntry) {
			LogLevelEnum logLevel = LogLevelEnum.Info;

			switch (log4netEntry.Substring(log4netEntry.IndexOf(']') + 1, 5).Trim()) {
				case "DEBUG":
					logLevel = LogLevelEnum.Debug;
					break;
				case "ERROR":
					logLevel = LogLevelEnum.Error;
					break;
				case "FATAL":
					logLevel = LogLevelEnum.Fatal;
					break;
				case "INFO":
					logLevel = LogLevelEnum.Info;
					break;
				case "WARN":
					logLevel = LogLevelEnum.Warn;
					break;
			}

			return new LogRecord {
				LogTime = DateTime.Parse(log4netEntry.Substring(0, 20)),
				LogLevel = logLevel,
				LogMessage = log4netEntry.Substring(log4netEntry.IndexOf(']') + 7).Replace(LineFeedPlaceHolder, "\r\n")
			};
		}

		public const string LineFeedPlaceHolder = "\t||\t";
		public const string LogEntryEndOfLineUniqueMarker = "\t";
		public const string LogEntryEndOfLine = LogEntryEndOfLineUniqueMarker + "\r\n";
	}
}