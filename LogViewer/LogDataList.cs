/* ICAD Renamer LogViewer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace LogViewer
{
	/// <summary>
	/// ログデータリストを表すクラス
	/// </summary>
	/// <seealso cref="System.Collections.Generic.List{LogViewer.LogDataRecord}" />
	public class LogDataList : List<LogDataRecord>
	{
		/// <summary>
		/// ファイルからログデータを取得する
		/// </summary>
		/// <param name="FilePath">ファイルパスを表す文字列</param>
		/// <returns></returns>
		public static LogDataList Load(string FilePath)
		{
			//読み取った文字列
			string result;
			//ファイルから読み取り
			using (var sr = new StreamReader(FilePath))
			{
				result = sr.ReadToEnd();
			}

			return CreateData(result);
		}

		/// <summary>
		/// 改行した文字列を取得する
		/// </summary>
		/// <param name="source">元の文字列</param>
		/// <returns></returns>
		private static string AddReturn(string source) => Regex.Replace(source, @"\s", "\r\n");

		/// <summary>
		/// 文字列からログデータを取得する
		/// </summary>
		/// <param name="source">読み取った文字列を表す文字列</param>
		/// <returns></returns>
		private static LogDataList CreateData(string source)
		{
			LogDataList dataRecords = new LogDataList();
			try
			{
				var records = source.Split(new string[] { "-END-" }, StringSplitOptions.RemoveEmptyEntries);
				foreach (var record in records)
				{
					if (record.Length < 3) continue;
					var fields = record.Split(new char[] { '|' });
					var ci = new CultureInfo("ja-JP");
					ci.DateTimeFormat.LongDatePattern = "yyyy-mm-dd hh:mm:ss.ffff";
					var logRecord = new LogDataRecord
					{
						Date = DateTime.TryParse(TrimString(fields[0]), ci, DateTimeStyles.AssumeLocal, out var d) ? d : (DateTime?) null
					};
					if (record.Length > 1)
					{
						logRecord.Level = TrimString(fields[1]);
					}
					if (record.Length > 2)
					{
						logRecord.Message = AddReturn(TrimString(fields[2]));
					}
					if (record.Length > 3)
					{
						logRecord.Trace = fields[3];
					}
					dataRecords.Add(logRecord);
				}
				return dataRecords;
			}
			catch(Exception)
			{
				return null;
			}
		}

		/// <summary>
		/// トリムしたログデータの文字列を取得する
		/// </summary>
		/// <param name="source">トリムしていないを表す文字列</param>
		/// <returns></returns>
		private static string TrimString(string source)
		{
			var trimed = source.Trim(new char[] { ']' });
			return trimed.TrimStart(new char[] { ' ' });
		}

		/// <summary>
		/// ファイルの保存を実行する
		/// </summary>
		/// <param name="sourcePath">元ファイルのパスを表す文字列</param>
		/// <param name="DestinationPath">保存先ファイルのパスを表す文字列</param>
		public static void Save(string sourcePath, string DestinationPath)
			=> File.Copy(sourcePath, DestinationPath, true);
	}
}
