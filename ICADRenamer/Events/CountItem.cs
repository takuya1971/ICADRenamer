/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/

namespace ICADRenamer.Events
{
	/// <summary>
	/// カウントアイテムに関する構造体
	/// </summary>
	public struct CountItem
	{
		/// <summary>
		/// 名前を保持するプロパティ
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// アイテム数を保持するプロパティ
		/// </summary>
		public int Items { get; set; }

		/// <summary>
		/// カウントを保持するプロパティ
		/// </summary>
		public int Counter { get; set; }
	}
}
