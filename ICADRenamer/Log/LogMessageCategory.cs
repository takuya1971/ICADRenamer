/* ICAD Renaler
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
namespace ICADRenamer.Log
{
	/// <summary>
	/// ログメッセージ区分に関する列挙型
	/// </summary>
	public enum LogMessageCategory
	{
		/// <summary>
		/// 発生元を保持するフィールド
		/// </summary>
		SourceForm,

		/// <summary>
		/// メッセージを保持するフィールド
		/// </summary>
		Message,

		/// <summary>
		/// 新番号を保持するフィールド
		/// </summary>
		NewNumber,

		/// <summary>
		/// 署名を保持するフィールド
		/// </summary>
		Signature,

		/// <summary>
		/// ファイルパスを保持するフィールド
		/// </summary>
		FilePath,

		/// <summary>
		/// コピー先を保持するフィールド
		/// </summary>
		DestinationPath,

		/// <summary>
		/// コピー元を保持するフィールド
		/// </summary>
		SourcePath,

		/// <summary>
		/// 移動先を保持するフィールド
		/// </summary>
		MoveTo,

		/// <summary>
		/// 結果を保持するフィールド
		/// </summary>
		Result,

		/// <summary>
		/// 開いているコントロールを保持するフィールド
		/// </summary>
		ActiveControl,

		/// <summary>
		/// 新しいデータを保持するフィールド
		/// </summary>
		NewData,

		/// <summary>
		/// 古いデータを保持するフィールド
		/// </summary>
		OldData,

		/// <summary>
		/// Regexテスト文字列を保持するフィールド
		/// </summary>
		TestSource,

		/// <summary>
		/// 正規表現を保持するフィールド
		/// </summary>
		Regex,

		/// <summary>
		/// 置き換え文字列を保持するフィールド
		/// </summary>
		ReplaceText,

		/// <summary>
		/// テスト結果を保持するフィールド
		/// </summary>
		RegexResult,

		/// <summary>
		/// テスト区分を保持するフィールド
		/// </summary>
		TestCategory,

		/// <summary>
		/// ファイル名を保持するフィールド
		/// </summary>
		FileName,

		/// <summary>
		/// キャンセルされたを保持するフィールド
		/// </summary>
		Canceled,
	}
}
