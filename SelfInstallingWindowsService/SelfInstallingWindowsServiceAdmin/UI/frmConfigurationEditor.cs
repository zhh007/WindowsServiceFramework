using SC_SAdminUi = Service.Core.ServiceAdmin.UI;

namespace SelfInstallingWindowsServiceAdmin.UI
{
	public partial class frmConfigurationEditor : SC_SAdminUi.frmConfigurationEditor
	{
		//public frmConfigurationEditor()
		//{
		//    InitializeComponent();
		//    InitializeControls();

		//    this.Text = Settings.Instance.ServiceDisplayName + " Configuration Editor";
		//}

		//private void InitializeControls()
		//{
		//}

		//private void frmConfigurationEditor_Load(object sender, EventArgs e)
		//{
		//    this.ActiveControl = cboConfiguration;
		//}

		//private void cboConfiguration_SelectedIndexChanged(object sender, EventArgs e)
		//{
		//    cboIniSection.Visible = false;
		//    grdConfiguration.DataBindings.Clear();

		//    if (cboConfiguration.SelectedItem.ToString() == "Extension")
		//    {
		//        List<string> iniSections = Settings.Instance.GetIniSectionNames();
		//        iniSections.Sort();
		//        cboIniSection.DataSource = iniSections;
		//        cboIniSection.Visible = true;
		//    }
		//    else if (cboConfiguration.SelectedItem.ToString() == "Service")
		//    {
		//        grdConfiguration.DataSource = Settings.Instance.GetServiceConfiguration();
		//    }
		//    else if (cboConfiguration.SelectedItem.ToString() == "Administration")
		//    {
		//        grdConfiguration.DataSource = Settings.Instance.GetAdminConfiguration();
		//    }
		//}

		//private void cboIniSection_SelectedIndexChanged(object sender, EventArgs e)
		//{
		//    grdConfiguration.DataSource = Settings.Instance.GetIniSectionValues(cboIniSection.SelectedItem.ToString());
		//}
	}
}