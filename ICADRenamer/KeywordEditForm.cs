/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using ICADRenamer.Settings;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ICADRenamer.Log;
using NLog;

namespace ICADRenamer
{
	/// <summary>検索規則編集フォームを表すクラス</summary>
	/// <seealso cref="System.Windows.Forms.Form"/>
	public partial class KeywordEditForm : Form
	{
		/// <summary>
		/// データの読み込みと保存を保持するフィールド
		/// </summary>
		private readonly FrameKeywordSerializer _keywordSerializer = new FrameKeywordSerializer();

		/// <summary>
		/// 検索規則データを保持するフィールド
		/// </summary>
		private FrameKeyword _keyword;

		/// <summary>
		///   <see cref="KeywordEditForm"/> classの初期化
		/// </summary>
		public KeywordEditForm() => InitializeComponent();

		/// <summary>
		/// 承諾ボタンイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void AcceptButton_Click(object sender, EventArgs e)
		{
			_keywordSerializer.Save(_keyword);
			//ログ
			RenameLogger.WriteLog(LogMessageKind.Operation, new List<(LogMessageCategory category, string message)>
			{
				(LogMessageCategory.SourceForm, Text),
				(LogMessageCategory.Message,$"{Text}を変更の反映をして終了しました。")
			});
			//
			Close();
		}

		/// <summary>
		/// 新規作成ボタンイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void AddNewButton_Click(object sender, EventArgs e) => AddNewRegexItem();

		/// <summary>
		/// 項目の追加を実行する
		/// </summary>
		private void AddNewRegexItem()
		{
			var regexInputForm = new RegexInputForm(GetButtonIndex());
			//
			if (regexInputForm.ShowDialog() == DialogResult.OK)
			{
				_viewBox.Items.Add(regexInputForm.SelectedItem);
				//ログ
				RenameLogger.WriteLog(LogMessageKind.Operation, new List<(LogMessageCategory category, string message)>
				{
					(LogMessageCategory.SourceForm, Text),
					(LogMessageCategory.Message, "新しい検索規則が追加されました。"),
					(LogMessageCategory.SourcePath, "新規"),
					(LogMessageCategory.DestinationPath, regexInputForm.SelectedItem)
				});
				regexInputForm.Dispose();
			}
		}

		/// <summary>
		/// キャンセルボタンイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void CancelButton_Click(object sender, EventArgs e)
		{
			//
			RenameLogger.WriteLog(LogMessageKind.Operation, new List<(LogMessageCategory category, string message)>
				{
					(LogMessageCategory.SourceForm, Text),
					(LogMessageCategory.Message,$"{Text}を変更せずに閉じました。")
				});
			//
			Close();
		}

		/// <summary>
		/// 新規作成要求イベントイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void CommonContextMenuStrip_AddNewRequest(object sender, EventArgs e) => AddNewRegexItem();

		/// <summary>
		/// 削除要求イベントイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void CommonContextMenuStrip_DeleteRequest(object sender, EventArgs e) => DeleteRegexItem();

		/// <summary>
		/// 編集要求イベントイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void CommonContextMenuStrip_EditRequest(object sender, EventArgs e) => EditRegexItem();

		/// <summary>
		/// コンテキストメニューオープンベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void CommonContextMenuStrip_Opened(object sender, EventArgs e)
		{
			var f = _viewBox.SelectedIndex == -1 ? false : true;
			_commonContextMenuStrip.EditItem.Enabled = f;
			_commonContextMenuStrip.DeleteItem.Enabled = f;
		}

		/// <summary>
		/// データの表示を実行する
		/// </summary>
		private void DataDisplay()
		{
			var index = GetButtonIndex();
			List<string> data = null;
			switch (index)
			{
				case RegexCategory.DrawNumber:
					data = _keyword.DrawNumberRegexes;
					break;
				case RegexCategory.Date:
					data = _keyword.DateRegexes;
					break;
				case RegexCategory.DeltaNote:
					data = _keyword.DeltaNoteRegexes;
					break;
				case RegexCategory.Signature:
					data = _keyword.Signatures;
					break;
			}
			//
			_viewBox.Items.Clear();
			_viewBox.Items.AddRange(data.ToArray());
			_viewBox.Refresh();
		}

		/// <summary>
		/// データ更新を実行する
		/// </summary>
		private void DataUpdate()
		{
			var data = _viewBox.Items.Cast<string>().ToList();
			var index = GetButtonIndex();
			switch (index)
			{
				case RegexCategory.DrawNumber:
					_keyword.DrawNumberRegexes = data;
					break;
				case RegexCategory.Date:
					_keyword.DateRegexes = data;
					break;
				case RegexCategory.DeltaNote:
					_keyword.DeltaNoteRegexes = data;
					break;
				case RegexCategory.Signature:
					_keyword.Signatures = data;
					break;
			}
		}

