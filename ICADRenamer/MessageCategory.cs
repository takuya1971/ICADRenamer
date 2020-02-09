/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
namespace ICADRenamer
{
	/// <summary>
	/// メッセージ区分に関する列挙型
	/// </summary>
	public enum MessageCategory
	{
		/// <summary>
		/// 情報を保持するフィールド
		/// </summary>
		Information = 1,

		/// <summary>
		/// 確認を保持するフィールド
		/// </summary>
		Confirm = 100,
		/// <summary>
		/// 入力エラーを保持するフィールド
		/// </summary>
		InputError = 1002,

		/// <summary>
		/// エラーを保持するフィールド
		/// </summary>
		Error = 1001,

	}
}
