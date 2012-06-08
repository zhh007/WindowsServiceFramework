namespace Service.Core.ServiceAdmin.UI
{
	partial class frmLogViewer
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.cboFilter = new System.Windows.Forms.ComboBox();
			this.cmdFilter = new System.Windows.Forms.Button();
			this.grdLog = new System.Windows.Forms.DataGridView();
			this.cboLog = new System.Windows.Forms.ComboBox();
			this.comboBox2 = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.grdLog)).BeginInit();
			this.SuspendLayout();
			// 
			// cboFilter
			// 
			this.cboFilter.FormattingEnabled = true;
			this.cboFilter.Location = new System.Drawing.Point(139, 12);
			this.cboFilter.Name = "cboFilter";
			this.cboFilter.Size = new System.Drawing.Size(121, 21);
			this.cboFilter.TabIndex = 1;
			// 
			// cmdFilter
			// 
			this.cmdFilter.Location = new System.Drawing.Point(513, 10);
			this.cmdFilter.Name = "cmdFilter";
			this.cmdFilter.Size = new System.Drawing.Size(75, 23);
			this.cmdFilter.TabIndex = 2;
			this.cmdFilter.Text = "Filter";
			this.cmdFilter.UseVisualStyleBackColor = true;
			this.cmdFilter.Click += new System.EventHandler(this.cmdFilter_Click);
			// 
			// grdLog
			// 
			this.grdLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grdLog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.grdLog.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.grdLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grdLog.Location = new System.Drawing.Point(0, 39);
			this.grdLog.Name = "grdLog";
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.grdLog.RowsDefaultCellStyle = dataGridViewCellStyle2;
			this.grdLog.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.grdLog.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.grdLog.Size = new System.Drawing.Size(599, 426);
			this.grdLog.TabIndex = 3;
			// 
			// cboLog
			// 
			this.cboLog.FormattingEnabled = true;
			this.cboLog.Location = new System.Drawing.Point(12, 12);
			this.cboLog.Name = "cboLog";
			this.cboLog.Size = new System.Drawing.Size(121, 21);
			this.cboLog.TabIndex = 1;
			// 
			// comboBox2
			// 
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Location = new System.Drawing.Point(-362, 12);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(121, 21);
			this.comboBox2.TabIndex = 1;
			// 
			// frmLogViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(600, 466);
			this.Controls.Add(this.grdLog);
			this.Controls.Add(this.comboBox2);
			this.Controls.Add(this.cmdFilter);
			this.Controls.Add(this.cboLog);
			this.Controls.Add(this.cboFilter);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "frmLogViewer";
			this.Load += new System.EventHandler(this.frmLogViewer_Load);
			((System.ComponentModel.ISupportInitialize)(this.grdLog)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox cboFilter;
		private System.Windows.Forms.Button cmdFilter;
		private System.Windows.Forms.DataGridView grdLog;
		private System.Windows.Forms.ComboBox cboLog;
		private System.Windows.Forms.ComboBox comboBox2;
	}
}