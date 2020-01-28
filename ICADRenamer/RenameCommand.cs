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
using System.Threading.Tasks;

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
		/// 実行可否条件のパラメータを保持するフィールド
		/// </summary>
		private RenameExecuteParams _param = new RenameExecuteParams();

		/// <summary>
		/// ICADのプロセスを保持するフィールド
		/// </summary>
		private Process _process;

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
		///  実行キャンセル時に動作するイベント
		/// </summary>
		public event EventHandler ExecuteCanceled;

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
			CancelByUSer
		}

		/// <summary>
		/// キャンセル要求の真偽値を保持するプロパティ
		/// </summary>
		public bool CancelRequest { get; set; } = false;

		/// <summary>
		/// 変換結果を保持するプロパティ
		/// </summary>
		public List<CsvRecordItem> RecordItems { get => _recordItems; private set => _recordItems = value; }

		#region Disposable Pattern		
		/// <summary>
		/// 破棄進行を保持するフィールド
		/// </summary>
		bool _disposed = false;
		private List<CsvRecordItem> _recordItems;

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
			_process?.Dispose();
			_disposed = true;
			//
			if (disposing) Dispose(true);
		}
		#endregion

		/// <summary>
		/// 変換を実行する
		/// </summary>
		/// <param name="executeParams">パラメータ類</param>
		public void Execute(RenameExecuteParams executeParams, out string filePath)
		{
			//実行パラメータ
			_param = executeParams;
			//変換結果アイテム
			_recordItems = new List<CsvRecordItem>();
			//変換レコーダ
			_recorder = new CsvRecorder(GetReseultFilePath());
			//結果ファイルパス
			filePath = _recorder.FilePath;
			//パラメータがないときの処理
			if (_param == null)
			{
				throw new ArgumentNullException(nameof(_param),
									"パラメータがNullです。");
			}
			//プロセスの起動
			_process = GetIcadProcess();
			//イベント
			ExecuteStarted?.Invoke(this, new EventArgs());
			//初期化
			SxSys.init(_param.Settings.ICADLinkPort, false);
			//フラグがあればアイコン化
			if (_param.Settings.ICADMinimize)
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
				_param.SourcePath
				, SystemSettings.IcadExtension
				, SearchOption.AllDirectories);
			//イベント
			FileCopyStarted?.Invoke(this, new EventArgs());
			//ファイルコピーと変更記録
			_recordItems.AddRange(CopyFiles(source));
			//イベント
			PartRenameStarted?.Invoke(this, new EventArgs());
			//パーツ変更
			ExecuteReplace(GetSuccessedFiles());
			//キャンセル
			if (CancelRequest)
			{
				ExecuteCanceled?.Invoke(this, new EventArgs());
				return;
			}
			//プロセスの開き直し
			_process.Close();
			_process = GetIcadProcess();
			//イベント
			DrawingTitleStarted?.Invoke(this, new EventArgs());
			//図面変更
			ExecuteDrawingTitle(Directory.GetFiles(
				_param.DestinationPath
				, SystemSettings.IcadExtension
				, SearchOption.AllDirectories));
			_recorder.WriteAll(_recordItems);
			//イベント
			ExecuteFinished?.Invoke(this, new EventArgs());

		}

		/// <summary>
		/// 変換のキャンセルを実行する
		/// </summary>
		/// <param name="files">変換しているファイル配列</param>
		/// <param name="index">キャンセルする1個手前のインデックス</param>
		private void CancelExecute(IEnumerable<(string file, int index)> files, int index)
		{
			var remainedFiles = files.Where(x => x.index + 1 >= index);
			foreach (var f in remainedFiles)
			{
				var record = _recordItems.FirstOrDefault(
					x => x.DestinationPath == f.file);
				//
				record.IsSuccess = false;
				record.Remark = GetRemark(record.Remark, ErrorCategory.CancelByUSer);
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
			//実行ルーチン
			Parallel.ForEach(files, (Action<string>) (file =>
			 {
				 //新しいファイル
				 var newFile = this.GetNewPath((string) file);
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
				 try
				 {
					 //コピー実行
					 File.Copy(file, newFile, true);
				 }
				 catch (Exception e)
				 {
					 var name = Path.GetFileName(newFile);
					 RenameLogger.WriteLog(new LogItem
					 {
						 Exception = e,
						 Level = LogLevel.Error,
						 Message = $"{newFile}は{newDir}にコピーできませんでした。"
					 });
					 //
					 item.IsSuccess = false;
					 item.Remark = $"ファイルコピーが失敗しました。\r\n{e.Message}";
				 }
				 finally
				 {
					 items.Add(item);
				 }
			 }));
			return items.OrderBy(x => x.SourcePath).ToArray();
		}

		/// <summary>
		/// 図面表題欄の変更を実行する
		/// </summary>
		/// <param name="files">変更するファイルパス配列</param>
		private void ExecuteDrawingTitle(IEnumerable<string> files)
		{
			/*	メインルーチン
				ここからスタート
			*/
			//CSVレコード
			CsvRecordItem record = null;
			//実行
			foreach (var (file, fileIndex) in files.Indexed())
			{
				//ファイルモデル
				var fModel = new SxFileModel(file);
				//モデル
				var model = fModel.open(false);
				//2次元
				SxSys.setDim(false);
				//研削タイプの設定
				var searchType = new SxInfSearchEntType();
				searchType.setStatus(SxEntSeg.SEGTYPE_DELTA, true);
				searchType.setStatus(SxEntSeg.SEGTYPE_TEXT, true);
				SxSys.setSearchEntType(searchType);
				//レコードを取得
				record = _recordItems.FirstOrDefault(x => x.DestinationPath == file);
				//ビューリスト
				var vsArray = model.getVSList();
				//
				foreach (var (vs, vsIndex) in vsArray.Indexed())
				{

					//セグメントリスト
					var segList = vs.getSegList(0, 0, false, true, false, false);
					//セグメントチェック
					if (segList == null)
					{
						DetailChanged?.Invoke(this,
							new ItemProgressedEventArgs
							{
								//ファイルカウント
								FileCount = new CountItem
								{
									Counter = fileIndex + 1,
									Items = files.Count(),
									Name = Path.GetFileName(file)
								},
								//ビュー情報
								ViewCount = new CountItem
								{
									Counter = vsIndex + 1,
									Items = vsArray.Length,
									Name = vs.getInf().name
								},
								//セグメント情報
								DetailCount = new CountItem
								{
									Counter = 0,
									Items = 0,
									Name = string.Empty
								}
							});
					}
					//セグメントを検索
					foreach (var (seg, segIndex) in segList.Indexed())
					{
						//イベント
						DetailChanged?.Invoke(this,
						new ItemProgressedEventArgs
						{
							//ファイル情報
							FileCount = new CountItem
							{
								Counter = fileIndex + 1,
								Items = files.Count(),
								Name = Path.GetFileName(file)
							},
							//ビュー情報
							ViewCount = new CountItem
							{
								Counter = vsIndex + 1,
								Items = vsArray.Length,
								Name = vs.getInf().name
							},
							//セグメント情報
							DetailCount = new CountItem
							{
								Counter = segIndex + 1,
								Items = segList.Length,
								Name = ((ulong) seg.ID).ToString()
							}
						});
						//セグメントタイプで実行するメソッドを選択
						switch (seg.Type)
						{
							case SxEntSeg.SEGTYPE_DELTA:
								DeleteDelta(seg);
								break;
							case SxEntSeg.SEGTYPE_TEXT:
								ChangeDrawNumber(seg);
								ChangeDate(seg);
								ChangeSignature(seg);
								DeleteDeltaRemark(seg);
								break;
						}
					}
				}
				//更新可能なら
				if (_param.Settings.CanExecuteUpdate)
				{
					//イベント
					UpdateStarted?.Invoke(this, new EventArgs());
					//更新コマンド発行
					SxSys.command(SystemSettings.UpdateCommand, false);
				}
				//保存
				model.save();
				//クローズ
				model.close(false);
				if (CancelRequest)
				{
					CancelExecute(files.Indexed(), fileIndex);
					WriteCancelByUserLog(file);
					break;
				}
			}
			return;
			//訂正記号削除
			void DeleteDelta(SxEntSeg seg)
			{
				try
				{
					//訂正記号削除可能なら
					if (_param.Settings.CanDeleteDelta)
					{
						//削除
						seg.delete();
					}
				}
				catch (Exception e)
				{
					record.IsSuccess = false;
					record.Remark = GetRemark(record.Remark, ErrorCategory.Delta, e);
					RenameLogger.WriteLog(new LogItem
					{
						Exception = e,
						Level = LogLevel.Error,
						Message = GetExchangeError(ErrorCategory.Delta)
					});
				}
			}
			//図番変更
			void ChangeDrawNumber(SxEntSeg seg)
			{
				//ジオメトリに変換
				if (seg.getGeom() is SxGeomText geomText)
				{
					//１行のテキストのみ
					if (geomText.text_line_num != 1) return;
					//一致パターンと照合
					foreach (var pattern in _keywords.DrawNumberRegexes)
					{
						//テキストを取得
						var gText = geomText.txt[0];
						var text = Strings.StrConv(gText, VbStrConv.Narrow);
						//照合
						if (!Regex.IsMatch(text, $"^({pattern})", RegexOptions.Compiled)) continue;
						//置き換えパターン
						foreach (var prefixPattern in _keywords.DrawNumberSplit)
						{
							//照合
							if ((Regex.IsMatch(text, $"^({prefixPattern})", RegexOptions.Compiled)))
							{
								try
								{
									//変更
									seg.editText(Regex.Replace(text, prefixPattern, _param.PrefixName));
									return;
								}
								catch (SxException e)
								{
									record.IsSuccess = false;
									record.Remark = GetRemark(record.Remark, ErrorCategory.DrawingNumber, e);
									RenameLogger.WriteLog(new LogItem
									{
										Exception = e,
										Level = LogLevel.Error,
										Message = GetExchangeError(ErrorCategory.DrawingNumber)
									});
									continue;
								}
							}
						}
					}
				}
			}
			//日付変更
			void ChangeDate(SxEntSeg seg)
			{
				//ジオメトリを取得
				if (seg.getGeom() is SxGeomText geomText)
				{
					//テキストが1行出なければスキップ
					if (geomText.text_line_num != 1) return;
					//日付パターンと照合
					foreach (var pattern in _keywords.DateRegexes)
					{
						try
						{
							//テキストを取得。半角に変更
							var text = Strings.StrConv(geomText.txt[0], VbStrConv.Narrow);
							//照合が一致すれば
							if (!Regex.IsMatch(text, $"^({pattern}$)", RegexOptions.Compiled)) continue;
							//変更
							seg.editText(DateTime.Today.ToString("yyyy/M/d"));
							return;
						}
						catch (SxException e)
						{
							record.IsSuccess = false;
							record.Remark = GetRemark(record.Remark, ErrorCategory.Date, e);
							RenameLogger.WriteLog(new LogItem
							{
								Exception = e,
								Level = LogLevel.Error,
								Message = GetExchangeError(ErrorCategory.Date)
							});
						}
					}
				}
			}
			//署名変更
			void ChangeSignature(SxEntSeg seg)
			{
				//ジオメトリを取得
				if (seg.getGeom() is SxGeomText geomText)
				{
					//1行
					if (geomText.text_line_num != 1) return;
					//照合
					foreach (var pattern in _keywords.Signatures)
					{
						try
						{
							//テキストを半角にして取得
							var text = Strings.StrConv(geomText.txt[0], VbStrConv.Narrow);
							//照合が一致すれば
							if (!Regex.IsMatch(text, $"^{pattern}", RegexOptions.Compiled)) continue;
							//変更
							seg.editText(_param.Signature);
							return;
						}
						catch (SxException e)
						{
							record.IsSuccess = false;
							record.Remark = GetRemark(record.Remark, ErrorCategory.Signature);
							RenameLogger.WriteLog(new LogItem
							{
								Exception = e,
								Level = LogLevel.Error,
								Message = GetExchangeError(ErrorCategory.Signature)
							});
							continue;
						}
					}
				}
			}
			//訂正注記変更
			void DeleteDeltaRemark(SxEntSeg seg)
			{
				//訂正記号削除許可なら
				if (!_param.Settings.CanDeleteDelta) return;
				//ジオメトリを取得
				if (seg.getGeom() is SxGeomText geomText)
				{
					//1行
					if (geomText.text_line_num != 1) return;
					//テキストを取得
					var text = geomText.txt[0];
					//訂正注記パターン
					foreach (var notePattern in _keywords.DeltaNoteRegexes)
					{
						//署名パターン
						foreach (var sigPattern in _keywords.Signatures)
						{
							try
							{
								//パターンの合成
								var pattern = $"({notePattern}{sigPattern})$";
								//照合
								if (!Regex.IsMatch(text, pattern, RegexOptions.Compiled)) continue;
								//削除
								seg.delete();
								return;
							}
							catch (SxException e)
							{
								record.IsSuccess = false;
								record.Remark = GetRemark(record.Remark, ErrorCategory.DeltaNote);
								RenameLogger.WriteLog(new LogItem
								{
									Exception = e,
									Level = LogLevel.Error,
									Message = GetExchangeError(ErrorCategory.DeltaNote)
								});
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// 変換を実行する
		/// </summary>
		/// <param name="files">変更するファイルリスト</param>
		private void ExecuteReplace(IEnumerable<string> files)
		{
			SxFileModel fModel = null;
			SxModel model = null;
			CsvRecordItem record = null;
			//
			foreach (var (file, fileIndex) in files.Indexed())
			{
				//ICADファイルモデル
				fModel = new SxFileModel(file);
				//ICADモデルを開く
				model = fModel.open(false);
				//変換記録レコード
				record = _recordItems.FirstOrDefault(x => x.DestinationPath == file);
				//実行
				ExecutePartName(file, fileIndex, files.Count());
				//保存
				SaveFile();
				//キャンセル
				if (CancelRequest)
				{
					CancelExecute(files.Indexed(), fileIndex);
					WriteCancelByUserLog(file);
					break;
				}
			}
			//元ファイル削除
			DeleteFile();
			return;

			//ファイルの削除
			void DeleteFile()
			{
				//イベント
				FileDeleteStarted?.Invoke(this, new EventArgs());
				//ファイルがあれば削除実行
				Parallel.ForEach(files,
					file =>
					{
						if (File.Exists(file)) File.Delete(file);
					});
			}
			//ファイルの保存
			void SaveFile()
			{
				//モデル情報
				var inf = model.getInf();
				//コピー先パス
				record.DestinationPath = $@"{inf.path}\{inf.name}.icd";
				//コピー先ファイル名
				record.DestinationFileName = $@"{inf.name}.icd";
				try
				{
					//新しいファイル名
					var newName = GetName(inf.name);
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
					model.close(false);
				}
			}

			/// パーツ名・パーツの図面名の変更
			void ExecutePartName(string file, int fileIndex, int Total)
			{
				/*	メインルーチン
					最上位パーツから子パーツへと降りていく*/
				//3D図面に切り替え
				SxSys.setDim(true);
				//3D空間リスト
				var wfArray = model.getWFList();
				foreach (var (wf, viewIndex) in wfArray.Indexed())
				{
					//3D空間リスト
					if (wf.getPartList() == null) continue;
					//最上位パーツ
					var topParts = wf.getTopPartList();
					//パーツがなければスキップ
					if (topParts == null) continue;
					//変更処理
					foreach (var (topPart, itemIndex) in topParts.Indexed())
					{
						//引数用パーツ
						SxEntPart refPart = topPart;
						//イベント
						DetailChanged?.Invoke(this,
							new ItemProgressedEventArgs
							{
								//ファイル
								FileCount = new CountItem
								{
									Counter = fileIndex + 1,
									Items = Total,
									Name = Path.GetFileName(file)
								},
								//3D空間
								ViewCount = new CountItem
								{
									Counter = viewIndex + 1,
									Items = wfArray.Length,
									Name = wf.getInf().name,
								},
								//進捗
								DetailCount = new CountItem
								{
									Counter = itemIndex + 1,
									Items = topParts.Length,
									Name = refPart.getInf().kind == SxInfEnt.KIND_PART ? topPart.getInfDetail().name : ""
								}
							});
						//新しい名前
						SetNewName(ref refPart);
						//子パーツ情報を変更
						ChangePart(ref refPart);
					}
				}
				//パーツ名の変更または置き換え
				void SetNewName(ref SxEntPart part)
				{
					if (part.getInf().kind != SxInfEnt.KIND_PART) return;
					//パーツ情報
					var inf = part.getInfDetail();
					//読取専用解除
					if (inf.is_read_only)
					{
						part.setAccess(false);
					}
					//新しい名前
					var newName = GetName(inf.name);
					//名前に図番を含むとき
					if (newName != null)
					{
						//新しいパス
						var newPath = newFilePath(inf.path, newName);
						//ファイルが存在するとき
						if (File.Exists(newPath))
						{
							//新しいパーツ名
							var newPartName = Path.GetFileNameWithoutExtension(newPath);
							//同じ名前が存在したらスキップ
							if (part.getInfDetail().name == newPartName) return;
							//ファイルモデル
							var fModel = new SxFileModel(newPath);
							//置換
							part.replace(fModel, true);
							//パーツ名の変更
							part.setName(fModel.getFileNameNoExt(), part.getInfDetail().comment, "", true);
						}
						else
						{
							//パーツ名を変更
							part.setName(newName, inf.comment, "", true);
							var partInf = part.getInfDetail();
						}
					}
				}
				//新しいファイル名
				static string newFilePath(string dir, string name)
					=> $@"{dir}\{name}.icd";
				//パーツ情報を変更する
				void ChangePart(ref SxEntPart parent)
				{
					if (parent.getInf().kind != SxInfEnt.KIND_PART) return;
					//子パーツリスト
					var childs = parent.getChildList();
					//なければ戻る(再帰処理の終了)
					if (childs == null) return;
					//子パーツの名前変更
					foreach (var child in childs)
					{
						try
						{
							//置き換え
							var refChild = child;
							//名前の変更
							SetNewName(ref refChild);
							//さらに子パーツを変更(再帰処理の開始）
							ChangePart(ref refChild);
							if (refChild.getChildList() == null) return;
						}
						catch (Exception e)
						{
							record.Remark = GetRemark(record.Remark, ErrorCategory.ChildPartName, e);
							//
							RenameLogger.WriteLog(new LogItem
							{
								Exception = e,
								Level = LogLevel.Error,
								Message = GetExchangeError(ErrorCategory.ChildPartName)
							});
							continue;
						}
					}
				}
			}
			//変更した名前を取得する
			string GetName(string name)
			{
				foreach (var pattern in _param.Settings.NewProjectRegex)
				{
					if (Regex.IsMatch(name, $"^{pattern}"))
					{
						return Regex.Replace(name, pattern, _param.PrefixName);
					}
				}
				return null;
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
			var relativePath = sourcePath.Remove(0, _param.SourcePath.Length);
			//
			return $"{_param.DestinationPath}{relativePath}";
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
				if (Regex.IsMatch(_param.SourcePath, pattern))
				{
					sourceName = Regex.Matches(_param.SourcePath, pattern)[0].Value;
					break;
				}
			}
			var filePath = $"{sourceName}→{_param.PrefixName}-変換結果-iCADRenamer.csv";
			return Path.Combine(_param.DestinationPath, filePath);
		}

		/// <summary>
		/// コピー成功ファイルの取得を実行する
		/// </summary>
		/// <returns></returns>
		private IEnumerable<string> GetSuccessedFiles() =>
					_recordItems.Where(x => x.IsSuccess).Select(x => x.DestinationPath);

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
