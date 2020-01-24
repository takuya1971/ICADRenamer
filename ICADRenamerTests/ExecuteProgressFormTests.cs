using Microsoft.VisualStudio.TestTools.UnitTesting;
using ICADRenamer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICADRenamer.Tests
{
	[TestClass()]
	public class ExecuteProgressFormTests
	{
		private DateTime start;

		[TestMethod()]
		public void ExecuteProgressFormTest()
		{
			start = DateTime.Now;
			ExecuteProgressForm form = new ExecuteProgressForm(
				new RenameExecuteParams
				{
					SourcePath = @"D:\M1124_KB治具 - コピー",
					DestinationPath = @"D:\M1200",
					PrefixName = "M1200",
					Signature = "木下",
					Settings = new Settings.OptionSettingsSerializer().Load()
				});
			form.ExecuteStarted += Form_ExecuteStarted;
			form.ExecuteFinished += Form_ExecuteFinished;
			form.ExecuteCanceled += Form_ExecuteCanceled;
			form.ShowDialog();

		}

		private void Form_ExecuteCanceled(object sender, EventArgs e)
		{
			if(sender is ExecuteProgressForm form)
			{
				var time = start.Subtract(DateTime.Now);
				Console.WriteLine($"キャンセルされました。実行時間:{time.TotalSeconds}秒");
			}
		}

		private void Form_ExecuteFinished(object sender, EventArgs e)
		{
			if(sender is ExecuteProgressForm form)
			{
				if (!form.IsDisposed) form.Dispose();
			}
			Console.WriteLine("実行完了しました。");
		}

		private void Form_ExecuteStarted(object sender, EventArgs e)
		{
			if(sender is Form form)
			{
				Console.WriteLine("実行開始しました。");
				form.ShowDialog();
			}
		}
	}
}