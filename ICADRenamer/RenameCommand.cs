/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using ICADRenamer.CSV;
using ICADRenamer.Events;
using ICADRenamer.Log;
using ICADRenamer.Settings;

using NLog;

using sxnet;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using Microsoft.VisualBasic;

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

		public event EventHandler FileCopyStarted;

		public event EventHandler FileDeleteStarted;

		public event EventHandler PartRenameStarted;

		public event EventHandler DrawingTitleStarted;

		public event EventHandler UpdateStarted;

		public event EventHandler<ItemProgressedEventArgs> DetailChanged;

		/// <summary>
		/// 実行開始イベント
		/// </summary>
		public event EventHandler ExecuteStarted;

		/// <summary>
		/// ICAD起動時に動作するイベント
		/// </summary>
		public event EventHandler ICADStarting;

		/// <summary>
		/// ICAD起動完了時に動作するイベント
		/// </summary>
		public event EventHandler ICADStarted;

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
		/// 結果ファイルノパスイベントを実行する
		/// </summary>
		/// <returns></returns>
		private string GetReseultFilePath()
		{
			//コピー元
			string sourceName = "M番なし";
			//パターン検索
			foreach (var pattern in _keywords.DrawNumberSplit)
			{
				if(Regex.IsMatch(_param.SourcePath, pattern))
				{
					sourceName = Regex.Matches(_param.SourcePath, pattern)[0].Value;
					break;
				}
			}
			var filePath = $"{sourceName}→{_param.PrefixName}-変換結果-iCADRenamer.csv";
			return Path.Combine(_param.DestinationPath, filePath);
		}

		public void Execute(RenameExecuteParams executeParams)
		{
			//実行パラメータ
			_param = executeParams;
			//変換結果アイテム
			_recordItems = new List<CsvRecordItem>();
			//変換レコーダ
			_recorder = new CsvRecorder(GetReseultFilePath());
			//パラメータがないときの処理
			if (_param == null)
			{
				throw new ArgumentNullException(nameof(_param),
									"パラメータがNullです。");
			}
			//プロセスの起動
			_process = GetIcadProcess();
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
		}

		private IEnumerable<string> GetSuccessedFiles() =>
			_recordItems.Where(x => x.IsSuccess).Select(x => x.DestinationPath);

		private void ExecuteReplace(IEnumerable<string> files)
		{
			SxFileModel fModel = null;
			SxModel model = null;
			CsvRecordItem record = null;
			//
			foreach (var (file,fileIndex) in files.Indexed())
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
			}
			//
			DeleteFile();
			return;

			//ファイルの削除
			void DeleteFile()
			{
				FileDeleteStarted?.Invoke(this, new EventArgs());
				Parallel.ForEach(files,
					file =>
					{
						if (File.Exists(file)) File.Delete(file);
					});
			}
			//ファイルの保存
			void SaveFile()
			{
				var inf = model.getInf();
				record.DestinationPath = $@"{inf.path}\{inf.name}.icd";
				record.DestinationFileName = $@"{inf.name}.icd";
				try
				{
					var newName = GetName(inf.name);
					if (File.Exists($@"{inf.path}\\{newName}.icd"))
					{
						model.save();
					}
					else
					{
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
			void ExecutePartName(string file,int fileIndex, int Total)
			{
				/*	メインルーチン
					最上位パーツから子パーツへと降りていく*/
				//
				SxSys.setDim(true);
				var wfArray = model.getWFList();
				foreach (var (wf,viewIndex) in wfArray.Indexed())
				{
					//3D空間リスト
					if (wf.getPartList() == null) continue;
					//最上位パーツ
					var topParts = wf.getTopPartList();
					//パーツがなければスキップ
					if (topParts==null) continue;
					//変更処理
					foreach (var (topPart,itemIndex) in topParts.Indexed())
					{
						SxEntPart refPart = topPart;

						DetailChanged?.Invoke(this,
							new ItemProgressedEventArgs
							{
								FileCount = new CountItem
								{
									Counter = fileIndex,
									Items = Total,
									Name = Path.GetFileName(file)
								},
								ViewCount = new CountItem
								{
									Counter = viewIndex,
									Items = wfArray.Length,
									Name = wf.getInf().name,
								},
								DetailCount = new CountItem
								{
									Counter = itemIndex,
									Items = topParts.Length,
									Name = refPart.getInf().kind==SxInfEnt.KIND_PART?topPart.getInfDetail().name:""
								}
							});
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
					if (inf.is_read_only)
					{
						part.setAccess(false);
					}
					var newName = GetName(inf.name);
					if (newName != null)
					{
						var newPath = newFilePath(inf.path, newName);
						if (File.Exists(newPath))
						{
							var newPartName = Path.GetFileNameWithoutExtension(newPath);
							if (part.getInfDetail().name == newPartName) return;
							var fModel = new SxFileModel(newPath);
							part.replace(fModel, true);
							part.setName(fModel.getFileNameNoExt(), part.getInfDetail().comment, "", true);
						}
						else
						{
							//パーツ名を変更
							part.setName(newName, inf.comment, "", true);
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

		private void ExecuteDrawingTitle(IEnumerable<string> files)
		{
			/*	メインルーチン
				ここからスタート*/
			CsvRecordItem record = null;
			foreach (var (file,fileIndex) in files.Indexed())
			{
				var fModel = new SxFileModel(file);
				var model = fModel.open(false);
				SxSys.setDim(false);
				record = _recordItems.FirstOrDefault(x => x.DestinationPath == file);
				var vsArray = model.getVSList();
				foreach(var (vs,vsIndex) in vsArray.Indexed())
				 {
					var segList = vs.getSegList(0, 0, true, true, true, true);
					foreach (var (seg,segIndex) in segList.Indexed())
					{
						//イベント
						DetailChanged?.Invoke(this,
						new ItemProgressedEventArgs
						{
							FileCount = new CountItem
							{
								Counter = fileIndex,
								Items = files.Count(),
								Name = Path.GetFileName(file)
							},
							ViewCount = new CountItem
							{
								Counter = vsIndex,
								Items = vsArray.Length,
								Name = vs.getInf().name
							},
							DetailCount = new CountItem
							{
								Counter = segIndex,
								Items = segList.Length,
								Name = seg.ID.ToString()
							}
						});
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
				if (_param.Settings.CanExecuteUpdate)
				{
					//イベント
					UpdateStarted?.Invoke(this, new EventArgs());
					//更新コマンド発行
					SxSys.command(SystemSettings.UpdateCommand, false);
				}
				model.save();
				model.close(false);
			}
			return;

			//訂正記号削除
			void DeleteDelta(SxEntSeg seg)
			{
				try
				{
					if (_param.Settings.CanDeleteDelta)
					{
						seg.delete();
					}
				}
				catch (Exception e)
				{
					record.IsSuccess = false;
					record.Remark = GetRemark(record.Remark, ErrorCategory.Delta, e);
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
								//変更
								seg.editText(Regex.Replace(text, prefixPattern, _param.PrefixName));
								return;
							}
						}
					}
				}
			}
			//日付変更
			void ChangeDate(SxEntSeg seg)
			{
				if (seg.getGeom() is SxGeomText geomText)
				{
					if (geomText.text_line_num != 1) return;
					foreach (var pattern in _keywords.DateRegexes)
					{
						var text = Strings.StrConv(geomText.txt[0], VbStrConv.Narrow);
						if (!Regex.IsMatch(text, $"^({pattern}$)", RegexOptions.Compiled)) continue;
						seg.editText(DateTime.Today.ToString("yyyy/M/d"));
						return;
					}
				}
			}
			//署名変更
			void ChangeSignature(SxEntSeg seg)
			{
				if (seg.getGeom() is SxGeomText geomText)
				{
					if (geomText.text_line_num != 1) return;
					foreach (var pattern in _keywords.Signatures)
					{
						var text = Strings.StrConv(geomText.txt[0], VbStrConv.Narrow);
						if (!Regex.IsMatch(text, $"^{pattern}", RegexOptions.Compiled)) continue;
						seg.editText(_param.Signature);
						return;
					}
				}
			}
			//訂正注記変更
			void DeleteDeltaRemark(SxEntSeg seg)
			{
				if (!_param.Settings.CanDeleteDelta) return;
				//
				if (seg.getGeom() is SxGeomText geomText)
				{
					if (geomText.text_line_num != 1) return;
					var text = geomText.txt[0];
					foreach (var notePattern in _keywords.DeltaNoteRegexes)
					{
						foreach (var sigPattern in _keywords.Signatures)
						{
							var pattern = $"({notePattern}{sigPattern})$";
							if (!Regex.IsMatch(text, pattern, RegexOptions.Compiled)) continue;
							seg.delete();
							return;
						}
					}
				}
			}
		}

		private string GetRemark(string remark, ErrorCategory category, Exception e = null)
		{
			StringBuilder sb;
			if (remark.Length > 0)
			{
				sb = new StringBuilder(remark);
				sb.AppendLine();
			}
			else
			{
				sb = new StringBuilder();
			}
			sb.AppendLine(GetExchangeError(category));
			if (e != null)
			{
				sb.AppendLine(e.Message);
			}
			return sb.ToString();
		}

		private string GetExchangeError(ErrorCategory category)
		{
			var mes = category switch
			{
				ErrorCategory.ChildPartName => "子パーツ名変更",
				ErrorCategory.Date => "日付変更",
				ErrorCategory.DelaNote => "改訂注記削除",
				ErrorCategory.Delta => "改訂記号削除",
				ErrorCategory.DrawingNumber => "図番変更",
				ErrorCategory.FileCopy => "ファイルコピー",
				ErrorCategory.ModelName => "モデル名変更",
				ErrorCategory.Signature => "署名変更",
				ErrorCategory.Save => "保存",
				_ => throw new NotImplementedException()
			};
			return $"{mes}エラー";
		}

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
			DelaNote,
			/// <summary>
			/// 保存
			/// </summary>
			Save,
		}

		private CsvRecordItem[] CopyFiles(string[] files)
		{
			var items = new List<CsvRecordItem>();

			Parallel.ForEach(files, (Action<string>) (file =>
			 {
				 var newFile = this.GetNewPath((string) file);
				 var newDir = Path.GetDirectoryName(newFile);
				 var item = new CsvRecordItem
				 {
					 Date = DateTime.Now,
					 SourcePath = file,
					 SourceFileName = Path.GetFileName(file),
					 DestinationPath = newFile,
					 DestinationFileName = Path.GetFileName(newFile),
					 IsSuccess = true,
				 };
				 if (!Directory.Exists(newDir))
				 {
					 Directory.CreateDirectory(newDir);
				 }
				 try
				 {
					 File.Copy(file, newFile);
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

		private string GetNewPath(string sourcePath)
		{
			var relativePath = sourcePath.Remove(0, _param.SourcePath.Length);
			//
			return $"{_param.DestinationPath}{relativePath}";
		}
	}
}
