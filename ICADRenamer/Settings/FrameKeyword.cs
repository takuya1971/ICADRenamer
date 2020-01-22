/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using Newtonsoft.Json;

using System.Collections.Generic;

namespace ICADRenamer.Settings
{
	/// <summary>
	/// 書き換えるフレーム要素を表すクラス
	/// </summary>
	[JsonObject("表題欄検索データ")]
	public class FrameKeyword
	{
		/// <summary>
		/// 訂正文字列判定の正規表現を保持するプロパティ
		/// </summary>
		[JsonProperty("改訂注記検索要素")]
		public List<string> DeltaNoteRegexes { get; set; }

		/// <summary>
		/// 日付文字列判定の正規表現を保持するプロパティ
		/// </summary>
		[JsonProperty("日付検索要素")]
		public List<string> DateRegexes { get; set; }

		/// <summary>
		/// 図番文字列判定の正規表現を保持するプロパティ
		/// </summary>
		[JsonProperty("図番検索要素")]
		public List<string> DrawNumberRegexes { get; set; }
		/// <summary>
		/// 署名文字列を保持するプロパティ
		/// </summary>
		[JsonProperty("署名要素")]
		public List<string> Signatures { get; set; }

		[JsonProperty("図番区切")]
		public List<string> DrawNumberSplit { get; set; }

		/// <summary>
		///   <see cref="FrameKeyword"/> classの初期化
		/// </summary>
		public FrameKeyword() { }
	}
}
