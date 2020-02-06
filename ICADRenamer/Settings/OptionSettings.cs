/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ICADRenamer.Settings
{
	/// <summary>
	/// オプション設定データを表すクラス
	/// </summary>
	[JsonObject("オプション設定")]
	public class OptionSettings
	{
		/// <summary>
		/// デルタマーク除去の真偽値を保持するプロパティ
		/// </summary>
		[JsonProperty("デルタマーク消去")]
		public bool CanDeleteDelta { get; set; }

		/// <summary>
		/// 図面更新の真偽値を保持するプロパティ
		/// </summary>
		[JsonProperty("図面更新可否")]
		public bool CanExecuteUpdate { get; set; }

		/// <summary>
		/// 初期コピー元フォルダを保持するプロパティ
		/// </summary>
		[JsonProperty("初期フォルダ")]
		public string DefaultFolder { get; set; }

		/// <summary>
		/// ICAD連携ポートを保持するプロパティ
		/// </summary>
		[JsonProperty("ICAD連携ポート")]
		public int ICADLinkPort { get; set; }

		/// <summary>
		/// 新規M番チェック用リスト
		/// </summary>
		[JsonProperty("新規M番チェック")]
		public List<string> NewProjectRegex { get; set; }

		/// <summary>
		/// 新規M番チェック可否の真偽値を保持するプロパティ
		/// </summary>
		[JsonProperty("新規工事番号チェック可否")]
		public bool UseProjectCheck { get; set; }

		/// <summary>
		/// ICADの最小化の真偽値を保持するプロパティ
		/// </summary>
		[JsonProperty("ICAD最小化")]
		public bool ICADMinimize { get; set; }

		/// <summary>
		/// ユーザー名を保持するプロパティ
		/// </summary>
		[JsonProperty("所有者")]
		public string UserName { get; set; }

		/// <summary>
		/// 年が４桁表示かどうかの状態を保持するプロパティ
		/// </summary>
		/// <remarks>
		/// 真の場合：4桁表示
		/// </remarks>
		[JsonProperty("年は４桁表示")]
		public bool IsYear4Digit { get; set; }

		/// <summary>
		/// 月日が２桁固定かどうかの状態を保持するプロパティ
		/// </summary>
		/// <remarks>
		/// 真の場合：２桁固定
		/// </remarks>
		[JsonProperty("月日は２桁表示")]
		public bool IsMonthAndDate2Digit { get; set; }

		/// <summary>
		/// 年月日区切りがスラッシュかどうかの状態を保持するプロパティ
		/// </summary>
		/// <remarks>
		/// 真の場合：スラッシュ
		/// </remarks>
		[JsonProperty("年月日区切がスラッシュ")]
		public bool IsDateSeparatorSlash { get; set; }

		/// <summary>
		/// 3D製品フォルダを登録するかどうかの真偽値を保持するプロパティ
		/// </summary>
		/// <remarks>芯の場合：登録する</remarks>
		public bool ResitTo3DSeihin { get; set; }

		/// <summary>
		/// ICAD再起動しきい値を保持するプロパティ
		/// </summary>
		public int RestartThredshold { get; set; }

		/// <summary>
		///   <see cref="OptionSettings"/> classの初期化
		/// </summary>
		public OptionSettings() { }
	}
}
