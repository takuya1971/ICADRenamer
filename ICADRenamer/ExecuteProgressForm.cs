/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using ICADRenamer.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ICADRenamer.Log;
using System.Threading.Tasks;
using NLog;

namespace ICADRenamer
{
	/// <summary>
	/// 実行進捗フォームを表すクラス
	/// </summary>
	/// <seealso cref="System.Windows.Forms.Form" />
	public partial class ExecuteProgressForm : Form
	{
		/// <summary>
		/// フォーム名を保持するフィールド
		/// </summary>
		private const string _formName = "変換進捗";

		/// <summary>
		/// 実行パラメータを保持するフィールド
		/// </summary>
		private readonly RenameExecuteParams _executeParams;

		/// <summary>
		/// コマンドを保持するフィールド
		/// </summary>
		private RenameCommand _command;

		/// <summary>
		/// ICAD起動中フォームを保持するフィールド
		/// </summary>
		private ICADStartingForm _iCadStartingForm = null;

		/// <summary>
		///   <see cref="ExecuteProgressForm"/> classの初期化
		/// </summary>
		/// <param name="executeParams">実行パラメータ</param>
		public ExecuteProgressForm(RenameExecuteParams executeParams)
		{
			InitializeComponent();
			Initialize();
			_executeParams = executeParams;
		}

		/// <summary>
		///  実行キャンセル時に動作するイベント
		/// </summary>
		public event EventHandler ExecuteCanceled;

		/// <summary>
		///  実行完了時に動作するイベント
		/// </summary>
		public event EventHandler ExecuteFinished;

		/// <summary>
		/// 実行開始時に動作するイベント
		/// </summary>
		public event EventHandler ExecuteStarted;

		/// <summary>
		/// 準備完了時に動作するイベント
		/// </summary>
		public event EventHandler Prepared;

		/// <summary>
		/// キャンセルボタンクリックイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void CancelButton_Click(object sender, EventArgs e)
		{
			if (_command == null) return;
			_command.CancelRequest = true;
			//ログ
			RenameLogger.WriteLog(LogMessageKind.Operation, new List<(LogMessageCategory category, string message)>
			{
				(LogMessageCategory.SourceForm,_formName),
				(LogMessageCategory.Message,$"{_formName}をキャンセルしました。")
			});
		}

		/// <summary>
		/// 進捗表示の更新を実行する
		/// </summary>
		/// <param name="index">区分インデックスを表す数値</param>
		/// <param name="e">イベント引数</param>
		private void ChangeProgress(int index, ItemProgressedEventArgs e)
		{
			ProgressBar pb = index switch
			{
				1 => _fileProgressBar,
				2 => _viewProgressBar,
				3 => _itemProgressBar,
				_ => null
			};
			Label nameLabel = index switch
			{
				1 => _fileNameLabel,
				2 => _categoryNameLabel,
				3 => _itemNameLabel,
				_ => null
			};
			Label countLabel = index switch
			{
				1 => _fileCountLabel,
				2 => _categoryCountLabel,
				3 => _itemCountLabel,
				_ => null
			};
			CountItem item = index switch
			{
				1 => e.FileCount,
				2 => e.ViewCount,
				3 => e.DetailCount,
				_ => new CountItem()
			};

			//
			pb.Maximum = item.Items;
			pb.Value = item.Counter;
			nameLabel.Text = item.Name;
			countLabel.Text = $"{item.Counter}/{item.Items}";
			var rate = e.DetailCount.Counter / e.DetailCount.Items;
			Text = $"{Text}{rate.ToString()}%";
		}

		/// <summary>
		/// 詳細変更イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void Command_DetailChanged(object sender, ItemProgressedEventArgs e)
		{
			for (var i = 1; i < 4; i++)
			{
				ChangeProgress(i, e);
			}
		}

		/// <summary>
		/// 図面表題欄進捗イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void Command_DrawingTitleStarted(object sender, EventArgs e)
		{
			Text = GetFormText("図面表題欄編集中...");
		}

		/// <summary>
		/// コマンド終了イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">イベント引数</param>
		/// <exception cref="NotImplementedException"></exception>
		private void Command_ExecuteFinished(object sender, EventArgs e)
		{
			if (_command.CancelRequest) ExecuteCanceled?.Invoke(this, new EventArgs());
			else ExecuteFinished?.Invoke(this, new EventArgs());
			_command?.Dispose();
			Close();
		}

		/// <summary>
		/// 変換開始イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void Command_ExecuteStarted(object sender, EventArgs e)
		{
			ExecuteStarted?.Invoke(this, new EventArgs());
		}

		delegate void EventMethod(object sender, EventArgs e);

		/// <summary>
		/// ファイルコピー開始イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void Command_FileCopyStarted(object sender, EventArgs e)
		{
			Text = GetFormText($"ファイルコピー中...{_executeParams.SourcePath}→{_executeParams.DestinationPath}");
		}

		/// <summary>
		/// ファイル削除開始イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void Command_FileDeleteStarted(object sender, EventArgs e)
		{
			Text = GetFormText("元ファイル削除中...");
		}

		/// <summary>
		/// ICADプロセス開始完了イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void Command_ICADStarted(object sender, EventArgs e)
		{
			if (!(_iCadStartingForm == null || _iCadStartingForm.IsDisposed))
			{
				_iCadStartingForm.Close();
			}
			_iCadStartingForm = null;
			Prepared?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// ICADプロセス開始中を実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void Command_ICADStarting(object sender, EventArgs e)
		{
			_iCadStartingForm = new ICADStartingForm();
			_iCadStartingForm.Show();
		}

		/// <summary>
		/// コマンド実行を実行する
		/// </summary>
		private void ExecuteCommand()
		{
			_command.Execute(_executeParams);
			ExecuteFinished?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// フォーム表示イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void ExecuteProgressForm_Load(object sender, EventArgs e)
		{
			//ログ
			RenameLogger.WriteLog(LogMessageKind.Operation, new List<(LogMessageCategory category, string message)>
			{
				(LogMessageCategory.SourceForm,_formName),
				(LogMessageCategory.Message,$"{_formName}を開始しました。"),
				(LogMessageCategory.SourcePath,_executeParams.SourcePath),
				(LogMessageCategory.DestinationPath,_executeParams.DestinationPath),
				(LogMessageCategory.NewNumber,_executeParams.PrefixName),
				(LogMessageCategory.Signature,_executeParams.Signature)
			});
			//
			ExecuteCommand();
		}

		/// <summary>
		/// フォームキャプション文字列を取得する
		/// </summary>
		/// <param name="message">メッセージを表す文字列</param>
		/// <returns></returns>
		private string GetFormText(string message) => $"変換実行中:{message}";

		/// <summary>
		/// 初期化を実行する
		/// </summary>
		private void Initialize()
		{
			_command = new RenameCommand();
			_command.DetailChanged += Command_DetailChanged;
			_command.DrawingTitleStarted += Command_DrawingTitleStarted;
			_command.ExecuteStarted += Command_ExecuteStarted;
			_command.FileCopyStarted += Command_FileCopyStarted;
			_command.FileDeleteStarted += Command_FileDeleteStarted;
			_command.ICADStarted += Command_ICADStarted;
			_command.ICADStarting += Command_ICADStarting;
			_command.UpdateStarted += Command_ExecuteFinished;
		}
	}
}
