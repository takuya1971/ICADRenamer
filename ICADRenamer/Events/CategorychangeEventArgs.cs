/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
namespace ICADRenamer.Events
{
	/// <summary>
	/// 実行区分進捗イベント引数を表すクラス
	/// </summary>
	/// <seealso cref="ICADRenamer.Events.ItemProgressedEventArgs" />
	public class CategoryChangeEventArgs : ItemProgressedEventArgs
	{
		/// <summary>
		/// 個数を保持するプロパティ
		/// </summary>
		public int TotalItem { get; set; }
	}
}
