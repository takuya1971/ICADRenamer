/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using ICADRenamer.Events;

namespace ICADRenamer
{
	/// <summary>
	/// 進捗アイテムに関するクラス
	/// </summary>
	public class ProgressItem
	{
		/// <summary>
		/// カウンタを保持するプロパティ
		/// </summary>
		public ProgressCounter Counters { get; set; }

		/// <summary>
		/// 名前を保持するプロパティ
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 区分を保持するプロパティ
		/// </summary>
		public ProgressCategory Category { get; set; }
	}
}
