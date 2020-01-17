/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using Newtonsoft.Json;

using System.IO;
using System.Text;

namespace ICADRenamer.Settings
{
	/// <summary>
	/// 設定を表すベースクラス
	/// </summary>
	public abstract class SettingBase
	{
		/// <summary>
		/// ファイルパスを保持するプロパティ
		/// </summary>
		protected abstract string FilePath { get; }
		/// <summary>
		/// 既定値を取得する
		/// </summary>
		/// <returns></returns>
		protected abstract object CreateDefault();

		/// <summary>
		/// ファイルの存在チェックを実行する
		/// </summary>
		/// <returns>設定オブジェクト</returns>
		protected object FileExistCheck()
		{
			if (File.Exists(FilePath)) return null;
			//
			var dir = Path.GetDirectoryName(FilePath);
			if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
			//
			var obj = CreateDefault();
			var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
			SaveToFile(json);
			return obj;
		}

		/// <summary>
		/// ファイルからデータのロードを実行する
		/// </summary>
		/// <returns>JSON文字列</returns>
		protected string LoadFromFile()
		{
			string json;
			using (var sr = new StreamReader(FilePath, Encoding.UTF8))
			{
				json = sr.ReadToEnd();
			}
			return json;
		}

		/// <summary>
		/// データのファイルへの保存を実行する
		/// </summary>
		/// <param name="json">JSONを表す文字列</param>
		protected void SaveToFile(string json)
		{
			var sw = new StreamWriter(FilePath, false);
			sw.Write(json);
			sw.Close();
		}
	}
}
