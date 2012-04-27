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

using System.Collections.Generic;
using Service.Core.Log;
using SC_BaseClasses = Service.Core.Utility.BaseClasses;

namespace SelfInstallingWindowsServiceAdmin.Utility {
	internal class Logging : SC_BaseClasses.LoggingBase {

		public static void Log(LogLevelEnum logLevel, string message) {
			Log(logLevel, message, Settings.Instance.LoggerConfiguration);
		}

		public static List<LogRecord> Parse(LogEnum log, LogLevelEnum logLevel) {
			switch (log) {
				case LogEnum.Admin: return Parse(logLevel, Settings.Instance.LoggerConfiguration);
				//case LogEnum.Service: return Parse(logLevel, Settings.Instance.ServiceLoggerConfiguration);
				default: return null;
			}
		}

		public static List<LogRecord> Parse(LogEnum log) {
			switch (log) {
				case LogEnum.Admin: return Parse(Settings.Instance.LoggerConfiguration);
				//case LogEnum.Service: return Parse(Settings.Instance.ServiceLoggerConfiguration);
				default: return null;
			}
		}
	}
}