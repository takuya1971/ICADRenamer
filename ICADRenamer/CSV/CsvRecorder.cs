/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

using CsvHelper;
using CsvHelper.Configuration;

namespace ICADRenamer.CSV
{
	/// <summary>
	/// CSVレコーダを表すクラス
	/// </summary>
	public class CsvRecorder
	{
		/// <summary>
		///   <see cref="CsvRecorder"/> classの初期化
		/// </summary>
		/// <param name="filePath">を表す文字列</param>
		public CsvRecorder(string filePath) => FilePath = filePath;

		/// <summary>
		/// 保存するファイルパスを保持するプロパティ
		/// </summary>
		public string FilePath { get; private set; }

		/// <summary>
		/// 書き込み設定を実行する
		/// </summary>
		/// <param name="config">設定</param>
		public static void SetWriteConfig(ref IWriterConfiguration config)
		{
			config.HasHeaderRecord = true;
			config.TypeConverterCache.AddConverter<bool>(new BooleanJapaneseConverter());
		}

		/// <summary>
		/// CSVファイルからの読み込みを実行する
		/// </summary>
		/// <returns></returns>
		public IEnumerable<CsvRecordItem> Read()
		{
			IEnumerable<CsvRecordItem> items;
			using var cr = new CsvReader(new StreamReader(FilePath), CultureInfo.CurrentUICulture);
			items = cr.GetRecords<CsvRecordItem>();
			return items;
		}

		/// <summary>
		/// CSVファイルへの1行書き込みを実行する
		/// </summary>
		/// <param name="item">書き込むアイテム</param>
		public void Write(CsvRecordItem item)
		{
			var sw = new StreamWriter(FilePath, true, Encoding.UTF8);
			try
			{
				using var cw = new CsvWriter(sw, CultureInfo.CurrentUICulture);
				var config = cw.Configuration;
				SetWriteConfig(ref config);
				cw.WriteRecord<CsvRecordItem>(item);
			}
			finally
			{ sw?.Dispose(); }
		}

		/// <summary>
		/// CSVファイルへの全部の書き込みを実行する
		/// </summary>
		/// <param name="items">アイテムリスト</param>
		public void WriteAll(IEnumerable<CsvRecordItem> items)
		{
			var sw = new StreamWriter(FilePath, false, Encoding.UTF8);
			try
			{
				using var cw = new CsvWriter(sw, CultureInfo.CurrentUICulture);
				var config = cw.Configuration;
				SetWriteConfig(ref config);
				cw.WriteRecords(items);
			}
			finally
			{ sw?.Dispose(); }
		}
	}
}
