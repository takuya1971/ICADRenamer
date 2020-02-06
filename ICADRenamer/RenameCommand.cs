/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

using ICADRenamer.CSV;
using ICADRenamer.Events;
using ICADRenamer.Log;
using ICADRenamer.Settings;

using Microsoft.VisualBasic;

using NLog;

using sxnet;

namespace ICADRenamer
{
	/// <summary>
	/// 名前変更コマンドを表すクラス
	/// </summary>
	public class RenameCommand : IDisposable
	{

		/// <summary>
		/// ICADのファイル名を保持するフィールド
		/// </summary>
		private const string _iCADName = "icadx4j";

		/// <summary>
		/// 図面変更検索キーワードを保持するフィールド
		/// </summary>
		private readonly FrameKeyword _keywords = new FrameKeywordSerializer().Load();

		/// <summary>
		/// 変換結果レコーダを保持するフィールド
		/// </summary>
		private CsvRecorder _recorder;

		/// <summary>
		///  実行進捗時に動作するイベント
		/// </summary>
		public event EventHandler<ItemProgressedEventArgs> DetailChanged;

		/// <summary>
		///  図面表題欄の変更時に動作するイベント
		/// </summary>
		public event EventHandler DrawingTitleStarted;

		/// <summary>
		/// 実行完了時に動作するイベント
		/// </summary>
		public event EventHandler ExecuteFinished;

		/// <summary>
		/// 実行開始イベント
		/// </summary>
		public event EventHandler ExecuteStarted;

		/// <summary>
		///  ファイルコピー開始時に動作するイベント
		/// </summary>
		public event EventHandler FileCopyStarted;

		/// <summary>
		///  ファイル削除開始時に動作するイベント
		/// </summary>
		public event EventHandler FileDeleteStarted;

		/// <summary>
		///  ICAD再起動完了時に動作するイベント
		/// </summary>
		public event EventHandler ICADRestarted;

		/// <summary>
		/// ICAD起動完了時に動作するイベント
		/// </summary>
		public event EventHandler ICADStarted;

		/// <summary>
		/// ICAD起動時に動作するイベント
		/// </summary>
		public event EventHandler ICADStarting;

		/// <summary>
		///  パーツ名変更開始時に動作するイベント
		/// </summary>
		public event EventHandler PartRenameStarted;

		/// <summary>
		/// メッセージボックス表示要求時に動作するイベント
		/// </summary>
		public event EventHandler ShowMessageRequest;

		/// <summary>
		///  更新開始時に動作するイベント
		/// </summary>
		public event EventHandler UpdateStarted;

		/// <summary>
		/// エラー区分
		/// </summary>
		private enum ErrorCategory
		{
			/// <summary>
			/// ファイルコピー
			/// </summary>
			FileCopy,
			/// <summary>
			/// モデル名
			/// </summary>
			ModelName,
			/// <summary>
			/// 子パーツ名
			/// </summary>
			ChildPartName,
			/// <summary>
			/// 図番
			/// </summary>
			DrawingNumber,
			/// <summary>
			/// 日付
			/// </summary>
			Date,
			/// <summary>
			/// 署名
			/// </summary>
			Signature,
			/// <summary>
			/// 訂正記号
			/// </summary>
			Delta,
			/// <summary>
			/// 訂正注記
			/// </summary>
			DeltaNote,
			/// <summary>
			/// 保存
			/// </summary>
			Save,
			/// <summary>
			/// ユーザーによるキャンセルを保持するフィールド
			/// </summary>
			CancelByUSer,

			/// <summary>
			/// アクセス権取得失敗を保持するフィールド
			/// </summary>
			FailedGetAccess,

			/// <summary>
			///	更新を保持するフィールド
			/// </summary>
			Update,

			/// <summary>
			/// ジオメトリ取得を保持するフィールド
			/// </summary>
			GetGeomError,
		}

		/// <summary>
		/// キャンセル要求の真偽値を保持するプロパティ
		/// </summary>
		public bool CancelRequest { get; set; } = false;

		/// <summary>
		/// 実行パラメータを保持するプロパティ
		/// </summary>
		public RenameExecuteParams ExecuteParams { get; set; } = new RenameExecuteParams();

		/// <summary>
		/// ICADプロセスを保持するプロパティ
		/// </summary>
		public Process IcadProcess { get; private set; }

		/// <summary>
		/// 変換結果を保持するプロパティ
		/// </summary>
		public List<CsvRecordItem> RecordItems { get; private set; }

		/// <summary>
		/// CSVファイルのパスを保持するプロパティ
		/// </summary>
		public string RecordPath { get; private set; }

		/// <summary>
		/// ICAD再起動要求の真偽値を保持するプロパティ
		/// </summary>
		public bool RestartRequest { get; set; } = false;

		#region Disposable Pattern		
		/// <summary>
		/// 破棄進行を保持するフィールド
		/// </summary>
		bool _disposed = false;

		/// <summary>
		///ファイナライズを実行する
		/// </summary>
		~RenameCommand()
		{
			Dispose(false);
		}

