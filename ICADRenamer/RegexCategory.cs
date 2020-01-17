/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
namespace ICADRenamer
{
	/// <summary>
	/// 検索項目に関する列挙型
	/// </summary>
	public enum RegexCategory
	{
		/// <summary>
		/// 図面番号検索規則を保持するフィールド
		/// </summary>
		DrawNumber = 1,

		/// <summary>
		/// 日付検索規則を保持するフィールド
		/// </summary>
		Date,

		/// <summary>
		/// 修正検索規則を保持するフィールド
		/// </summary>
		DeltaNote,

		/// <summary>
		/// 署名検索規則を保持するフィールド
		/// </summary>
		Signature,

		/// <summary>
		/// 新しい工事番号
		/// </summary>
		NewProject,
	}
}

