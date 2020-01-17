/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using ICADRenamer.Log;
using ICADRenamer.Settings;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ICADRenamer
{
	/// <summary>
	/// 正規表現の入力とテストを表すクラス
	/// </summary>
	/// <seealso cref="System.Windows.Forms.Form" />
	public partial class RegexInputForm : Form
	{
		/// <summary>
		/// 規則区分を保持するフィールド
		/// </summary>
		private readonly RegexCategory _category;

		/// <summary>
		/// 検索規則キーワードを保持するフィールド
		/// </summary>
		private readonly FrameKeyword _keyword;

		/// <summary>
		///   <see cref="RegexInputForm"/> classの初期化
		/// </summary>
		/// <param name="regexCategory">検索区分</param>
		public RegexInputForm(RegexCategory regexCategory)
		{
			_category = regexCategory;
			if (_category != RegexCategory.NewProject) _keyword = new FrameKeywordSerializer().Load();
			//
			InitializeComponent();
		}

		/// <summary>
		/// 選択した規則を保持するプロパティ
		/// </summary>
		public string SelectedItem { get; private set; }

		/// <summary>
		/// 承諾ボタンクリックイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void AcceptButton_Click(object sender, EventArgs e)
		{
			SelectedItem = _regexBox.Text;
			DialogResult = DialogResult.OK;
			Close();
		}

		/// <summary>
		/// Bテキストボックスの検証イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void Boxes_Validated(object sender, EventArgs e)
		{
			TextBox box = (TextBox) sender;
			//ログ
			RenameLogger.WriteLog(LogMessageKind.ActionComplete,
				new List<(LogMessageCategory category, string message)>
				{
					(LogMessageCategory.SourceForm,Text),
					(LogMessageCategory.Message,"入力されました。"),
					(LogMessageCategory.ActiveControl,SystemMethods.GetLabelText(this,box.Tag)),
					(LogMessageCategory.NewData,box.Text)
				});
		}

		/// <summary>
		/// キャンセルボタンクリックイベントを実行する
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
					(LogMessageCategory.Message,"入力をキャンセルしました。")
				});
			Close();
		}

		/// <summary>
		/// ヘルプボタンクリックイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void HelpButton_Click(object sender, EventArgs e)
		{
			var helpForm = new HelpBrowser
			{
				Target = @"Help\Help.html#RegexDescription"
			};
			helpForm.Show();
		}

		/// <summary>
		/// テスト区分のインデックス変更イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void MethodComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			bool flag;
			switch (_methodComboBox.SelectedIndex)
			{
				case 0:
				case 2:
					flag = false;
					break;
				case 1:
				default:
					flag = true;
					break;
			}
			_replaceBox.Enabled = flag;
			//ログ
			RenameLogger.WriteLog(LogMessageKind.Operation
				, new List<(LogMessageCategory category, string message)>
				{
					(LogMessageCategory.SourceForm,Text),
					(LogMessageCategory.Message,"テスト区分が変更されました。"),
					(LogMessageCategory.NewData,_methodComboBox.SelectedItem.ToString())
				});
		}

		/// <summary>
		/// フォームロードイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void RegexInputForm_Load(object sender, EventArgs e)
		{
			List<string> item = new List<string>() { "一致判断" };

			if (_category != RegexCategory.DeltaNote) item.Add("置き換え");
			if (_category == RegexCategory.NewProject) item.Add("一致抽出");
			//
			_methodComboBox.Items.Clear();
			_methodComboBox.Items.AddRange(item.ToArray());
			_methodComboBox.SelectedIndex = 0;
		}

		/// <summary>
		/// フォーム表示イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void RegexInputForm_Shown(object sender, EventArgs e)
					=> RenameLogger.WriteLog(LogMessageKind.Operation
						, new List<(LogMessageCategory category, string message)>
						{
					(LogMessageCategory.SourceForm,Text),
					(LogMessageCategory.Message,$"{Text}が開きました。")
						});

		/// <summary>
		/// テストボタンクリックkイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void TestButton_Click(object sender, EventArgs e)
		{
			//規則リスト
			List<string> regex = new List<string>();
			switch (_category)
			{
				case RegexCategory.Date:
				case RegexCategory.Signature:
					regex.Add($"^{_regexBox.Text}$");
					break;
				case RegexCategory.DrawNumber:
					regex.Add($"^{_regexBox.Text}");
					break;
				case RegexCategory.DeltaNote:
					foreach (var s in _keyword.Signatures)
					{
						regex.Add($"{_regexBox.Text}{s}$");
					}
					break;
				case RegexCategory.NewProject:
					regex.Add(_regexBox.Text);
					break;
			}
			//
			string answer;
			switch (_methodComboBox.SelectedIndex)
			{
				case 0:
					answer = regex.Exists(
						x => Regex.IsMatch(_inputBox.Text, x) == true) ? "一致" : "不一致";
					break;
				case 1:
					answer = Regex.Replace(_inputBox.Text, regex[0], _replaceBox.Text);
					break;
				case 2:
					answer = Regex.Match(_inputBox.Text, regex[0]).Value;
					break;
				default:
					return;
			}
			_resultBox.Text = answer;
			//ログ
			RenameLogger.WriteLog(LogMessageKind.ActionComplete
				, new List<(LogMessageCategory category, string message)>
				{
					(LogMessageCategory.SourceForm,Text),
					(LogMessageCategory.Message,"テストしました。"),
					(LogMessageCategory.Regex,_regexBox.Text),
					(LogMessageCategory.TestCategory,_methodComboBox.SelectedItem.ToString()),
					(LogMessageCategory.TestSource,_inputBox.Text),
					(LogMessageCategory.ReplaceText,_replaceBox.Enabled?_replaceBox.Text:"なし"),
					(LogMessageCategory.RegexResult,_resultBox.Text)
				});
		}

		/// <summary>
		/// テキストボックスのLeaveイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void TextBoxes_Leave(object sender, EventArgs e)
		{
			//
			if (_regexBox.Text.Length > 0) _acceptButton.Enabled = true;
			else _acceptButton.Enabled = false;
		}
	}
}