		/// <summary>
		/// アンマネージ リソースの解放またはリセットに関連付けられているアプリケーション定義のタスクを実行します。
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// オブジェクトの破棄を実行する
		/// </summary>
		/// <param name="disposing">破棄中かどうかを表す値</param>
		protected virtual void Dispose(bool disposing)
		{
			if (_disposed) return;
			IcadProcess?.Dispose();
			_disposed = true;
			//
			if (disposing) Dispose(true);
		}
		#endregion

		/// <summary>
		/// 変換を実行する
		/// </summary>
		/// <param name="executeParams">パラメータ類</param>
		public void Execute(RenameExecuteParams executeParams)
		{
			//実行パラメータ
			ExecuteParams = executeParams;
			//変換結果アイテム
			RecordItems = new List<CsvRecordItem>();
			//変換レコーダ
			_recorder = new CsvRecorder(GetReseultFilePath());
			//結果ファイルパス
			RecordPath = _recorder.FilePath;
			//パラメータがないときの処理
			if (ExecuteParams == null)
			{
				throw new ArgumentNullException(nameof(ExecuteParams),
									"パラメータがNullです。");
			}
			//プロセスの起動
			IcadProcess = GetIcadProcess();
			//イベント
			ExecuteStarted?.Invoke(this, new EventArgs());
			//初期化
			SxSys.init(ExecuteParams.Settings.ICADLinkPort, false);
			//フラグがあればアイコン化
			if (ExecuteParams.Settings.ICADMinimize)
			{
				//最小化
				SxSys.setWindowStatus(SxWindow.STATUS_ICON);
			}
			else
			{
				//通常ウィンドウサイズ
				SxSys.setWindowStatus(SxWindow.STATUS_NORMAL);
			}
			//コピー元ファイルの取得
			var source = Directory.GetFiles(
				ExecuteParams.SourcePath
				, SystemSettings.IcadExtension
				, SearchOption.AllDirectories);
			//イベント
			FileCopyStarted?.Invoke(this, new EventArgs());
			//ファイルコピーと変更記録
			var copiedFiles = CopyFiles(source).OrderBy(x => x.DestinationPath);
			RecordItems.AddRange(copiedFiles);
			//読取専用解除
			ReleaseReadOnly();
			//製品フォルダの登録
			if (ExecuteParams.Settings.ResitTo3DSeihin)
			{
				SetProductFolder();
			}
			//イベント
			PartRenameStarted?.Invoke(this, new EventArgs());
			//パーツ変更
			ExecuteRename(GetSuccessedFiles());
			//キャンセル
			if (CancelRequest)
			{
				ExecuteFinished?.Invoke(this, new EventArgs());
				return;
			}
			//プロセスの開き直し
			IcadProcess.Close();
			IcadProcess = GetIcadProcess();
			//イベント
			DrawingTitleStarted?.Invoke(this, new EventArgs());
			//図面変更
			var files = Directory.GetFiles(ExecuteParams.DestinationPath, "*.icd", SearchOption.AllDirectories);
			ExecuteDrawingTitle(files);
			_recorder.WriteAll(RecordItems);
			//イベント
			ExecuteFinished?.Invoke(this, new EventArgs());
			IcadProcess?.Dispose();
		}

		/// <summary>
		/// 製品フォルダへのフォルダの登録を実行する
		/// </summary>
		public void SetProductFolder()
		{
			//製品ファイルの中身
			string seihinFile;
			//エンコーディング　Shift-JIS
			Encoding enc = SystemSettings.SeihinEnc;
			//読み込み
			using (var sr = new StreamReader(SystemSettings.ProductFile, enc, true))
			{
				seihinFile = sr.ReadToEnd();
				enc = sr.CurrentEncoding;
			}
			//フォルダパスのラインを分割
			var seihinArray = seihinFile.Split(new string[] { "\r\n" }
			, StringSplitOptions.RemoveEmptyEntries).ToList();

			//製品フォルダの追加
			StringBuilder sb = new StringBuilder(seihinFile);
			for (var i = 0; i < 2; i++)
			{
				//0ならコピー元パス、1ならコピー先パス
				var dirPath = (i == 0) ? ExecuteParams.SourcePath : ExecuteParams.DestinationPath;
				//登録するフォルダ配列
				var dirArray = Directory.GetDirectories(dirPath, "*"
					, SearchOption.AllDirectories);
				//既に登録されているかチェックして、未登録なら追加
				foreach (var dir in dirArray)
				{
					if (!seihinArray.Exists(x => x == dir))
					{
						sb.AppendLine(dir);
					}
				}
			}
			//製品ファイルへの書き戻し
			seihinFile = sb.ToString();
			/*
				 行数が255行以上あるときは上から削除
			*/
			//行に分ける
			var lines = seihinFile.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
			//ラインが255以上なら
			if (lines.Count > 255)
			{
				do
				{
					//削除
					lines.RemoveAt(0);
				} while (lines.Count > 255);
				//
				sb = new StringBuilder();
				//再構築
				foreach (var line in lines)
				{
					sb.AppendLine(line);
				}
				//製品ファイルに書き戻し
				seihinFile = sb.ToString();
			}
			//書き込み
			using var sw = new StreamWriter(SystemSettings.ProductFile, false, enc);
			sw.Write(seihinFile);
		}

