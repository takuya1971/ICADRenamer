/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using ICADRenamer.Settings;

using System;

namespace ICADRenamer
{
	/// <summary>
	/// 名前変換パラメータを表すクラス
	/// </summary>
	public class RenameExecuteParams
	{
		/// <summary>
		///   <see cref="RenameExecuteParams"/> classの初期化
		/// </summary>
		public RenameExecuteParams()
		{
			Settings = new OptionSettingsSerializer().Load();
			Signature = Settings.UserName;
		}

		/// <summary>
		/// 変換先のパスを保持するプロパティ
		/// </summary>
		public string DestinationPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

		/// <summary>
		/// 変換する名前を保持するプロパティ
		/// </summary>
		public string PrefixName { get; set; } = string.Empty;

		/// <summary>
		/// オプション設定を保持するプロパティ
		/// </summary>
		public OptionSettings Settings { get; set; }

		/// <summary>
		/// 署名を保持するプロパティ
		/// </summary>
		public string Signature { get; set; }

		/// <summary>
		/// 変換元のパスを保持するプロパティ
		/// </summary>
		public string SourcePath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

		/// <summary>
		/// 実行区分を保持するプロパティ
		/// </summary>
		public ExecuteItem ExecuteCategory { get; set; } = ExecuteItem.All;
	}
}
