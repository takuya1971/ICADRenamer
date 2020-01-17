/* ICAD Renamer LogViewer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace LogViewer
{
	/// <summary>
	/// セルのクリップボードコピーをExcel互換にするクラス
	/// </summary>
	/// <seealso cref="System.Windows.Forms.DataGridView" />
	public class DataGridViewEx : DataGridView
	{
		/// <summary>
		///   <see cref="T:System.Windows.Forms.Clipboard" /> にコピーするために、選択されたセルの内容を表す書式設定された値を取得します。
		/// </summary>
		/// <returns>
		/// 選択されたセルの内容を表す <see cref="T:System.Windows.Forms.DataObject" />。
		/// </returns>
		public override DataObject GetClipboardContent()
		{
			//元データ
			var oldData = base.GetClipboardContent();
			//新しいDataObjectを作成する
			var newData = new DataObject();
			//テキスト形式のデータをセットする（UnicodeTextもセットされる）
			newData.SetData(DataFormats.Text, oldData.GetData(DataFormats.Text));
			//HTML形式のデータを取得する
			object htmlObj = oldData.GetData(DataFormats.Html);
			MemoryStream htmlStrm = null;
			if (htmlObj is string)
			{
				//String型の時は、MemoryStreamに変換する
				htmlStrm = new MemoryStream(
					Encoding.UTF8.GetBytes((string) htmlObj));
			}
			else if (htmlObj is MemoryStream)
			{
				//.NET Framework 4.0以降では、MemoryStreamとなる
				htmlStrm = (MemoryStream) htmlObj;
			}
			if (htmlStrm != null)
			{
				//HTML形式のデータをセットする
				newData.SetData(DataFormats.Html, htmlStrm);
			}

			//CSV形式のデータを取得する
			object csvObj = oldData.GetData(DataFormats.CommaSeparatedValue);
			MemoryStream csvStrm = null;
			if (csvObj is string)
			{
				//MemoryStreamに変換する
				csvStrm = new MemoryStream(
					Encoding.Default.GetBytes((string) csvObj));
			}
			else if (csvObj is MemoryStream)
			{
				//今のところこうなることはないが、将来を見据えて...
				csvStrm = (MemoryStream) csvObj;
			}
			if (csvStrm != null)
			{
				//CSV形式のデータをセットする
				newData.SetData(DataFormats.CommaSeparatedValue, csvStrm);
			}

			return newData;
		}
	}
}