		/// <summary>
		/// 削除ボタンクリックイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void DeleteButton_Click(object sender, EventArgs e) => DeleteRegexItem();

		/// <summary>
		/// 項目の削除を実行する
		/// </summary>
		private void DeleteRegexItem()
		{
			var result = SystemMethods.GetMessageBox(MessageCategory.Confirm, "選択した項目を削除してもいいですか？");
			if (result != DialogResult.Yes) return;
			//
			var index = _viewBox.SelectedIndex;
			//ログ
			RenameLogger.WriteLog(LogMessageKind.Operation
				, new List<(LogMessageCategory category, string message)>
				{
					(LogMessageCategory.SourceForm,Text),
					(LogMessageCategory.Message,"項目が削除されました。"),
					(LogMessageCategory.ActiveControl,GetCheckBoxText()??string.Empty),
					(LogMessageCategory.OldData,_viewBox.Items[index].ToString())
				});
			//削除
			_viewBox.Items.RemoveAt(index);
			//表示更新
			DataUpdate();
			//ボタン使用可
			_deleteButton.Enabled = false;
		}

		/// <summary>
		/// 項目の編集を実行する
		/// </summary>
		private void EditRegexItem()
		{
			//
			using var regexInputForm = new RegexInputForm(GetButtonIndex());
			//
			if (regexInputForm.ShowDialog() == DialogResult.OK)
			{
				var items = _viewBox.Items;
				items[_viewBox.SelectedIndex] = regexInputForm.SelectedItem;
			}
			regexInputForm.Dispose();
			//
			_viewBox.Refresh();
		}

		/// <summary>
		/// ラジオボタンのインデックスを取得する
		/// </summary>
		/// <returns></returns>
		private RegexCategory GetButtonIndex()
		{
			if (_drawNumberButton.Checked) return RegexCategory.DrawNumber;
			if (_dateButton.Checked) return RegexCategory.Date;
			if (_deltaNoteButton.Checked) return RegexCategory.DeltaNote;
			if (_signatureButton.Checked) return RegexCategory.Signature;
			else return 0;
		}

		/// <summary>
		/// 選択されているチェックボックスのテキストを取得する
		/// </summary>
		/// <returns></returns>
		private string GetCheckBoxText()
		{
			foreach (var box in _groupBox.Controls)
			{
				if (box is CheckBox check)
				{
					if (check.Checked)
					{
						return check.Text;
					}
				}
			}
			return null;
		}
		/// <summary>
		///ヘルプボタンクリックイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void HelpButton_Click(object sender, EventArgs e)
		{
			HelpBrowser helpForm = null;
			try
			{
				helpForm = new HelpBrowser
				{
					Target = $@"{SystemSettings.HelpPath}#RegexScreen"
				};
				helpForm.Show();
			}
			catch (Exception ex)
			{
				helpForm?.Dispose();
				throw ex;
			}
		}

		/// <summary>
		/// フォームロードイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void KeywordEditForm_Load(object sender, EventArgs e)
		{
			_keyword = _keywordSerializer.Load();
			DataDisplay();
		}

		private void KeywordEditForm_Shown(object sender, EventArgs e)
					=> RenameLogger.WriteLog(LogMessageKind.Operation
						, new List<(LogMessageCategory category, string message)>
						{
					(LogMessageCategory.SourceForm,Text),
					(LogMessageCategory.Message,$"{Text}を開きました。")
						});

		/// <summary>
		/// ラジオボタン変更イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void RadioButtons_CheckedChanged(object sender, EventArgs e) => DataDisplay();

		/// <summary>
		/// 保存ボタンクリックイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void SaveButton_Click(object sender, EventArgs e)
		{
			_keywordSerializer.Save(_keyword);
			//メッセージボックスとログ
			const string mes = "保存しました。";
			SystemMethods.GetMessageBox(MessageCategory.Information
				, mes
				, LogMessageKind.Operation
				, new List<(LogMessageCategory category, string message)>
				{
					(LogMessageCategory.SourceForm,Text),
					(LogMessageCategory.Message,mes),
				});
		}

		/// <summary>
		/// リストボックスのダブルクリックイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void ViewBox_DoubleClicked(object sender, EventArgs e) => EditRegexItem();

		/// <summary>
		/// リストボックスのLeaveイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void ViewBox_Leave(object sender, EventArgs e) => DataUpdate();

		/// <summary>
		/// リストボックスのアイテムインデックス変更イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void ViewBox_SelectedIndexChanged(object sender, EventArgs e) => _deleteButton.Enabled = true;
	}
}
