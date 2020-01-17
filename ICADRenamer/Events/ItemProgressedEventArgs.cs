/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using System;

namespace ICADRenamer.Events
{
	/// <summary>
	/// 変換アイテムの進行を表すクラス
	/// </summary>
	/// <seealso cref="System.EventArgs" />
	public class ItemProgressedEventArgs : EventArgs
	{
		/// <summary>
		/// 進行した区分を保持するプロパティ
		/// </summary>
		public ProgressCategory Category { get; set; }

		/// <summary>
		/// 名前を保持するプロパティ
		/// </summary>
		public string Name { get; set; } = "";

		/// <summary>
		/// 進捗カウンタ
		/// </summary>

		public int Counter { get; set; } = 0;
	}
}
