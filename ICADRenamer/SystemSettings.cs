/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using System.IO;
using System;

namespace ICADRenamer
{
	/// <summary>
	/// 基本設定を表すクラス
	/// </summary>
	public static class SystemSettings
	{

		/// <summary>
		/// 設定フォルダを保持するプロパティ
		/// </summary>
		public static string SettingFolder { get; } = Directory.GetCurrentDirectory() + @"\Settings";

		/// <summary>
		/// 図面変更検索文字ファイルを保持するプロパティ
		/// </summary>
		public static string FrameKeywordSetting { get; } = SettingFolder + @"\FrameKeywordSettings.json";

		/// <summary>
		/// ヘルプファイルのパスを保持するプロパティ
		/// </summary>
		public static string HelpPath { get; } = @"Help\Help.html";

		/// <summary>
		/// ICAD拡張子を保持するプロパティ
		/// </summary>
		public static string IcadExtension { get; } = "*.icd";

		/// <summary>
		/// ログ拡張子を保持するプロパティ
		/// </summary>
		public static string LogExtension { get; } = "*.log";

		/// <summary>
		/// ログフォルダを保持するプロパティ
		/// </summary>
		public static string LogFolder { get; } = $@"{Environment.CurrentDirectory}\Log";

		/// <summary>
		/// メッセージ設置ファイルを保持するプロパティ
		/// todo:未実装
		/// </summary>
		public static string MessageSettings { get; } = $@"{SettingFolder}\Messages.json";

		/// <summary>
		/// オプション設定ファイルを保持するプロパティ
		/// </summary>
		public static string OptionSetting { get; } = SettingFolder + @"\OptionSettings.json";
	}
}