		/// <summary>
		/// 変換のキャンセルを実行する
		/// </summary>
		/// <param name="files">変換しているファイル配列</param>
		/// <param name="index">キャンセルする1個手前のインデックス</param>
		private void CancelExecute(string[] files, int index)
		{
			//残りのファイル
			var remainedFiles = GetRemained();
			//
			foreach (var f in remainedFiles)
			{
				//レコードを取得
				var record = RecordItems.FirstOrDefault(
					x => x.DestinationPath == f);
				//記録の変更
				record.IsSuccess = false;
				record.Remark = GetRemark(record.Remark, ErrorCategory.CancelByUSer);
			}
			ExecuteFinished?.Invoke(this, new EventArgs());
			/*
			 * ローカル関数
			 */
			//実行していないファイル名を取得
			string[] GetRemained()
			{
				List<string> remained = new List<string>();
				for (var i = index; i < files.Length; i++)
				{
					remained.Add(files[i]);
				}
				return remained.ToArray();
			}
		}

		/// <summary>
		/// ファイルのコピーを実行し、CSVに書き込むレコードを取得する
		/// </summary>
		/// <param name="files">ファイル名リスト</param>
		/// <returns></returns>
		private CsvRecordItem[] CopyFiles(string[] files)
		{
			//CSVアイテム
			var items = new List<CsvRecordItem>();
			//カウンタ
			//var progressCounter = 0;
			//実行ルーチン
			for (var i = 0; i < files.Length; i++)
			{
				var file = files[i];
				//新しいファイル
				var newFile = GetNewPath(file);
				//ファイル名
				var newFileName = Path.GetFileNameWithoutExtension(newFile);
				//新しいディレクトリ
				var newDir = Path.GetDirectoryName(newFile);
				//CSVアイテム
				var item = new CsvRecordItem
				{
					Date = DateTime.Now,
					SourcePath = file,
					SourceFileName = Path.GetFileName(file),
					DestinationPath = newFile,
					DestinationFileName = Path.GetFileName(newFile),
					IsSuccess = true,
				};
				//新しいディレクトリが存在しないとき
				if (!Directory.Exists(newDir))
				{
					//ディレクトリ作成
					Directory.CreateDirectory(newDir);
				}
				//エラートラップ
				try
				{
					//コピー実行
					File.Copy(file, newFile, true);
				}
				catch (Exception e)
				{
					//ログ
					RenameLogger.WriteLog(new LogItem
					{
						Exception = e,
						Level = LogLevel.Error,
						Message = $"{newFileName}は{newDir}にコピーできませんでした。"
					});
					//
					item.IsSuccess = false;
					item.Remark = $"ファイルコピーが失敗しました。\r\n{e.Message}";
				}
				finally
				{
					//CSVレコードの追加
					items.Add(item);
					//イベント
					DetailChanged?.Invoke(this,
						new ItemProgressedEventArgs
						{
							FileCount = new CountItem
							{
								Counter = i,
								Items = files.Length,
								Name = newFileName,
							},
							ViewCount = new CountItem
							{
								Counter = 0,
								Items = 0,
								Name = string.Empty,
							},
							DetailCount = new CountItem
							{
								Counter = 0,
								Items = 0,
								Name = string.Empty
							}
						});
				}
			}
			return items.OrderBy(x => x.SourcePath).ToArray();
		}

