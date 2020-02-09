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
			MainForm form;
			try
			{
				if (!Directory.Exists(args[0]))
				{
					form = new MainForm();
					Application.Run(form);
				}
				else
				{
					form = new MainForm(args[0]);
					Application.Run(form);
				}
			}
			catch (Exception)
			{
				form = new MainForm();
				Application.Run(form);
			}
		}
	}
}
