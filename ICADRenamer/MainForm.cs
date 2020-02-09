/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using ICADRenamer.Log;
using ICADRenamer.Settings;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Diagnostics;

namespace ICADRenamer
{
	/// <summary>
	/// メインフォームを表すクラス
	/// </summary>
	/// <seealso cref="System.Windows.Forms.Form" />
	public partial class MainForm : Form

	{
		/// <summary>
		/// オプション設定を保持するフィールド
		/// </summary>
		private OptionSettings _settings;

		/// <summary>
		///   <see cref="MainForm"/> classの初期化
		/// </summary>
		public MainForm(CommandLineArgs args)
		{
			if (args.UseConsole) RenameLogger.ConsoleMode = true;
			if (args.UseDebugMode) RenameLogger.DebugMode = true;

			_settings = new OptionSettingsSerializer().Load();
			InitializeComponent();
		}

		/// <summary>
		/// アプリケーションの戻り値を保持するプロパティ
		/// </summary>
		public int Result { get; private set; } = 0;

		/// <summary>
		/// テキストボックスの検証イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void Boxes_Validated(object sender, EventArgs e)
		{
			var box = (TextBox) sender;
			//ログ
			RenameLogger.WriteLog(LogMessageKind.Operation, new List<(LogMessageCategory category, string message)>
			{
				(LogMessageCategory.SourceForm,Text),
				(LogMessageCategory.Message,"入力が変更されました。"),
				(LogMessageCategory.ActiveControl,(SystemMethods.GetLabelText(this,box.Tag))),
				(LogMessageCategory.NewData,box.Text)
			});
			if (Equals(box, _sourceBox) || Equals(box, _destinationBox))
			{
				//
				DisplayFiles((TextBox) sender);
			}
			_executeButton.Enabled = ExecuteEnableCheck();
		}

		/// <summary>
		/// フォルダ選択イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">イベント引数</param>
		private void BrowseButton_Click(object sender, EventArgs e)
		{
			//ボタン
			var button = (Button) sender;
			//タグ
			var tag = Convert.ToInt32(button.Tag);
			//フォルダ選択ダイアログ
			var dialog = SystemMethods.GetFolderBrowserDialog(SystemMethods.GetFolderBrowseDesc(tag), _settings);
			//ダイアログがキャンセルされたらスキップ
			if (dialog.ShowDialog() != DialogResult.OK)
			{
				dialog.Dispose();
				return;
			}

			//テキストボックス
			TextBox box = (tag) switch
			{
				//コピー元
				3 => _sourceBox,
				//コピー先
				4 => _destinationBox,
				_ => null
			};
			if (tag == 3)
			{
				//ICADファイルがない場合
				try
				{
					if (Directory.GetFiles(dialog.SelectedPath, SystemSettings.IcadExtension, SearchOption.AllDirectories).Length == 0)
					{
						//エラー表示
						const string mes = "選択したフォルダにはICADファイルがありません。";
						//メッセ―ジとログ
						SystemMethods.GetMessageBox(MessageCategory.InputError
							, mes
							, LogMessageKind.Operation
							, new List<(LogMessageCategory category, string message)>
							{
							(LogMessageCategory.SourceForm,Text),
							(LogMessageCategory.Message,mes),
							(LogMessageCategory.FilePath,dialog.SelectedPath),
						});
						//選択したパスを消す
						dialog.SelectedPath = string.Empty;
						return;
					}
				}
				catch (Exception) { }
			}
			//フォルダが選択されていた時
			if (dialog.SelectedPath.Length > 0)
			{
				//フォルダ元パスボックスにパスを入力
				box.Text = dialog.SelectedPath;
				//ログ
				RenameLogger.WriteLog(LogMessageKind.ActionComplete
					, new List<(LogMessageCategory category, string message)>
					{
						(LogMessageCategory.SourceForm,Text),
						(LogMessageCategory.Message,"フォルダが選択されました。"),
						(LogMessageCategory.ActiveControl,SystemMethods.GetLabelText(box,box.Tag)),
						(LogMessageCategory.NewData,box.Text)
					});
				//ファイルリストを表示
				SetFileList(box);
			}
			_executeButton.Enabled = ExecuteEnableCheck();
			dialog.Dispose();
		}

		/// <summary>
		/// フォームを閉じるイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void CloseButton_Click(object sender, EventArgs e) => Close();

