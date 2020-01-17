/* ICAD Renamer LogViewer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace LogViewer
{
	/// <summary>
	/// ログデータを表すクラス
	/// </summary>
	public class LogDataRecord
	{
		/// <summary>
		/// 日付を保持するプロパティ
		/// </summary>
		public DateTime? Date { get; set; }

		/// <summary>
		/// ログレベルを保持するプロパティ
		/// </summary>
		public string Level { get; set; }

		/// <summary>
		/// ログメッセージを保持するプロパティ
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// スタックトレースを保持するプロパティ
		/// </summary>
		public string Trace { get; set; }
	}
}
