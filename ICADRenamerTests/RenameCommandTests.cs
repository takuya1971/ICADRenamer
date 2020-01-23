using Microsoft.VisualStudio.TestTools.UnitTesting;
using ICADRenamer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICADRenamer.Settings;
using System.IO;

namespace ICADRenamer.Tests
{
	[TestClass()]
	public class RenameCommandTests
	{
		[TestMethod()]
		public void Execute2Test()
		{
			DateTime start = DateTime.Now;
			var command = new RenameCommand();
			command.Execute(new RenameExecuteParams
			{
				SourcePath = @"D:\M1124_KB治具 - コピー",
				DestinationPath = @"D:\M1200",
				PrefixName = "M1120",
				Signature = "木下",
				Settings = new OptionSettingsSerializer().Load(),
			});
			foreach (var resultRecord in command.RecordItems)
			{
				Console.WriteLine(resultRecord);
			}
			var time = start.Subtract(DateTime.Now);
			Console.WriteLine($"実行時間:{time.TotalSeconds}sec.");
		}
	}
}