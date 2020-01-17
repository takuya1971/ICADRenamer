/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using Ookii.CommandLine;

using System;
using System.ComponentModel;

namespace ICADRenamer
{
	/// <summary>
	/// コマンドライン引数を表すクラス
	/// </summary>
	public class CommandLineArgs
	{
		/// <summary>
		/// ヘルプを保持するプロパティ
		/// </summary>
		[CommandLineArgument("Help")
			, Alias("?")
			, Description("使い方を表示します。")]
		public bool Help { get; set; }

		/// <summary>
		/// 直接実行モードを保持するプロパティ
		/// </summary>
		[CommandLineArgument("Silent", ValueDescription = "[コピー元フォルダ],[コピー先フォルダ],[新しいM番],[署名]")
			, Alias("s"), MultiValueSeparator(",")
			, Description(@"直接実行モードです。引数はカンマ(,)で区切ります。")]
		public string[] SilentMode { get; set; }

		/// <summary>
		/// コンソール使用の真偽値を保持するプロパティ
		/// </summary>
		[CommandLineArgument(argumentName: "WithConsole", DefaultValue = false)
			, Alias("c")
			, Description("コンソールモードでフォームを起動します。")]
		public bool UseConsole
		{ get; set; }

		/// <summary>
		/// デバッグモード使用の真偽値を保持するプロパティ
		/// </summary>
		[CommandLineArgument(argumentName: "DebugMode", DefaultValue = false)
			, Alias("d")
			, Description("デバッグモードでフォームを起動します。")]
		public bool UseDebugMode { get; set; }

		/// <summary>
		/// コマンドライン引数の生成
		/// </summary>
		/// <param name="args">コマンドライン引数</param>
		/// <returns></returns>
		public static CommandLineArgs Create(string[] args)
		{
			var parser = new CommandLineParser(typeof(CommandLineArgs));
			//
			parser.ArgumentParsed += Parser_ArgumentParsed;
			//
			try
			{
				var result = (CommandLineArgs) parser.Parse(args);
				if (result != null) return result;
			}
			catch (CommandLineArgumentException ex)
			{
				using (var writer = LineWrappingTextWriter.ForConsoleError())
				{
					NativeMethods.AttachConsole(uint.MaxValue);
					writer.WriteLine(GetParsingErrorMessage(ex));
					writer.WriteLine();
				}
				return new CommandLineArgs();
			}
			//
			var options = new WriteUsageOptions()
			{
				IncludeAliasInDescription = true,
				IncludeDefaultValueInDescription = true
			};
			if (NativeMethods.AttachConsole(uint.MaxValue))
			{
				parser.WriteUsageToConsole(options);
			}
			return null;
		}

		/// <summary>
		/// エラーメッセージを取得する
		/// </summary>
		/// <param name="exception">例外</param>
		/// <returns></returns>
		private static string GetParsingErrorMessage(CommandLineArgumentException exception)
		{
			string categoryMessage = exception.Category switch
			{
				CommandLineArgumentErrorCategory.ApplyValueError => "引数の型が不正です。",
				CommandLineArgumentErrorCategory.ArgumentValueConversion => "引数名が不正です。",
				CommandLineArgumentErrorCategory.CreateArgumentsTypeError => "コンストラクタエラーです。",
				CommandLineArgumentErrorCategory.DuplicateArgument => "引数が複数回指定されました。",
				CommandLineArgumentErrorCategory.InvalidDictionaryValue => "キーの重複があります。",
				CommandLineArgumentErrorCategory.MissingNamedArgumentValue => "引数の指定値が足りません。",
				CommandLineArgumentErrorCategory.MissingRequiredArgument => "引数の数が足りません。",
				CommandLineArgumentErrorCategory.TooManyArguments => "引数が多すぎます。",
				CommandLineArgumentErrorCategory.UnknownArgument => "引数が不明です。",
				CommandLineArgumentErrorCategory.Unspecified => "不明なエラーです。",
				_ => throw new InvalidOperationException()
			};
			return $"{exception.ArgumentName}の{categoryMessage}";
		}

		/// <summary>
		/// パーサーのパースイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private static void Parser_ArgumentParsed(object sender, ArgumentParsedEventArgs e)
		{
			if (e.Argument.ArgumentName == "Help") e.Cancel = true;
		}
	}
}
