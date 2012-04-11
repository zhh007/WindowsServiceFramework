using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Configuration;

namespace Service.Core.ExceptionHandler.Utility
{
	[Serializable]
	internal abstract class ConfigurationHandler
	{
		private const string SectionName = "ExceptionHandlerConfig";
		private KeyValueConfigurationCollection Kvcc;

		public bool IsWebApp { get; private set; }

		/// <summary>
		/// Loads this instance.
		/// </summary>
		/// <exception cref="ConfigurationErrorsException">The ExceptionHandler section is present in the .config file, but it does not appear to be a name value collection.</exception>
		private void Load()
		{
			if (null != Kvcc)
				return;

			AppSettingsSection o = null;
			try
			{
				IsWebApp = HttpContext.Current != null;
				string configPath = string.Empty;

				if (IsWebApp)
				{
					configPath = HttpContext.Current.Request.CurrentExecutionFilePath;
				}
				else
				{
					configPath = System.AppDomain.CurrentDomain.GetData("APP_CONFIG_FILE").ToString();
				}

				configPath = configPath.Substring(0, configPath.LastIndexOf(IsWebApp ? '/' : '\\'));
				if (configPath.Length == 0)
					configPath = IsWebApp ? "/" : "\\";
				Configuration rootConfig = null;

				if (IsWebApp)
				{
					rootConfig = WebConfigurationManager.OpenWebConfiguration(configPath);
				}
				else
				{
					//ConfigurationManager.OpenExeConfiguration(configPath);
					rootConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
				}
				o = rootConfig.GetSection(SectionName) as AppSettingsSection;
			}
			catch
			{
				// We are in an unhandled exception handler.
			}

			if (null == o)
			{
				// we can work without any configuration at all ( all defaults).
				Kvcc = new KeyValueConfigurationCollection();
				return;
			}

			try
			{
				Kvcc = o.Settings;
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException("The <" + SectionName + "> section is present in the .config file, but it does not appear to be a name value collection.", ex);
			}
		}

		protected int GetInteger(string key, int defaultValue)
		{
			Load();

			if (!HasKey(key) || string.IsNullOrEmpty(Kvcc[key].Value))
			{
				return defaultValue;
			}

			try
			{
				return Int32.Parse(Kvcc[key].Value);
			}
			catch
			{
				return defaultValue;
			}
		}

		protected bool GetBoolean(string key)
		{
			return GetBoolean(key, false);
		}

		protected bool GetBoolean(string key, bool defaultValue)
		{
			Load();

			if (!HasKey(key) || string.IsNullOrEmpty(Kvcc[key].Value))
			{
				return defaultValue;
			}

			switch (Kvcc[key].Value.ToLower())
			{
				case "1":
				case "true":
					return true;
				default:
					return false;
			}
		}

		protected string GetString(string key)
		{
			return GetString(key, string.Empty);
		}

		protected string GetString(string key, string defaultValue)
		{
			Load();

			if (!HasKey(key) || string.IsNullOrEmpty(Kvcc[key].Value))
			{
				return defaultValue;
			}

			return Kvcc[key].Value;
		}

		protected string GetPath(string key)
		{
			Load();

			string strPath = GetString(key);

			// Users might think we're using Server.MapPath, but we're not.
			// Strip this because it's unecessary (we assume website root, if path isn't rooted).
			if (strPath.StartsWith("~/"))
			{
				strPath = strPath.Replace("~/", string.Empty);
			}

			return Path.IsPathRooted(strPath) ? strPath : Path.Combine(AppBase, strPath);
		}

		private string AppBase
		{
			get
			{
				return (string)AppDomain.CurrentDomain.GetData("APPBASE");
			}
		}

		private bool HasKey(string key)
		{
			try
			{
				object o = Kvcc[key].Value;
			}
			catch
			{
				return false;
			}

			return true;
		}
	}
}