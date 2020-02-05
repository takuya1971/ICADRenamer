/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using ICADRenamer.Settings;

using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ICADRenamer.Log;
using System.Collections.Generic;

namespace ICADRenamer
{
	/// <summary>
	/// 設定フォームを表すクラス
	/// </summary>
	/// <seealso cref="System.Windows.Forms.Form" />
	public partial class OptionSettingForm : Form
	{
		/// <summary>
		/// 読み出し・保存するオブジェクトを保持するフィールド
		/// </summary>
		private readonly OptionSettingsSerializer _serializer = new OptionSettingsSerializer();

		/// <summary>
		///   <see cref="OptionSettingForm"/> classの初期化
		/// </summary>
		public OptionSettingForm() => InitializeComponent();

		/// <summary>
		/// 設定を保持するプロパティ
		/// </summary>
		public OptionSettings Option { get; private set; }

		/// <summary>
		/// OKボタンイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void AcceptButton_Click(object sender, EventArgs e)
		{
			DataUpdate();
			_serializer.Save(Option);
			//
			DialogResult = DialogResult.OK;
			//ログ
			RenameLogger.WriteLog(LogMessageKind.Operation,
				new List<(LogMessageCategory category, string message)>
				{
					(LogMessageCategory.SourceForm,Text),
					(LogMessageCategory.Message,"変更を保存して終了しました。")
				});
			Close();
		}

		/// <summary>
		/// 新規追加ボタンクリックイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void AddNewButton_Click(object sender, EventArgs e) => AddNewRegex();

		/// <summary>
		/// アイテムの新規作成を実行する
		/// </summary>
		private void AddNewRegex()
		{
			using var regexInputForm = new RegexInputForm(RegexCategory.NewProject);
			//
			if (regexInputForm.ShowDialog() == DialogResult.OK)
			{
				_regexBox.Items.Add(regexInputForm.SelectedItem);
				//ログ
				RenameLogger.WriteLog(LogMessageKind.ActionComplete
					, new List<(LogMessageCategory category, string message)>
					{
					(LogMessageCategory.SourceForm,"Text"),
					(LogMessageCategory.Message,"新しい入力規則が追加されました。"),
					(LogMessageCategory.NewData,regexInputForm.SelectedItem)
					});
			}
			//
			regexInputForm.Dispose();
		}

		/// <summary>
		/// チェックボックスの状態変更イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void Boxes_CheckedChanged(object sender, EventArgs e)
		{
			var box = (CheckBox) sender;
			//ログ
			RenameLogger.WriteLog(LogMessageKind.Operation
				, new System.Collections.Generic.List<(LogMessageCategory category, string message)>
				{
					(LogMessageCategory.SourceForm, Text),
					(LogMessageCategory.Message,"値が変更されました。"),
					(LogMessageCategory.ActiveControl,box.Text),
					(LogMessageCategory.NewData,GetCheckBoxText(box.Checked))
				});
		}

		/// <summary>
		/// テキストボックス検証イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void Boxes_Validated(object sender, EventArgs e)
		{
			var box = (TextBox) sender;
			//ログ
			RenameLogger.WriteLog(LogMessageKind.Operation
				, new System.Collections.Generic.List<(LogMessageCategory category, string message)>
				{
					(LogMessageCategory.SourceForm,Text),
					(LogMessageCategory.Message,"入力が変更されました。"),
					(LogMessageCategory.ActiveControl,SystemMethods.GetLabelText(this,box.Text)?? string.Empty),
					(LogMessageCategory.NewData,box.Text)
				});
		}

		/// <summary>
		/// キャンセルボタンイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void CancelButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			//ログ
			RenameLogger.WriteLog(LogMessageKind.Operation
				, new List<(LogMessageCategory category, string message)>
				{
					(LogMessageCategory.SourceForm,Text),
					(LogMessageCategory.Message,"変更を保存せずに終了しました。")
				});
			Close();
		}