		/// <summary>
		/// ファイルビューへの表示を実行する
		/// </summary>
		/// <param name="box">sourceBoxまたはdestinationBox</param>
		private void DisplayFiles(TextBox box)
		{
			ListBox view;
			if (box.Equals(_sourceBox))
			{
				view = _sourceFileView;
			}
			else
			{
				view = _destinationFileView;
			}
			view.Items.Clear();
			//
			var files = Directory.GetFiles(box.Text,
				SystemSettings.IcadExtension,
				SearchOption.AllDirectories);
			//
			if (files.Length > 0)
			{
				var names = files.Select(x => Path.GetFileName(x)).ToArray();
				view.Items.AddRange(names);
			}
		}

		/// <summary>実行ボタンイベント</summary>
		/// <param name="sender">呼び出し元オブジェクト</param>
		/// <param name="e">イベント引数</param>
		private void ExecuteButton_Click(object sender, EventArgs e)
		{
			//ボタンを押せないように
			_executeButton.Enabled = false;
			_closeButton.Enabled = false;
			//進捗フォームの作成
			var progressForm = new ExecuteProgressForm(new RenameExecuteParams
			{
				//コピー元
				SourcePath = _sourceBox.Text,
				//コピー先
				DestinationPath = _destinationBox.Text,
				//M番
				PrefixName = _newProjectBox.Text,
				//設定
				Settings = _settings,
				//署名
				Signature = _signatureBox.Text
			});
			//実行完了イベント
			progressForm.ExecuteFinished += ProgressForm_ExecuteFinished;
			progressForm.Prepared += ProgressForm_Prepared;
			//フォーム表示
			progressForm.Show();
		}

		private void ProgressForm_Prepared(object sender, EventArgs e)
		{
			if (sender is Form progressForm)
			{
				progressForm.Show();
			}
		}

		/// <summary>
		/// 実行可能かどうかを返す
		/// </summary>
		/// <returns></returns>
		private bool ExecuteEnableCheck()
			=> (Directory.Exists(_sourceBox.Text)
				&& Directory.Exists(_destinationBox.Text)
				&& _newProjectBox.Text.Length > 0
				&& _signatureBox.Text.Length > 0);

		/// <summary>
		/// メインフォームのClosedイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void MainForm_FormClosed(object sender,
								   FormClosedEventArgs e)
		{
			NativeMethods.FreeConsole();
			RenameLogger.WriteLog(LogMessageKind.Operation
				, new List<(LogMessageCategory category, string message)>
				{
					(LogMessageCategory.SourceForm,Text),
					(LogMessageCategory.Message,"ICAD_Renamerを終了しました。")
				});
		}

		/// <summary>
		/// フォームロードイベント
		/// </summary>
		/// <param name="sender">呼び出し元オブジェクト</param>
		/// <param name="e">イベント引数</param>
		private void MainForm_Load(object sender, EventArgs e) => _signatureBox.Text = _settings.UserName;

		/// <summary>
		/// メインフォームの表示イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void MainForm_Shown(object sender, EventArgs e) =>
			//ログ
			RenameLogger.WriteLog(LogMessageKind.Operation
				, new List<(LogMessageCategory category, string message)>
				{
					(LogMessageCategory.SourceForm,Text),
					(LogMessageCategory.Message,"ICAD_Renamerを開始しました。")
				});

		/// <summary>
		/// M番ボックスLeaveイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void NewProjectBox_Leave(object sender, EventArgs e) => _newProjectBox.Text = _newProjectBox.Text.ToUpper();

		/// <summary>
		/// M番ボックスの検証イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void NewProjectBox_Validating(object sender, CancelEventArgs e)
		{
			//入力がブランクだったら抜ける
			if (_newProjectBox.Text.Length == 0)
			{
				return;
			}
			//M番チェックなしだったら抜ける
			if (!_settings.UseProjectCheck) return;
			//M番チェック
			foreach (var pattern in _settings.NewProjectRegex)
			{
				if (Regex.IsMatch(_newProjectBox.Text, pattern))
				{
					return;
				}
			}
			const string message = "M番の入力規則に違反しています。";
			//メッセージボックス
			SystemMethods.GetMessageBox(MessageCategory.InputError
				, message
				, LogMessageKind.Operation
				, new List<(LogMessageCategory category, string message)>
				{
					(LogMessageCategory.SourceForm,Text),
					(LogMessageCategory.Message,message),
					(LogMessageCategory.ActiveControl,SystemMethods.GetLabelText(_newProjectBox,_newProjectBox.Tag)),
					(LogMessageCategory.NewData,_newProjectBox.Text)
				});
			//検証キャンセル
			e.Cancel = true;
		}