		/// <summary>
		/// 図面表題欄の変更を実行する
		/// </summary>
		/// <param name="files">変更するファイルパス配列</param>
		private void ExecuteDrawingTitle(string[] files)
		{
			/*
			 * ここからスタート
			 */
			//CSVレコード
			CsvRecordItem record = null;
			for (var i = 0; i < files.Length; i++)
			{
				//ファイルパス
				var file = files[i];
				//csvレコード
				record = RecordItems.FirstOrDefault(x => x.DestinationPath == file);
				//ファイル名
				var fileName = Path.GetFileNameWithoutExtension(file);
				//ファイルモデル
				var fModel = new SxFileModel(file);
				//モデル
				var model = fModel.open(false);
				//２次元画面
				SxSys.setDim(false);
				//ビューリスト
				var vsList = model.getVSList();
				//セグメント研削タイプのセット
				var searchType = new SxInfSearchEntType();
				//デルタマークをセット
				searchType.setStatus(SxEntSeg.SEGTYPE_DELTA, true);
				//テキストをセット
				searchType.setStatus(SxEntSeg.SEGTYPE_TEXT, true);
				//反映
				SxSys.setSearchEntType(searchType);
				//
				for (var j = 0; j < vsList.Length; j++)
				{
					//ビュー
					var vs = vsList[j];
					//ビュー名
					var vsName = vs.getInf().name;
					//対象のセグメントリスト
					var segList = vs.getSegList(0, 0, false, true, true, false);
					SxGeom[] geomList = null;
					try
					{
						//対象セグメントのジオメトリリスト
						geomList = SxEntSeg.getGeomList(segList);
					}
					catch (SxException e)
					{
						SetRecordRemark(ref record, ErrorCategory.GetGeomError, e);
						RenameLogger.WriteLog(new LogItem
						{
							Exception = e,
							Level = LogLevel.Error,
							Message = GetExchangeError(ErrorCategory.GetGeomError)
						});
						continue;
					}
					//ジオメトリがなければスキップ
					if (geomList == null) continue;
					//削除するリスト
					var deleteList = new List<SxEntSeg>();
					//セグメントを走査
					for (var k = 0; k < segList.Length; k++)
					{
						//セグメント
						var seg = segList[k];
						//イベント
						DetailChanged?.Invoke(this,
							new ItemProgressedEventArgs
							{
								//ファイル情報
								FileCount = new CountItem
								{
									Counter = i + 1,
									Items = files.Length,
									Name = fileName
								},
								//ビュー情報
								ViewCount = new CountItem
								{
									Counter = j + 1,
									Items = vsList.Length,
									Name = vsName,
								},
								//アイテム情報
								DetailCount = new CountItem
								{
									Counter = k + 1,
									Items = segList.Length,
									Name = ((uint) seg.ID).ToString()
								}
							});
						//セグメントタイプ
						switch (seg.Type)
						{
							//デルタマーク
							case SxEntSeg.SEGTYPE_DELTA:
								if (ExecuteParams.Settings.CanDeleteDelta)
								{
									deleteList.Add(seg);
								}
								break;
							//テキスト
							case SxEntSeg.SEGTYPE_TEXT:
								if (geomList[k] is SxGeomText textGeom)
								{
									if (textGeom.text_line_num == 1)
									{
										if (ChangeDrawNumber(textGeom)) { continue; }
										if (ChangeDate(textGeom)) { continue; }
										if (ChangeSignature(textGeom)) { continue; }
									}
									if (ExecuteParams.Settings.CanDeleteDelta)
									{
										if (DeleteDeltaNote(textGeom))
										{ deleteList.Add(seg); }
									}
								}
								break;
						}
						/*
						 *	ローカル関数
						 */
						//図番変更
						bool ChangeDrawNumber(SxGeomText geomText)
						{
							foreach (var pattern in _keywords.DrawNumberRegexes)
							{
								//現在の文字列
								var oldText = Strings.StrConv(geomText.txt[0], VbStrConv.Narrow);
								if (Regex.IsMatch(oldText, $"^{pattern}"))
								{
									foreach (var replacePattern in _keywords.DrawNumberSplit)
									{
										if (Regex.IsMatch(ExecuteParams.PrefixName, $"^{replacePattern}"))
										{
											try
											{
												segList[k].editText(Regex.Replace(oldText, replacePattern, ExecuteParams.PrefixName));
												return true;
											}
											catch (SxException e)
											{
												//CSV
												SetRecordRemark(ref record, ErrorCategory.DrawingNumber, e);
												//ログ
												RenameLogger.WriteLog(new LogItem
												{
													Exception = e,
													Level = LogLevel.Error,
													Message = $"{ErrorCategory.DrawingNumber} 図番:{oldText}"
												});
												return false;
											}
										}
									}
								}
							}
							return false;
						}
						//日付変更
						bool ChangeDate(SxGeomText geomText)
						{
							foreach (var pattern in _keywords.DateRegexes)
							{
								var oldText = Strings.StrConv(geomText.txt[0], VbStrConv.Narrow);
								if (Regex.IsMatch(oldText, $"^{pattern}$"))
								{
									//年月日区切
									var separator = ExecuteParams.Settings.IsDateSeparatorSlash ? "/" : ".";
									//年表示
									var yearFormat = ExecuteParams.Settings.IsYear4Digit ? "yyyy" : "yy";
									//月表示
									var monthFormat = ExecuteParams.Settings.IsMonthAndDate2Digit ? "MM" : "M";
									//日表示
									var dateFormat = ExecuteParams.Settings.IsMonthAndDate2Digit ? "dd" : "d";
									try
									{
										//変更
										segList[k].editText(DateTime.Today.ToString($"{yearFormat}{separator}{monthFormat}{separator}{dateFormat}"));
										return true;
									}
									catch (SxException e)
									{
										//CSV
										SetRecordRemark(ref record, ErrorCategory.Date, e);
										//ログ
										RenameLogger.WriteLog(new LogItem
										{
											Exception = e,
											Level = LogLevel.Error,
											Message = GetExchangeError(ErrorCategory.Date)
										});
									}
								}
							}
							return false;
						}
						//署名変更
						bool ChangeSignature(SxGeomText geomText)
						{
							foreach (var pattern in _keywords.Signatures)
							{
								//現在のテキスト
								var oldText = Strings.StrConv(geomText.txt[0], VbStrConv.Narrow);
								if (Regex.IsMatch(oldText, $"^{pattern}$"))
								{
									//エラートラップ
									try
									{
										segList[k].editText(ExecuteParams.Signature);
										return true;
									}
									catch (SxException e)
									{
										//CSV
										SetRecordRemark(ref record, ErrorCategory.Signature, e);
										//ロガー
										RenameLogger.WriteLog(new LogItem
										{
											Exception = e,
											Level = LogLevel.Error,
											Message = GetExchangeError(ErrorCategory.Signature)
										});
									}
								}
							}
							return false;
						}
						//訂正注記削除
						bool DeleteDeltaNote(SxGeomText geomText)
						{
							foreach (var pattern1 in _keywords.DeltaNoteRegexes)
							{
								foreach (var pattern2 in _keywords.Signatures)
								{
									var pattern = $"{pattern1}{pattern2}$";
									foreach (var text in geomText.txt)
									{
										var narrow = Strings.StrConv(text, VbStrConv.Narrow);
										if (Regex.IsMatch(narrow, pattern))
										{
											return true;
										}
									}
								}
							}
							return false;
						}
					}
					//削除リストを処理
					foreach (var seg in deleteList)
					{
						//削除
						seg.delete();
					}
				}
				//
				//更新可能なら
				if (ExecuteParams.Settings.CanExecuteUpdate)
				{
					//イベント
					UpdateStarted?.Invoke(this, new EventArgs());
					try
					{
						//更新コマンド発行
						SxSys.command(SystemSettings.UpdateCommand, false);
					}
					catch (SxException e)
					{
						//CSV
						SetRecordRemark(ref record, ErrorCategory.Update, e);
						//ログ
						RenameLogger.WriteLog(new LogItem
						{
							Exception = e,
							Level = LogLevel.Error,
							Message = GetExchangeError(ErrorCategory.Update)
						});
					}
				}
				//保存
				model.save();
				//閉じる
				model.close(false);
				//
				if (RestartRequest)
				{
					var models = SxModel.getModelList();
					if (models != null)
					{
						ShowMessageRequest?.Invoke(this, new EventArgs());
					}
					Thread.Sleep(5000);
					RestartIcadProcess();
				}
			}
		}