		/// <summary>
		/// 編集内容の更新を実行する
		/// </summary>
		private void DataUpdate()
		{
			Option.DefaultFolder = _defaultFolderBox.Text;
			Option.CanDeleteDelta = _isDeleteDelta.Checked;
			Option.CanExecuteUpdate = _isUpdate.Checked;
			Option.UseProjectCheck = _isValidateProject.Checked;
			Option.ICADMinimize = _isIcadMinimize.Checked;
			Option.ICADLinkPort = Convert.ToInt32(_iCADLinkBox.Text);
			Option.UserName = _userBox.Text;
			Option.NewProjectRegex = _regexBox.Items.Cast<string>().ToList();
			Option.IsYear4Digit = _isYear4digitBox.Checked;
			Option.IsMonthAndDate2Digit = _isMonthAndDate2DigitBox.Checked;
			Option.IsDateSeparatorSlash = _isDateSeparatorSlashBox.Checked;
			Option.ResitTo3DSeihin = _resit3dSeihinBox.Checked;
		}

		/// <summary>
		/// 削除ボタンクリックイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void DeleteButton_Click(object sender, EventArgs e) => DeleteRegexItem();

		/// <summary>
		/// アイテムの削除を実行する
		/// </summary>
		private void DeleteRegexItem()
		{
			var result = SystemMethods.GetMessageBox(MessageCategory.Confirm, "選択した項目を削除してもいいですか？");
			if (result != DialogResult.Yes) return;
			var index = _regexBox.SelectedIndex;
			//ログ
			RenameLogger.WriteLog(LogMessageKind.ActionComplete,
				new List<(LogMessageCategory category, string message)>
				{
					(LogMessageCategory.SourceForm,Text),
					(LogMessageCategory.Message,"入力規則が削除されました。"),
					(LogMessageCategory.OldData,_regexBox.Items[index].ToString())
				});

			_regexBox.Items.RemoveAt(index);
			DataUpdate();
			_deleteButton.Enabled = false;
		}

		/// <summary>
		/// 内容の表示を実行する
		/// </summary>
		private void DisplayUpdate()
		{
			_iCADLinkBox.Text = Option.ICADLinkPort.ToString();
			_defaultFolderBox.Text = Option.DefaultFolder;
			_userBox.Text = Option.UserName;
			_isDeleteDelta.Checked = Option.CanDeleteDelta;
			_isUpdate.Checked = Option.CanExecuteUpdate;
			_isValidateProject.Checked = Option.UseProjectCheck;
			_isIcadMinimize.Checked = Option.ICADMinimize;
			_isYear4digitBox.Checked = Option.IsYear4Digit;
			_isMonthAndDate2DigitBox.Checked = Option.IsMonthAndDate2Digit;
			_isDateSeparatorSlashBox.Checked = Option.IsDateSeparatorSlash;
			_resit3dSeihinBox.Checked = Option.ResitTo3DSeihin;
			_regexBox.Items.Clear();
			_regexBox.Items.AddRange(Option.NewProjectRegex.ToArray());
		}

		/// <summary>
		/// アイテムの新規作成を実行する
		/// </summary>
		private void EditRegexItem()
		{
			//
			using var regexInputForm = new RegexInputForm(RegexCategory.NewProject);
			//
			if (regexInputForm.ShowDialog() == DialogResult.OK)
			{
				var items = _regexBox.Items;
				items[_regexBox.SelectedIndex] = regexInputForm.SelectedItem;
			}
			regexInputForm.Dispose();
			//
			_regexBox.Refresh();
		}

		/// <summary>
		/// フォルダ検索ボタンイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void FolderBrowseButton_Click(object sender, EventArgs e)
		{
			var dialog = SystemMethods.GetFolderBrowserDialog("既定の", Option);
			if (dialog.ShowDialog() != DialogResult.OK) return;
			//
			_defaultFolderBox.Text = dialog.SelectedPath;
			dialog.Dispose();
		}

