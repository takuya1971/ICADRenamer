/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using NLog;

using System;

namespace ICADRenamer.Log
{
	/// <summary>
	/// ログアイテムを表すクラス
	/// </summary>
	public class LogItem
	{
		/// <summary>
		/// ログレベルを保持するプロパティ
		/// </summary>
		public LogLevel Level { get; set; }

		/// <summary>
		/// ログメッセージを保持するプロパティ
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// 例外を保持するプロパティ
		/// </summary>
		public Exception Exception { get; set; } = null;

	}
}
