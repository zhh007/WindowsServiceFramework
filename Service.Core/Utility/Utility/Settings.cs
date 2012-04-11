using System;

namespace Service.Core.Utility.Utility
{
	internal class Settings : BaseClasses.SettingsBase
	{
		private static readonly Lazy<Settings> instance = new Lazy<Settings>(() => new Settings());

		private Settings() { }

		public static Settings Instance
		{
			get
			{
				return instance.Value;
			}
		}
	}
}