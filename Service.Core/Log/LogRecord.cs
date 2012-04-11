using System;

namespace Service.Core.Log
{
	public class LogRecord
	{
		public DateTime LogTime { get; private set; }

		public LogLevelEnum LogLevel { get; private set; }

		public string LogMessage { get; private set; }

		public LogRecord() { }

		public LogRecord(string log4netEntry)
		{
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
		public static LogRecord Parse(string log4netEntry)
		{
			LogLevelEnum logLevel = LogLevelEnum.Info;

			switch (log4netEntry.Substring(log4netEntry.IndexOf(']') + 1, 5).Trim())
			{
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

			return new LogRecord
			{
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