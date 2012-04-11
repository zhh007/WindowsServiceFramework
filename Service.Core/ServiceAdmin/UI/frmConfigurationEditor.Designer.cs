namespace Service.Core.ServiceAdmin.UI
{
	partial class frmConfigurationEditor
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.grdConfiguration = new System.Windows.Forms.DataGridView();
			this.cboConfiguration = new System.Windows.Forms.ComboBox();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdOk = new System.Windows.Forms.Button();
			this.cboIniSection = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.grdConfiguration)).BeginInit();
			this.SuspendLayout();
			// 
			// grdConfiguration
			// 
			this.grdConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grdConfiguration.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.grdConfiguration.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.grdConfiguration.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grdConfiguration.Location = new System.Drawing.Point(0, 39);
			this.grdConfiguration.Name = "grdConfiguration";
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.grdConfiguration.RowsDefaultCellStyle = dataGridViewCellStyle1;
			this.grdConfiguration.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.grdConfiguration.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.grdConfiguration.Size = new System.Drawing.Size(635, 414);
			this.grdConfiguration.TabIndex = 3;
			// 
			// cboConfiguration
			// 
			this.cboConfiguration.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.cboConfiguration.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cboConfiguration.FormattingEnabled = true;
			this.cboConfiguration.Items.AddRange(new object[] {
            "Extension",
            "Service",
            "Administration"});
			this.cboConfiguration.Location = new System.Drawing.Point(12, 12);
			this.cboConfiguration.Name = "cboConfiguration";
			this.cboConfiguration.Size = new System.Drawing.Size(121, 21);
			this.cboConfiguration.TabIndex = 1;
			this.cboConfiguration.SelectedIndexChanged += new System.EventHandler(this.cboConfiguration_SelectedIndexChanged);
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(548, 10);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(75, 23);
			this.cmdCancel.TabIndex = 2;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			// 
			// cmdOk
			// 
			this.cmdOk.Location = new System.Drawing.Point(467, 10);
			this.cmdOk.Name = "cmdOk";
			this.cmdOk.Size = new System.Drawing.Size(75, 23);
			this.cmdOk.TabIndex = 2;
			this.cmdOk.Text = "OK";
			this.cmdOk.UseVisualStyleBackColor = true;
			// 
			// cboIniSection
			// 
			this.cboIniSection.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.cboIniSection.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cboIniSection.FormattingEnabled = true;
			this.cboIniSection.Items.AddRange(new object[] {
            "Extension",
            "Service",
            "Administration"});
			this.cboIniSection.Location = new System.Drawing.Point(139, 12);
			this.cboIniSection.Name = "cboIniSection";
			this.cboIniSection.Size = new System.Drawing.Size(121, 21);
			this.cboIniSection.TabIndex = 1;
			this.cboIniSection.Visible = false;
			this.cboIniSection.SelectedIndexChanged += new System.EventHandler(this.cboIniSection_SelectedIndexChanged);
			// 
			// frmConfigurationEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(635, 453);
			this.Controls.Add(this.cmdOk);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cboIniSection);
			this.Controls.Add(this.cboConfiguration);
			this.Controls.Add(this.grdConfiguration);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "frmConfigurationEditor";
			this.Load += new System.EventHandler(this.frmConfigurationEditor_Load);
			((System.ComponentModel.ISupportInitialize)(this.grdConfiguration)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView grdConfiguration;
		private System.Windows.Forms.ComboBox cboConfiguration;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdOk;
		private System.Windows.Forms.ComboBox cboIniSection;
	}
}