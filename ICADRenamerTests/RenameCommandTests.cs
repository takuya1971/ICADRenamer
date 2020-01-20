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
			//ClearDir();
			var command = new RenameCommand();
			command.Execute2(new RenameExecuteParams
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
		}

		void ClearDir()
		{
			var files = Directory.GetFiles(@"D:\M1200", SystemSettings.IcadExtension, SearchOption.AllDirectories);
			var i = 0;
			while(files.Length>0)
			{
				File.Delete(files[i]);
				i++;
			}
		}
	}
}