		/// <summary>
		/// パーツ名の変更を実行する。
		/// ICADへのアクセスを減らしたバージョン
		/// </summary>
		/// <param name="files">ファイルパスリスト</param>
		private void ExecuteRename(string[] files)
		{
			//ファイルモデル
			SxFileModel fModel = null;
			//モデル
			SxModel model = null;
			//CSVレコード
			CsvRecordItem record = null;
			//3D画面に変更
			SxSys.setDim(true);
			//すべてのファイルをループ
			for (var i = 0; i < files.Length; i++)
			{
				//ファイルパス
				var file = files[i];
				//ファイル名
				var fileName = Path.GetFileNameWithoutExtension(file);
				//
				if (FileExists(file))
				{
					continue;
				}
				//ファイルモデルを作成
				fModel = new SxFileModel(file);
				//モデルを作成
				model = fModel.open(false);

				//CSVレコードを抽出
				record = RecordItems.FirstOrDefault(
					x => x.DestinationPath == file);
				//名前変更リスト
				var parmList = new List<SxParmPartName>();
				//ビューリスト
				var wfList = model.getWFList();
				//3D空間を走査
				for (var j = 0; j < wfList.Length; j++)
				{
					//キャンセル
					if (CancelRequest)
					{
						CancelExecute(files, i);
						WriteCancelByUserLog(file);
						return;
					}
					//ビュー
					var wf = wfList[j];
					//ビュー名
					var wfName = wf.getInf().name;
					//イベント
					DetailChanged?.Invoke(this,
						new ItemProgressedEventArgs
						{
							FileCount = new CountItem
							{
								Counter = i + 1,
								Items = files.Length,
								Name = fileName,
							},
							ViewCount = new CountItem
							{
								Counter = j + 1,
								Items = wfList.Length,
								Name = wfName,
							},
							DetailCount = new CountItem
							{
								Counter = 0,
								Items = 0,
								Name = string.Empty,
							}
						});
					//パーツツリーを取得
					var partTree = wf.getInfPartTree();
					//子パーツリスト
					var childs = partTree.child_list;
					//子パーツがあれば
					if (childs != null)
					{
						//子パーツを走査
						for (var k = 0; k < partTree.child_list.Length; k++)
						{
							//子パーツ
							var child = childs[k];
							//子パーツ情報
							var inf = child.inf;
							//未解決パーツ
							if (inf.is_unloaded)
							{
								//ファイルモデル
								var fileModel = TryResolveUnloaded(child);
								if (fileModel != null)
								{
									//置き換え
									child.entpart.replace(fileModel, true);
								}
								//解決しなければスキップ
								else continue;
							}
							//子パーツ条件と合えば
							if (IsMatch(child))
							{
								//エラートラップ
								try
								{
									//アクセス権取得
									if (inf.is_read_only) child.entpart.setAccess(false);
								}
								catch (SxException)
								{
									//読取専用強制解除
									if (!ResetReadOnly(child)) continue;
								}
								//改名リストに追記
								parmList.Add(GetParmName(child));
								//イベント
								DetailChanged?.Invoke(this,
									new ItemProgressedEventArgs
									{
										//ファイル情報
										FileCount = new CountItem
										{
											Counter = i + 1,
											Items = files.Length,
											Name = fileName,
										},
										//ビュー情報
										ViewCount = new CountItem
										{
											Counter = j + 1,
											Items = wfList.Length,
											Name = wfName,
										},
										//アイテム情報
										DetailCount = new CountItem
										{
											Counter = k + 1,
											Items = childs.Length,
											Name = child.inf.name,
										}
									});
							}
							//子パーツの改名情報を追加
							parmList.AddRange(RenameChild(child));
							/*
							 * ローカル関数
							 */
							//子パーツの改名情報を再帰で取得
							List<SxParmPartName> RenameChild(SxInfPartTree tree)
							{
								List<SxParmPartName> parmList = new List<SxParmPartName>();
								if (tree.child_list != null)
								{
									foreach (var child in tree.child_list)
									{
										var parm = GetParmName(child);
										if (parm != null) parmList.Add(parm);
										if (child.child_list != null) parmList.AddRange(RenameChild(child));
									}
								}
								return parmList;
							}
							//改名オブジェクトの取得
							SxParmPartName GetParmName(SxInfPartTree tree)
							{
								//新しい図番
								var newName = NewName(tree.inf.name);
								//図番がnullならnullを返す
								if (newName == null) return null;
								//未解決パーツならnullを返す
								if (tree.inf.is_unloaded) return null;
								//エラートラップ
								try
								{
									//読取専用解除
									if (tree.inf.is_external && tree.inf.is_read_only)
									{
										tree.entpart.setAccess(false);
									}
								}
								catch (SxException)
								{
									return null;
								}
								//パスを取得
								var path = Path.Combine(tree.inf.path, $"{newName}.icd");
								//CSVレコードの情報を更新
								record.DestinationPath = path;
								record.DestinationFileName = Path.GetFileNameWithoutExtension(path);
								//ファイルが存在したとき
								if (File.Exists(path) && tree.inf.is_external)
								{
									//パーツ名と同じ場合はスキップ
									if (newName == tree.inf.name) return null;
									//ファイルモデル
									var fModel = new SxFileModel(path);
									//置換
									try
									{
										tree.entpart.replace(fModel, true);
										return null;
									}
									catch (SxException)
									{
										return null;
									}
								}
								else
								{ return new SxParmPartName(tree.entpart, newName, tree.inf.comment, ""); }

							}
							//未解決パーツの解決試行
							SxFileModel TryResolveUnloaded(SxInfPartTree tree)
							{
								var name = NewName(tree.inf.name);
								if (name == null) return null;
								var dirs = Directory.GetDirectories(ExecuteParams.DestinationPath);
								//ファイルを検索する
								foreach (var dir in dirs)
								{
									//改名後のパス
									var path = Path.Combine(dir, name, ".icd");
									if (File.Exists(path))
									{
										return new SxFileModel(path);
									}
									else
									{
										//改名前のパス
										path = Path.Combine(dir, tree.inf.name, ".icd");
										if (File.Exists(path))
										{
											return new SxFileModel(path);
										}
									}
								}
								return null;
							}
						}
					}
				}
				/*
				 *	ローカル関数
				 */
				//一括改名
				var updateName = parmList.Where(x => x != null).ToArray();
				try
				{
					if (updateName != null || updateName.Length > 0)
					{ SxEntPart.setName(updateName, true); }
				}
				catch (SxException) { }
				//保存
				SaveFile();
				if (RestartRequest)
				{
					RestartIcadProcess();
				}
			}
			//元ファイルの削除
			DeleteFile();
			return;
			/*
			 * ローカル関数
			 */
			//ファイルが既存かどうか
			bool FileExists(string filePath)
			{
				var dir = Path.GetDirectoryName(filePath);
				var fileName = Path.GetFileName(filePath);
				var newName = NewName(fileName) ?? fileName;
				var newPath = Path.Combine(dir, newName);
				if (File.Exists(newPath))
				{ return true; }
				else return false;
			}
			//読取専用解除(パーツ専用）
			static bool ResetReadOnly(SxInfPartTree part)
			{

				var arg = $@"/OFF ""{part.inf.path}\{part.inf.ref_model_name}.icd""";
				var p = Process.Start(SystemSettings.DblockPath, arg);
				p.WaitForExit();
				try
				{
					part.entpart.setAccess(false);
				}
				catch (SxException)
				{
					return false;
				}
				finally
				{
					p?.Dispose();
				}
				return true;
			}
			//ファイルの削除
			void DeleteFile()
			{
				//イベント
				FileDeleteStarted?.Invoke(this, new EventArgs());
				//ファイルがあれば削除実行
				for (var i = 0; i < files.Length; i++)
				{
					var file = files[i];
					if (File.Exists(file)) File.Delete(file);
				}
			}
			//ファイルの保存
			void SaveFile()
			{
				//モデル情報
				var inf = model.getInf();

				try
				{
					//新しいファイル名
					var newName = NewName(inf.name);
					if (newName == null) return;
					//コピー先ファイル名
					record.DestinationFileName = $@"{newName}.icd";
					//新しいファイルがあれば
					if (File.Exists($@"{inf.path}\\{newName}.icd"))
					{
						//上書き
						model.save();
					}
					else
					{
						//名前を付けて保存
						model.save("", newName, inf.comment, 0, 0);
					}
					//コピー先パス
					record.DestinationPath = $@"{inf.path}\{newName}.icd";
				}
				catch (Exception e)
				{
					record.IsSuccess = false;
					record.Remark = GetRemark(record.Remark, ErrorCategory.Save, e);
					RenameLogger.WriteLog(new LogItem
					{
						Exception = e,
						Level = LogLevel.Error,
						Message = GetExchangeError(ErrorCategory.Save)
					});
				}
				finally
				{
					try
					{
						model.close();
					}
					catch (SxException) { }
				}
			}

			//新しい図面名
			string NewName(string oldName)
			{
				foreach (var pattern in _keywords.DrawNumberRegexes)
				{
					if (Regex.IsMatch(oldName, $"^{pattern}"))
					{
						foreach (var namePattern in _keywords.DrawNumberSplit)
						{
							if (Regex.IsMatch(oldName, $"^{namePattern}"))
							{
								var name = Regex.Replace(oldName, $"^{namePattern}", ExecuteParams.PrefixName);
								name = name.Replace(" ", string.Empty);
								var enc = Encoding.GetEncoding("Shift_JIS");
								if (enc.GetByteCount(name) > 40)
								{
									return GetOverLength(name, 40, enc);
								}
								return name;
							}
						}
					}
				}
				return null;
			}
			//ファイル名超過処理
			static string GetOverLength(string source, int byteLength, Encoding enc)
			{
				var newString = source.Substring(0, source.Length - 1);
				if (enc.GetByteCount(newString) > byteLength)
				{
					GetOverLength(newString, byteLength, enc);
				}
				return newString;
			}
			//検索するセグメントのタイプの一致を返す
			bool IsMatch(SxInfPartTree tree)
			{
				if (tree.entpart == null) return true;
				if (tree.entpart.getInf().kind != SxInfEnt.KIND_PART) return false;
				//
				foreach (var pattern in _keywords.DrawNumberRegexes)
				{
					if (Regex.IsMatch(tree.inf.name, pattern)) return true;
				}
				return false;
			}
		}

