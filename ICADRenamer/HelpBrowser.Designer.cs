namespace ICADRenamer
{
	partial class HelpBrowser
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
		[System.Obsolete]
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpBrowser));
			this._webView = new Microsoft.Toolkit.Forms.UI.Controls.WebView();
			this._toolStrip = new System.Windows.Forms.ToolStrip();
			this._undoButton = new System.Windows.Forms.ToolStripButton();
			this._redoButton = new System.Windows.Forms.ToolStripButton();
			this._homeButton = new System.Windows.Forms.ToolStripButton();
			this._updateButton = new System.Windows.Forms.ToolStripButton();
			((System.ComponentModel.ISupportInitialize)(this._webView)).BeginInit();
			this._toolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// _webView
			// 
			this._webView.Dock = System.Windows.Forms.DockStyle.Fill;
			this._webView.Location = new System.Drawing.Point(0, 25);
			this._webView.MinimumSize = new System.Drawing.Size(20, 20);
			this._webView.Name = "_webView";
			this._webView.Size = new System.Drawing.Size(1034, 536);
			this._webView.TabIndex = 0;
			this._webView.NavigationCompleted += new System.EventHandler<Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNavigationCompletedEventArgs>(this.WebView_NavigationCompleted);
			// 
			// _toolStrip
			// 
			this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._undoButton,
            this._redoButton,
            this._homeButton,
            this._updateButton});
			this._toolStrip.Location = new System.Drawing.Point(0, 0);
			this._toolStrip.Name = "_toolStrip";
			this._toolStrip.Size = new System.Drawing.Size(1034, 25);
			this._toolStrip.TabIndex = 1;
			this._toolStrip.Text = "toolStrip1";
			// 
			// _undoButton
			// 
			this._undoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._undoButton.Image = global::ICADRenamer.Properties.Resources.UndoIcon;
			this._undoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._undoButton.Name = "_undoButton";
			this._undoButton.Size = new System.Drawing.Size(23, 22);
			this._undoButton.Text = "前のページに戻ります";
			this._undoButton.Click += new System.EventHandler(this.UndoButton_Click);
			// 
			// _redoButton
			// 
			this._redoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._redoButton.Image = global::ICADRenamer.Properties.Resources.RedoIcon;
			this._redoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._redoButton.Name = "_redoButton";
			this._redoButton.Size = new System.Drawing.Size(23, 22);
			this._redoButton.Text = "次のページへ進みます";
			this._redoButton.Click += new System.EventHandler(this.RedoButton_Click);
			// 
			// _homeButton
			// 
			this._homeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._homeButton.Image = global::ICADRenamer.Properties.Resources.HomeIcon;
			this._homeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._homeButton.Name = "_homeButton";
			this._homeButton.Size = new System.Drawing.Size(23, 22);
			this._homeButton.Text = "ホームへ移動します";
			this._homeButton.Click += new System.EventHandler(this.HomeButton_Click);
			// 
			// _updateButton
			// 
			this._updateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._updateButton.Image = global::ICADRenamer.Properties.Resources.UpdateIcon;
			this._updateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._updateButton.Name = "_updateButton";
			this._updateButton.Size = new System.Drawing.Size(23, 22);
			this._updateButton.Text = "このページを更新します";
			this._updateButton.Click += new System.EventHandler(this.UpdateButton_Click);
			// 
			// HelpBrowser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1034, 561);
			this.Controls.Add(this._webView);
			this.Controls.Add(this._toolStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "HelpBrowser";
			this.Text = "ヘルプブラウザ";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.HelpBrowser_FormClosed);
			this.Load += new System.EventHandler(this.HelpBrowser_Load);
			this.Shown += new System.EventHandler(this.HelpBrowser_Shown);
			((System.ComponentModel.ISupportInitialize)(this._webView)).EndInit();
			this._toolStrip.ResumeLayout(false);
			this._toolStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		[System.Obsolete]
		private Microsoft.Toolkit.Forms.UI.Controls.WebView _webView;
		private System.Windows.Forms.ToolStrip _toolStrip;
		private System.Windows.Forms.ToolStripButton _undoButton;
		private System.Windows.Forms.ToolStripButton _redoButton;
		private System.Windows.Forms.ToolStripButton _homeButton;
		private System.Windows.Forms.ToolStripButton _updateButton;
	}
}