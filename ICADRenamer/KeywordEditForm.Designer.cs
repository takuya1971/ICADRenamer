namespace ICADRenamer
{
	partial class KeywordEditForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeywordEditForm));
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this._helpButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			this._acceptButton = new System.Windows.Forms.Button();
			this._saveButton = new System.Windows.Forms.Button();
			this._deleteButton = new System.Windows.Forms.Button();
			this._addNewButton = new System.Windows.Forms.Button();
			this._groupBox = new System.Windows.Forms.GroupBox();
			this._signatureButton = new System.Windows.Forms.RadioButton();
			this._deltaNoteButton = new System.Windows.Forms.RadioButton();
			this._dateButton = new System.Windows.Forms.RadioButton();
			this._drawNumberButton = new System.Windows.Forms.RadioButton();
			this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this._viewBox = new System.Windows.Forms.ListBox();
			this._toolTip = new System.Windows.Forms.ToolTip(this.components);
			this._commonContextMenuStrip = new ICADRenamer.RegexContextMenuStrip();
			this._drawNumberCategory = new System.Windows.Forms.RadioButton();
			this.flowLayoutPanel1.SuspendLayout();
			this._groupBox.SuspendLayout();
			this._tableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this._helpButton);
			this.flowLayoutPanel1.Controls.Add(this._cancelButton);
			this.flowLayoutPanel1.Controls.Add(this._acceptButton);
			this.flowLayoutPanel1.Controls.Add(this._saveButton);
			this.flowLayoutPanel1.Controls.Add(this._deleteButton);
			this.flowLayoutPanel1.Controls.Add(this._addNewButton);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 276);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(504, 45);
			this.flowLayoutPanel1.TabIndex = 0;
			// 
			// _helpButton
			// 
			this._helpButton.Image = global::ICADRenamer.Properties.Resources.HelpIcon;
			this._helpButton.Location = new System.Drawing.Point(426, 3);
			this._helpButton.Name = "_helpButton";
			this._helpButton.Size = new System.Drawing.Size(75, 40);
			this._helpButton.TabIndex = 5;
			this._toolTip.SetToolTip(this._helpButton, "ヘルプを表示します");
			this._helpButton.UseVisualStyleBackColor = true;
			this._helpButton.Click += new System.EventHandler(this.HelpButton_Click);
			// 
			// _cancelButton
			// 
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Image = global::ICADRenamer.Properties.Resources.CancelIcon;
			this._cancelButton.Location = new System.Drawing.Point(345, 3);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new System.Drawing.Size(75, 40);
			this._cancelButton.TabIndex = 0;
			this._toolTip.SetToolTip(this._cancelButton, "保存しないで終了しします。");
			this._cancelButton.UseVisualStyleBackColor = true;
			this._cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// _acceptButton
			// 
			this._acceptButton.Image = global::ICADRenamer.Properties.Resources.OKIcon;
			this._acceptButton.Location = new System.Drawing.Point(264, 3);
			this._acceptButton.Name = "_acceptButton";
			this._acceptButton.Size = new System.Drawing.Size(75, 40);
			this._acceptButton.TabIndex = 1;
			this._toolTip.SetToolTip(this._acceptButton, "保存して終了します。");
			this._acceptButton.UseVisualStyleBackColor = true;
			this._acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
			// 
			// _saveButton
			// 
			this._saveButton.Image = global::ICADRenamer.Properties.Resources.SaveIcon;
			this._saveButton.Location = new System.Drawing.Point(183, 3);
			this._saveButton.Name = "_saveButton";
			this._saveButton.Size = new System.Drawing.Size(75, 40);
			this._saveButton.TabIndex = 2;
			this._toolTip.SetToolTip(this._saveButton, "内容を保存します。");
			this._saveButton.UseVisualStyleBackColor = true;
			this._saveButton.Click += new System.EventHandler(this.SaveButton_Click);
			// 
			// _deleteButton
			// 
			this._deleteButton.Enabled = false;
			this._deleteButton.Image = global::ICADRenamer.Properties.Resources.DeleteIcon;
			this._deleteButton.Location = new System.Drawing.Point(102, 3);
			this._deleteButton.Name = "_deleteButton";
			this._deleteButton.Size = new System.Drawing.Size(75, 40);
			this._deleteButton.TabIndex = 4;
			this._toolTip.SetToolTip(this._deleteButton, "選択している項目を削除します。\r\n項目が選択していないときは押せません。");
			this._deleteButton.UseVisualStyleBackColor = true;
			this._deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
			// 
			// _addNewButton
			// 
			this._addNewButton.Image = global::ICADRenamer.Properties.Resources.AddNewIcon;
			this._addNewButton.Location = new System.Drawing.Point(21, 3);
			this._addNewButton.Name = "_addNewButton";
			this._addNewButton.Size = new System.Drawing.Size(75, 40);
			this._addNewButton.TabIndex = 3;
			this._toolTip.SetToolTip(this._addNewButton, "新しい規則を作成します。");
			this._addNewButton.UseVisualStyleBackColor = true;
			this._addNewButton.Click += new System.EventHandler(this.AddNewButton_Click);
			// 
			// _groupBox
			// 
			this._groupBox.Controls.Add(this._drawNumberCategory);
			this._groupBox.Controls.Add(this._signatureButton);
			this._groupBox.Controls.Add(this._deltaNoteButton);
			this._groupBox.Controls.Add(this._dateButton);
			this._groupBox.Controls.Add(this._drawNumberButton);
			this._groupBox.Location = new System.Drawing.Point(3, 3);
			this._groupBox.Name = "_groupBox";
			this._groupBox.Size = new System.Drawing.Size(130, 147);
			this._groupBox.TabIndex = 1;
			this._groupBox.TabStop = false;
			this._groupBox.Text = "編集する検索規則";
			// 
			// _signatureButton
			// 
			this._signatureButton.AutoSize = true;
			this._signatureButton.Location = new System.Drawing.Point(6, 99);
			this._signatureButton.Name = "_signatureButton";
			this._signatureButton.Size = new System.Drawing.Size(97, 19);
			this._signatureButton.TabIndex = 3;
			this._signatureButton.Text = "署名検索規則";
			this._toolTip.SetToolTip(this._signatureButton, "署名の検索規則です。\r\n検索する人の名前で、完全一致です。");
			this._signatureButton.UseVisualStyleBackColor = true;
			this._signatureButton.EnabledChanged += new System.EventHandler(this.RadioButtons_CheckedChanged);
			// 
			// _deltaNoteButton
			// 
			this._deltaNoteButton.AutoSize = true;
			this._deltaNoteButton.Location = new System.Drawing.Point(6, 73);
			this._deltaNoteButton.Name = "_deltaNoteButton";
			this._deltaNoteButton.Size = new System.Drawing.Size(121, 19);
			this._deltaNoteButton.TabIndex = 2;
			this._deltaNoteButton.Text = "改訂注記検索規則";
			this._toolTip.SetToolTip(this._deltaNoteButton, "改訂注記の検索規則です。\r\n後方一致で、署名規則と組み合わせて判定します。");
			this._deltaNoteButton.UseVisualStyleBackColor = true;
			this._deltaNoteButton.CheckedChanged += new System.EventHandler(this.RadioButtons_CheckedChanged);
			// 
			// _dateButton
			// 
			this._dateButton.AutoSize = true;
			this._dateButton.Location = new System.Drawing.Point(6, 48);
			this._dateButton.Name = "_dateButton";
			this._dateButton.Size = new System.Drawing.Size(97, 19);
			this._dateButton.TabIndex = 1;
			this._dateButton.Text = "日付検索規則";
			this._toolTip.SetToolTip(this._dateButton, "日付の検索規則です。\r\n完全一致です。");
			this._dateButton.UseVisualStyleBackColor = true;
			this._dateButton.CheckedChanged += new System.EventHandler(this.RadioButtons_CheckedChanged);
			// 
			// _drawNumberButton
			// 
			this._drawNumberButton.AutoSize = true;
			this._drawNumberButton.Checked = true;
			this._drawNumberButton.Location = new System.Drawing.Point(6, 22);
			this._drawNumberButton.Name = "_drawNumberButton";
			this._drawNumberButton.Size = new System.Drawing.Size(97, 19);
			this._drawNumberButton.TabIndex = 0;
			this._drawNumberButton.TabStop = true;
			this._drawNumberButton.Text = "図番検索規則";
			this._toolTip.SetToolTip(this._drawNumberButton, "図番の検索規則です。\r\n前方一致になります。");
			this._drawNumberButton.UseVisualStyleBackColor = true;
			this._drawNumberButton.CheckedChanged += new System.EventHandler(this.RadioButtons_CheckedChanged);
			// 
			// _tableLayoutPanel
			// 
			this._tableLayoutPanel.ColumnCount = 2;
			this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
			this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel.Controls.Add(this._groupBox, 0, 0);
			this._tableLayoutPanel.Controls.Add(this._viewBox, 1, 0);
			this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this._tableLayoutPanel.Name = "_tableLayoutPanel";
			this._tableLayoutPanel.RowCount = 1;
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel.Size = new System.Drawing.Size(504, 276);
			this._tableLayoutPanel.TabIndex = 2;
			// 
			// _viewBox
			// 
			this._viewBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._viewBox.FormattingEnabled = true;
			this._viewBox.ItemHeight = 15;
			this._viewBox.Location = new System.Drawing.Point(153, 3);
			this._viewBox.Name = "_viewBox";
			this._viewBox.Size = new System.Drawing.Size(348, 270);
			this._viewBox.TabIndex = 2;
			this._toolTip.SetToolTip(this._viewBox, "研削規則のリストが表示されます");
			this._viewBox.SelectedIndexChanged += new System.EventHandler(this.ViewBox_SelectedIndexChanged);
			this._viewBox.DoubleClick += new System.EventHandler(this.ViewBox_DoubleClicked);
			this._viewBox.Leave += new System.EventHandler(this.ViewBox_Leave);
			// 
			// _commonContextMenuStrip
			// 
			this._commonContextMenuStrip.Name = "commonContextMenuStrip";
			this._commonContextMenuStrip.Size = new System.Drawing.Size(140, 70);
			this._commonContextMenuStrip.AddNewRequest += new System.EventHandler(this.CommonContextMenuStrip_AddNewRequest);
			this._commonContextMenuStrip.DeleteRequest += new System.EventHandler(this.CommonContextMenuStrip_DeleteRequest);
			this._commonContextMenuStrip.EditRequest += new System.EventHandler(this.CommonContextMenuStrip_EditRequest);
			this._commonContextMenuStrip.Opened += new System.EventHandler(this.CommonContextMenuStrip_Opened);
			// 
			// _drawNumberCategory
			// 
			this._drawNumberCategory.AutoSize = true;
			this._drawNumberCategory.Location = new System.Drawing.Point(6, 122);
			this._drawNumberCategory.Name = "_drawNumberCategory";
			this._drawNumberCategory.Size = new System.Drawing.Size(97, 19);
			this._drawNumberCategory.TabIndex = 4;
			this._drawNumberCategory.Text = "署名検索規則";
			this._toolTip.SetToolTip(this._drawNumberCategory, "図番の区切です");
			this._drawNumberCategory.UseVisualStyleBackColor = true;
			// 
			// KeywordEditForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this._cancelButton;
			this.ClientSize = new System.Drawing.Size(504, 321);
			this.ContextMenuStrip = this._commonContextMenuStrip;
			this.Controls.Add(this._tableLayoutPanel);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "KeywordEditForm";
			this.Text = "図面検索キーワード編集";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.KeywordEditForm_Load);
			this.Shown += new System.EventHandler(this.KeywordEditForm_Shown);
			this.flowLayoutPanel1.ResumeLayout(false);
			this._groupBox.ResumeLayout(false);
			this._groupBox.PerformLayout();
			this._tableLayoutPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.Button _acceptButton;
		private System.Windows.Forms.GroupBox _groupBox;
		private System.Windows.Forms.RadioButton _signatureButton;
		private System.Windows.Forms.RadioButton _deltaNoteButton;
		private System.Windows.Forms.RadioButton _dateButton;
		private System.Windows.Forms.RadioButton _drawNumberButton;
		private System.Windows.Forms.Button _saveButton;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
		private System.Windows.Forms.ListBox _viewBox;
		private System.Windows.Forms.Button _addNewButton;
		private System.Windows.Forms.Button _deleteButton;
		private System.Windows.Forms.ToolTip _toolTip;
		private RegexContextMenuStrip _commonContextMenuStrip;
		private System.Windows.Forms.Button _helpButton;
		private System.Windows.Forms.RadioButton _drawNumberCategory;
	}
}