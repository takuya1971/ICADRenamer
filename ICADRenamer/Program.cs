/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ookii.CommandLine;
using System.IO;
using ICADRenamer.Log;
using NLog;
using System.Text.RegularExpressions;
using ICADRenamer.Settings;

namespace ICADRenamer
{
	/// <summary>
	/// プログラムエントリを表すクラス・コントロール
	/// </summary>
	public static class Program
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		public static int Main(string[] args)
		{
			//標準出力
			int result;
			//Logフォルダ作成
			if (!Directory.Exists(SystemSettings.LogFolder)) Directory.CreateDirectory(SystemSettings.LogFolder);
			//
			try
			{
				//コマンドラインを取得
				var arguments = CommandLineArgs.Create(args);
				//サイレントモードでなければフォームを表示
				if (arguments == null)
				{
					result = 0;
				}
				else if (arguments.SilentMode == null)
				{
					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(false);
					var form = new MainForm(arguments);
					Application.Run(form);
					result = form.Result;
					form.Dispose();
				}
				//サイレントモードの時
				else return RunSilent(arguments.SilentMode);
			}
			//例外発生
			catch (CommandLineArgumentException e)
			{
				NativeMethods.AttachConsole(uint.MaxValue);
				Console.WriteLine(e.Message);
				result = e.HResult;
			}
			return result;
		}

		/// <summary>
		/// 不正なコマンド文字列を実行する
		/// </summary>
		/// <returns></returns>
		static private string InvalidCommandLineParam() => "不正なコマンドラインパラメータ";

		/// <summary>
		/// 新規M番規則と合致しているかどうか取得する
		/// </summary>
		/// <param name="name">新しいM番を表す文字列</param>
		/// <returns>
		/// 合致しているの場合は真
		/// </returns>
		static bool IsMatchNewProduct(string name)
		{
			var options = new OptionSettingsSerializer().Load();
			foreach (var op in options.NewProjectRegex)
			{
				if (Regex.IsMatch(name, op)) return true;
			}
			return false;
		}

		/// <summary>
		/// コマンドラインが正しいかどうか取得する
		/// </summary>
		/// <param name="args">コマンドライン</param>
		/// <param name="msg">結果のメッセージを表す文字列</param>
		/// <returns>
		/// 正しい場合は真
		/// </returns>
		static bool IsValidCommandLine(string[] args, out string msg)
		{
			if (args.Length != 4)
			{
				msg = "パラメータが足りません。";
				return false;
			}
			if (!Directory.Exists(args[0]))
			{
				msg = "コピー元フォルダが存在しません。";
				return false;
			}
			if (!Directory.Exists(args[1]))
			{
				msg = "コピー先フォルダが存在しません。";
				return false;
			}
			if (!IsMatchNewProduct(args[2]))
			{
				msg = "M付番規則に違反しています。";
				return false;
			}
			msg = string.Empty;
			return true;
		}

		/// <summary>
		/// サイレントモードを実行する
		/// </summary>
		/// <param name="param">パラメータ</param>
		/// <returns></returns>
		static int RunSilent(string[] param)
		{
			RenameLogger.ConsoleMode = true;
			NativeMethods.AttachConsole(uint.MaxValue);
			var args = param;
			//実行結果出力
			var result = 0;
			//コマンドライン不正時の処理
			if (!IsValidCommandLine(args, out string msg))
			{
				var e = new CommandLineArgumentException(
					msg, CommandLineArgumentErrorCategory.ApplyValueError);
				RenameLogger.WriteLog(new LogItem
				{
					Exception = e,
					Level = LogLevel.Error,
					Message = InvalidCommandLineParam()
				}); ;
				throw e;
			}
			//実行コマンド
			var command = new RenameCommand();
			//実行
			command.Execute(new RenameExecuteParams
			{
				SourcePath = args[0],
				DestinationPath = args[1],
				PrefixName = args[2],
				Signature = args[3],
				Settings = new OptionSettingsSerializer().Load()
			});
			//コマンドの破棄
			command.Dispose();
			//結果を返す
			return result;
		}
	}
}
