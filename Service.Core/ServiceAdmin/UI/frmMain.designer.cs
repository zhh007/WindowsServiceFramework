namespace Service.Core.ServiceAdmin.UI
{
	partial class frmMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnServiceControl = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnViewLog = new System.Windows.Forms.Button();
			this.btnConfig = new System.Windows.Forms.Button();
			this.btnUninstall = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.btnRestart = new System.Windows.Forms.Button();
			this.btnStart = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.txtServiceAddressWSDL = new System.Windows.Forms.TextBox();
			this.txtServiceAddress = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.txtStatusMessage = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnServiceControl
			// 
			this.btnServiceControl.Location = new System.Drawing.Point(12, 27);
			this.btnServiceControl.Name = "btnServiceControl";
			this.btnServiceControl.Size = new System.Drawing.Size(139, 23);
			this.btnServiceControl.TabIndex = 1;
			this.btnServiceControl.Text = "Service Control";
			this.btnServiceControl.UseVisualStyleBackColor = true;
			this.btnServiceControl.Click += new System.EventHandler(this.ServiceControl_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnViewLog);
			this.groupBox1.Controls.Add(this.btnConfig);
			this.groupBox1.Controls.Add(this.btnUninstall);
			this.groupBox1.Controls.Add(this.btnStop);
			this.groupBox1.Controls.Add(this.btnRestart);
			this.groupBox1.Controls.Add(this.btnStart);
			this.groupBox1.Location = new System.Drawing.Point(157, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(297, 108);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Service Administration (Password Required)";
			// 
			// btnViewLog
			// 
			this.btnViewLog.Location = new System.Drawing.Point(152, 77);
			this.btnViewLog.Name = "btnViewLog";
			this.btnViewLog.Size = new System.Drawing.Size(139, 23);
			this.btnViewLog.TabIndex = 7;
			this.btnViewLog.Text = "View Log";
			this.btnViewLog.UseVisualStyleBackColor = true;
			this.btnViewLog.Click += new System.EventHandler(this.ServiceAdminButton_Click);
			// 
			// btnConfig
			// 
			this.btnConfig.Location = new System.Drawing.Point(151, 19);
			this.btnConfig.Name = "btnConfig";
			this.btnConfig.Size = new System.Drawing.Size(139, 23);
			this.btnConfig.TabIndex = 6;
			this.btnConfig.Text = "Configuration";
			this.btnConfig.UseVisualStyleBackColor = true;
			this.btnConfig.Click += new System.EventHandler(this.ServiceAdminButton_Click);
			// 
			// btnUninstall
			// 
			this.btnUninstall.Location = new System.Drawing.Point(152, 48);
			this.btnUninstall.Name = "btnUninstall";
			this.btnUninstall.Size = new System.Drawing.Size(139, 23);
			this.btnUninstall.TabIndex = 5;
			this.btnUninstall.Text = "Uninstall";
			this.btnUninstall.UseVisualStyleBackColor = true;
			this.btnUninstall.Visible = false;
			this.btnUninstall.Click += new System.EventHandler(this.ServiceAdminButton_Click);
			// 
			// btnStop
			// 
			this.btnStop.Location = new System.Drawing.Point(6, 77);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(139, 23);
			this.btnStop.TabIndex = 4;
			this.btnStop.Text = "Stop";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.ServiceAdminButton_Click);
			// 
			// btnRestart
			// 
			this.btnRestart.Location = new System.Drawing.Point(6, 48);
			this.btnRestart.Name = "btnRestart";
			this.btnRestart.Size = new System.Drawing.Size(139, 23);
			this.btnRestart.TabIndex = 3;
			this.btnRestart.Text = "Restart";
			this.btnRestart.UseVisualStyleBackColor = true;
			this.btnRestart.Click += new System.EventHandler(this.ServiceAdminButton_Click);
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(6, 19);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(139, 23);
			this.btnStart.TabIndex = 2;
			this.btnStart.Text = "Start";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.ServiceAdminButton_Click);
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(12, 85);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(139, 23);
			this.btnClose.TabIndex = 0;
			this.btnClose.Text = "Close Window";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// txtServiceAddressWSDL
			// 
			this.txtServiceAddressWSDL.Location = new System.Drawing.Point(139, 148);
			this.txtServiceAddressWSDL.Name = "txtServiceAddressWSDL";
			this.txtServiceAddressWSDL.ReadOnly = true;
			this.txtServiceAddressWSDL.Size = new System.Drawing.Size(315, 20);
			this.txtServiceAddressWSDL.TabIndex = 9;
			// 
			// txtServiceAddress
			// 
			this.txtServiceAddress.Location = new System.Drawing.Point(139, 125);
			this.txtServiceAddress.Name = "txtServiceAddress";
			this.txtServiceAddress.ReadOnly = true;
			this.txtServiceAddress.Size = new System.Drawing.Size(315, 20);
			this.txtServiceAddress.TabIndex = 8;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 148);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(121, 13);
			this.label2.TabIndex = 7;
			this.label2.Text = "Service WSDL address:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 125);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(86, 13);
			this.label1.TabIndex = 6;
			this.label1.Text = "Service address:";
			// 
			// btnRefresh
			// 
			this.btnRefresh.Location = new System.Drawing.Point(12, 56);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(139, 23);
			this.btnRefresh.TabIndex = 10;
			this.btnRefresh.Text = "Refresh";
			this.btnRefresh.UseVisualStyleBackColor = true;
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// txtStatusMessage
			// 
			this.txtStatusMessage.Location = new System.Drawing.Point(139, 173);
			this.txtStatusMessage.Name = "txtStatusMessage";
			this.txtStatusMessage.ReadOnly = true;
			this.txtStatusMessage.Size = new System.Drawing.Size(315, 20);
			this.txtStatusMessage.TabIndex = 12;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 173);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(85, 13);
			this.label3.TabIndex = 11;
			this.label3.Text = "Status message:";
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(465, 205);
			this.Controls.Add(this.txtStatusMessage);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.btnRefresh);
			this.Controls.Add(this.txtServiceAddressWSDL);
			this.Controls.Add(this.txtServiceAddress);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnServiceControl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.Name = "frmMain";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnServiceControl;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnUninstall;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.Button btnRestart;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnConfig;
		private System.Windows.Forms.Button btnViewLog;
		private System.Windows.Forms.TextBox txtServiceAddressWSDL;
		private System.Windows.Forms.TextBox txtServiceAddress;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnRefresh;
		private System.Windows.Forms.TextBox txtStatusMessage;
		private System.Windows.Forms.Label label3;
	}
}