		/// <summary>
		/// 変換エラー文字列を取得する
		/// </summary>
		/// <param name="category">エラー区分</param>
		/// <returns></returns>
		private string GetExchangeError(ErrorCategory category)
		{
			var mes = category switch
			{
				ErrorCategory.ChildPartName => "子パーツ名変更",
				ErrorCategory.Date => "日付変更",
				ErrorCategory.DeltaNote => "改訂注記削除",
				ErrorCategory.Delta => "改訂記号削除",
				ErrorCategory.DrawingNumber => "図番変更",
				ErrorCategory.FileCopy => "ファイルコピー",
				ErrorCategory.ModelName => "モデル名変更",
				ErrorCategory.Signature => "署名変更",
				ErrorCategory.Save => "保存",
				ErrorCategory.CancelByUSer => "ユーザーによるキャンセル要求",
				ErrorCategory.FailedGetAccess => "アクセス権取得失敗",
				ErrorCategory.Update => "更新失敗",
				ErrorCategory.GetGeomError => "ジオメトリ取得",
				_ => throw new NotImplementedException()
			};
			return $"{mes}エラー";
		}

		/// <summary>
		/// ICADプロセスを取得する
		/// </summary>
		/// <returns></returns>
		private Process GetIcadProcess()
		{
			var processes = Process.GetProcessesByName(_iCADName);
			if (processes.Length > 0)
			{
				return processes[0];
			}
			else
			{
				//イベント
				ICADStarting?.Invoke(this, new EventArgs());
				var p = new Process();
				p.StartInfo.FileName = @$"{Environment.GetEnvironmentVariable("ICADDIR")}\bin\icadx4j.exe";
				//ICADプロセス開始
				var f = p.Start();
				//ICAD起動完了
				p.WaitForInputIdle();
				//イベント
				ICADStarted?.Invoke(this, new EventArgs());

				if (!f)
				{
					string mes = "ICADがスタートできません。";
					var ex = new Exception(mes);
					var item = new LogItem()
					{
						Level = LogLevel.Error,
						Message = mes,
						Exception = ex,
					};
					RenameLogger.WriteLog(item);
					throw ex;
				}
				else return p;
			}
		}

