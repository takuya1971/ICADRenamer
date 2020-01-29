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
		/// ファイルカウントを保持するプロパティ
		/// </summary>
		public CountItem FileCount { get; set; } 

		/// <summary>
		/// 詳細カウントを保持するプロパティ
		/// </summary>
		public CountItem DetailCount { get; set; }

		/// <summary>
		/// ビューカウントを保持するプロパティ
		/// </summary>
		public CountItem ViewCount { get; set; }
	}
}
