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

using SC_SAdminUi = Service.Core.ServiceAdmin.UI;

namespace MonitorServiceAdmin.UI {
	public partial class frmConfigurationEditor : SC_SAdminUi.frmConfigurationEditor {
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