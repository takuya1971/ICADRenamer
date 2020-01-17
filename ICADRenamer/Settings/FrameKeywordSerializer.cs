/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using Newtonsoft.Json;

using System.Collections.Generic;

namespace ICADRenamer.Settings
{
	/// <summary>
	/// 図枠検索文字ファイルの操作を表すクラス
	/// </summary>
	/// <seealso cref="ICADRenamer.Settings.SettingBase" />
	public class FrameKeywordSerializer : SettingBase
	{
		/// <summary>
		///   <see cref="FrameKeywordSerializer"/> classの初期化
		/// </summary>
		public FrameKeywordSerializer()
		{
			//ファイルがあるかのチェック
			var obj = FileExistCheck();
			if (obj != null) Keywords = (FrameKeyword) obj;
		}

		/// <summary>
		/// 図枠関係検索要素を保持するプロパティ
		/// </summary>
		public FrameKeyword Keywords { get; private set; }

		/// <summary>
		/// ファイルパスを保持するプロパティ
		/// </summary>
		protected override string FilePath => SystemSettings.FrameKeywordSetting;

		/// <summary>
		/// 設定のロードを実行する
		/// </summary>
		/// <returns><see cref="FrameKeyword"/></returns>
		public FrameKeyword Load()
		{
			var json = LoadFromFile();
			Keywords = JsonConvert.DeserializeObject<FrameKeyword>(json);
			return Keywords;
		}

		/// <summary>
		/// 保存処理を実行する
		/// </summary>
		public void Save()
		{
			var json = JsonConvert.SerializeObject(Keywords, Formatting.Indented);
			SaveToFile(json);
		}

		/// <summary>
		/// 保存を実行する
		/// </summary>
		/// <param name="keyword">検索規則</param>
		public void Save(FrameKeyword keyword)
		{
			Keywords = keyword;
			Save();
		}

		/// <summary>
		/// デフォルト設定の生成を実行する
		/// </summary>
		/// <returns><see cref="FrameKeyword"/></returns>
		protected override object CreateDefault() => new FrameKeyword
		{
			//図番検索
			DrawNumberRegexes = new List<string>()
				{
					"(M[0-9]{4}|M[0-9]{3})-([0-9]{4}|[0-9]{3})-[0-9]{2}"
				},
			//日付検索
			DateRegexes = new List<string>()
				{
					@"([0-9]{4}|[0-9]{2})(/|.)([0-9]{2}|[0-9])(/|.)([0-9]{2}|[0-9])"
				},
			//改訂検索
			DeltaNoteRegexes = new List<string>()
				{
					@"．{3,}",
					@"・{3,}"
				},
			//署名検索
			Signatures = new List<string>()
				{
					"山本",
					"藤居",
					"隅野",
					"辻本",
					"堀田",
					"木下",
					"T.K.",
					"Techno",
					"techno",
					"TECHNO"
				}
		};
	}
}
