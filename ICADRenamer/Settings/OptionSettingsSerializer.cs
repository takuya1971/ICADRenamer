/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using Newtonsoft.Json;

using System.Collections.Generic;

namespace ICADRenamer.Settings
{
	/// <summary>
	/// オプション設定シリアライザを表すクラス
	/// </summary>
	/// <seealso cref="ICADRenamer.Settings.SettingBase" />
	public class OptionSettingsSerializer : SettingBase
	{
		/// <summary>
		///   <see cref="OptionSettingsSerializer"/> classの初期化
		/// </summary>
		public OptionSettingsSerializer()
		{
			//ファイルがあるかのチェック
			var obj = FileExistCheck();
			if (obj != null) Options = (OptionSettings) obj;
		}

		/// <summary>
		/// 設定を保持するプロパティ
		/// </summary>
		public OptionSettings Options { get; private set; }

		/// <summary>
		/// ファイルパスを保持するプロパティ
		/// </summary>
		protected override string FilePath => SystemSettings.OptionSetting;
		/// <summary>
		/// ファイルからのロードを実行する
		/// </summary>
		/// <returns></returns>
		public OptionSettings Load()
		{
			var json = LoadFromFile();
			Options = JsonConvert.DeserializeObject<OptionSettings>(json);
			return Options;
		}

		/// <summary>
		/// ファイルへの保存を実行する
		/// </summary>
		public void Save()
		{
			var json = JsonConvert.SerializeObject(Options, Formatting.Indented);
			SaveToFile(json);
		}

		/// <summary>
		/// ファイルへの保存を実行する
		/// </summary>
		/// <param name="option">設定データ</param>
		public void Save(OptionSettings option)
		{
			Options = option;
			Save();
		}

		/// <summary>
		/// 既定値を取得する
		/// </summary>
		/// <returns></returns>
		protected override object CreateDefault()
			=> new OptionSettings()
			{
				CanDeleteDelta = true,
				DefaultFolder = @"Z:\",
				ICADLinkPort = 3999,
				CanExecuteUpdate = true,
				UseProjectCheck = true,
				ICADMinimize = true,
				UserName = "",
				NewProjectRegex = new List<string>
				{
					@"M[0-9]{4}",
					@"MA[0-9]{6}",
				},
			};
	}
}
