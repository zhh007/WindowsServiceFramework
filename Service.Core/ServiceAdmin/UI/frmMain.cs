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
using System.ServiceProcess;
using System.Windows.Forms;
using Service.Core.ServiceAdmin.Utility;
using SC_StatusDb = Service.Core.StatusDatabase;
using SC_Svc = Service.Core.WindowsService.Service;

namespace Service.Core.ServiceAdmin.UI {
	public partial class frmMain : Form {
		public enum ServiceCommands {
			ServiceControl = 128,
			WcfRestart = 251,
			WcfStop = 252,
			WcfStart = 253,
			Restart = 253,
			Uninstall = 255
		}

		public frmMain() {
			InitializeComponent();
			InitializeControlState();
			this.Text = Settings.Instance.ServiceDisplayName;
		}

		public void ServiceAdminButton_Click(object sender, EventArgs e) {
			if (!Login()) return;

			switch (((Button)sender).Name) {
				case "btnStart":
					SC_Svc.ServiceControl.ExecuteCommand(SC_Svc.ServiceControl.ServiceCommandEnum.Start);
					break;

				case "btnRestart":
					SC_Svc.ServiceControl.ExecuteCommand(SC_Svc.ServiceControl.ServiceCommandEnum.Restart);
					break;

				case "btnStop":
					SC_Svc.ServiceControl.ExecuteCommand(SC_Svc.ServiceControl.ServiceCommandEnum.Stop);
					break;

				// BUG: For some reason this fails.
				//case "btnUninstall":
				//    try
				//    {
				//        if (MessageBox.Show(string.Format("Are you sure you want to uninstall the {0} from this computer?\n(This application will close.)", Settings.Instance.ServiceName), string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
				//        {
				//            ServiceControl.ExecuteCommand(ServiceControl.ServiceCommandEnum.Uninstall);
				//            Application.Exit();
				//        }
				//    }
				//    catch
				//    {
				//        return;
				//    }
				//    break;

				case "btnConfig":
					frmConfigurationEditor configEditor = new frmConfigurationEditor();
					if (MessageBox.Show(string.Format("Note: This option will cause a restart of the {0} if values are changed.\n\nDo you want to continue?", Settings.Instance.ServiceDisplayName), "Configuration Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK) {
						if (configEditor.ShowDialog() != DialogResult.Cancel && SC_Svc.ServiceControl.ServiceStatus == ServiceControllerStatus.Running) {
							SC_Svc.ServiceControl.ExecuteCommand(SC_Svc.ServiceControl.ServiceCommandEnum.Restart);
						}
					}
					break;

				case "btnViewLog":
					frmLogViewer logViewer = new frmLogViewer();
					logViewer.ShowDialog();
					break;
			}

			InitializeControlState();
		}

		private void ServiceControl_Click(object sender, EventArgs e) {
			frmServiceControl serviceControl = new frmServiceControl();
			serviceControl.ShowDialog();
			//ServiceController sc = new ServiceController(Settings.Instance.ServiceName);
			//sc.ExecuteCommand((int)ServiceCommands.ProcessFullReload);
		}

		private void btnRefresh_Click(object sender, EventArgs e) {
			// TODO: Insert status message update code here.
			// Reset our button states and the service messages.
			InitializeControlState();
		}

		private void btnClose_Click(object sender, EventArgs e) {
			Application.Exit();
		}

		private void InitializeControlState() {
			// If we are a hub or we have service commands, then we want to show the service control button.
			bool showServiceControlButton = (Settings.Instance.IsHub || Settings.Instance.UseWcf) ||
				(!Settings.Instance.IsHub &&
				 !string.IsNullOrEmpty(Settings.Instance.ServiceStatusDatabasePath) &&
				 !Utilities.IsNullOrEmpty(SC_StatusDb.DatabaseController.GetServiceCommands(new SC_StatusDb.Service {
					 ServiceId = Settings.Instance.ServiceId
				 }, Settings.Instance.LoggerConfiguration)));

			// Eat the initial status exception
			try {
				ServiceController sc = new ServiceController(Settings.Instance.ServiceName);

				switch (sc.Status) {
					case ServiceControllerStatus.Running:
						if (showServiceControlButton) {
							btnServiceControl.Visible = true;
							btnServiceControl.Enabled = true;
						}
						else {
							btnServiceControl.Visible = false;
						}
						btnRestart.Enabled = true;
						btnStart.Enabled = false;
						btnStop.Enabled = true;
						if (Settings.Instance.IsHub || Settings.Instance.UseWcf) {
							txtServiceAddress.Text = string.Format(Settings.Instance.WcfURLFormat, Utilities.GetIPv4Address(), Settings.Instance.WcfPort);
							txtServiceAddressWSDL.Text = string.Format(Settings.Instance.WcfWSDLFormat, Utilities.GetIPv4Address(), Settings.Instance.WcfMexPort);
						}
						txtStatusMessage.Text = string.Format("{0}: status = Running", Settings.Instance.ServiceDisplayName);
						break;

					case ServiceControllerStatus.Stopped:
						if (showServiceControlButton) {
							btnServiceControl.Visible = true;
							btnServiceControl.Enabled = false;
						}
						else {
							btnServiceControl.Visible = false;
						}

						btnRestart.Enabled = false;
						btnStart.Enabled = true;
						btnStop.Enabled = false;
						txtServiceAddress.Text = string.Empty;
						txtServiceAddressWSDL.Text = string.Empty;
						txtStatusMessage.Text = string.Format("{0}: status = Stopped", Settings.Instance.ServiceDisplayName);
						break;
				}
			}
			catch {
				if (showServiceControlButton) {
					btnServiceControl.Visible = true;
					btnServiceControl.Enabled = false;
				}
				else {
					btnServiceControl.Visible = false;
				}
				btnRestart.Enabled = false;
				btnStart.Enabled = true;
				btnStop.Enabled = false;
			}
		}

		// Simple login check.
		private bool Login() {
			frmLogin login = new frmLogin();
			return login.ShowDialog() == DialogResult.OK;
		}
	}
}