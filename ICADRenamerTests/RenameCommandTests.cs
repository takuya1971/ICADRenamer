using Microsoft.VisualStudio.TestTools.UnitTesting;
using ICADRenamer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using sxnet;

namespace ICADRenamer.Tests
{
	[TestClass()]
	public class RenameCommandTests
	{
		[TestMethod()]
		public void ExecuteTest()
		{
			var command = new RenameCommand();
			command.Execute( new RenameExecuteParams
			{
				DestinationPath = @"d:\M1200",
				SourcePath = @"D:\M1124_KB治具 - コピー",
				PrefixName = "M1200",
				Signature = "木下",
				Settings = new Settings.OptionSettingsSerializer().Load()
			});
		}
	}
}