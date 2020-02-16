/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using System;
using System.Collections.Generic;
using System.Windows.Forms;

using ICADRenamer.Events;
using ICADRenamer.Log;
using sxnet;
using NLog;
using System.Text;
using System.Threading.Tasks;

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

		private string _formText;

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
		/// 結果のファイルパスを保持するプロパティ
		/// </summary>
		public string ResultFilePath => _resultFilePath;

		private string _resultFilePath;

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
			Application.DoEvents();
			pb.Maximum = item.Items;
			pb.Value = item.Counter;
			nameLabel.Text = item.Name;
			countLabel.Text = $"{item.Counter}/{item.Items}";
			int rate = 0;
			if (e.DetailCount.Items > 1)
			{
				rate = 100 * e.DetailCount.Counter / e.DetailCount.Items;
			}
			Text = $"{GetFormText()}{rate.ToString()}%";
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
		delegate void UpdateProgress(int index, ItemProgressedEventArgs arg);

		/// <summary>
		/// 図面表題欄進捗イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void Command_DrawingTitleStarted(object sender, EventArgs e)
		{
			_formText = "図面表題欄編集中...";
			Text = GetFormText();
		}

		/// <summary>
		/// コマンド終了イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">イベント引数</param>
		/// <exception cref="NotImplementedException"></exception>
		private void Command_ExecuteFinished(object sender, EventArgs e)
		{
			//ファイルパス
			_resultFilePath = _command.RecordPath;
			//キャンセ要求時
			if (_command.CancelRequest) ExecuteCanceled?.Invoke(this, new EventArgs());
			else ExecuteFinished?.Invoke(this, new EventArgs());
			//コマンドの破棄
			_command?.Dispose();
			Close();
		}

		/// <summary>
		/// 変換開始イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void Command_ExecuteStarted(object sender, EventArgs e) => ExecuteStarted?.Invoke(this, new EventArgs());
		/// <summary>
		/// ファイルコピー開始イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void Command_FileCopyStarted(object sender, EventArgs e)
		{
			Application.DoEvents();
			_formText = "ファイルをコピー中...";
			Text = GetFormText();
		}

		/// <summary>
		/// ファイル削除開始イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void Command_FileDeleteStarted(object sender, EventArgs e)
		{
			_formText = "元ファイル削除中...";
			Text = GetFormText();
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
			Application.DoEvents();
		}

		/// <summary>
		/// コマンド実行を実行する
		/// todo:部分実行を追加する
		/// </summary>
		private void ExecuteCommand()
		{
			switch (_executeParams.ExecuteCategory)
			{
				case ExecuteItem.All:
					_command.Execute(_executeParams);
					break;
				case ExecuteItem.DrawingTitiles:
					_command.ExecuteDrawingTitle(_executeParams);
					break;
				case ExecuteItem.RenameParts:
					_command.ExecuteRename(_executeParams);
					break;
			}
			_resultFilePath = _command.RecordPath;
		}

		/// <summary>
		/// フォーム表示イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void ExecuteProgressForm_Load(object sender, EventArgs e) =>
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

		/// <summary>
		/// フォームキャプション文字列を取得する
		/// </summary>
		/// <param name="message">メッセージを表す文字列</param>
		/// <returns></returns>
		private string GetFormText() => $"変換実行中:{_formText}";

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
			_command.ExecuteFinished += Command_ExecuteFinished;
			_command.PartRenameStarted += Command_PartRenameStarted;
			_command.ICADRestarted += Command_ICADRestarted;
			_command.ShowMessageRequest += Command_ShowMessageRequest;
		}

		/// <summary>
		/// ICAD再起動中表示フォームを保持するフィールド
		/// </summary>
		private IcadRestartForm _icadRestartForm;

		/// <summary>
		/// メッセージボックス要求を実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void Command_ShowMessageRequest(object sender, EventArgs e)
		{
			LoopMessageBox(false);
			//
			_icadRestartForm = new IcadRestartForm();
			_icadRestartForm.Show();
			Application.DoEvents();
		}

		/// <summary>
		/// ICAD再起動後イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void Command_ICADRestarted(object sender, EventArgs e)
			=> _icadRestartForm?.Close();

		/// <summary>
		/// パーツ名変更開始イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void Command_PartRenameStarted(object sender, EventArgs e)
		{
			_formText = "パーツ名変更中...";
			Text = GetFormText();
		}

		/// <summary>
		/// フォーム表示イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void ExecuteProgressForm_Shown(object sender, EventArgs e) => ExecuteCommand();

		/// <summary>
		/// ICADにモデルが残っているときのクローズ処理のメッセージボックスを実行する
		/// </summary>
		private void LoopMessageBox(bool looped)
		{
			//モデルリスト
			var models = SxModel.getModelList();
			//メッセージボックス
			var result = MessageBox.Show(GetMessage(models, looped)
				, "ICADの再起動前の確認"
				, MessageBoxButtons.YesNo
				, MessageBoxIcon.Question);
			//確認なしですべて閉じる
			if (result == DialogResult.Yes)
			{
				try
				{
					foreach (var model in models)
					{
						model.close(false);
					}
				}
				catch (SxException e)
				{
					RenameLogger.WriteLog(new LogItem
					{
						Exception = e,
						Level = LogLevel.Error,
						Message = "モデルクローズに失敗しました。"
					});
				}
			}
			//手動で閉じる
			else
			{
				//モデルリストを取得
				models = SxModel.getModelList();
				//モデルがあれば
				if (models != null)
				{
					//メッセージボックスをもう一度表示
					LoopMessageBox(true);
				}
			}
			//メッセージの取得
			static string GetMessage(SxModel[] models, bool looped)
			{
				var sb = new StringBuilder();
				//2度目以降
				if (looped)
				{
					sb.AppendLine("手動で閉じられていません。");
				}
				sb.AppendLine("ICADに未保存のモデルが残っています。");
				sb.AppendLine("保存せず自動的に閉じてもいいですか？");
				sb.AppendLine("手動で閉じるときは、閉じた後に「No」を選択してください。");
				//モデルがあるときのメッセージ
				if (models != null)
				{
					sb.AppendLine("開いているモデル:");
					foreach (var model in models)
					{
						sb.AppendLine($"  {model.getInf().name}");
					}
				}
				return sb.ToString();
			}
		}
	}
}
