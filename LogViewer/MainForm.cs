/* ICAD Renamer LogViewer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using ICADRenamer;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LogViewer
{
	public partial class MainForm : Form
	{
		/// <summary>
		/// 全選択を保持するフィールド
		/// </summary>
		private const string _allSelect = "すべて";

		/// <summary>
		/// フォルダ内のファイルリストを保持するフィールド
		/// </summary>
		private string[] _files;

		/// <summary>
		/// フォルダパスを保持するフィールド
		/// </summary>
		private string _folderPath = SystemSettings.LogFolder;

		/// <summary>
		/// ログデータを保持するフィールド
		/// </summary>
		private LogDataList _logDataRecords;

		/// <summary>
		///   <see cref="MainForm"/> classの初期化
		/// </summary>
		public MainForm() => InitializeComponent();

		/// <summary>
		///   <see cref="MainForm"/> classの初期化
		/// </summary>
		/// <param name="FolderPath">フォルダパスを表す文字列</param>
		public MainForm(string FolderPath)
		{
			_folderPath = FolderPath;
			InitializeComponent();
		}

		/// <summary>
		/// フィルタの適用を実行する
		/// </summary>
		/// <returns></returns>
		private LogDataRecord[] Filtered()
			=> _logDataRecords.Where(x => x.Level
			== _logLevelComboBox.SelectedItem.ToString()).ToArray();

		/// <summary>
		/// ファイル名を取得する
		/// </summary>
		/// <param name="Files">ファイルパスのリスト</param>
		/// <returns></returns>
		private string[] GetFileNames(string[] Files)
		{
			//ファイル名リスト
			var _fileList = new List<string>();
			//ファイル名を取得してリストに追加
			foreach (var file in Files)
			{
				_fileList.Add(Path.GetFileNameWithoutExtension(file));
			}
			return _fileList.ToArray();
		}

		/// <summary>
		/// フォルダ内のファイルパスを取得する
		/// </summary>
		/// <returns></returns>
		private string[] GetFiles()
		{
			//フォルダがなければnullを返す
			if (!Directory.Exists(_folderPath)) return null;
			//ファイルを取得
			var files = Directory.GetFiles(_folderPath, SystemSettings.LogExtension);
			//保存ボタンの使用可否
			var f = files.Length > 0 ? true : false;
			_saveAsButton.Enabled = f;
			//ファイルがないときはnullを返す
			if (!f) return null;
			//
			return files;
		}

		/// <summary>
		/// フォルダパスを取得する
		/// </summary>
		/// <param name="path">パスを表す文字列</param>
		/// <returns></returns>
		private string GetFolderPath(string path, string description)
		{
			_folderBrowserDialog.SelectedPath = path;
			_folderBrowserDialog.Description = description;
			//ダイアログを開く
			var result = _folderBrowserDialog.ShowDialog();
			//キャンセルなら戻る
			if (result != DialogResult.OK)
			{
				_folderBrowserDialog.SelectedPath = string.Empty;
				return string.Empty;
			}
			if (Directory.GetFiles(_folderBrowserDialog.SelectedPath
				, SystemSettings.LogExtension).Length == 0)
			{
				GetFolderPath(_folderBrowserDialog.SelectedPath, @"ログファイルがありません。\r\nフォルダを選択しなおしてください。");
			}
			return _folderBrowserDialog.SelectedPath;
		}

		/// <summary>
		/// ファイル選択コンボボックスの選択インデックス変更イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void LogFileComboBox_SelectedIndexChanged(object sender, EventArgs e) => SetData();

		/// <summary>
		/// ログレベルコンボボックスのインデックス変更イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void LogLevelComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			var item = _logLevelComboBox.SelectedItem.ToString();
			//
			if (item == _allSelect)
			{
				_logDataListBindingSource.DataSource = _logDataRecords;
			}
			else
			{
				_logDataListBindingSource.DataSource = Filtered();
			}
		}

		/// <summary>
		/// フォームのロードイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void MainForm_Load(object sender, EventArgs e)
		{
			//ファイルリストの取得
			_files = GetFiles();
			//ファイルがなければ戻る
			if (_files == null) return;
			//ファイルコンボボックスの更新
			SetFileComboBox();
		}

		/// <summary>
		/// フォルダを開くボタンのクリックイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void OpenFolderButton_Click(object sender, EventArgs e)
		{
			//フォルダパスがなければデフォルト
			_folderPath = Directory.Exists(_folderPath)
				? _folderPath
				: Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
			//フォルダパスを取得
			var path = GetFolderPath(_folderPath, "フォルダを選択してください。");
			//フォルダ選択がキャンセルされたなら戻る
			if (path.Length == 0) return;
			//パスを設定
			_folderPath = _folderBrowserDialog.SelectedPath;
			//データ更新
			_files = GetFiles();
			SetFileComboBox();
		}

		/// <summary>
		/// ログデータの読み込みを実行する
		/// </summary>
		private void SetData()
		{
			//データを読み込む
			_logDataRecords = LogDataList.Load(_files[_logFileComboBox.SelectedIndex]);
			_logDataListBindingSource.DataSource = _logDataRecords;
			//レコードを最初へ
			_logDataListBindingSource.MoveFirst();
			//レベルリスト更新
			SetLevelComboBox();
		}

		/// <summary>
		/// ファイルコンボボックスのデータ更新を実行する
		/// </summary>
		private void SetFileComboBox()
		{
			//リストアイテムクリア
			_logFileComboBox.Items.Clear();
			//ファイル名のみのリストを追加
			_logFileComboBox.Items.AddRange(GetFileNames(_files));
			//インデックス更新
			_logFileComboBox.SelectedIndex = 0;
			//データの読み込み
		}

		/// <summary>
		/// レベルコンボボックスのデータ更新を実行する
		/// </summary>
		private void SetLevelComboBox()
		{
			//アイテムクリア
			_logLevelComboBox.Items.Clear();
			//全部表示
			_logLevelComboBox.Items.Add(_allSelect);
			//データにあるレベルを取得
			var levels = _logDataRecords.Select(
				x => x.Level).Distinct();
			_logLevelComboBox.Items.AddRange(levels.ToArray());
			//インデックス更新
			_logLevelComboBox.SelectedIndex = 0;
		}

		/// <summary>
		/// コピーメニュー選択イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//セルがひとつの時はテキストコピー
			if (_dataGridView.GetCellCount(DataGridViewElementStates.Selected) == 1)
			{
				Clipboard.SetText(_dataGridView.SelectedCells[0].Value.ToString(), TextDataFormat.UnicodeText);
			}
			//2個以上ならオブジェクトコピー
			else
			{
				Clipboard.SetDataObject(_dataGridView.GetClipboardContent(), true);
			}
		}

		/// <summary>
		/// コンテキストメニューオープンイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void ContextMenuStrip_Opened(object sender, EventArgs e)
		{
			//データが選択されているか調べてコピーメニューの使用可否を決定する
			var f = _dataGridView.GetCellCount(DataGridViewElementStates.Selected) > 0
				? true : false;
			_copyToolStripMenuItem.Enabled = f;
		}

		/// <summary>
		/// 保存ボタンクリックイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void SaveAsButton_Click(object sender, EventArgs e)
		{
			//ダイアログの初期フォルダ
			_saveFileDialog.InitialDirectory = _folderPath;
			//選択結果
			var result = _saveFileDialog.ShowDialog();
			//キャンセルしたら戻る
			if (result != DialogResult.OK) return;
			//ファイルコンボボックスのインデックスを取得
			var index = _logFileComboBox.SelectedIndex;
			//ファイルパスを取得
			var path = _files[index];
			//保存
			LogDataList.Save(path, _saveFileDialog.FileName);
		}

		/// <summary>
		/// データグリッドビューのコンテキストメニュー要求イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void DataGridView_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
		{
			_dataGridView.ClearSelection();
			//
			if (e.RowIndex < 0)
			{
				return;
			}
			else if (e.ColumnIndex < 0)
			{
				var row = _dataGridView.Rows[e.RowIndex];
				row.Selected = true;
			}
			else
			{
				var cell = _dataGridView[e.ColumnIndex, e.RowIndex];
				cell.Selected = true;
			}
		}

		/// <summary>
		/// 終了ボタンクリックイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void ExitButton_Click(object sender, EventArgs e) => Close();

		/// <summary>
		/// ヘルプボタンクリックイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void HelpButton_Click(object sender, EventArgs e)
		{
			var helpForm = new HelpBrowser
			{
				Target = $"{SystemSettings.HelpPath}#LogViewer"
			};
			helpForm.Show();
		}
	}
}
