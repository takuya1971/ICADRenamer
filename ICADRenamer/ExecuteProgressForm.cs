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
		/// 進捗アイテムを保持するフィールド
		/// </summary>
		private List<ProgressItem> _progressItems;

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
		/// 実行している項目の名称を取得する
		/// </summary>
		/// <param name="item">進捗アイテム</param>
		/// <returns></returns>
		private static string GetName(ProgressItem item) => item.Category switch
		{
			ProgressCategory.ChangePartName => $"パーツ名変更:{item.Name}",
			ProgressCategory.File => item.Name,
			ProgressCategory.Segment => "図面文字変更",
			ProgressCategory.Update => "更新処理",
			ProgressCategory.View => $"{item.Name}ビュー",
			ProgressCategory.Model => $"{item.Name}モデル",
			ProgressCategory.ChangeDrawing => "",
			_ => "",
		};

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
		/// 実行区分変更イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void Command_CategoryChanged(object sender, CategoryChangeEventArgs e)
		{
			var item = GetItem(e.Category);
			if (item.Counters == null) item.Counters = new ProgressCounter { Count = 0, Counter = 0 };
			item.Counters.Count = e.TotalItem;
			DisplayUpdate(item);
			Application.DoEvents();
		}

		/// <summary>
		/// 実行区分進捗イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void Command_CategoryProgressed(object sender, ItemProgressedEventArgs e)
		{
			var item = GetItem(e.Category);
			item.Counters.Counter++;
			DisplayUpdate(item);
			Application.DoEvents();
		}

		/// <summary>
		/// 実行区分開始イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void Command_CategoryStarted(object sender, ItemProgressedEventArgs e)
		{
			if (e.Category == ProgressCategory.File)
			{
				RenameLogger.WriteLog(LogMessageKind.FileStart
					, new List<(LogMessageCategory category, string message)>
				{
					(LogMessageCategory.SourceForm,_formName),
					(LogMessageCategory.Message,"ファイル変換開始"),
					(LogMessageCategory.FilePath,e.Name)
				});
			}
			//
			var item = GetItem(e.Category);
			item.Name = e.Name;
			item.Counters.Counter=e.Counter;
			DisplayUpdate(item);
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
		/// 表示の変更を実行する
		/// </summary>
		/// <param name="item">進捗アイテム</param>
		private void DisplayUpdate(ProgressItem item)
		{
			ProgressBar bar = GetProgressBar(item.Category);
			Label nameLabel = GetNameLabel(item.Category);
			Label counterLabel = GetCounterLabel(item.Category);
			//
			if (bar != null)
			{
				bar.Value = item.Counters.Counter;
				bar.Maximum = item.Counters.Count;
			}
			if (nameLabel != null) nameLabel.Text = GetName(item);
			if (counterLabel != null) counterLabel.Text = $"{ item.Counters.Counter}/{item.Counters.Count}";
			//ファイルの進捗データ
			var fileItem = _progressItems.First(x => x.Category == ProgressCategory.File);
			//進捗率
			if (fileItem.Counters.Count > 0)
			{
				int progRate = (int) (fileItem.Counters.Counter / fileItem.Counters.Count);
				Text = $"変換進捗 実行中...{progRate}%";
			}
			Application.DoEvents();
		}

		/// <summary>
		/// カウンタラベルを取得する
		/// </summary>
		/// <param name="category">進捗区分</param>
		/// <returns></returns>
		private Label GetCounterLabel(ProgressCategory category)
		{
			switch (category)
			{
				case ProgressCategory.ChangePartName:
				case ProgressCategory.Segment:
					return _itemCountLabel;
				case ProgressCategory.File:
					return _fileCountLabel;
				case ProgressCategory.Update:
				case ProgressCategory.View:
				case ProgressCategory.Model:
					return _categoryCountLabel;
				default:
					return null;
			}
		}

		/// <summary>
		/// 進捗アイテムを取得する
		/// </summary>
		/// <param name="category">進捗区分</param>
		/// <returns></returns>
		private ProgressItem GetItem(ProgressCategory category)
			=> _progressItems.First(x => x.Category == category);
		/// <summary>
		/// 名前ラベルを取得する
		/// </summary>
		/// <param name="category">進捗区分</param>
		/// <returns></returns>
		private Label GetNameLabel(ProgressCategory category)
		{
			switch (category)
			{
				case ProgressCategory.Update:
				case ProgressCategory.View:
				case ProgressCategory.Model:
					return _categoryNameLabel;
				case ProgressCategory.File:
					return _fileNameLabel;
				case ProgressCategory.Segment:
				case ProgressCategory.ChangePartName:
					return _itemNameLabel;
				default:
					return null;
			}
		}

		/// <summary>
		/// プログレスバーを取得する
		/// </summary>
		/// <param name="category">進捗区分</param>
		/// <returns></returns>
		private ProgressBar GetProgressBar(ProgressCategory category)
		{
			switch (category)
			{
				case ProgressCategory.ChangePartName:
				case ProgressCategory.Segment:
					return _itemProgressBar;
				case ProgressCategory.File:
					return _fileProgressBar;
				case ProgressCategory.Update:
				case ProgressCategory.View:
				case ProgressCategory.Model:
					return _viewProgressBar;
				default:
					return null;
			}
		}

		/// <summary>
		/// 初期化を実行する
		/// </summary>
		private void Initialize()
		{
			//実行コマンド
			_command = new RenameCommand();
			_command.CategoryChanged += Command_CategoryChanged;
			_command.CategoryPregressed += Command_CategoryProgressed;
			_command.CategoryStarted += Command_CategoryStarted;
			_command.ExecuteFinished += Command_ExecuteFinished;
			_command.ICADStarted += Command_ICADStarted;
			_command.ICADStarting += Command_ICADStarting;
			//進捗リスト
			_progressItems = new List<ProgressItem>
			{
				new ProgressItem{
					Category= ProgressCategory.ChangeDrawing,
					Counters=new ProgressCounter{Count=0, Counter=0},
				},
				new ProgressItem{
					Category= ProgressCategory.ChangePartName,
					Counters=new ProgressCounter
					{
						Count=0,
						Counter=0,
					}
				},
				new ProgressItem{Category= ProgressCategory.File,
					Counters= new ProgressCounter
					{
						Count=0,
						Counter=0,
					}
				},
				new ProgressItem{Category= ProgressCategory.Segment,
					Counters=new ProgressCounter
					{
						Count=0,
						Counter=0,
					}
				},
				new ProgressItem{Category= ProgressCategory.Update,
					Counters=new ProgressCounter
					{
						Count=0,
						Counter=0,
					}
				},
				new ProgressItem{Category= ProgressCategory.View,
					Counters=new ProgressCounter
					{
						Count=0,
						Counter=0,
					}
				}
			};
		}

		/// <summary>
		/// ICAD起動中フォームを保持するフィールド
		/// </summary>
		private ICADStartingForm _iCadStartingForm = null;

		private void Command_ICADStarting(object sender, EventArgs e)
		{
			_iCadStartingForm = new ICADStartingForm();
			_iCadStartingForm.Show();
		}

		private void Command_ICADStarted(object sender, EventArgs e)
		{
			if(!(_iCadStartingForm==null || _iCadStartingForm.IsDisposed))
			{
				_iCadStartingForm.Close();
			}
			_iCadStartingForm = null;
			Prepared?.Invoke(this, new EventArgs());
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
			_command.Execute(_executeParams);
		}

		/// <summary>
		/// 準備完了時に動作するイベント
		/// </summary>
		public event EventHandler Prepared;
	}
}
