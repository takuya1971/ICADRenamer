using System;
using System.Diagnostics;
using WixSharp;
using WixSharp.UI.Forms;

namespace WixSharpSetup
{
	public partial class CustomDialog : ManagedForm, IManagedDialog
	{
		public CustomDialog()
		{
			//NOTE: If this assembly is compiled for v4.0.30319 runtime, it may not be compatible with the MSI hosted CLR.
			//The incompatibility is particularly possible for the Embedded UI scenarios.
			//The safest way to avoid the problem is to compile the assembly for v3.5 Target Framework.ICADRenamerSetup
			InitializeComponent();
		}

		void Dialog_Load(object sender, EventArgs e)
		{
			banner.Image = Runtime.Session.GetResourceBitmap("WixUI_Bmp_Banner");
			Text = "[ProductName] Setup";

			//resolve all Control.Text cases with embedded MSI properties (e.g. 'ProductName') and *.wxl file entries
			base.Localize();
		}

		void Back_Click(object sender, EventArgs e)
		{
			Shell.GoPrev();
		}

		void Next_Click(object sender, EventArgs e)
		{
			Shell.GoNext();
		}

		void Cancel_Click(object sender, EventArgs e)
		{
			Shell.Cancel();
		}
	}
}