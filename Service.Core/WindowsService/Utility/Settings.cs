using System;
using System.Configuration;
using SC_Utility = Service.Core.Utility;

namespace Service.Core.WindowsService.Utility
{
	public class Settings : SC_Utility.BaseClasses.SettingsBase
	{
		private static readonly Lazy<Settings> instance = new Lazy<Settings>(() => new Settings());

		public override int ServiceId
		{
			get
			{
				int serviceId = -1;
				int.TryParse(ConfigurationManager.AppSettings[ServiceIdKey], out serviceId);
				return serviceId;
			}
			set
			{
				// Leave this alone. We don't want the service changing its ID...
				throw new NotImplementedException();
			}
		}

		private Settings() { }

		public static Settings Instance
		{
			get
			{
				return instance.Value;
			}
		}

		#region Application Process Settings

		public string SQLConnectionString
		{
			get
			{
				return ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;
			}
		}

		#endregion Application Process Settings
	}
}