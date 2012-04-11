using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceProcess;
using SelfInstallingWindowsService.Service;
using SelfInstallingWindowsService.Utility;
using Service.Core.Log;
using Service.Core.StatusDatabase;
using SC_Wcf = Service.Core.WindowsService.WCF;

namespace SelfInstallingWindowsService.WCF
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class WcfService : IWcfContract, IDisposable
	{
		/// <summary>
		/// TODO: Replace this method in the interface file and here with any method that is needed for operational function.
		/// </summary>
		/// <returns></returns>
		public string ContractMethod()
		{
			return "Hello, World";
		}

		public List<SC_Wcf.Service> GetServices()
		{
			if (!Settings.Instance.IsHub)
			{
				throw new NotImplementedException("This service is not running as hub.");
			}

			List<SC_Wcf.Service> returnList = new List<SC_Wcf.Service>();
			DatabaseController.GetServices(Settings.Instance.LoggerConfiguration).ForEach(s => returnList.Add(new SC_Wcf.Service
			{
				ServiceId = s.ServiceId,
				ServiceCommands = s.ServiceCommands.Convert(),
				ServiceName = s.ServiceName,
				ServiceDisplayName = s.ServiceDisplayName,
				ServiceDescription = s.ServiceDescription,
				ServiceStatus = (SC_Wcf.ServiceStatusEnum)s.ServiceStatus,
				InstallPath = s.InstallPath
			}));
			return returnList;
		}

		public void SendServiceCommand(int serviceId, int serviceCommand)
		{
		}

		#region Windows Service control methods

		public bool RestartWindowsService()
		{
			try
			{
				ServiceController sc = new ServiceController(Settings.Instance.ServiceName);
				if (sc.Status.Equals(ServiceControllerStatus.Running))
					sc.ExecuteCommand((int)ServiceCommands.WcfRestart);
				sc.Dispose();
			}
			catch (Exception ex)
			{
				Logging.Log(LogLevelEnum.Fatal, "Wcf Restart failed: " + FileLogger.GetInnerException(ex).Message);
				return false;
			}

			return true;
		}

		public bool StartWindowsService()
		{
			try
			{
				ServiceController sc = new ServiceController(Settings.Instance.ServiceName);
				if (sc.Status.Equals(ServiceControllerStatus.Running))
					sc.ExecuteCommand((int)ServiceCommands.WcfStart);
				sc.Dispose();
			}
			catch (Exception ex)
			{
				Logging.Log(LogLevelEnum.Fatal, "Wcf start failed: " + FileLogger.GetInnerException(ex).Message);
				return false;
			}

			return true;
		}

		public bool StopWindowsService()
		{
			try
			{
				ServiceController sc = new ServiceController(Settings.Instance.ServiceName);
				if (sc.Status.Equals(ServiceControllerStatus.Running))
					sc.ExecuteCommand((int)ServiceCommands.WcfStop);
				sc.Dispose();
			}
			catch (Exception ex)
			{
				Logging.Log(LogLevelEnum.Fatal, "Wcf stop failed: " + FileLogger.GetInnerException(ex).Message);
				return false;
			}

			return true;
		}

		#endregion Windows Service control methods

		public void Dispose()
		{
			// TODO: Insert WCF service dispose code here.
		}
	}
}