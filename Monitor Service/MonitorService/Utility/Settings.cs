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
using System.Configuration;
using SC_BaseClasses = Service.Core.Utility.BaseClasses;

namespace MonitorService.Utility {
	public class Settings : SC_BaseClasses.SettingsBase {
		private static readonly Lazy<Settings> instance = new Lazy<Settings>(() => new Settings());

		public override int ServiceId {
			get {
				int serviceId = -1;
				int.TryParse(ConfigurationManager.AppSettings[ServiceIdKey], out serviceId);
				return serviceId;
			}
			set {
				// Leave this alone. We don't want the service changing its ID...
				throw new NotImplementedException();
			}
		}

		private Settings() { }

		public static Settings Instance {
			get {
				return instance.Value;
			}
		}

		#region Application Process Settings

		public string SQLConnectionString {
			get {
				return ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;
			}
		}

		#endregion Application Process Settings
	}
}