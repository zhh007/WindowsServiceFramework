using System;
using System.Windows.Forms;

namespace Service.Core.ServiceAdmin.UI
{
	public partial class frmLogin : Form
	{
		private const string password = "Passw0rd1";

		public frmLogin()
		{
			InitializeComponent();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			if (txtPassword.Text.Equals(password))
			{
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
			else
			{
				MessageBox.Show("Incorrect password. Please try again.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void frmLogin_Load(object sender, EventArgs e)
		{
			this.ActiveControl = txtPassword;
		}
	}
}