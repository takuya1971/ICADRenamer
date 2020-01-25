/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using NLog;

using System;
using System.Collections.Generic;
using System.Text;

namespace ICADRenamer.Log
{
	/// <summary>
	/// ログを表すクラス
	/// </summary>
	public static class RenameLogger
	{
		/// <summary>
		/// エラーロガーを保持するフィールド
		/// </summary>
		private static readonly Logger _errorLogger = LogManager.GetLogger("ErrorLogger");

		/// <summary>
		/// コンソール出力ログを保持するフィールド
		/// </summary>
		private static Logger _consoleLogger;

		/// <summary>
		/// デバッグロガーを保持するフィールド
		/// </summary>
		private static Logger _debugLogger;

		/// <summary>
		/// 使用するロガーリストを保持するフィールド
		/// </summary>
		private static List<Logger> _loggers = new List<Logger>
		{
			_errorLogger
		};

		/// <summary>
		/// コンソール出力をするかどうかを保持するフィールド
		/// </summary>
		private static bool _useConsole = false;

		/// <summary>
		/// デバッグモードの使用・不使用を保持するフィールド
		/// </summary>
		private static bool _useDebug = false;

		/// <summary>
		/// コンソールモードの真偽値を保持するプロパティ
		/// </summary>
		public static bool ConsoleMode
		{
			get => _useConsole;
			//コンソールロガーのセット
			set
			{
				if (_useConsole == value) return;
				_useConsole = value;
				if (_useConsole)
				{
					if (!NativeMethods.AttachConsole(uint.MaxValue))
					{ NativeMethods.AllocConsole(); }
					_consoleLogger = LogManager.GetLogger("ConsoleLogger");
					_loggers.Add(_consoleLogger);
				}
				else
				{
					NativeMethods.FreeConsole();
					_loggers.Remove(_consoleLogger);
					_consoleLogger = null;
				}
			}
		}

		/// <summary>
		/// デバッグモードの真偽値を保持するプロパティ
		/// </summary>
		public static bool DebugMode
		{
			get => _useDebug;
			//デバッグロガーのセット
			set
			{
				if (_useDebug == value) return;
				_useDebug = value;
				if (_useDebug)
				{
					_debugLogger = LogManager.GetLogger("DebugLogger");
					_loggers.Add(_debugLogger);
				}
				else
				{
					_loggers.Remove(_debugLogger);
					_debugLogger = null;
				}
			}
		}

		/// <summary>
		/// ログ書き込みを実行する
		/// </summary>
		/// <param name="item">書き込む<see cref="LogItem"/></param>
		public static void WriteLog(LogItem item)
		{
			//有効なロガーに書き込む
			foreach (var logger in _loggers)
			{
				//書き込むログアクション
				var action = GetLogAction(logger, item);
				//エラーならスキップ
				if (action == null) return;
				//書き込み
				action(item.Exception, item.Message);
			}
		}

		/// <summary>
		/// ログに書き込む
		/// </summary>
		/// <param name="kind">メッセージ種類</param>
		/// <param name="messages">メッセージ区分とメッセージのリスト</param>
		public static void WriteLog(LogMessageKind? kind, List<(LogMessageCategory category, string message)> messages)
		{
			//nullチェック
			if (kind == null) return;
			//メッセージ組立
			var sb = new StringBuilder();
			//メッセージを作成
			foreach (var mes in messages)
			{
				sb.Append(GetMessage(mes) ?? string.Empty);
			}
			//ログレベルを設定
			LogLevel level = (kind) switch
			{
				LogMessageKind k when (k == LogMessageKind.ActionComplete
				|| k == LogMessageKind.FileStart
				|| k == LogMessageKind.HelpPageNavigate
				|| k == LogMessageKind.StartExecute)
				|| k== LogMessageKind.CancelByUser => LogLevel.Info,
				LogMessageKind.Operation => LogLevel.Trace,
				LogMessageKind.Error => LogLevel.Error,
				_ => LogLevel.Off
			};
			//ログ書き込み
			WriteLog(new LogItem
			{
				Level = level,
				Message = sb.ToString()
			});
		}

		/// <summary>
		/// ログアクションを取得する
		/// </summary>
		/// <param name="logger">対象ロガー。<see cref="Logger"/></param>
		/// <param name="item">ログレベルを含む<see cref="LogItem"/></param>
		/// <returns></returns>
		private static Action<Exception, string> GetLogAction(Logger logger, LogItem item)
			=> item.Level.Ordinal switch
			{
				//Trace
				0 => logger.Trace,
				//Debug
				1 => logger.Debug,
				//Info
				2 => logger.Info,
				//Warn
				3 => logger.Warn,
				//Error
				4 => logger.Error,
				//Fatal
				5 => logger.Fatal,
				_ => null,
			};
		/// <summary>
		/// メッセージ区分の文字列を取得する
		/// </summary>
		/// <param name="category">メッセージ区分</param>
		/// <returns></returns>
		private static string GetMesageCategory(LogMessageCategory category) => (category) switch
		{
			LogMessageCategory.DestinationPath => "コピー先",
			LogMessageCategory.FilePath => "ファイルパス",
			LogMessageCategory.Message => "メッセージ",
			LogMessageCategory.MoveTo => "移動先",
			LogMessageCategory.NewNumber => "新番号",
			LogMessageCategory.Result => "実行結果",
			LogMessageCategory.Signature => "署名",
			LogMessageCategory.SourceForm => "発生元",
			LogMessageCategory.SourcePath => "コピー元",
			LogMessageCategory.ActiveControl => "場所",
			LogMessageCategory.NewData => "変更データ",
			LogMessageCategory.OldData => "変更前データ",
			LogMessageCategory.Regex => "正規表現",
			LogMessageCategory.RegexResult => "テスト結果",
			LogMessageCategory.ReplaceText => "置換文字列",
			LogMessageCategory.TestSource => "テスト文字列",
			LogMessageCategory.TestCategory => "テスト区分",
			LogMessageCategory.FileName => "ファイル名",
			_ => string.Empty
		};

		/// <summary>
		/// ログメッセージを取得する
		/// </summary>
		/// <param name="messagePair">メッセージ</param>
		/// <returns></returns>
		private static string GetMessage((LogMessageCategory category, string mes) messagePair)
		{
			var category = GetMesageCategory(messagePair.category);
			var mes = messagePair.category == LogMessageCategory.Result
				? GetMessageBool(messagePair.mes) : messagePair.mes;
			return $" {category}:{mes}";
		}

		/// <summary>
		/// メッセージのbool文字列を取得する
		/// </summary>
		/// <param name="message">メッセージを表す文字列</param>
		/// <returns></returns>
		private static string GetMessageBool(string message) => Convert.ToBoolean(message) ? "成功" : "失敗";
	}
}
