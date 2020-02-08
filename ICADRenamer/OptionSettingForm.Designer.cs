namespace ICADRenamer
{
	partial class OptionSettingForm
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionSettingForm));
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this._helpButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			this._acceptButton = new System.Windows.Forms.Button();
			this._deleteButton = new System.Windows.Forms.Button();
			this._addNewButton = new System.Windows.Forms.Button();
			this._isDeleteDelta = new System.Windows.Forms.CheckBox();
			this._isUpdate = new System.Windows.Forms.CheckBox();
			this._isValidateProject = new System.Windows.Forms.CheckBox();
			this._iCADLinkBox = new System.Windows.Forms.TextBox();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this._isIcadMinimize = new System.Windows.Forms.CheckBox();
			this._resit3dSeihinBox = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this._isDateSeparatorSlashBox = new System.Windows.Forms.CheckBox();
			this._isMonthAndDate2DigitBox = new System.Windows.Forms.CheckBox();
			this._isYear4digitBox = new System.Windows.Forms.CheckBox();
			this._icadLinkLabel = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this._userLabel = new System.Windows.Forms.Label();
			this._userBox = new System.Windows.Forms.TextBox();
			this._defaultFolderLabel = new System.Windows.Forms.Label();
			this._defaultFolderBox = new System.Windows.Forms.TextBox();
			this._folderBrowseButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this._toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.label2 = new System.Windows.Forms.Label();
			this._regexBox = new System.Windows.Forms.ListBox();
			this._tabControl1 = new System.Windows.Forms.TabControl();
			this._generalPage = new System.Windows.Forms.TabPage();
			this._regexPage = new System.Windows.Forms.TabPage();
			this._regexContextMenuStrip = new ICADRenamer.RegexContextMenuStrip();
			this.flowLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this._tabControl1.SuspendLayout();
			this._generalPage.SuspendLayout();
			this._regexPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this._helpButton);
			this.flowLayoutPanel1.Controls.Add(this._cancelButton);
			this.flowLayoutPanel1.Controls.Add(this._acceptButton);
			this.flowLayoutPanel1.Controls.Add(this._deleteButton);
			this.flowLayoutPanel1.Controls.Add(this._addNewButton);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 339);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(434, 45);
			this.flowLayoutPanel1.TabIndex = 0;
			// 
			// _helpButton
			// 
			this._helpButton.Image = global::ICADRenamer.Properties.Resources.HelpIcon;
			this._helpButton.Location = new System.Drawing.Point(356, 3);
			this._helpButton.Name = "_helpButton";
			this._helpButton.Size = new System.Drawing.Size(75, 40);
			this._helpButton.TabIndex = 4;
			this._toolTip.SetToolTip(this._helpButton, "ヘルプを表示します。");
			this._helpButton.UseVisualStyleBackColor = true;
			this._helpButton.Click += new System.EventHandler(this.HelpButton_Click);
			// 
			// _cancelButton
			// 
			this._cancelButton.Image = global::ICADRenamer.Properties.Resources.CancelIcon;
			this._cancelButton.Location = new System.Drawing.Point(275, 3);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new System.Drawing.Size(75, 40);
			this._cancelButton.TabIndex = 0;
			this._toolTip.SetToolTip(this._cancelButton, "変更を反映せず終了します。");
			this._cancelButton.UseVisualStyleBackColor = true;
			this._cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// _acceptButton
			// 
			this._acceptButton.Image = global::ICADRenamer.Properties.Resources.OKIcon;
			this._acceptButton.Location = new System.Drawing.Point(194, 3);
			this._acceptButton.Name = "_acceptButton";
			this._acceptButton.Size = new System.Drawing.Size(75, 40);
			this._acceptButton.TabIndex = 1;
			this._toolTip.SetToolTip(this._acceptButton, "変更を反映して終了します。");
			this._acceptButton.UseVisualStyleBackColor = true;
			this._acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
			// 
			// _deleteButton
			// 
			this._deleteButton.Image = global::ICADRenamer.Properties.Resources.DeleteIcon;
			this._deleteButton.Location = new System.Drawing.Point(113, 3);
			this._deleteButton.Name = "_deleteButton";
			this._deleteButton.Size = new System.Drawing.Size(75, 40);
			this._deleteButton.TabIndex = 3;
			this._toolTip.SetToolTip(this._deleteButton, "作成規則を削除します。");
			this._deleteButton.UseVisualStyleBackColor = true;
			this._deleteButton.Visible = false;
			this._deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
			// 
			// _addNewButton
			// 
			this._addNewButton.Image = global::ICADRenamer.Properties.Resources.AddNewIcon;
			this._addNewButton.Location = new System.Drawing.Point(32, 3);
			this._addNewButton.Name = "_addNewButton";
			this._addNewButton.Size = new System.Drawing.Size(75, 40);
			this._addNewButton.TabIndex = 2;
			this._toolTip.SetToolTip(this._addNewButton, "新規入力規則を作成します。");
			this._addNewButton.UseVisualStyleBackColor = true;
			this._addNewButton.Visible = false;
			this._addNewButton.Click += new System.EventHandler(this.AddNewButton_Click);
			// 
			// _isDeleteDelta
			// 
			this._isDeleteDelta.AutoSize = true;
			this._isDeleteDelta.Location = new System.Drawing.Point(3, 3);
			this._isDeleteDelta.Name = "_isDeleteDelta";
			this._isDeleteDelta.Size = new System.Drawing.Size(132, 19);
			this._isDeleteDelta.TabIndex = 1;
			this._isDeleteDelta.Text = "デルタマークを削除する";
			this._toolTip.SetToolTip(this._isDeleteDelta, "図面修正時にデルタマークを削除するか指定します。");
			this._isDeleteDelta.UseVisualStyleBackColor = true;
			this._isDeleteDelta.CheckedChanged += new System.EventHandler(this.Boxes_CheckedChanged);
			// 
			// _isUpdate
			// 
			this._isUpdate.AutoSize = true;
			this._isUpdate.Location = new System.Drawing.Point(141, 3);
			this._isUpdate.Name = "_isUpdate";
			this._isUpdate.Size = new System.Drawing.Size(165, 19);
			this._isUpdate.TabIndex = 2;
			this._isUpdate.Text = "パーツ変更後更新処理をする";
			this._toolTip.SetToolTip(this._isUpdate, "パーツ変更後の更新処理を実施するか指定します。");
			this._isUpdate.UseVisualStyleBackColor = true;
			this._isUpdate.CheckStateChanged += new System.EventHandler(this.Boxes_CheckedChanged);
			// 
			// _isValidateProject
			// 
			this._isValidateProject.AutoSize = true;
			this._isValidateProject.Location = new System.Drawing.Point(3, 28);
			this._isValidateProject.Name = "_isValidateProject";
			this._isValidateProject.Size = new System.Drawing.Size(135, 19);
			this._isValidateProject.TabIndex = 3;
			this._isValidateProject.Text = "M番の入力制限をする";
			this._toolTip.SetToolTip(this._isValidateProject, "新規M番に入力規則を適用するか指定します。");
			this._isValidateProject.UseVisualStyleBackColor = true;
			this._isValidateProject.CheckStateChanged += new System.EventHandler(this.Boxes_CheckedChanged);
			// 
			// _iCADLinkBox
			// 
			this.tableLayoutPanel1.SetColumnSpan(this._iCADLinkBox, 2);
			this._iCADLinkBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._iCADLinkBox.Location = new System.Drawing.Point(126, 7);
			this._iCADLinkBox.Margin = new System.Windows.Forms.Padding(3, 7, 3, 3);
			this._iCADLinkBox.Name = "_iCADLinkBox";
			this._iCADLinkBox.Size = new System.Drawing.Size(291, 23);
			this._iCADLinkBox.TabIndex = 4;
			this._iCADLinkBox.Tag = "1";
			this._iCADLinkBox.Validating += new System.ComponentModel.CancelEventHandler(this.ICADLinkBox_Validating);
			this._iCADLinkBox.Validated += new System.EventHandler(this.Boxes_Validated);
			// 
			// flowLayoutPanel2
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel2, 3);
			this.flowLayoutPanel2.Controls.Add(this._isDeleteDelta);
			this.flowLayoutPanel2.Controls.Add(this._isUpdate);
			this.flowLayoutPanel2.Controls.Add(this._isValidateProject);
			this.flowLayoutPanel2.Controls.Add(this._isIcadMinimize);
			this.flowLayoutPanel2.Controls.Add(this._resit3dSeihinBox);
			this.flowLayoutPanel2.Controls.Add(this.groupBox1);
			this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 140);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(420, 165);
			this.flowLayoutPanel2.TabIndex = 5;
			// 
			// _isIcadMinimize
			// 
			this._isIcadMinimize.AutoSize = true;
			this._isIcadMinimize.Location = new System.Drawing.Point(144, 28);
			this._isIcadMinimize.Name = "_isIcadMinimize";
			this._isIcadMinimize.Size = new System.Drawing.Size(152, 19);
			this._isIcadMinimize.TabIndex = 4;
			this._isIcadMinimize.Text = "ICAD起動時に最小化する";
			this._toolTip.SetToolTip(this._isIcadMinimize, "ICADを最小化するか指定します");
			this._isIcadMinimize.UseVisualStyleBackColor = true;
			// 
			// _resit3dSeihinBox
			// 
			this._resit3dSeihinBox.AutoSize = true;
			this._resit3dSeihinBox.Location = new System.Drawing.Point(3, 53);
			this._resit3dSeihinBox.Name = "_resit3dSeihinBox";
			this._resit3dSeihinBox.Size = new System.Drawing.Size(151, 19);
			this._resit3dSeihinBox.TabIndex = 7;
			this._resit3dSeihinBox.Text = "3D製品フォルダに登録する";
			this._toolTip.SetToolTip(this._resit3dSeihinBox, "3D製品フォルダに登録します");
			this._resit3dSeihinBox.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this._isDateSeparatorSlashBox);
			this.groupBox1.Controls.Add(this._isMonthAndDate2DigitBox);
			this.groupBox1.Controls.Add(this._isYear4digitBox);
			this.groupBox1.Location = new System.Drawing.Point(160, 53);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(149, 100);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "日付の表示";
			// 
			// _isDateSeparatorSlashBox
			// 
			this._isDateSeparatorSlashBox.AutoSize = true;
			this._isDateSeparatorSlashBox.Location = new System.Drawing.Point(6, 73);
			this._isDateSeparatorSlashBox.Name = "_isDateSeparatorSlashBox";
			this._isDateSeparatorSlashBox.Size = new System.Drawing.Size(121, 19);
			this._isDateSeparatorSlashBox.TabIndex = 7;
			this._isDateSeparatorSlashBox.Text = "年月日区切りは「/」";
			this._toolTip.SetToolTip(this._isDateSeparatorSlashBox, "年月日の区切を「/」（スラッシュ）にするか選択します。\r\nチェックすると「/」になり、チェックをはずすと、「.」になります。\r\nチェックありのときは、2020年1" +
        "0月10日は、「2020/10/10」にチェックなしのときは、「2020.10.10」になります。");
			this._isDateSeparatorSlashBox.UseVisualStyleBackColor = true;
			// 
			// _isMonthAndDate2DigitBox
			// 
			this._isMonthAndDate2DigitBox.AutoSize = true;
			this._isMonthAndDate2DigitBox.Location = new System.Drawing.Point(6, 48);
			this._isMonthAndDate2DigitBox.Name = "_isMonthAndDate2DigitBox";
			this._isMonthAndDate2DigitBox.Size = new System.Drawing.Size(136, 19);
			this._isMonthAndDate2DigitBox.TabIndex = 6;
			this._isMonthAndDate2DigitBox.Text = "日付の月日表示は2桁";
			this._toolTip.SetToolTip(this._isMonthAndDate2DigitBox, "日付の月と日の表示が1桁のとき、前にゼロを入れるかどうか選択します。\r\nチェックすると、3月1日は「03/01」と表示されます。");
			this._isMonthAndDate2DigitBox.UseVisualStyleBackColor = true;
			// 
			// _isYear4digitBox
			// 
			this._isYear4digitBox.AutoSize = true;
			this._isYear4digitBox.Location = new System.Drawing.Point(6, 22);
			this._isYear4digitBox.Name = "_isYear4digitBox";
			this._isYear4digitBox.Size = new System.Drawing.Size(124, 19);
			this._isYear4digitBox.TabIndex = 5;
			this._isYear4digitBox.Text = "日付の年表示は4桁";
			this._toolTip.SetToolTip(this._isYear4digitBox, "図面の年の表示を4桁にするか2桁にするか選択します。\r\nチェックすると4桁になります。");
			this._isYear4digitBox.UseVisualStyleBackColor = true;
			// 
			// _icadLinkLabel
			// 
			this._icadLinkLabel.AutoSize = true;
			this._icadLinkLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._icadLinkLabel.Location = new System.Drawing.Point(3, 0);
			this._icadLinkLabel.Name = "_icadLinkLabel";
			this._icadLinkLabel.Size = new System.Drawing.Size(117, 35);
			this._icadLinkLabel.TabIndex = 4;
			this._icadLinkLabel.Tag = "1";
			this._icadLinkLabel.Text = "ICAD連携ポート";
			this._icadLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._toolTip.SetToolTip(this._icadLinkLabel, "ICADとの通信ポートを設定します。\r\n通常は既定値のまま使用します。\r\n変更したときはICAD側の設定変更が必要です。");
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.66667F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel1.Controls.Add(this._userLabel, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this._userBox, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this._defaultFolderLabel, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this._defaultFolderBox, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this._icadLinkLabel, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this._iCADLinkBox, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this._folderBrowseButton, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.textBox1, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.label3, 2, 3);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 5;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(420, 305);
			this.tableLayoutPanel1.TabIndex = 6;
			// 
			// _userLabel
			// 
			this._userLabel.AutoSize = true;
			this._userLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._userLabel.Location = new System.Drawing.Point(3, 70);
			this._userLabel.Name = "_userLabel";
			this._userLabel.Size = new System.Drawing.Size(117, 35);
			this._userLabel.TabIndex = 8;
			this._userLabel.Tag = "3";
			this._userLabel.Text = "ユーザー";
			this._userLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._toolTip.SetToolTip(this._userLabel, "あなたの名前を入力します。\r\n署名の既定値として使用されます。");
			// 
			// _userBox
			// 
			this.tableLayoutPanel1.SetColumnSpan(this._userBox, 2);
			this._userBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._userBox.Location = new System.Drawing.Point(126, 77);
			this._userBox.Margin = new System.Windows.Forms.Padding(3, 7, 3, 3);
			this._userBox.Name = "_userBox";
			this._userBox.Size = new System.Drawing.Size(291, 23);
			this._userBox.TabIndex = 9;
			this._userBox.Tag = "3";
			this._userBox.Validated += new System.EventHandler(this.Boxes_Validated);
			// 
			// _defaultFolderLabel
			// 
			this._defaultFolderLabel.AutoSize = true;
			this._defaultFolderLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._defaultFolderLabel.Location = new System.Drawing.Point(3, 35);
			this._defaultFolderLabel.Name = "_defaultFolderLabel";
			this._defaultFolderLabel.Size = new System.Drawing.Size(117, 35);
			this._defaultFolderLabel.TabIndex = 6;
			this._defaultFolderLabel.Tag = "2";
			this._defaultFolderLabel.Text = "初期フォルダ";
			this._defaultFolderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._toolTip.SetToolTip(this._defaultFolderLabel, "コピー元フォルダを検索するときの既定のフォルダを指定します。");
			// 
			// _defaultFolderBox
			// 
			this._defaultFolderBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._defaultFolderBox.Location = new System.Drawing.Point(126, 38);
			this._defaultFolderBox.Multiline = true;
			this._defaultFolderBox.Name = "_defaultFolderBox";
			this._defaultFolderBox.Size = new System.Drawing.Size(240, 29);
			this._defaultFolderBox.TabIndex = 7;
			this._defaultFolderBox.Tag = "2";
			this._defaultFolderBox.Validated += new System.EventHandler(this.Boxes_Validated);
			// 
			// _folderBrowseButton
			// 
			this._folderBrowseButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this._folderBrowseButton.Image = global::ICADRenamer.Properties.Resources.FolderBrowseIcon;
			this._folderBrowseButton.Location = new System.Drawing.Point(372, 38);
			this._folderBrowseButton.Name = "_folderBrowseButton";
			this._folderBrowseButton.Size = new System.Drawing.Size(45, 29);
			this._folderBrowseButton.TabIndex = 10;
			this._folderBrowseButton.Tag = "2";
			this._toolTip.SetToolTip(this._folderBrowseButton, "フォルダをダイアログで指定します。");
			this._folderBrowseButton.UseVisualStyleBackColor = true;
			this._folderBrowseButton.Click += new System.EventHandler(this.FolderBrowseButton_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(3, 105);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(117, 35);
			this.label1.TabIndex = 11;
			this.label1.Text = "ICAD再起動閾値";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._toolTip.SetToolTip(this.label1, "ICADが再起動する閾値を入力します。\r\n通常は既定値のままにしてください。");
			// 
			// textBox1
			// 
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Location = new System.Drawing.Point(126, 108);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(240, 23);
			this.textBox1.TabIndex = 12;
			this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label3.Location = new System.Drawing.Point(372, 105);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(45, 35);
			this.label3.TabIndex = 13;
			this.label3.Text = "MB";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 3);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(113, 15);
			this.label2.TabIndex = 1;
			this.label2.Text = "工事番号の入力規則";
			this._toolTip.SetToolTip(this.label2, "入力規則を正規表現で入力します。");
			// 
			// _regexBox
			// 
			this._regexBox.Dock = System.Windows.Forms.DockStyle.Right;
			this._regexBox.FormattingEnabled = true;
			this._regexBox.ItemHeight = 15;
			this._regexBox.Location = new System.Drawing.Point(240, 3);
			this._regexBox.Name = "_regexBox";
			this._regexBox.Size = new System.Drawing.Size(183, 307);
			this._regexBox.TabIndex = 0;
			this._regexBox.Tag = "4";
			this._toolTip.SetToolTip(this._regexBox, "入力規則のリストを表示します。");
			this._regexBox.SelectedIndexChanged += new System.EventHandler(this.RegexBox_SelectedIndexChanged);
			this._regexBox.DoubleClick += new System.EventHandler(this.RegexBox_DoubleClick);
			// 
			// _tabControl1
			// 
			this._tabControl1.Controls.Add(this._generalPage);
			this._tabControl1.Controls.Add(this._regexPage);
			this._tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tabControl1.Location = new System.Drawing.Point(0, 0);
			this._tabControl1.Name = "_tabControl1";
			this._tabControl1.SelectedIndex = 0;
			this._tabControl1.Size = new System.Drawing.Size(434, 339);
			this._tabControl1.TabIndex = 7;
			// 
			// _generalPage
			// 
			this._generalPage.Controls.Add(this.tableLayoutPanel1);
			this._generalPage.Location = new System.Drawing.Point(4, 24);
			this._generalPage.Name = "_generalPage";
			this._generalPage.Padding = new System.Windows.Forms.Padding(3);
			this._generalPage.Size = new System.Drawing.Size(426, 311);
			this._generalPage.TabIndex = 0;
			this._generalPage.Text = "全般";
			this._generalPage.UseVisualStyleBackColor = true;
			this._generalPage.Enter += new System.EventHandler(this.GeneralPage_Enter);
			// 
			// _regexPage
			// 
			this._regexPage.Controls.Add(this.label2);
			this._regexPage.Controls.Add(this._regexBox);
			this._regexPage.Location = new System.Drawing.Point(4, 22);
			this._regexPage.Name = "_regexPage";
			this._regexPage.Padding = new System.Windows.Forms.Padding(3);
			this._regexPage.Size = new System.Drawing.Size(426, 313);
			this._regexPage.TabIndex = 1;
			this._regexPage.Tag = "4";
			this._regexPage.Text = "入力規則";
			this._regexPage.UseVisualStyleBackColor = true;
			this._regexPage.Enter += new System.EventHandler(this.RegexPage_Enter);
			// 
			// _regexContextMenuStrip
			// 
			this._regexContextMenuStrip.Name = "_regexContextMenuStrip";
			this._regexContextMenuStrip.Size = new System.Drawing.Size(140, 70);
			this._regexContextMenuStrip.AddNewRequest += new System.EventHandler(this.RegexContextMenuStrip_AddNewRequest);
			this._regexContextMenuStrip.DeleteRequest += new System.EventHandler(this.RegexContextMenuStrip_DeleteRequest);
			this._regexContextMenuStrip.EditRequest += new System.EventHandler(this.RegexContextMenuStrip_EditRequest);
			// 
			// OptionSettingForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(434, 384);
			this.Controls.Add(this._tabControl1);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptionSettingForm";
			this.Text = "オプション設定";
			this.TopMost = true;
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OptionSettingForm_FormClosed);
			this.Load += new System.EventHandler(this.SettingForm_Load);
			this.Shown += new System.EventHandler(this.OptionSettingForm_Shown);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this._tabControl1.ResumeLayout(false);
			this._generalPage.ResumeLayout(false);
			this._regexPage.ResumeLayout(false);
			this._regexPage.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.CheckBox _isDeleteDelta;
		private System.Windows.Forms.CheckBox _isUpdate;
		private System.Windows.Forms.CheckBox _isValidateProject;
		private System.Windows.Forms.TextBox _iCADLinkBox;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Label _icadLinkLabel;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label _userLabel;
		private System.Windows.Forms.TextBox _userBox;
		private System.Windows.Forms.Label _defaultFolderLabel;
		private System.Windows.Forms.TextBox _defaultFolderBox;
		private System.Windows.Forms.Button _folderBrowseButton;
		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.Button _acceptButton;
		private System.Windows.Forms.ToolTip _toolTip;
		private System.Windows.Forms.TabControl _tabControl1;
		private System.Windows.Forms.TabPage _generalPage;
		private System.Windows.Forms.TabPage _regexPage;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListBox _regexBox;
		private System.Windows.Forms.Button _addNewButton;
		private System.Windows.Forms.Button _deleteButton;
		private System.Windows.Forms.Button _helpButton;
		private System.Windows.Forms.CheckBox _isIcadMinimize;
		private RegexContextMenuStrip _regexContextMenuStrip;
		private System.Windows.Forms.CheckBox _isYear4digitBox;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox _isDateSeparatorSlashBox;
		private System.Windows.Forms.CheckBox _isMonthAndDate2DigitBox;
		private System.Windows.Forms.CheckBox _resit3dSeihinBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label3;
	}
}