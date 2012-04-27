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

namespace SelfInstallingWindowsServiceAdmin.UI {
	public partial class frmLogViewer : SC_SAdminUi.frmLogViewer {
		//public frmLogViewer()
		//{
		//    InitializeComponent();
		//    InitializeControls();

		//    this.Text = Settings.Instance.ServiceDisplayName + " Log Viewer";
		//}

		//private void InitializeControls()
		//{
		//    #region Logs list

		//    List<LogToView> logs = new List<LogToView>();

		//    string[] logNames = Enum.GetNames(typeof(LogEnum));
		//    var logValues = (LogEnum[])Enum.GetValues(typeof(LogEnum));

		//    for (int i = 0; i < logNames.Length; i++)
		//    {
		//        logs.Add(new LogToView { Text = logNames[i], Value = logValues[i] });
		//    }

		//    cboLog.DataSource = logs;
		//    cboLog.DisplayMember = "Text";
		//    cboLog.ValueMember = "Value";

		//    #endregion Logs list

		//    #region Filters list

		//    List<LogFilter> filters = new List<LogFilter>();

		//    filters.Add(new LogFilter { Text = "All", Value = 0 });

		//    string[] logLevelNames = Enum.GetNames(typeof(LogLevelEnum));
		//    var logLevelValues = (LogLevelEnum[])Enum.GetValues(typeof(LogLevelEnum));

		//    for (int i = 0; i < logLevelNames.Length; i++)
		//    {
		//        filters.Add(new LogFilter { Text = logLevelNames[i], Value = logLevelValues[i] });
		//    }

		//    cboFilter.DataSource = filters;
		//    cboFilter.DisplayMember = "Text";
		//    cboFilter.ValueMember = "Value";

		//    #endregion Filters list
		//}

		//private void cmdFilter_Click(object sender, EventArgs e)
		//{
		//    if ((int)cboFilter.SelectedValue == 0)
		//    {
		//        grdLog.DataSource = Logging.Parse((LogEnum)cboLog.SelectedValue);
		//    }
		//    else
		//    {
		//        grdLog.DataSource = Logging.Parse((LogEnum)cboLog.SelectedValue, (LogLevelEnum)cboFilter.SelectedValue);
		//    }
		//}

		//private class LogFilter
		//{
		//    public string Text { get; set; }

		//    public LogLevelEnum Value { get; set; }
		//}

		//private class LogToView
		//{
		//    public string Text { get; set; }

		//    public LogEnum Value { get; set; }
		//}

		//private void frmLogViewer_Load(object sender, EventArgs e)
		//{
		//    this.ActiveControl = cboLog;
		//}
	}
}