		/// <summary>
		/// 新しいパスを取得する
		/// </summary>
		/// <param name="sourcePath">元のパスを表す文字列</param>
		/// <returns></returns>
		private string GetNewPath(string sourcePath)
		{
			var relativeDir = Path.GetDirectoryName(sourcePath).Remove(0, ExecuteParams.SourcePath.Length);
			var fileName = Path.GetFileName(sourcePath);

			foreach (var pattern in _keywords.DrawNumberSplit)
			{
				if (Regex.IsMatch(relativeDir, pattern))
				{

					relativeDir = Regex.Replace(relativeDir, pattern, ExecuteParams.PrefixName);
				}
			}
			//
			return $@"{ExecuteParams.DestinationPath}{relativeDir}\{fileName}";
		}

		/// <summary>
		/// 備考欄を取得する
		/// </summary>
		/// <param name="remark">元の備考欄を表す文字列</param>
		/// <param name="category">メッセージ区分</param>
		/// <param name="e">例外</param>
		/// <returns></returns>
		private string GetRemark(string remark, ErrorCategory category, Exception e = null)
		{
			//文字列ビルダ
			StringBuilder sb;
			//備考欄に記入があれば改行を追加
			if (remark.Length > 0)
			{
				sb = new StringBuilder(remark);
				sb.AppendLine();
			}
			else
			{
				sb = new StringBuilder();
			}
			//変換エラーを追加
			sb.AppendLine(GetExchangeError(category));
			//エラーがあれば
			if (e != null)
			{
				//エラーを追加
				sb.AppendLine(e.Message);
			}
			return sb.ToString();
		}

