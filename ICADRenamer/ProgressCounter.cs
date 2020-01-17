/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
namespace ICADRenamer
{
	/// <summary>
	/// 進捗アイテムに関するクラス
	/// </summary>
	public class ProgressCounter
	{
		/// <summary>
		/// 現在値を保持するプロパティ
		/// </summary>
		public int Counter { get; set; }

		/// <summary>
		/// 総数を保持するプロパティ
		/// </summary>
		public int Count { get; set; }
	}
}
