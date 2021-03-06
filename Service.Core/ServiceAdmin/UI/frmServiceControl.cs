﻿#region

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
using System.Collections.Generic;
using System.ServiceProcess;
using System.Windows.Forms;
using Service.Core.ServiceAdmin.Utility;
using SC_StatusDb = Service.Core.StatusDatabase;
using SC_Svc = Service.Core.WindowsService.Service;

namespace Service.Core.ServiceAdmin.UI {
	public partial class frmServiceControl : Form {

		public frmServiceControl() {
			InitializeComponent();
			InitializeControls();
		}

		public void InitializeControls() {
			if (Settings.Instance.IsHub) {
				List<SC_StatusDb.Service> services = SC_StatusDb.DatabaseController.GetServices(Settings.Instance.LoggerConfiguration);
				services.Insert(0, new SC_StatusDb.Service {
					ServiceId = -1,
					ServiceDisplayName = "Please select a service..."
				});
				serviceList.DataSource = services;
				serviceList.DisplayMember = "ServiceDisplayName";
				serviceList.ValueMember = "ServiceId";
			}

			InitializeControlState();
		}

		private void serviceList_SelectedIndexChanged(object sender, EventArgs e) {
			int serviceId = ((SC_StatusDb.Service)serviceList.SelectedItem).ServiceId;
			if (!serviceId.Equals(-1)) {
				List<SC_StatusDb.ServiceCommandStruct> serviceCommands = SC_StatusDb.DatabaseController.GetServiceCommands(SC_StatusDb.DatabaseController.GetService(int.Parse(serviceList.SelectedValue.ToString()), Settings.Instance.LoggerConfiguration), Settings.Instance.LoggerConfiguration);

				if (!Utilities.IsNullOrEmpty(serviceCommands)) {
					serviceCommands.Insert(0, new SC_StatusDb.ServiceCommandStruct {
						CommandId = -1,
						CommandDescription = "Please select a command..."
					});
					serviceCommandList.DataSource = serviceCommands;
					serviceCommandList.DisplayMember = "CommandDescription";
					serviceCommandList.ValueMember = "CommandId";
				}

				serviceCommandList.Visible = !Utilities.IsNullOrEmpty(serviceCommands);
				btnCustomCommand.Visible = !Utilities.IsNullOrEmpty(serviceCommands);
			}

			InitializeControlState();
		}

		private void ServiceAdminButton_Click(object sender, EventArgs e) {
			switch (((Button)sender).Name) {
				case "btnStart":
					SC_Svc.ServiceControl.ExecuteCommand(((SC_StatusDb.Service)serviceList.SelectedItem).ServiceId, SC_Svc.ServiceControl.ServiceCommandEnum.Start);
					break;

				case "btnRestart":
					SC_Svc.ServiceControl.ExecuteCommand(((SC_StatusDb.Service)serviceList.SelectedItem).ServiceId, SC_Svc.ServiceControl.ServiceCommandEnum.Restart);
					break;

				case "btnStop":
					SC_Svc.ServiceControl.ExecuteCommand(((SC_StatusDb.Service)serviceList.SelectedItem).ServiceId, SC_Svc.ServiceControl.ServiceCommandEnum.Stop);
					break;

				case "btnCustomCommand":
					SC_Svc.ServiceControl.ExecuteCommand(((SC_StatusDb.Service)serviceList.SelectedItem).ServiceId, SC_Svc.ServiceControl.ServiceCommandEnum.Stop);
					break;
			}

			InitializeControlState();
		}

		private void InitializeControlState() {
			int serviceId = Settings.Instance.IsHub ? ((SC_StatusDb.Service)serviceList.SelectedItem).ServiceId : Settings.Instance.ServiceId;
			if (!serviceId.Equals(-1)) {
				if (Settings.Instance.IsHub) {
					switch (SC_Svc.ServiceControl.GetServiceStatus(((SC_StatusDb.Service)serviceList.SelectedItem).ServiceName)) {
						case ServiceControllerStatus.Running:
							btnStart.Enabled = false;
							btnRestart.Enabled = true;
							btnStop.Enabled = true;
							break;

						case ServiceControllerStatus.Stopped:
							btnStart.Enabled = true;
							btnRestart.Enabled = false;
							btnStop.Enabled = false;
							break;
					}
				}
				else {
					btnStart.Visible = Settings.Instance.IsHub;
					btnRestart.Visible = Settings.Instance.IsHub;
					btnStop.Visible = Settings.Instance.IsHub;
				}
			}
			else {
				btnStart.Enabled = false;
				btnRestart.Enabled = false;
				btnStop.Enabled = false;
				serviceCommandList.Visible = false;
				btnCustomCommand.Visible = false;
			}
		}
	}
}