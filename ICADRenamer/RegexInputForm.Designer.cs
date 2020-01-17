namespace ICADRenamer
{
	partial class RegexInputForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegexInputForm));
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this._helpButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			this._acceptButton = new System.Windows.Forms.Button();
			this._testButton = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this._resultBox = new System.Windows.Forms.TextBox();
			this._replaceBox = new System.Windows.Forms.TextBox();
			this._inputBox = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this._regexBox = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this._methodComboBox = new System.Windows.Forms.ComboBox();
			this._toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.flowLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this._helpButton);
			this.flowLayoutPanel1.Controls.Add(this._cancelButton);
			this.flowLayoutPanel1.Controls.Add(this._acceptButton);
			this.flowLayoutPanel1.Controls.Add(this._testButton);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 142);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(384, 45);
			this.flowLayoutPanel1.TabIndex = 0;
			// 
			// _helpButton
			// 
			this._helpButton.Image = global::ICADRenamer.Properties.Resources.HelpIcon;
			this._helpButton.Location = new System.Drawing.Point(331, 3);
			this._helpButton.Name = "_helpButton";
			this._helpButton.Size = new System.Drawing.Size(50, 40);
			this._helpButton.TabIndex = 9;
			this._toolTip.SetToolTip(this._helpButton, "ヘルプを表示します");
			this._helpButton.UseVisualStyleBackColor = true;
			this._helpButton.Click += new System.EventHandler(this.HelpButton_Click);
			// 
			// _cancelButton
			// 
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Image = global::ICADRenamer.Properties.Resources.CancelIcon;
			this._cancelButton.Location = new System.Drawing.Point(250, 3);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new System.Drawing.Size(75, 40);
			this._cancelButton.TabIndex = 8;
			this._toolTip.SetToolTip(this._cancelButton, "このダイアログを閉じます。");
			this._cancelButton.UseVisualStyleBackColor = true;
			this._cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// _acceptButton
			// 
			this._acceptButton.Image = global::ICADRenamer.Properties.Resources.OKIcon;
			this._acceptButton.Location = new System.Drawing.Point(169, 3);
			this._acceptButton.Name = "_acceptButton";
			this._acceptButton.Size = new System.Drawing.Size(75, 40);
			this._acceptButton.TabIndex = 7;
			this._toolTip.SetToolTip(this._acceptButton, "検索規則がOKのとき押すと内容が元フォーム反映されます。");
			this._acceptButton.UseVisualStyleBackColor = true;
			this._acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
			// 
			// _testButton
			// 
			this._testButton.Image = global::ICADRenamer.Properties.Resources.TestIcon;
			this._testButton.Location = new System.Drawing.Point(88, 3);
			this._testButton.Name = "_testButton";
			this._testButton.Size = new System.Drawing.Size(75, 40);
			this._testButton.TabIndex = 6;
			this._toolTip.SetToolTip(this._testButton, "テストします。\r\n灰色の時は実行できません。");
			this._testButton.UseVisualStyleBackColor = true;
			this._testButton.Click += new System.EventHandler(this.TestButton_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.82292F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 73.17708F));
			this.tableLayoutPanel1.Controls.Add(this._resultBox, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this._replaceBox, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this._inputBox, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.label4, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this._regexBox, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.label5, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this._methodComboBox, 1, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 5;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(384, 142);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// _resultBox
			// 
			this._resultBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._resultBox.Location = new System.Drawing.Point(106, 115);
			this._resultBox.Name = "_resultBox";
			this._resultBox.ReadOnly = true;
			this._resultBox.Size = new System.Drawing.Size(275, 23);
			this._resultBox.TabIndex = 5;
			this._resultBox.TabStop = false;
			this._resultBox.Tag = "5";
			// 
			// _replaceBox
			// 
			this._replaceBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._replaceBox.Location = new System.Drawing.Point(106, 87);
			this._replaceBox.Name = "_replaceBox";
			this._replaceBox.Size = new System.Drawing.Size(275, 23);
			this._replaceBox.TabIndex = 4;
			this._replaceBox.Tag = "4";
			this._replaceBox.Leave += new System.EventHandler(this.TextBoxes_Leave);
			this._replaceBox.Validated += new System.EventHandler(this.Boxes_Validated);
			// 
			// _inputBox
			// 
			this._inputBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._inputBox.Location = new System.Drawing.Point(106, 59);
			this._inputBox.Name = "_inputBox";
			this._inputBox.Size = new System.Drawing.Size(275, 23);
			this._inputBox.TabIndex = 3;
			this._inputBox.Tag = "3";
			this._inputBox.Leave += new System.EventHandler(this.TextBoxes_Leave);
			this._inputBox.Validated += new System.EventHandler(this.Boxes_Validated);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label4.Location = new System.Drawing.Point(3, 112);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(97, 30);
			this.label4.TabIndex = 3;
			this.label4.Tag = "5";
			this.label4.Text = "テスト結果";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._toolTip.SetToolTip(this.label4, "テスト結果が表示されます。");
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label3.Location = new System.Drawing.Point(3, 84);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(97, 28);
			this.label3.TabIndex = 2;
			this.label3.Tag = "4";
			this.label3.Text = "置換文字列";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._toolTip.SetToolTip(this.label3, "置き換えテストをするとき、置き換える文字列を入力します。");
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Location = new System.Drawing.Point(3, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(97, 28);
			this.label2.TabIndex = 1;
			this.label2.Tag = "3";
			this.label2.Text = "入力文字列";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._toolTip.SetToolTip(this.label2, "置き換えテストをするとき、元になる文字列を入力します。");
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(97, 28);
			this.label1.TabIndex = 0;
			this.label1.Tag = "1";
			this.label1.Text = "検索規則";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._toolTip.SetToolTip(this.label1, "検索規則を入力します。\r\n規則は正規表現で表します。");
			// 
			// _regexBox
			// 
			this._regexBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._regexBox.Location = new System.Drawing.Point(106, 3);
			this._regexBox.Name = "_regexBox";
			this._regexBox.Size = new System.Drawing.Size(275, 23);
			this._regexBox.TabIndex = 1;
			this._regexBox.Tag = "1";
			this._regexBox.Leave += new System.EventHandler(this.TextBoxes_Leave);
			this._regexBox.Validated += new System.EventHandler(this.Boxes_Validated);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label5.Location = new System.Drawing.Point(3, 28);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(97, 28);
			this.label5.TabIndex = 8;
			this.label5.Tag = "2";
			this.label5.Text = "テスト区分";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._toolTip.SetToolTip(this.label5, "テスト方法を選択します。\r\nテストは、置き換えと一致判断があります。");
			// 
			// _methodComboBox
			// 
			this._methodComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._methodComboBox.FormattingEnabled = true;
			this._methodComboBox.Location = new System.Drawing.Point(106, 31);
			this._methodComboBox.Name = "_methodComboBox";
			this._methodComboBox.Size = new System.Drawing.Size(275, 23);
			this._methodComboBox.TabIndex = 2;
			this._methodComboBox.Tag = "2";
			this._methodComboBox.SelectedIndexChanged += new System.EventHandler(this.MethodComboBox_SelectedIndexChanged);
			// 
			// RegexInputForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this._cancelButton;
			this.ClientSize = new System.Drawing.Size(384, 187);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RegexInputForm";
			this.Text = "検索規則入力";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.RegexInputForm_Load);
			this.Shown += new System.EventHandler(this.RegexInputForm_Shown);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.Button _acceptButton;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox _resultBox;
		private System.Windows.Forms.TextBox _replaceBox;
		private System.Windows.Forms.TextBox _inputBox;
		private System.Windows.Forms.TextBox _regexBox;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox _methodComboBox;
		private System.Windows.Forms.Button _helpButton;
		private System.Windows.Forms.Button _testButton;
		private System.Windows.Forms.ToolTip _toolTip;
	}
}