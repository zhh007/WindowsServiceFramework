using System.ServiceProcess;

namespace Service.Core.WindowsService
{
	partial class WindowsService : ServiceBase
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{			
			// This line will cause an error in Design mode. Ignore and don't use Design mode.			
			this.ServiceName = Utility.Settings.Instance.ServiceName;
		}

		#endregion
	}
}
