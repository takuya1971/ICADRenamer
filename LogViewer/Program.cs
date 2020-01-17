using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LogViewer
{
	static class Program
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			if (args == null || !Directory.Exists(args[0]))
			{
				var form = new MainForm();
				Application.Run(form);
			}
			else
			{
				var form = new MainForm(args[0]);
				Application.Run(form);
			}
		}
	}
}
