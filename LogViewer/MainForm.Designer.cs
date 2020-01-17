namespace LogViewer
{
	partial class MainForm
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this._toolStrip = new System.Windows.Forms.ToolStrip();
			this._openFolderButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this._saveAsButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this._logFileComboBox = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
			this._logLevelComboBox = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this._helpButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this._exitButton = new System.Windows.Forms.ToolStripButton();
			this._contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this._copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this._dataGridView = new LogViewer.DataGridViewEx();
			this.dateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.levelDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.messageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Trace = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._logDataListBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this._toolStrip.SuspendLayout();
			this._contextMenuStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._dataGridView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._logDataListBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// _toolStrip
			// 
			this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._openFolderButton,
            this.toolStripSeparator1,
            this._saveAsButton,
            this.toolStripSeparator4,
            this.toolStripLabel1,
            this._logFileComboBox,
            this.toolStripSeparator2,
            this.toolStripLabel2,
            this._logLevelComboBox,
            this.toolStripSeparator3,
            this._helpButton,
            this.toolStripSeparator5,
            this._exitButton});
			this._toolStrip.Location = new System.Drawing.Point(0, 0);
			this._toolStrip.Name = "_toolStrip";
			this._toolStrip.Size = new System.Drawing.Size(784, 25);
			this._toolStrip.TabIndex = 0;
			this._toolStrip.Text = "toolStrip1";
			// 
			// _openFolderButton
			// 
			this._openFolderButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._openFolderButton.Image = global::LogViewer.Properties.Resources.OpenFolder;
			this._openFolderButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._openFolderButton.Name = "_openFolderButton";
			this._openFolderButton.Size = new System.Drawing.Size(23, 22);
			this._openFolderButton.ToolTipText = "対象フォルダを変更します";
			this._openFolderButton.Click += new System.EventHandler(this.OpenFolderButton_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// _saveAsButton
			// 
			this._saveAsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._saveAsButton.Image = global::LogViewer.Properties.Resources.SaveAs;
			this._saveAsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._saveAsButton.Name = "_saveAsButton";
			this._saveAsButton.Size = new System.Drawing.Size(23, 22);
			this._saveAsButton.ToolTipText = "名前を付けて保存します";
			this._saveAsButton.Click += new System.EventHandler(this.SaveAsButton_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(65, 22);
			this.toolStripLabel1.Text = "ファイル選択";
			// 
			// _logFileComboBox
			// 
			this._logFileComboBox.Name = "_logFileComboBox";
			this._logFileComboBox.Size = new System.Drawing.Size(121, 25);
			this._logFileComboBox.ToolTipText = "今選択中のフォルダの中にあるファイルを選択します";
			this._logFileComboBox.SelectedIndexChanged += new System.EventHandler(this.LogFileComboBox_SelectedIndexChanged);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripLabel2
			// 
			this.toolStripLabel2.Name = "toolStripLabel2";
			this.toolStripLabel2.Size = new System.Drawing.Size(70, 22);
			this.toolStripLabel2.Text = "レベルフィルタ";
			// 
			// _logLevelComboBox
			// 
			this._logLevelComboBox.Name = "_logLevelComboBox";
			this._logLevelComboBox.Size = new System.Drawing.Size(121, 25);
			this._logLevelComboBox.ToolTipText = "ログレベルにフィルタをかけます";
			this._logLevelComboBox.SelectedIndexChanged += new System.EventHandler(this.LogLevelComboBox_SelectedIndexChanged);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// _helpButton
			// 
			this._helpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._helpButton.Image = global::LogViewer.Properties.Resources.Help16;
			this._helpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._helpButton.Name = "_helpButton";
			this._helpButton.Size = new System.Drawing.Size(23, 22);
			this._helpButton.Text = "ヘルプを表示します";
			this._helpButton.Click += new System.EventHandler(this.HelpButton_Click);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
			// 
			// _exitButton
			// 
			this._exitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._exitButton.Image = global::LogViewer.Properties.Resources.Close;
			this._exitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._exitButton.Name = "_exitButton";
			this._exitButton.Size = new System.Drawing.Size(23, 22);
			this._exitButton.Text = "終了します";
			this._exitButton.Click += new System.EventHandler(this.ExitButton_Click);
			// 
			// _contextMenuStrip
			// 
			this._contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._copyToolStripMenuItem});
			this._contextMenuStrip.Name = "_contextMenuStrip";
			this._contextMenuStrip.Size = new System.Drawing.Size(115, 26);
			this._contextMenuStrip.Opened += new System.EventHandler(this.ContextMenuStrip_Opened);
			// 
			// _copyToolStripMenuItem
			// 
			this._copyToolStripMenuItem.Name = "_copyToolStripMenuItem";
			this._copyToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
			this._copyToolStripMenuItem.Text = "コピー(&C)";
			this._copyToolStripMenuItem.Click += new System.EventHandler(this.CopyToolStripMenuItem_Click);
			// 
			// _saveFileDialog
			// 
			this._saveFileDialog.DefaultExt = "log";
			this._saveFileDialog.Filter = "ログファイル|*.log|すべてのファイル|*.*";
			this._saveFileDialog.RestoreDirectory = true;
			this._saveFileDialog.SupportMultiDottedExtensions = true;
			this._saveFileDialog.Title = "ログファイルの保存";
			// 
			// _dataGridView
			// 
			this._dataGridView.AllowUserToAddRows = false;
			this._dataGridView.AllowUserToDeleteRows = false;
			this._dataGridView.AutoGenerateColumns = false;
			this._dataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
			this._dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this._dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dateDataGridViewTextBoxColumn,
            this.levelDataGridViewTextBoxColumn,
            this.messageDataGridViewTextBoxColumn,
            this.Trace});
			this._dataGridView.ContextMenuStrip = this._contextMenuStrip;
			this._dataGridView.DataSource = this._logDataListBindingSource;
			this._dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
			this._dataGridView.Location = new System.Drawing.Point(0, 25);
			this._dataGridView.Name = "_dataGridView";
			this._dataGridView.ReadOnly = true;
			dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this._dataGridView.RowsDefaultCellStyle = dataGridViewCellStyle4;
			this._dataGridView.RowTemplate.Height = 30;
			this._dataGridView.RowTemplate.ReadOnly = true;
			this._dataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this._dataGridView.Size = new System.Drawing.Size(784, 536);
			this._dataGridView.TabIndex = 1;
			this._dataGridView.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.DataGridView_CellContextMenuStripNeeded);
			// 
			// dateDataGridViewTextBoxColumn
			// 
			this.dateDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			this.dateDataGridViewTextBoxColumn.DataPropertyName = "Date";
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dateDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
			this.dateDataGridViewTextBoxColumn.FillWeight = 20F;
			this.dateDataGridViewTextBoxColumn.HeaderText = "日付";
			this.dateDataGridViewTextBoxColumn.Name = "dateDataGridViewTextBoxColumn";
			this.dateDataGridViewTextBoxColumn.ReadOnly = true;
			this.dateDataGridViewTextBoxColumn.Width = 56;
			// 
			// levelDataGridViewTextBoxColumn
			// 
			this.levelDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			this.levelDataGridViewTextBoxColumn.DataPropertyName = "Level";
			this.levelDataGridViewTextBoxColumn.FillWeight = 15F;
			this.levelDataGridViewTextBoxColumn.HeaderText = "ログレベル";
			this.levelDataGridViewTextBoxColumn.Name = "levelDataGridViewTextBoxColumn";
			this.levelDataGridViewTextBoxColumn.ReadOnly = true;
			this.levelDataGridViewTextBoxColumn.Width = 79;
			// 
			// messageDataGridViewTextBoxColumn
			// 
			this.messageDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.messageDataGridViewTextBoxColumn.DataPropertyName = "Message";
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.messageDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
			this.messageDataGridViewTextBoxColumn.FillWeight = 30F;
			this.messageDataGridViewTextBoxColumn.HeaderText = "メッセージ";
			this.messageDataGridViewTextBoxColumn.Name = "messageDataGridViewTextBoxColumn";
			this.messageDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// Trace
			// 
			this.Trace.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Trace.DataPropertyName = "Trace";
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.Trace.DefaultCellStyle = dataGridViewCellStyle3;
			this.Trace.FillWeight = 40F;
			this.Trace.HeaderText = "スタックトレース";
			this.Trace.Name = "Trace";
			this.Trace.ReadOnly = true;
			// 
			// _logDataListBindingSource
			// 
			this._logDataListBindingSource.DataSource = typeof(LogViewer.LogDataList);
			// 
			// MainForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(784, 561);
			this.Controls.Add(this._dataGridView);
			this.Controls.Add(this._toolStrip);
			this.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.Text = "iCAD Renamer ログビューワ";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this._toolStrip.ResumeLayout(false);
			this._toolStrip.PerformLayout();
			this._contextMenuStrip.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._dataGridView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._logDataListBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip _toolStrip;
		private System.Windows.Forms.ToolStripButton _openFolderButton;
		private System.Windows.Forms.ToolStripComboBox _logFileComboBox;
		private LogViewer.DataGridViewEx _dataGridView;
		private System.Windows.Forms.BindingSource _logDataListBindingSource;
		private System.Windows.Forms.ToolStripComboBox _logLevelComboBox;
		private System.Windows.Forms.FolderBrowserDialog _folderBrowserDialog;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripLabel toolStripLabel2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.DataGridViewTextBoxColumn dateDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn levelDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn messageDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn Trace;
		private System.Windows.Forms.ContextMenuStrip _contextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem _copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton _saveAsButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.SaveFileDialog _saveFileDialog;
		private System.Windows.Forms.ToolStripButton _helpButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripButton _exitButton;
	}
}

