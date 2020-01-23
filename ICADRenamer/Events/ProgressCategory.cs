/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
namespace ICADRenamer.Events
{
	/// <summary>
	/// 進捗区分に関する列挙型
	/// </summary>
	public enum ProgressCategory
	{
		/// <summary>
		/// ファイルを保持するフィールド
		/// </summary>
		/// 
		File,
		/// <summary>
		/// パーツ名変更を保持するフィールド
		/// </summary>
		ChangePartName,

		/// <summary>
		/// 図面変更を保持するフィールド
		/// </summary>
		ChangeDrawing,

		/// <summary>
		/// 図面更新を保持するフィールド
		/// </summary>
		Update,

		/// <summary>
		/// ビューを保持するフィールド
		/// </summary>
		View,

		/// <summary>
		/// 2Dセグメントを保持するフィールド
		/// </summary>
		Segment,

		/// <summary>
		/// モデルを保持するフィールド
		/// </summary>
		Model,

		/// <summary>
		/// 詳細を保持するフィールド
		/// </summary>
		Detail,
	}
}
