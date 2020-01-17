namespace ICADRenamer
{
	partial class ExecuteProgressForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExecuteProgressForm));
			this._flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this._cancelButton = new System.Windows.Forms.Button();
			this._toolTip = new System.Windows.Forms.ToolTip(this.components);
			this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this._itemProgressBar = new System.Windows.Forms.ProgressBar();
			this._viewProgressBar = new System.Windows.Forms.ProgressBar();
			this._fileProgressBar = new System.Windows.Forms.ProgressBar();
			this._itemCountLabel = new System.Windows.Forms.Label();
			this._categoryCountLabel = new System.Windows.Forms.Label();
			this._fileCountLabel = new System.Windows.Forms.Label();
			this._itemNameLabel = new System.Windows.Forms.Label();
			this._categoryNameLabel = new System.Windows.Forms.Label();
			this._fileNameLabel = new System.Windows.Forms.Label();
			this._flowLayoutPanel.SuspendLayout();
			this._tableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// _flowLayoutPanel
			// 
			this._flowLayoutPanel.Controls.Add(this._cancelButton);
			this._flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this._flowLayoutPanel.Location = new System.Drawing.Point(0, 221);
			this._flowLayoutPanel.Name = "_flowLayoutPanel";
			this._flowLayoutPanel.Size = new System.Drawing.Size(384, 40);
			this._flowLayoutPanel.TabIndex = 0;
			// 
			// _cancelButton
			// 
			this._cancelButton.Image = global::ICADRenamer.Properties.Resources.CancelExecuteIcon;
			this._cancelButton.Location = new System.Drawing.Point(306, 3);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new System.Drawing.Size(75, 35);
			this._cancelButton.TabIndex = 0;
			this._cancelButton.Text = "キャンセル";
			this._cancelButton.UseVisualStyleBackColor = true;
			this._cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// _tableLayoutPanel
			// 
			this._tableLayoutPanel.ColumnCount = 2;
			this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 81.77084F));
			this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.22917F));
			this._tableLayoutPanel.Controls.Add(this._itemProgressBar, 0, 1);
			this._tableLayoutPanel.Controls.Add(this._viewProgressBar, 0, 3);
			this._tableLayoutPanel.Controls.Add(this._fileProgressBar, 0, 5);
			this._tableLayoutPanel.Controls.Add(this._itemCountLabel, 1, 0);
			this._tableLayoutPanel.Controls.Add(this._categoryCountLabel, 1, 2);
			this._tableLayoutPanel.Controls.Add(this._fileCountLabel, 1, 4);
			this._tableLayoutPanel.Controls.Add(this._itemNameLabel, 0, 0);
			this._tableLayoutPanel.Controls.Add(this._categoryNameLabel, 0, 2);
			this._tableLayoutPanel.Controls.Add(this._fileNameLabel, 0, 4);
			this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this._tableLayoutPanel.Name = "_tableLayoutPanel";
			this._tableLayoutPanel.RowCount = 6;
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this._tableLayoutPanel.Size = new System.Drawing.Size(384, 221);
			this._tableLayoutPanel.TabIndex = 1;
			// 
			// _itemProgressBar
			// 
			this._tableLayoutPanel.SetColumnSpan(this._itemProgressBar, 2);
			this._itemProgressBar.Dock = System.Windows.Forms.DockStyle.Fill;
			this._itemProgressBar.Location = new System.Drawing.Point(3, 39);
			this._itemProgressBar.Name = "_itemProgressBar";
			this._itemProgressBar.Size = new System.Drawing.Size(378, 30);
			this._itemProgressBar.TabIndex = 0;
			// 
			// _viewProgressBar
			// 
			this._tableLayoutPanel.SetColumnSpan(this._viewProgressBar, 2);
			this._viewProgressBar.Dock = System.Windows.Forms.DockStyle.Fill;
			this._viewProgressBar.Location = new System.Drawing.Point(3, 111);
			this._viewProgressBar.Name = "_viewProgressBar";
			this._viewProgressBar.Size = new System.Drawing.Size(378, 30);
			this._viewProgressBar.TabIndex = 1;
			// 
			// _fileProgressBar
			// 
			this._tableLayoutPanel.SetColumnSpan(this._fileProgressBar, 2);
			this._fileProgressBar.Dock = System.Windows.Forms.DockStyle.Fill;
			this._fileProgressBar.Location = new System.Drawing.Point(3, 183);
			this._fileProgressBar.Name = "_fileProgressBar";
			this._fileProgressBar.Size = new System.Drawing.Size(378, 35);
			this._fileProgressBar.TabIndex = 2;
			// 
			// _itemCountLabel
			// 
			this._itemCountLabel.AutoSize = true;
			this._itemCountLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._itemCountLabel.Location = new System.Drawing.Point(316, 0);
			this._itemCountLabel.Name = "_itemCountLabel";
			this._itemCountLabel.Size = new System.Drawing.Size(65, 36);
			this._itemCountLabel.TabIndex = 3;
			this._itemCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _categoryCountLabel
			// 
			this._categoryCountLabel.AutoSize = true;
			this._categoryCountLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._categoryCountLabel.Location = new System.Drawing.Point(316, 72);
			this._categoryCountLabel.Name = "_categoryCountLabel";
			this._categoryCountLabel.Size = new System.Drawing.Size(65, 36);
			this._categoryCountLabel.TabIndex = 4;
			this._categoryCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _fileCountLabel
			// 
			this._fileCountLabel.AutoSize = true;
			this._fileCountLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._fileCountLabel.Location = new System.Drawing.Point(316, 144);
			this._fileCountLabel.Name = "_fileCountLabel";
			this._fileCountLabel.Size = new System.Drawing.Size(65, 36);
			this._fileCountLabel.TabIndex = 5;
			this._fileCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _itemNameLabel
			// 
			this._itemNameLabel.AutoSize = true;
			this._itemNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._itemNameLabel.Location = new System.Drawing.Point(3, 0);
			this._itemNameLabel.Name = "_itemNameLabel";
			this._itemNameLabel.Size = new System.Drawing.Size(307, 36);
			this._itemNameLabel.TabIndex = 6;
			this._itemNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _categoryNameLabel
			// 
			this._categoryNameLabel.AutoSize = true;
			this._categoryNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._categoryNameLabel.Location = new System.Drawing.Point(3, 72);
			this._categoryNameLabel.Name = "_categoryNameLabel";
			this._categoryNameLabel.Size = new System.Drawing.Size(307, 36);
			this._categoryNameLabel.TabIndex = 7;
			this._categoryNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _fileNameLabel
			// 
			this._fileNameLabel.AutoSize = true;
			this._fileNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._fileNameLabel.Location = new System.Drawing.Point(3, 144);
			this._fileNameLabel.Name = "_fileNameLabel";
			this._fileNameLabel.Size = new System.Drawing.Size(307, 36);
			this._fileNameLabel.TabIndex = 8;
			this._fileNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ExecuteProgressForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(384, 261);
			this.Controls.Add(this._tableLayoutPanel);
			this.Controls.Add(this._flowLayoutPanel);
			this.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (128)));
			this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ExecuteProgressForm";
			this.Text = "実行中";
			this.TopMost = true;
			this.Shown += new System.EventHandler(this.ExecuteProgressForm_Shown);
			this._flowLayoutPanel.ResumeLayout(false);
			this._tableLayoutPanel.ResumeLayout(false);
			this._tableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel _flowLayoutPanel;
		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.ToolTip _toolTip;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
		private System.Windows.Forms.ProgressBar _itemProgressBar;
		private System.Windows.Forms.ProgressBar _viewProgressBar;
		private System.Windows.Forms.ProgressBar _fileProgressBar;
		private System.Windows.Forms.Label _itemCountLabel;
		private System.Windows.Forms.Label _categoryCountLabel;
		private System.Windows.Forms.Label _fileCountLabel;
		private System.Windows.Forms.Label _itemNameLabel;
		private System.Windows.Forms.Label _categoryNameLabel;
		private System.Windows.Forms.Label _fileNameLabel;
	}
}