		/// <summary>
		/// 結果ファイルのパスを取得する
		/// </summary>
		/// <returns></returns>
		private string GetReseultFilePath()
		{
			//コピー元
			string sourceName = "M番なし";
			//パターン検索
			foreach (var pattern in _keywords.DrawNumberSplit)
			{
				if (Regex.IsMatch(ExecuteParams.SourcePath, pattern))
				{
					sourceName = Regex.Matches(ExecuteParams.SourcePath, pattern)[0].Value;
					break;
				}
			}
			var filePath = $"{sourceName}→{ExecuteParams.PrefixName}-変換結果-iCADRenamer.csv";
			return Path.Combine(ExecuteParams.DestinationPath, filePath);
		}

		/// <summary>
		/// コピー成功ファイルの取得を実行する
		/// </summary>
		/// <returns></returns>
		private string[] GetSuccessedFiles()
			=> RecordItems.Where(x => x.IsSuccess).Select(x => x.DestinationPath).OrderBy(x => x).ToArray();

		/// <summary>
		/// 読取専用解除
		/// </summary>
		private void ReleaseReadOnly()
		{
			//対象のフォルダリスト
			var folders = Directory.GetDirectories(ExecuteParams.DestinationPath, "*", SearchOption.AllDirectories);
			//プロセス
			var pInfo = new ProcessStartInfo
			{
				CreateNoWindow = true,
				RedirectStandardOutput = true,
				UseShellExecute = false,
				FileName = SystemSettings.DblockPath
			};
			/*	DBLOCKのオプション
			 */
			//ファイル状況の出力
			const string dispArg = "/DISP";
			//強制解除
			const string offArg = "/OFF ";
			//実行
			foreach (var folder in folders)
			{
				//コマンドオプション　/DISP
				pInfo.Arguments = $@"{dispArg} ""{folder}""";
				//プロセスのスタート
				var pDisp = Process.Start(pInfo);
				//出力をリダイレクト
				var output = pDisp.StandardOutput.ReadToEnd();
				pDisp?.Dispose();
				//出力を行に分けて取得
				var lines = output.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
				//ラインを走査
				foreach (var line in lines)
				{
					//フィールドを分割
					var fields = line.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
					//OFFはスキップ
					if (fields[0].Trim() == "OFF") continue;
					//パスを取得
					var path = Path.Combine(folder, $"{fields[1].Trim()}.icd");
					//コマンドライン 解除
					pInfo.Arguments = $@"{offArg}""{path}""";
					//プロセスを始動
					var pOff = Process.Start(pInfo);
					//出力
					Console.WriteLine(pOff.StandardOutput.ReadToEnd());
					//クローズ
					pOff.WaitForExit();
					pOff?.Dispose();
				}
			}
		}

		/// <summary>
		/// ICADプロセスの再起動を実行する
		/// </summary>
		private void RestartIcadProcess()
		{
			if (IcadProcess == null) return;
			//ICAD終了コマンド
			SxSys.command(SystemSettings.EndIcad, false);
			//プロセスの終了
			IcadProcess?.Close();
			Thread.Sleep(5000);
			//新しいプロセスを取得
			IcadProcess = GetIcadProcess();
			//待機
			IcadProcess.WaitForInputIdle();
			//ICAD初期化
			SxSys.init(ExecuteParams.Settings.ICADLinkPort, false);
			//再起動要求をリセット
			RestartRequest = false;
			//イベント
			ICADRestarted?.Invoke(this, new EventArgs());
			//
			Thread.Sleep(5000);
		}

		/// <summary>
		/// 変更失敗の備考入力を実行する
		/// </summary>
		/// <param name="record">record</param>
		/// <param name="category">category</param>
		/// <param name="e">e</param>
		private void SetRecordRemark(ref CsvRecordItem record, ErrorCategory category, SxException e)
		{
			if (record == null) return;
			record.IsSuccess = false;
			record.Remark = GetRemark(record.Remark, category, e);
		}

		/// <summary>
		/// CSVファイルへのユーザーによるキャンセルを記録する
		/// </summary>
		/// <param name="path">キャンセルしたファイルのパス</param>
		private void WriteCancelByUserLog(string path)
			=> RenameLogger.WriteLog(LogMessageKind.CancelByUser
				, new List<(LogMessageCategory category, string message)>
				{
					(LogMessageCategory.Canceled, $"{path}実行時にユーザーによってキャンセルされました。")
				});
	}
}