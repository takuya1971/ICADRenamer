/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using CsvHelper.Configuration.Attributes;

using System;

namespace ICADRenamer.CSV
{
	/// <summary>変換結果データを表すクラス</summary>
	public class CsvRecordItem
	{
		/// <summary>日付を保持するプロパティ</summary>
		[Name("日付")]
		public DateTime Date { get; set; }

		/// <summary>コピー元ファイルパスを保持するプロパティ</summary>
		[Name("コピー元パス")]
		public string SourcePath { get; set; } = "";

		/// <summary>
		/// コピー元ファイル名を保持するプロパティ
		/// </summary>
		[Name("コピー元ファイル名")]
		public string SourceFileName { get; set; } = "";

		/// <summary>
		/// コピー先ファイルパスを保持するプロパティ
		/// </summary>
		[Name("コピー先パス")]
		public string DestinationPath { get; set; } = "";

		/// <summary>
		/// コピー先ファイル名を保持するプロパティ
		/// </summary>
		[Name("コピー先ファイル名")]
		public string DestinationFileName { get; set; } = "";

		/// <summary>
		/// 変換結果の真偽値を保持するプロパティ
		/// </summary>
		[Name("結果")]
		[BooleanFalseValues("失敗")]
		[BooleanTrueValues("成功")]
		public bool IsSuccess { get; set; } = false;

		/// <summary>
		/// 備考を保持するプロパティ
		/// </summary>
		[Name("備考")]
		[Default("")]
		public string Remark { get; set; } = "";
	}
}
