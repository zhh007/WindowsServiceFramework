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
using SelfInstallingWindowsService.Utility;
using Service.Core.Log;
using Service.Core.ServiceExtensibility;
using SC_StatusDb = Service.Core.StatusDatabase;
using SC_Svc = Service.Core.WindowsService.Service;
using SC_Wcf = Service.Core.WindowsService.WCF;
using SU_Timer = Service.Core.Utility.BaseClasses;

namespace SelfInstallingWindowsService.Service {
	partial class WindowsService : SC_Svc.WindowsService {

		protected override void OnCustomCommand(int command) {
			SC_Wcf.ServiceStatusEnum serviceStatus = SC_Wcf.ServiceStatusEnum.Nothing;
			try {
				base.OnCustomCommand(command);
				switch (command) {
					case ServiceCommands.ServiceControl:
						serviceStatus = SC_Wcf.ServiceStatusEnum.PerformingAction;
						SU_Timer.Timers.StartStopWatch();

						SC_Svc.ServiceControl.SetServiceStatus(Settings.Instance.ServiceId, SC_StatusDb.Service.ServiceStatusEnum.PerformingAction);

						ActionResult result = PerformAction(new ActionParameters {
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
			catch (Exception ex) {
				Logging.Log(LogLevelEnum.Fatal, string.Format("Command failed: {0}: {1}", command, FileLogger.GetInnerException(ex).Message));
				Logging.HandleException(ex);
			}

			SC_Svc.ServiceControl.SetServiceStatus(Settings.Instance.ServiceId, (SC_StatusDb.Service.ServiceStatusEnum)serviceStatus);
		}
	}
}