		/// <summary>
		/// 全般ページのEnterイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void GeneralPage_Enter(object sender, EventArgs e)
		{
			_addNewButton.Visible = false;
			_deleteButton.Visible = false;
		}

		/// <summary>
		/// チェックボックスの状態文字列を取得する
		/// </summary>
		/// <param name="flag">チェックかどうかを表す値</param>
		/// <returns></returns>
		private string GetCheckBoxText(bool flag) => flag ? "する" : "しない";

		/// <summary>
		/// ヘルプボタンクリックイベントを実行する
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
					Target = $@"{SystemSettings.HelpPath}#OptionSettingDialog"
				};
				helpForm.Show();
			}
			catch (Exception)
			{
				if (!helpForm.IsDisposed) helpForm.Dispose();
				throw;
			}
		}

		/// <summary>
		/// ICADLinkBoxの検証イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void ICADLinkBox_Validating(object sender, CancelEventArgs e)
		{
			if (!Int32.TryParse(_iCADLinkBox.Text, out int _))
			{
				SystemMethods.GetMessageBox(MessageCategory.InputError, @"数字以外入力できません。\r\n既定値は3999です。");
				e.Cancel = true;
			}
		}

		/// <summary>
		///フォームクローズイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void OptionSettingForm_FormClosed(object sender, FormClosedEventArgs e) =>
			//ログ
			RenameLogger.WriteLog(LogMessageKind.Operation,
				new System.Collections.Generic.List<(LogMessageCategory category, string message)>
				{
					(LogMessageCategory.SourceForm,Text),
					(LogMessageCategory.Message,"終了しました。")
				});

		/// <summary>
		/// フォーム表示イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void OptionSettingForm_Shown(object sender, EventArgs e) =>
			//ログ
			RenameLogger.WriteLog(LogMessageKind.Operation,
				new System.Collections.Generic.List<(LogMessageCategory category, string message)>
				{
					(LogMessageCategory.SourceForm,Text),
					(LogMessageCategory.Message,"表示されました。")
				});

		/// <summary>
		/// リストボックスのダブルクリックイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void RegexBox_DoubleClick(object sender, EventArgs e) => EditRegexItem();

		/// <summary>
		/// リストボックスの選択変更イベントイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void RegexBox_SelectedIndexChanged(object sender, EventArgs e) => _deleteButton.Enabled = true;

		/// <summary>
		/// 新規作成要求イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void RegexContextMenuStrip_AddNewRequest(object sender, EventArgs e) => AddNewRegex();

		/// <summary>
		/// 削除要求イベントイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void RegexContextMenuStrip_DeleteRequest(object sender, EventArgs e) => DeleteRegexItem();

		/// <summary>
		/// 編集要求イベントイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void RegexContextMenuStrip_EditRequest(object sender, EventArgs e) => EditRegexItem();

		/// <summary>
		/// コンテキストメニューオープン時イベントを実行する。
		/// <para>コンテキストメニューのEnabledを制御する</para>
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void RegexContextMenuStrip_Opened(object sender, EventArgs e)
		{
			if (_tabControl1.SelectedTab == _regexPage)
			{
				_regexContextMenuStrip.AddNewItem.Enabled = true;
				var f = _regexBox.SelectedIndex == -1 ? false : true;
				_regexContextMenuStrip.EditItem.Enabled = f;
				_regexContextMenuStrip.DeleteItem.Enabled = f;
			}
			else
			{
				foreach (ToolStripMenuItem item in _regexContextMenuStrip.Items)
				{
					item.Enabled = false;
				}
			}
		}
		/// <summary>
		/// 入力規則ページのEnterイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void RegexPage_Enter(object sender, EventArgs e)
		{
			_addNewButton.Visible = true;
			_deleteButton.Visible = true;
		}

		/// <summary>
		/// フォームのロードイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void SettingForm_Load(object sender, EventArgs e)
		{
			Option = _serializer.Load();
			DisplayUpdate();
		}
	}
}
