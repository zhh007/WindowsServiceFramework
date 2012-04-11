namespace Service.Core.ServiceAdmin.UI
{
	partial class frmServiceControl
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
			this.serviceList = new System.Windows.Forms.ComboBox();
			this.btnStart = new System.Windows.Forms.Button();
			this.btnRestart = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.serviceCommandList = new System.Windows.Forms.ComboBox();
			this.btnCustomCommand = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// serviceList
			// 
			this.serviceList.FormattingEnabled = true;
			this.serviceList.Location = new System.Drawing.Point(13, 13);
			this.serviceList.Name = "serviceList";
			this.serviceList.Size = new System.Drawing.Size(193, 21);
			this.serviceList.TabIndex = 0;
			this.serviceList.SelectedIndexChanged += new System.EventHandler(this.serviceList_SelectedIndexChanged);
			// 
			// btnStart
			// 
			this.btnStart.Enabled = false;
			this.btnStart.Location = new System.Drawing.Point(13, 40);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(193, 21);
			this.btnStart.TabIndex = 1;
			this.btnStart.Text = "Start";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.ServiceAdminButton_Click);
			// 
			// btnRestart
			// 
			this.btnRestart.Enabled = false;
			this.btnRestart.Location = new System.Drawing.Point(13, 67);
			this.btnRestart.Name = "btnRestart";
			this.btnRestart.Size = new System.Drawing.Size(193, 21);
			this.btnRestart.TabIndex = 2;
			this.btnRestart.Text = "Restart";
			this.btnRestart.UseVisualStyleBackColor = true;
			this.btnRestart.Click += new System.EventHandler(this.ServiceAdminButton_Click);
			// 
			// btnStop
			// 
			this.btnStop.Enabled = false;
			this.btnStop.Location = new System.Drawing.Point(13, 94);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(193, 21);
			this.btnStop.TabIndex = 3;
			this.btnStop.Text = "Stop";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.ServiceAdminButton_Click);
			// 
			// serviceCommandList
			// 
			this.serviceCommandList.FormattingEnabled = true;
			this.serviceCommandList.Location = new System.Drawing.Point(13, 121);
			this.serviceCommandList.Name = "serviceCommandList";
			this.serviceCommandList.Size = new System.Drawing.Size(193, 21);
			this.serviceCommandList.TabIndex = 4;
			this.serviceCommandList.Visible = false;
			// 
			// btnCustomCommand
			// 
			this.btnCustomCommand.Location = new System.Drawing.Point(13, 148);
			this.btnCustomCommand.Name = "btnCustomCommand";
			this.btnCustomCommand.Size = new System.Drawing.Size(193, 21);
			this.btnCustomCommand.TabIndex = 5;
			this.btnCustomCommand.Text = "Go";
			this.btnCustomCommand.UseVisualStyleBackColor = true;
			this.btnCustomCommand.Visible = false;
			// 
			// frmServiceControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(219, 183);
			this.Controls.Add(this.btnCustomCommand);
			this.Controls.Add(this.serviceCommandList);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.btnRestart);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.serviceList);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmServiceControl";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Service Control";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox serviceList;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnRestart;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.ComboBox serviceCommandList;
		private System.Windows.Forms.Button btnCustomCommand;
	}
}