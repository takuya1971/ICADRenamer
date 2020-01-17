/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using ICADRenamer.Log;
using ICADRenamer.Settings;

using NLog;

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ICADRenamer
{
	/// <summary>
	/// よく使うメソッドクラス
	/// </summary>
	public static class SystemMethods
	{
		/// <summary>
		/// フォルダ選択ダイアログの標題を取得する
		/// </summary>
		/// <param name="tag">タグを表す数値</param>
		/// <returns></returns>
		public static string GetFolderBrowseDesc(int tag) => tag switch
		{
			3 => "コピー元",
			4 => "コピー先",
			_ => null,
		};

		/// <summary>
		/// フォルダ選択ダイアログを取得する
		/// </summary>
		/// <param name="desc">標題を表す文字列</param>
		/// <param name="options">オプション設定</param>
		/// <returns></returns>
		public static FolderBrowserDialog GetFolderBrowserDialog(string desc, OptionSettings options)
			=> new FolderBrowserDialog
			{
				Description = desc + "フォルダの選択",
				ShowNewFolderButton = true,
				SelectedPath = options.DefaultFolder,
				RootFolder = Environment.SpecialFolder.Desktop
			};

		/// <summary>
		/// コントロールからラベルのテキストを取得する
		/// </summary>
		/// <param name="control">フォーム</param>
		/// <param name="tag">タグ</param>
		/// <returns></returns>
		public static string GetLabelText(Control control, object tag)
		{
			if (control.HasChildren)
			{
				foreach (Control c in control.Controls)
				{
					var result = GetLabelText(c, tag);
					if (result != null) return result;
				}
			}
			else
			{
				if (control.Tag == tag)
				{
					if (control.GetType() == typeof(Label))
					{
						return control.Text;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// ライセンス情報の文字列を取得する
		/// </summary>
		/// <returns></returns>
		public static string GetLisence()
		{
			var path = $@"{Environment.CurrentDirectory}\License.txt";
			string text;
			using (var sw = new StreamReader(path))
			{
				text = sw.ReadToEnd();
			}
			return text;
		}

		/// <summary>
		/// メッセージボックスの実行結果を取得する
		/// </summary>
		/// <param name="category">メッセージボックス区分</param>
		/// <param name="message">メッセージを表す文字列</param>
		/// <param name="kind">ログメッセージ種類</param>
		/// <param name="messages">ログメッセージリスト</param>
		/// <returns></returns>
		public static DialogResult GetMessageBox(MessageCategory category
			, string message
			, LogMessageKind? kind = null
			, List<(LogMessageCategory category, string message)> messages = null)
		{
			MessageBoxIcon icon = MessageBoxIcon.Information;
			MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
			string caption = "情報";

			if ((int) category > 1000 && (int) category < 2000)
			{
				icon = MessageBoxIcon.Error;
				buttons = MessageBoxButtons.OK;
			}

			switch (category)
			{
				case MessageCategory.Information:
					caption = "情報";
					buttons = MessageBoxButtons.OK;
					break;
				case MessageCategory.Confirm:
					caption = "確認";
					break;
				case MessageCategory.InputError:
					caption = "入力エラー";
					break;
				case MessageCategory.Error:
					caption = "エラー";
					break;
			}
			//
			LogLevel level = category switch
			{
				MessageCategory.Information => LogLevel.Info,
				MessageCategory.InputError => LogLevel.Trace,
				MessageCategory.Error => LogLevel.Error,
				_ => null
			};
			//
			if (level != null)
			{
				RenameLogger.WriteLog(kind, messages);
			}
			//
			return MessageBox.Show(message, caption, buttons, icon);
		}
	}
}


