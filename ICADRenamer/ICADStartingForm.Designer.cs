namespace ICADRenamer
{
	partial class ICADStartingForm
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
			this._messageLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// _messageLabel
			// 
			this._messageLabel.AutoSize = true;
			this._messageLabel.Location = new System.Drawing.Point(12, 9);
			this._messageLabel.Name = "_messageLabel";
			this._messageLabel.Size = new System.Drawing.Size(149, 20);
			this._messageLabel.TabIndex = 0;
			this._messageLabel.Text = "ICADを起動しています...";
			// 
			// ICADStartingForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(195, 130);
			this.ControlBox = false;
			this.Controls.Add(this._messageLabel);
			this.Font = new System.Drawing.Font("Yu Gothic UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "ICADStartingForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ICADの起動";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label _messageLabel;
	}
}