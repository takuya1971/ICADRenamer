/* ICAD Renaler
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
namespace ICADRenamer.Log
{
	/// <summary>
	/// ログメッセージ種類に関する列挙型
	/// </summary>
	public enum LogMessageKind
	{
		/// <summary>
		/// 操作を保持するフィールド
		/// </summary>
		Operation,

		/// <summary>
		/// 処理完了を保持するフィールド
		/// </summary>
		ActionComplete,

		/// <summary>
		/// 開始を保持するフィールド
		/// </summary>
		StartExecute,
		/// <summary>
		/// ファイルの開始を保持するフィールド
		/// </summary>
		FileStart,
		/// <summary>
		/// ヘルプページの移動を保持するフィールド
		/// </summary>
		HelpPageNavigate,
		/// <summary>
		/// エラーを保持するフィールド
		/// </summary>
		Error,
	}
}
