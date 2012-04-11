using System;
using SelfInstallingWindowsService.Utility;
using Service.Core.Log;
using Service.Core.ServiceExtensibility;
using SC_StatusDb = Service.Core.StatusDatabase;
using SC_Svc = Service.Core.WindowsService.Service;
using SC_Wcf = Service.Core.WindowsService.WCF;
using SU_Timer = Service.Core.Utility.BaseClasses;

namespace SelfInstallingWindowsService.Service
{
	partial class WindowsService : SC_Svc.WindowsService
	{
		protected override void OnCustomCommand(int command)
		{
			SC_Wcf.ServiceStatusEnum serviceStatus = SC_Wcf.ServiceStatusEnum.Nothing;
			try
			{
				base.OnCustomCommand(command);
				switch (command)
				{
					case ServiceCommands.ServiceControl:
						serviceStatus = SC_Wcf.ServiceStatusEnum.PerformingAction;
						SU_Timer.Timers.StartStopWatch();

						SC_Svc.ServiceControl.SetServiceStatus(Settings.Instance.ServiceId, SC_StatusDb.Service.ServiceStatusEnum.PerformingAction);

						ActionResult result = PerformAction(new ActionParameters
															{
																ActionType = ActionParameters.ActionTypes.Manual
															});

						Logging.Log(LogLevelEnum.Debug, string.Format("ActionResult = \n\tSuccess = {0}\n\tMessage = {1}", result.Success, result.Message));

						SC_Svc.ServiceControl.SetServiceStatus(Settings.Instance.ServiceId, SC_StatusDb.Service.ServiceStatusEnum.Running, result.Success);

						TimeSpan executeTime = SU_Timer.Timers.StopStopwatch();
						Logging.Log(LogLevelEnum.Debug, string.Format("Time to execute: {0}", SU_Timer.Timers.FormatTimeSpan(executeTime)));
						serviceStatus = SC_Wcf.ServiceStatusEnum.Running;
						break;

					default:
						base.OnCustomCommand(command);
						break;
				}
			}
			catch (Exception ex)
			{
				Logging.Log(LogLevelEnum.Fatal, string.Format("Command failed: {0}: {1}", command, FileLogger.GetInnerException(ex).Message));
				Logging.HandleException(ex);
			}

			SC_Svc.ServiceControl.SetServiceStatus(Settings.Instance.ServiceId, (SC_StatusDb.Service.ServiceStatusEnum)serviceStatus);
		}
	}
}