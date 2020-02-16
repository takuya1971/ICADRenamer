namespace ICADRenamer
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this._toolStrip = new System.Windows.Forms.ToolStrip();
			this._toolStripSettingButton = new System.Windows.Forms.ToolStripButton();
			this._toolStripEditorButton = new System.Windows.Forms.ToolStripButton();
			this._toolStripHelpButton = new System.Windows.Forms.ToolStripButton();
			this._toolStripInformationButton = new System.Windows.Forms.ToolStripButton();
			this._toolTip = new System.Windows.Forms.ToolTip(this.components);
			this._destinationBrowseButton = new System.Windows.Forms.Button();
			this._destinationBox = new System.Windows.Forms.TextBox();
			this._sourceBrowseButton = new System.Windows.Forms.Button();
			this._sourceBox = new System.Windows.Forms.TextBox();
			this._closeButton = new System.Windows.Forms.Button();
			this._executeButton = new System.Windows.Forms.Button();
			this._newProjectLabel = new System.Windows.Forms.Label();
			this._signatureLabel = new System.Windows.Forms.Label();
			this._sourceFileView = new System.Windows.Forms.ListBox();
			this._destinationFileView = new System.Windows.Forms.ListBox();
			this._tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this._tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this._tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this._sourceLabel = new System.Windows.Forms.Label();
			this._destinationLabel = new System.Windows.Forms.Label();
			this._flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this._tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this._newProjectBox = new System.Windows.Forms.TextBox();
			this._signatureBox = new System.Windows.Forms.TextBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this._AllChangeButton = new System.Windows.Forms.RadioButton();
			this._PartOnlyButton = new System.Windows.Forms.RadioButton();
			this._drawOnlyButton = new System.Windows.Forms.RadioButton();
			this._toolStrip.SuspendLayout();
			this._tableLayoutPanel1.SuspendLayout();
			this._tableLayoutPanel2.SuspendLayout();
			this._tableLayoutPanel3.SuspendLayout();
			this._flowLayoutPanel1.SuspendLayout();
			this._tableLayoutPanel4.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _toolStrip
			// 
			this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._toolStripSettingButton,
            this._toolStripEditorButton,
            this._toolStripHelpButton,
            this._toolStripInformationButton});
			this._toolStrip.Location = new System.Drawing.Point(0, 0);
			this._toolStrip.Name = "_toolStrip";
			this._toolStrip.Size = new System.Drawing.Size(674, 25);
			this._toolStrip.TabIndex = 0;
			this._toolStrip.Text = "toolStrip1";
			// 
			// _toolStripSettingButton
			// 
			this._toolStripSettingButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._toolStripSettingButton.Image = ((System.Drawing.Image)(resources.GetObject("_toolStripSettingButton.Image")));
			this._toolStripSettingButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._toolStripSettingButton.Name = "_toolStripSettingButton";
			this._toolStripSettingButton.Size = new System.Drawing.Size(23, 22);
			this._toolStripSettingButton.Text = "設定画面を開きます";
			this._toolStripSettingButton.Click += new System.EventHandler(this.ToolStripSettingButton_Click);
			// 
			// _toolStripEditorButton
			// 
			this._toolStripEditorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._toolStripEditorButton.Image = ((System.Drawing.Image)(resources.GetObject("_toolStripEditorButton.Image")));
			this._toolStripEditorButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._toolStripEditorButton.Name = "_toolStripEditorButton";
			this._toolStripEditorButton.Size = new System.Drawing.Size(23, 22);
			this._toolStripEditorButton.Text = "図面検索文字エディタを開きます";
			this._toolStripEditorButton.Click += new System.EventHandler(this.ToolStripEditorButton_Click);
			// 
			// _toolStripHelpButton
			// 
			this._toolStripHelpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._toolStripHelpButton.Image = ((System.Drawing.Image)(resources.GetObject("_toolStripHelpButton.Image")));
			this._toolStripHelpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._toolStripHelpButton.Name = "_toolStripHelpButton";
			this._toolStripHelpButton.Size = new System.Drawing.Size(23, 22);
			this._toolStripHelpButton.Text = "ヘルプを参照します";
			this._toolStripHelpButton.Click += new System.EventHandler(this.ToolStripHelpButton_Click);
			// 
			// _toolStripInformationButton
			// 
			this._toolStripInformationButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._toolStripInformationButton.Image = ((System.Drawing.Image)(resources.GetObject("_toolStripInformationButton.Image")));
			this._toolStripInformationButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._toolStripInformationButton.Name = "_toolStripInformationButton";
			this._toolStripInformationButton.Size = new System.Drawing.Size(23, 22);
			this._toolStripInformationButton.Text = "情報パネルを開きます";
			this._toolStripInformationButton.Click += new System.EventHandler(this.ToolStripInformationButton_Click);
			// 
			// _destinationBrowseButton
			// 
			this._destinationBrowseButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this._destinationBrowseButton.Image = global::ICADRenamer.Properties.Resources.FolderBrowseIcon;
			this._destinationBrowseButton.Location = new System.Drawing.Point(284, 3);
			this._destinationBrowseButton.Name = "_destinationBrowseButton";
			this._destinationBrowseButton.Size = new System.Drawing.Size(44, 33);
			this._destinationBrowseButton.TabIndex = 3;
			this._destinationBrowseButton.Tag = "4";
			this._toolTip.SetToolTip(this._destinationBrowseButton, "コピー先フォルダ選択ダイアログをン表示します");
			this._destinationBrowseButton.UseVisualStyleBackColor = true;
			this._destinationBrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
			// 
			// _destinationBox
			// 
			this._destinationBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._destinationBox.Location = new System.Drawing.Point(3, 3);
			this._destinationBox.Multiline = true;
			this._destinationBox.Name = "_destinationBox";
			this._destinationBox.Size = new System.Drawing.Size(275, 33);
			this._destinationBox.TabIndex = 4;
			this._destinationBox.Tag = "4";
			this._toolTip.SetToolTip(this._destinationBox, "コピー先フォルダを入力します");
			this._destinationBox.Validating += new System.ComponentModel.CancelEventHandler(this.PathBox_Validating);
			this._destinationBox.Validated += new System.EventHandler(this.Boxes_Validated);
			// 
			// _sourceBrowseButton
			// 
			this._sourceBrowseButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this._sourceBrowseButton.Image = global::ICADRenamer.Properties.Resources.FolderBrowseIcon;
			this._sourceBrowseButton.Location = new System.Drawing.Point(284, 3);
			this._sourceBrowseButton.Name = "_sourceBrowseButton";
			this._sourceBrowseButton.Size = new System.Drawing.Size(44, 33);
			this._sourceBrowseButton.TabIndex = 0;
			this._sourceBrowseButton.Tag = "3";
			this._toolTip.SetToolTip(this._sourceBrowseButton, "コピー元フォルダ選択のダイアログを表示します");
			this._sourceBrowseButton.UseVisualStyleBackColor = true;
			this._sourceBrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
			// 
			// _sourceBox
			// 
			this._sourceBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._sourceBox.Location = new System.Drawing.Point(3, 3);
			this._sourceBox.Multiline = true;
			this._sourceBox.Name = "_sourceBox";
			this._sourceBox.Size = new System.Drawing.Size(275, 33);
			this._sourceBox.TabIndex = 1;
			this._sourceBox.Tag = "3";
			this._toolTip.SetToolTip(this._sourceBox, "コピー元フォルダを入力します");
			this._sourceBox.Validating += new System.ComponentModel.CancelEventHandler(this.PathBox_Validating);
			this._sourceBox.Validated += new System.EventHandler(this.Boxes_Validated);
			// 
			// _closeButton
			// 
			this._closeButton.Image = global::ICADRenamer.Properties.Resources.CancelIcon;
			this._closeButton.Location = new System.Drawing.Point(584, 4);
			this._closeButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this._closeButton.Name = "_closeButton";
			this._closeButton.Size = new System.Drawing.Size(87, 44);
			this._closeButton.TabIndex = 0;
			this._toolTip.SetToolTip(this._closeButton, "終了します");
			this._closeButton.UseVisualStyleBackColor = true;
			this._closeButton.Click += new System.EventHandler(this.CloseButton_Click);
			// 
			// _executeButton
			// 
			this._executeButton.Enabled = false;
			this._executeButton.ForeColor = System.Drawing.SystemColors.ControlText;
			this._executeButton.Image = global::ICADRenamer.Properties.Resources.OKIcon;
			this._executeButton.Location = new System.Drawing.Point(491, 4);
			this._executeButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this._executeButton.Name = "_executeButton";
			this._executeButton.Size = new System.Drawing.Size(87, 44);
			this._executeButton.TabIndex = 1;
			this._toolTip.SetToolTip(this._executeButton, "変換を実行します");
			this._executeButton.UseVisualStyleBackColor = true;
			this._executeButton.Click += new System.EventHandler(this.ExecuteButton_Click);
			// 
			// _newProjectLabel
			// 
			this._newProjectLabel.AutoSize = true;
			this._newProjectLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._newProjectLabel.Location = new System.Drawing.Point(3, 0);
			this._newProjectLabel.Name = "_newProjectLabel";
			this._newProjectLabel.Size = new System.Drawing.Size(74, 33);
			this._newProjectLabel.TabIndex = 0;
			this._newProjectLabel.Tag = "1";
			this._newProjectLabel.Text = "新しい番号";
			this._newProjectLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this._toolTip.SetToolTip(this._newProjectLabel, "新しいM番を入力します");
			// 
			// _signatureLabel
			// 
			this._signatureLabel.AutoSize = true;
			this._signatureLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._signatureLabel.Location = new System.Drawing.Point(339, 0);
			this._signatureLabel.Name = "_signatureLabel";
			this._signatureLabel.Size = new System.Drawing.Size(61, 33);
			this._signatureLabel.TabIndex = 2;
			this._signatureLabel.Tag = "2";
			this._signatureLabel.Text = "署名";
			this._signatureLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this._toolTip.SetToolTip(this._signatureLabel, "図面の署名欄に表示する名前を入力します");
			// 
			// _sourceFileView
			// 
			this._sourceFileView.Dock = System.Windows.Forms.DockStyle.Fill;
			this._sourceFileView.ItemHeight = 15;
			this._sourceFileView.Location = new System.Drawing.Point(3, 88);
			this._sourceFileView.Name = "_sourceFileView";
			this._sourceFileView.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this._sourceFileView.Size = new System.Drawing.Size(331, 295);
			this._sourceFileView.Sorted = true;
			this._sourceFileView.TabIndex = 2;
			this._sourceFileView.TabStop = false;
			this._toolTip.SetToolTip(this._sourceFileView, "コピー元のファイルリストが表示されます");
			this._sourceFileView.UseTabStops = false;
			// 
			// _destinationFileView
			// 
			this._destinationFileView.Dock = System.Windows.Forms.DockStyle.Fill;
			this._destinationFileView.ItemHeight = 15;
			this._destinationFileView.Location = new System.Drawing.Point(340, 88);
			this._destinationFileView.Name = "_destinationFileView";
			this._destinationFileView.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this._destinationFileView.Size = new System.Drawing.Size(331, 295);
			this._destinationFileView.Sorted = true;
			this._destinationFileView.TabIndex = 3;
			this._toolTip.SetToolTip(this._destinationFileView, "コピー先のファイルリストが表示されます");
			this._destinationFileView.UseTabStops = false;
			// 
			// _tableLayoutPanel1
			// 
			this._tableLayoutPanel1.ColumnCount = 2;
			this._tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tableLayoutPanel1.Controls.Add(this._tableLayoutPanel2, 1, 1);
			this._tableLayoutPanel1.Controls.Add(this._tableLayoutPanel3, 0, 1);
			this._tableLayoutPanel1.Controls.Add(this._sourceFileView, 0, 2);
			this._tableLayoutPanel1.Controls.Add(this._destinationFileView, 1, 2);
			this._tableLayoutPanel1.Controls.Add(this._sourceLabel, 0, 0);
			this._tableLayoutPanel1.Controls.Add(this._destinationLabel, 1, 0);
			this._tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutPanel1.Location = new System.Drawing.Point(0, 93);
			this._tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this._tableLayoutPanel1.Name = "_tableLayoutPanel1";
			this._tableLayoutPanel1.RowCount = 3;
			this._tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this._tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
			this._tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel1.Size = new System.Drawing.Size(674, 386);
			this._tableLayoutPanel1.TabIndex = 1;
			// 
			// _tableLayoutPanel2
			// 
			this._tableLayoutPanel2.ColumnCount = 2;
			this._tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this._tableLayoutPanel2.Controls.Add(this._destinationBrowseButton, 1, 0);
			this._tableLayoutPanel2.Controls.Add(this._destinationBox, 0, 0);
			this._tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutPanel2.Location = new System.Drawing.Point(340, 43);
			this._tableLayoutPanel2.Name = "_tableLayoutPanel2";
			this._tableLayoutPanel2.RowCount = 1;
			this._tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel2.Size = new System.Drawing.Size(331, 39);
			this._tableLayoutPanel2.TabIndex = 1;
			// 
			// _tableLayoutPanel3
			// 
			this._tableLayoutPanel3.ColumnCount = 2;
			this._tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this._tableLayoutPanel3.Controls.Add(this._sourceBrowseButton, 1, 0);
			this._tableLayoutPanel3.Controls.Add(this._sourceBox, 0, 0);
			this._tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutPanel3.Location = new System.Drawing.Point(3, 43);
			this._tableLayoutPanel3.Name = "_tableLayoutPanel3";
			this._tableLayoutPanel3.RowCount = 1;
			this._tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel3.Size = new System.Drawing.Size(331, 39);
			this._tableLayoutPanel3.TabIndex = 0;
			// 
			// _sourceLabel
			// 
			this._sourceLabel.AutoSize = true;
			this._sourceLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._sourceLabel.Location = new System.Drawing.Point(3, 0);
			this._sourceLabel.Name = "_sourceLabel";
			this._sourceLabel.Size = new System.Drawing.Size(331, 40);
			this._sourceLabel.TabIndex = 4;
			this._sourceLabel.Tag = "3";
			this._sourceLabel.Text = "コピー元フォルダ";
			this._sourceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// _destinationLabel
			// 
			this._destinationLabel.AutoSize = true;
			this._destinationLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._destinationLabel.Location = new System.Drawing.Point(340, 0);
			this._destinationLabel.Name = "_destinationLabel";
			this._destinationLabel.Size = new System.Drawing.Size(331, 40);
			this._destinationLabel.TabIndex = 5;
			this._destinationLabel.Tag = "4";
			this._destinationLabel.Text = "コピー先フォルダ";
			this._destinationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// _flowLayoutPanel1
			// 
			this._flowLayoutPanel1.Controls.Add(this._closeButton);
			this._flowLayoutPanel1.Controls.Add(this._executeButton);
			this._flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this._flowLayoutPanel1.Location = new System.Drawing.Point(0, 479);
			this._flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this._flowLayoutPanel1.Name = "_flowLayoutPanel1";
			this._flowLayoutPanel1.Size = new System.Drawing.Size(674, 50);
			this._flowLayoutPanel1.TabIndex = 2;
			// 
			// _tableLayoutPanel4
			// 
			this._tableLayoutPanel4.ColumnCount = 4;
			this._tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12F));
			this._tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38F));
			this._tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this._tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
			this._tableLayoutPanel4.Controls.Add(this._newProjectLabel, 0, 0);
			this._tableLayoutPanel4.Controls.Add(this._newProjectBox, 1, 0);
			this._tableLayoutPanel4.Controls.Add(this._signatureLabel, 2, 0);
			this._tableLayoutPanel4.Controls.Add(this._signatureBox, 3, 0);
			this._tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Top;
			this._tableLayoutPanel4.Location = new System.Drawing.Point(0, 60);
			this._tableLayoutPanel4.Name = "_tableLayoutPanel4";
			this._tableLayoutPanel4.RowCount = 1;
			this._tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutPanel4.Size = new System.Drawing.Size(674, 33);
			this._tableLayoutPanel4.TabIndex = 3;
			// 
			// _newProjectBox
			// 
			this._newProjectBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._newProjectBox.Location = new System.Drawing.Point(83, 5);
			this._newProjectBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
			this._newProjectBox.Name = "_newProjectBox";
			this._newProjectBox.Size = new System.Drawing.Size(250, 23);
			this._newProjectBox.TabIndex = 1;
			this._newProjectBox.Tag = "1";
			this._newProjectBox.Leave += new System.EventHandler(this.NewProjectBox_Leave);
			this._newProjectBox.Validating += new System.ComponentModel.CancelEventHandler(this.NewProjectBox_Validating);
			this._newProjectBox.Validated += new System.EventHandler(this.Boxes_Validated);
			// 
			// _signatureBox
			// 
			this._signatureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._signatureBox.Location = new System.Drawing.Point(406, 5);
			this._signatureBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
			this._signatureBox.Name = "_signatureBox";
			this._signatureBox.Size = new System.Drawing.Size(265, 23);
			this._signatureBox.TabIndex = 3;
			this._signatureBox.Tag = "2";
			this._signatureBox.Validated += new System.EventHandler(this.Boxes_Validated);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this._AllChangeButton);
			this.flowLayoutPanel1.Controls.Add(this._PartOnlyButton);
			this.flowLayoutPanel1.Controls.Add(this._drawOnlyButton);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 25);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(674, 35);
			this.flowLayoutPanel1.TabIndex = 4;
			// 
			// _AllChangeButton
			// 
			this._AllChangeButton.Appearance = System.Windows.Forms.Appearance.Button;
			this._AllChangeButton.AutoSize = true;
			this._AllChangeButton.Checked = true;
			this._AllChangeButton.Location = new System.Drawing.Point(3, 3);
			this._AllChangeButton.Name = "_AllChangeButton";
			this._AllChangeButton.Size = new System.Drawing.Size(88, 25);
			this._AllChangeButton.TabIndex = 0;
			this._AllChangeButton.TabStop = true;
			this._AllChangeButton.Text = "パーツ名+図番";
			this._AllChangeButton.UseVisualStyleBackColor = true;
			this._AllChangeButton.CheckedChanged += new System.EventHandler(this.RadioButtons_CheckedChanged);
			// 
			// _PartOnlyButton
			// 
			this._PartOnlyButton.Appearance = System.Windows.Forms.Appearance.Button;
			this._PartOnlyButton.AutoSize = true;
			this._PartOnlyButton.Location = new System.Drawing.Point(97, 3);
			this._PartOnlyButton.Name = "_PartOnlyButton";
			this._PartOnlyButton.Size = new System.Drawing.Size(101, 25);
			this._PartOnlyButton.TabIndex = 1;
			this._PartOnlyButton.Text = "パーツ名変更のみ";
			this._PartOnlyButton.UseVisualStyleBackColor = true;
			this._PartOnlyButton.CheckedChanged += new System.EventHandler(this.RadioButtons_CheckedChanged);
			// 
			// _drawOnlyButton
			// 
			this._drawOnlyButton.Appearance = System.Windows.Forms.Appearance.Button;
			this._drawOnlyButton.AutoSize = true;
			this._drawOnlyButton.Location = new System.Drawing.Point(204, 3);
			this._drawOnlyButton.Name = "_drawOnlyButton";
			this._drawOnlyButton.Size = new System.Drawing.Size(86, 25);
			this._drawOnlyButton.TabIndex = 2;
			this._drawOnlyButton.Text = "図番変更のみ";
			this._drawOnlyButton.UseVisualStyleBackColor = true;
			this._drawOnlyButton.CheckedChanged += new System.EventHandler(this.RadioButtons_CheckedChanged);
			// 
			// MainForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(674, 529);
			this.Controls.Add(this._tableLayoutPanel1);
			this.Controls.Add(this._tableLayoutPanel4);
			this.Controls.Add(this._flowLayoutPanel1);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this._toolStrip);
			this.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "MainForm";
			this.Text = "iCAD Renamer";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.Shown += new System.EventHandler(this.MainForm_Shown);
			this._toolStrip.ResumeLayout(false);
			this._toolStrip.PerformLayout();
			this._tableLayoutPanel1.ResumeLayout(false);
			this._tableLayoutPanel1.PerformLayout();
			this._tableLayoutPanel2.ResumeLayout(false);
			this._tableLayoutPanel2.PerformLayout();
			this._tableLayoutPanel3.ResumeLayout(false);
			this._tableLayoutPanel3.PerformLayout();
			this._flowLayoutPanel1.ResumeLayout(false);
			this._tableLayoutPanel4.ResumeLayout(false);
			this._tableLayoutPanel4.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip _toolStrip;
		private System.Windows.Forms.ToolTip _toolTip;
		private System.Windows.Forms.ToolStripButton _toolStripSettingButton;
		private System.Windows.Forms.ToolStripButton _toolStripEditorButton;
		private System.Windows.Forms.ToolStripButton _toolStripHelpButton;
		private System.Windows.Forms.ToolStripButton _toolStripInformationButton;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel1;
		private System.Windows.Forms.FlowLayoutPanel _flowLayoutPanel1;
		private System.Windows.Forms.Button _closeButton;
		private System.Windows.Forms.Button _executeButton;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel2;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel3;
		private System.Windows.Forms.Button _sourceBrowseButton;
		private System.Windows.Forms.Button _destinationBrowseButton;
		private System.Windows.Forms.TextBox _destinationBox;
		private System.Windows.Forms.TextBox _sourceBox;
		private System.Windows.Forms.ListBox _sourceFileView;
		private System.Windows.Forms.ListBox _destinationFileView;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel4;
		private System.Windows.Forms.Label _newProjectLabel;
		private System.Windows.Forms.TextBox _newProjectBox;
		private System.Windows.Forms.Label _signatureLabel;
		private System.Windows.Forms.TextBox _signatureBox;
		private System.Windows.Forms.Label _sourceLabel;
		private System.Windows.Forms.Label _destinationLabel;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.RadioButton _AllChangeButton;
		private System.Windows.Forms.RadioButton _PartOnlyButton;
		private System.Windows.Forms.RadioButton _drawOnlyButton;
	}
}

