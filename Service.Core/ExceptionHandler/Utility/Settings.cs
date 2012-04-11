using System.Net.Mail;
using System.Reflection;

namespace Service.Core.ExceptionHandler.Utility
{
	internal class Settings : ConfigurationHandler
	{
		// Constants we will be using
		public readonly string ViewStateKey = "__VIEWSTATE";
		public readonly string RootException = "System.Web.HttpUnhandledException";
		public readonly string RootWsException = "System.Web.Service.Protocols.SoapException";
		public readonly string HttpException = "System.Web.HttpException";
		public readonly string DefaultLogName = "ExceptionLog.txt";

		private const int defaultSystemID = 0;
		private const int defaultLocationID = 0;
		private const int defaultApplicationID = 0;

		public bool LogToEventLog { get { return GetBoolean("LogToEventLog", bool.Parse("false")); } }

		public bool LogToFile { get { return GetBoolean("LogToFile", false); } }

		public bool LogToEmail { get { return GetBoolean("LogToEmail", false); } }

		public bool LogToUi { get { return GetBoolean("LogToUI", true); } }

		public bool LogToSQL { get { return GetBoolean("LogToSQL", false); } }

		public string LogFileName { get { return GetPath("LogFileName"); } }

		public string IgnoreRegExp { get { return GetString("IgnoreRegExp", string.Empty); } }

		public bool IgnoreDebugErrors { get { return GetBoolean("IgnoreDebugErrors", true); } }

		public bool IgnoreHttpErrors { get { return GetBoolean("IgnoreHttpErrors", false); } }

		public string EmailServer { get { return GetString("EmailServer"); } }

		public string EmailFromAddress { get { return GetString("EmailFromAddress"); } }

		public string EmailAddressFromName { get { return GetString("EmailAddressFromName", string.Empty); } }

		public string EmailToList { get { return GetString("EmailToAddressList"); } }

		public MailAddressCollection EmailToAddressList
		{
			get
			{
				string[] to = GetString("EmailToAddressList", string.Empty).Split(';');
				MailAddressCollection toAddresses = new MailAddressCollection();
				for (int i = 0; i < to.Length; i++)
				{
					toAddresses.Add(new MailAddress(to[i]));
				}

				return toAddresses;
			}
		}

		public string AppName { get { return GetString("AppName", string.Empty); } }

		public string ContactInfo { get { return GetString("ContactInfo", string.Empty); } }

		public int SystemID { get { return GetInteger("SystemID", defaultSystemID); } }

		public int LocationID { get { return GetInteger("LocationID", defaultLocationID); } }

		public int ApplicationID { get { return GetInteger("ApplicationID", defaultApplicationID); } }

		public string ReportedBy { get { return GetString("ReportedBy", string.Empty); } }

		public string SQLConnectionString { get { return GetString("JBSAppMonitorConnString", string.Empty); } }

		private Assembly parentAssembly = null;

		public Assembly ParentAssembly
		{
			get
			{
				if (parentAssembly == null)
				{
					if (Assembly.GetEntryAssembly() == null)
					{
						parentAssembly = Assembly.GetCallingAssembly();
					}
					else
					{
						parentAssembly = Assembly.GetEntryAssembly();
					}
				}
				return parentAssembly;
			}
		}
	}
}