		/// <summary>
		/// コピー元ボックス検証イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void PathBox_Validating(object sender, CancelEventArgs e)
		{
			var box = (TextBox) sender;
			if (box.Text.Length == 0)
			{
				e.Cancel = false;
				return;
			}
			if (!Directory.Exists(_sourceBox.Text))
			{
				//メッセージ
				const string Message = "フォルダパスが存在しません。";
				//メッセージボックス
				SystemMethods.GetMessageBox(MessageCategory.InputError
					, Message
					, LogMessageKind.Operation
					, new List<(LogMessageCategory category, string message)>
					{
						(LogMessageCategory.SourceForm,Text),
						(LogMessageCategory.Message,Message),
						(LogMessageCategory.ActiveControl,SystemMethods.GetLabelText(box,box.Tag)),
						(LogMessageCategory.NewData,box.Text)
					});
				e.Cancel = true;
				return;
			}
			if (Equals(box, _sourceBox))
			{
				if (Directory.GetFiles(_sourceBox.Text, SystemSettings.IcadExtension).Length == 0)
				{
					//メッセージ
					const string Message = "ファイルがありません。";
					//ログメッセージ
					SystemMethods.GetMessageBox(MessageCategory.InputError
						, Message
						, LogMessageKind.Operation
						, new List<(LogMessageCategory category, string message)>
						{
							(LogMessageCategory.SourceForm,Text),
							(LogMessageCategory.Message,Message),
							(LogMessageCategory.ActiveControl,SystemMethods.GetLabelText(box,box.Tag)),
							(LogMessageCategory.NewData,box.Text)
						});
					e.Cancel = true;
					return;
				}
			}
		}

		/// <summary>変換実行完了イベント</summary>
		/// <param name="sender">呼び出し元オブジェクト</param>
		/// <param name="e">イベント引数</param>
		private void ProgressForm_ExecuteFinished(object sender, EventArgs e)
		{
			if (sender is ExecuteProgressForm form)
			{
				//結果表示の確認
				var result = SystemMethods.GetMessageBox(
				MessageCategory.Confirm
				, "処理が終了しました。\r\n結果を表示しますか？"
				, LogMessageKind.ActionComplete
				, new List<(LogMessageCategory category, string message)>
				{
					(LogMessageCategory.Message,"処理完了。"),
				});
				//結果表示の可否
				if (result == DialogResult.Yes)
				{
					//ファイルを開く
					Process.Start(form.ResultFilePath);
				}
				//form?.Dispose();
			}
			_executeButton.Enabled = true;
			_closeButton.Enabled = true;
		}

		/// <summary>
		/// 指定フォルダ以下のファイル名のリストボックスへの表示を実行する
		/// </summary>
		/// <param name="pathBox">テキストボックス</param>
		private void SetFileList(TextBox pathBox)
		{
			string[] files;
			//ファイルの取得
			try
			{
				files = Directory.GetFiles(pathBox.Text, SystemSettings.IcadExtension, SearchOption.AllDirectories);
			}
			catch (UnauthorizedAccessException)
			{
				const string errMes = @"ファイルが取得できません。アクセス権のないフォルダです。";
				//ログ
				SystemMethods.GetMessageBox(MessageCategory.Error
					, errMes
					, LogMessageKind.Operation
					, new List<(LogMessageCategory category, string message)>
					{
						(LogMessageCategory.SourceForm,Text),
						(LogMessageCategory.Message,errMes),
						(LogMessageCategory.ActiveControl,SystemMethods.GetLabelText(pathBox,pathBox.Tag)),
						(LogMessageCategory.NewData,pathBox.Text)
					});
				pathBox.Text = "";
				return;
			}
			//ファイルの表示
			DisplayFiles(pathBox);
			return;
		}

		/// <summary>
		/// 検索規則エディタボタンクリックイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void ToolStripEditorButton_Click(object sender, EventArgs e)
		{
			using var editForm = new KeywordEditForm();
			editForm.ShowDialog();
		}

		/// <summary>
		/// ヘルプボタンクリックイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void ToolStripHelpButton_Click(object sender, EventArgs e)
		{
			var target = $"{SystemSettings.HelpPath}";
			HelpBrowser helpForm = null;
			try
			{
				helpForm = new HelpBrowser
				{
					Target = target
				};
			}
			catch (Exception ex)
			{
				helpForm?.Dispose();
				throw ex;
			}
			helpForm.Show();
		}

		/// <summary>
		/// 情報ボタンクリックイベントイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void ToolStripInformationButton_Click(object sender, EventArgs e)
		{
			using var informationForm = new AboutBox();
			informationForm.ShowDialog();
		}

		/// <summary>
		/// 設定ボタンイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void ToolStripSettingButton_Click(object sender, EventArgs e)
		{
			//オプションフォーム
			using var settingForm = new OptionSettingForm();
			if (settingForm.ShowDialog() == DialogResult.OK)
			{
				_settings = settingForm.Option;
			}
		}
	}
}
