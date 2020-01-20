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

namespace ICADRenamer
{
	/// <summary>
	/// 名前変更コマンドを表すクラス
	/// </summary>
	public class RenameCommand : IDisposable
	{
		/// <summary>
		/// iCADの拡張子を保持するフィールド
		/// </summary>
		private const string _icadExtension = "*.icd";

		/// <summary>
		/// ICADのファイル名を保持するフィールド
		/// </summary>
		private const string _iCADName = "icadx4j";

		/// <summary>
		/// 工事番号のパターンを保持するフィールド
		/// </summary>
		private const string _projectCodePattern = @"^M\d\d\d\d";

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
		///  詳細項目変更時に動作するイベント
		/// </summary>
		public event EventHandler<CategoryChangeEventArgs> CategoryChanged;

		/// <summary>
		///  実行区分終了時に動作するイベント
		/// </summary>
		public event EventHandler<ItemProgressedEventArgs> CategoryPregressed;

		/// <summary>
		///  実行区分開始時に動作するイベント
		/// </summary>
		public event EventHandler<ItemProgressedEventArgs> CategoryStarted;

		/// <summary>
		/// ICAD起動時に動作するイベント
		/// </summary>
		public event EventHandler ICADStarting;

		/// <summary>
		/// ICAD起動完了時に動作するイベント
		/// </summary>
		public event EventHandler ICADStarted;

		/// <summary>
		///  実行終了時に動作するイベント
		/// </summary>
		public event EventHandler ExecuteFinished;

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
		/// コマンド実行する
		/// </summary>
		/// <param name="parameter">コマンドパラメータ。無いときは、 <see langword="null" />.</param>
		/// <exception cref="NotImplementedException"></exception>
		public void Execute(RenameExecuteParams parameter)
		{
			//実行パラメータ
			_param = parameter;
			//変換結果アイテム
			RecordItems = new List<CsvRecordItem>();
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
			if (parameter.Settings.ICADMinimize)
			{
				SxSys.setWindowStatus(SxWindow.STATUS_ICON);
			}
			else
			{
				SxSys.setWindowStatus(SxWindow.STATUS_NORMAL);
			}

			//コピー元ファイルの取得
			var files = Directory.GetFiles(_param.SourcePath
				, _icadExtension
				, SearchOption.AllDirectories);
			//イベント
			CategoryChanged?.Invoke(this,
				new CategoryChangeEventArgs
				{
					Category = ProgressCategory.File,
					TotalItem = files.Length
				});
			//変換処理
			foreach (var (file, index) in files.Indexed())
			{
				CsvRecordItem item = new CsvRecordItem
				{
					SourcePath = file,
					SourceFileName = Path.GetFileName(file),
				};
				//イベント
				CategoryStarted?.Invoke(this,
					new ItemProgressedEventArgs
					{
						Category = ProgressCategory.File,
						Name = Path.GetFileName(file),
						Counter = index
					});
				//実行
				item = Rename(file, item);
				//イベント
				CategoryPregressed?.Invoke(this,
					new ItemProgressedEventArgs
					{
						Category = ProgressCategory.File,
						Counter = index
					});
				//キャンセル要求が合ったら抜ける
				if (CancelRequest)
				{
					RenameLogger.WriteLog(
						new LogItem
						{
							Level = LogLevel.Info,
							Message = "ユーザーによってキャンセルされました。",
							Exception = null
						});
					break;
				}
			}
			//プロセスのクローズ
			_process?.Close();
			//変換結果の記録
			_recorder.WriteAll(RecordItems);
			//ログ
			RenameLogger.WriteLog(new LogItem
			{
				Level = LogLevel.Info,
				Message = "変換終了"
			});
			//イベント発生
			ExecuteFinished?.Invoke(this, new EventArgs());
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

		private CsvRecordItem Rename(string file, CsvRecordItem item)
		{
			//ファイルモデル
			var fModel = new SxFileModel(file);
			//モデル
			var model = fModel.open(false);
			_process?.WaitForInputIdle();
			//
			model.setActive();
			//実行
			try
			{
				//モデルが既にあればスキップ
				if (model.getInf().path != GetNewDir(
					Path.GetDirectoryName(file)))
				{
					//パーツの処理
					RenameParts(ref model);
					//モデルの処理
					RenameModel(ref model, ref item);
				}
				//図面の処理
				RenameDrawing(ref model);
				//保存
				model.save();
				_process.WaitForInputIdle();
				//クローズ
				model.close();
				_process.WaitForInputIdle();
				//変換成功
				item.IsSuccess = true;
				//日付
				item.Date = DateTime.Now;
				//ログ
				RenameLogger.WriteLog(LogMessageKind.ActionComplete
					, new List<(LogMessageCategory category, string message)>
					{
							(LogMessageCategory.SourceForm,"変換実行"),
							(LogMessageCategory.Message,"変換が完了しました。"),
							(LogMessageCategory.FileName,model.getInf().name),
					});
			}
			//エラー時
			catch (Exception e)
			{
				//記録
				item.Date = DateTime.Now;
				item.IsSuccess = false;
				item.Remark = $"{e.Message} [発生元]:{e.Source}";
				RecordItems.Add(item);
				//ログ
				RenameLogger.WriteLog(
					new LogItem
					{
						Level = LogLevel.Error,
						Message = "変換エラー",
						Exception = e
					});

				model?.close();
			}

			return item;
		}

		/// <summary>
		/// 削除する改訂注記を取得する
		/// </summary>
		/// <param name="segList">テキストセグメントのリスト</param>
		/// <returns>セグメントリスト。無いときはnull</returns>
		private SxEntSeg[] DeleteNoteSeg(IEnumerable<SxEntSeg> segList)
		{
			//セグメントリスト
			var segs = new List<SxEntSeg>();
			//
			foreach (var seg in segList)
			{
				//ジオメトリ
				var geom = (SxGeomText) seg.getGeom();
				foreach (var txt in geom.txt)
				{
					foreach (var notePattern in _keywords.DeltaNoteRegexes)
					{
						foreach (var sigPattern in _keywords.Signatures)
						{
							//検索パターン
							var reg = $"{notePattern}{sigPattern}$";
							//一致したらリストに追加
							if (Regex.IsMatch(txt, reg)) segs.Add(seg);
						}
					}
				}
			}
			return segs.ToArray(); ;
		}

		/// <summary>
		/// 新しい日付を取得する
		/// </summary>
		/// <param name="seg">検索するセグメント</param>
		/// <returns>日付。無いときはnull</returns>
		private string GetNewDate(SxEntSeg seg)
		{
			var geom = (SxGeomText) seg.getGeom();
			if (geom.txt.Length > 1) return null;
			//
			var regex = _keywords.DateRegexes;
			foreach (var pattern in regex)
			{
				//
				if (Regex.IsMatch(geom.txt[0], $"^{pattern}$"))
				{
					return DateTime.Today.ToShortDateString();
				}
			}
			return null;
		}

		/// <summary>
		/// 新しいフォルダを取得する
		/// </summary>
		/// <param name="sourceDir">コピー元のフォルダを表す文字列</param>
		/// <returns>コピー先のディレクトリパス</returns>
		private string GetNewDir(string sourceDir)
		{
			var pos = _param.SourcePath.Length;
			var relativeDir = sourceDir.Remove(0, pos);
			var newDir = _param.DestinationPath + relativeDir;
			if (!Directory.Exists(newDir)) Directory.CreateDirectory(newDir);
			return newDir;
		}

		/// <summary>
		/// 新しい図番を取得する
		/// </summary>
		/// <param name="seg">検索するセグメント</param>
		/// <returns>図番。ないときはnull</returns>
		private string GetNewDrawNumber(SxEntSeg seg)
		{
			//ジオメトリを取得
			var geom = (SxGeomText) seg.getGeom();
			if (geom.txt.Length > 1) return null;
			//検索パターン
			var regexes = _keywords.DrawNumberRegexes;
			//検索
			foreach (var pattern in regexes)
			{
				//パターンに一致しなければスキップ
				if (Regex.IsMatch(geom.txt[0], $"^{pattern}"))
				{
					//最初の"-"までのパターン
					var repRegex = pattern.Split('-')[0];
					//置換
					return Regex.Replace(geom.txt[0], repRegex, _param.PrefixName);
				}
			}
			return null;
		}

		/// <summary>
		/// M番を置き換えた図面名またはパーツ名の文字列を取得する
		/// </summary>
		/// <param name="name">置き換える文字列</param>
		/// <returns></returns>
		private string GetNewPath(string name)
			=> Regex.Replace(name, _projectCodePattern, _param.PrefixName);

		/// <summary>
		/// 結果ファイルノパスイベントを実行する
		/// </summary>
		/// <returns></returns>
		private string GetReseultFilePath()
		{
			var sourceRegex = Regex.Matches(_param.SourcePath, $"M[0-9]{4}|M[0-9]{3}");
			string sourcefileName = "M番なし";
			string postName = "変換結果-ICADRenamer";
			if (sourceRegex.Count > 0) sourcefileName = sourceRegex[0].Value;
			sourcefileName = $"{sourcefileName}→{_param.PrefixName}{postName}.csv";
			//
			return Path.Combine(_param.DestinationPath, sourcefileName);
		}

		/// <summary>
		/// 変更する署名を取得する
		/// </summary>
		/// <param name="seg">セグメント</param>
		/// <returns>名前。ないときはnull</returns>
		private string GetSignature(SxEntSeg seg)
		{
			//ジオメトリ
			var geom = (SxGeomText) seg.getGeom();
			//テキストの行数が2行以上あればnullを返す
			if (geom.txt.Length > 1) return null;
			//
			var regexes = _keywords.Signatures;
			//
			foreach (var pattern in regexes)
			{
				if (Regex.IsMatch(geom.txt[0], $"^{pattern}$"))
				{
					return _param.Signature;
				}
			}
			return null;
		}

		/// <summary>
		///   <para>
		///  図面の変更を実行する
		/// 。</para>
		///   <list type="bullet">
		///     <item>図面名を新しいM番に変更</item>
		///     <item>日付を今日に変更</item>
		///     <item>署名を変更</item>
		///     <item>更新処理</item>
		///   </list>
		/// </summary>
		/// <remarks>ここはasync/awaitで高速化できるかも</remarks>
		/// <param name="model"> 参照するモデル</param>
		private void RenameDrawing(ref SxModel model)
		{
			//2Dにする
			model.setDim(false);
			//ビューリスト
			var vsList = model.getVSList();
			//イベント
			CategoryChanged?.Invoke(this,
				new CategoryChangeEventArgs
				{
					Category = ProgressCategory.ChangeDrawing,
					TotalItem = vsList.Length
				});
			//実行
			foreach (var (vs, index) in vsList.Indexed())
			{
				//イベント
				CategoryStarted?.Invoke(this,
					new ItemProgressedEventArgs
					{
						Category = ProgressCategory.View,
						Name = vs.getInf().name,
						Counter = index
					});
				//
				vs.setActive();
				//テキストのセグメントを取得する
				var textSegList = vs.getSegList(0, 0, false, true)
					.Where(x => x.Type == SxEntSeg.SEGTYPE_TEXT);
				//イベント
				CategoryChanged?.Invoke(this,
					new CategoryChangeEventArgs
					{
						Category = ProgressCategory.Segment,
						TotalItem = textSegList.Count()
					});
				foreach (var (seg, j) in textSegList.Indexed())
				{
					//イベント
					CategoryStarted?.Invoke(this,
						new ItemProgressedEventArgs
						{
							Category = ProgressCategory.Segment,
							Name = seg.Type.ToString(),
							Counter = j
						});
					//図番変更
					var repNumber = GetNewDrawNumber(seg);
					//日付変更	
					var repDate = GetNewDate(seg);
					//署名
					var repSignature = GetSignature(seg);
					//変更の実行
					if (repDate != null) seg.editText(repDate);
					if (repNumber != null) seg.editText(repNumber);
					if (repSignature != null) seg.editText(repSignature);
					//イベント
					CategoryPregressed?.Invoke(this,
						new ItemProgressedEventArgs
						{
							Category = ProgressCategory.Segment,
							Name = seg.Type.ToString(),
							Counter = j
						});
				}
				//改訂記号削除
				if (_param.Settings.CanDeleteDelta)
				{
					//改訂注記
					var noteSeg = DeleteNoteSeg(textSegList);
					//改訂記号のリスト
					var deltaSegList = vs.getSegList(0, 0, false, true).Where(
						x => x.Type == SxEntSeg.SEGTYPE_DELTA).ToArray();
					//削除の実行
					if (noteSeg.Length > 0) SxEnt.delete(noteSeg);
					if (deltaSegList.Length > 0) SxEnt.delete(deltaSegList);
				}
				//イベント
				CategoryPregressed?.Invoke(this,
					new ItemProgressedEventArgs
					{
						Category = ProgressCategory.View,
						Name = vs.getInf().name,
						Counter = index,
					});
			}
			//更新処理
			if (_param.Settings.CanExecuteUpdate)
			{
				//イベント
				CategoryChanged?.Invoke(this,
					new CategoryChangeEventArgs
					{
						Category = ProgressCategory.Update,
						TotalItem = 1
					});
				//コマンドの実行
				SxSys.command(new string[]
				{
					@";MENUCVPO",
					@";CVREEXEC",
					@"@GO",
				}, true);
				//処理を待つ
				_process.WaitForInputIdle();
				//イベント
				CategoryPregressed?.Invoke(this,
					new ItemProgressedEventArgs
					{
						Category = ProgressCategory.Update,
						Counter = 1,
					});
			}
		}

		/// <summary>モデル名の変更を実行する</summary>
		/// <param name="model">変更するモデル</param>
		/// <param name="item">CSVアイテム</param>
		private void RenameModel(ref SxModel model, ref CsvRecordItem item)
		{
			//イベント
			CategoryChanged?.Invoke(this,
				new CategoryChangeEventArgs
				{
					Category = ProgressCategory.Model,
					Name = model.getInf().name,
					TotalItem = 1
				});
			var inf = model.getInf();
			var name = GetNewPath(inf.name);
			var dir = GetNewDir(inf.path);
			model.save(dir, name, inf.comment, 0, 0);
			//
			item.DestinationPath = $@"{dir}\{inf.name}{_icadExtension}";
			item.DestinationFileName = $@"{inf.name}{_icadExtension}";
			//イベント
			CategoryPregressed?.Invoke(this,
				new ItemProgressedEventArgs
				{
					Category = ProgressCategory.Model,
					Name = model.getInf().name,
					Counter = 1,
				});
		}

		/// <summary>
		///   <para>
		///  パーツ名の変更を実行する。</para>
		///   <para>
		/// 外部パーツを含む子パーツの名前も同時に変更する。</para>
		/// </summary>
		/// <param name="model">参照するモデル</param>
		private void RenameParts(ref SxModel model)
		{
			//3Dにセット
			model.setDim(true);
			//3D空間リスト
			var WFList = model.getWFList();
			//イベント
			CategoryChanged?.Invoke(this,
				new CategoryChangeEventArgs
				{
					Category = ProgressCategory.View,
					TotalItem = WFList.Length
				});
			//処理
			foreach (var (WF, i) in WFList.Indexed())
			{
				WF.setActive();
				//M番で始まるパーツを取得
				var parts = WF.getPartList().Where(
					x => Regex.IsMatch(x.getInfDetail().name, _projectCodePattern)).Indexed();
				//イベント
				CategoryStarted?.Invoke(this,
					new CategoryChangeEventArgs
					{
						Category = ProgressCategory.ChangePartName,
						TotalItem = parts.Count()
					});
				//パーツがなければスキップ
				if (!parts.Any()) continue;
				//実行
				foreach (var (part, j) in parts)
				{
					//イベント
					CategoryStarted?.Invoke(this,
						new ItemProgressedEventArgs
						{
							Category = ProgressCategory.ChangePartName,
							Name = part.getInfDetail().name,
							Counter = j,
						});
					//パーツをアクティブに
					//part.setActive();
					//パーツ詳細情報
					var partInf = part.getInfDetail();
					//未解決パーツならスキップ
					if (partInf.is_unloaded) continue;
					//新しいM番の名前を取得
					var partName = GetNewPath2(partInf.name);
					//パーツ名チェック
					try
					{
						//パーツ名変更
						part.setName(partName, partInf.comment, "", true);
						//外部パーツの処理
						if (partInf.is_external)
						{
							//アクセス権の取得
							//part.setAccess(false);
							//パーツの新しいフォルダ
							var newDir = GetNewDir(partInf.path);

							partInf.ref_model_name = partName;
							partInf.path = newDir;
							partInf.name = partName;
							part.saveAs(newDir, partName, partInf.comment);
							_process.WaitForInputIdle();
						}
					}
					catch (SxException e)
					{
						RenameLogger.WriteLog(new LogItem
						{
							Exception = e,
							Level = LogLevel.Error,
							Message = e.Message,
						});
					}
					//イベント
					CategoryPregressed?.Invoke(this,
						new ItemProgressedEventArgs
						{
							Category = ProgressCategory.ChangePartName,
							Name = part.getInfDetail().name,
							Counter = j,
						});
				}
				//イベント
				CategoryPregressed?.Invoke(this,
					new ItemProgressedEventArgs
					{
						Category = ProgressCategory.View,
						Name = WF.getInf().name,
						Counter = i,
					});
			}
		}

		public void Execute2(RenameExecuteParams executeParams)
		{
			_param = executeParams;
			//
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
				SxSys.setWindowStatus(SxWindow.STATUS_ICON);
			}
			else
			{
				SxSys.setWindowStatus(SxWindow.STATUS_NORMAL);
			}
			//
			var source = Directory.GetFiles(_param.SourcePath, SystemSettings.IcadExtension, SearchOption.AllDirectories);
			_recordItems.AddRange(CopyFiles(source));
			ExecuteReplace(GetSuccessedFiles());
			_recorder.WriteAll(_recordItems);
		}

		private IEnumerable<string> GetSuccessedFiles() =>
			_recordItems.Where(x => x.IsSuccess).Select(x => x.DestinationPath);

		private void ExecuteReplace(IEnumerable<string> files)
		{
			SxFileModel fModel = null;
			SxModel model = null;
			CsvRecordItem record = null;

			foreach (var file in files)
			{
				fModel = new SxFileModel(file);
				model = fModel.open(false);
				record = _recordItems.FirstOrDefault(x => x.DestinationPath == file);
				ExecutePartName();
				ExecuteDrawingTitle();
				SaveFile();
			}
			DeleteFile();
			return;

			//ファイルの削除
			void DeleteFile()
			{
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
					model.save();
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
					model.close();
				}
			}

			/// パーツ名・パーツの図面名の変更
			void ExecutePartName()
			{
				/*	メインルーチン
					最上位パーツから子パーツへと降りていく*/
				//
				foreach (var wf in model.getWFList())
				{
					//3D空間リスト
					if (wf.getPartList().Length == 0) continue;
					//最上位パーツ
					var topParts = wf.getTopPartList();
					if (!topParts.Any()) continue;
					//変更処理
					foreach (var topPart in topParts)
					{
						//パーツ情報
						var inf = topPart.getInfDetail();
						//パーツの置き換え
						var refPart = topPart;
						//外部パーツかつ読み取り専用のとき
						if (inf.is_external && inf.is_read_only)
						{
							//アクセス権取得
							topPart.setAccess(false);
						}
						//パーツ名を変更
						topPart.setName(GetName(inf.name), inf.comment, "", true);
						//子パーツ情報を変更
						ChangeChild(ref refPart);
					}
				}
				return;
				//子パーツ情報を変更する
				void ChangeChild(ref SxEntPart parent)
				{
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
							//パーツ情報
							var inf = child.getInfDetail();
							//新しい名前
							var newName = GetName(inf.name);
							if (newName == null) continue;
							//
							//外部パーツかつ読み取り専用のとき
							if (inf.is_external && inf.is_read_only)
							{
								//アクセス権取得
								child.setAccess(false);
							}
							//名前の変更
							child.setName(newName, inf.comment, "", true);
							//さらに子パーツを変更(再帰処理の開始）
							ChangeChild(ref refChild);
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
				//変更した名前を取得する
				string GetName(string name)
				{
					foreach (var pattern in _keywords.DrawNumberRegexes)
					{
						if (Regex.IsMatch(name, $"^{pattern}"))
						{
							return Regex.Replace(name, pattern, _param.PrefixName);
						}
					}
					return null;
				}
			}

			//図面表題欄の変更
			void ExecuteDrawingTitle()
			{
				/*	メインルーチン
					ビューリストから要素を変更する*/
				//ビューリスト
				var vsArray = model.getVSList();
				//
				foreach (var vs in vsArray)
				{
					//
					ChangeDrawNumber(vs);
					ChangeDate(vs);
					ChangeSignature(vs);
					if (_param.Settings.CanDeleteDelta)
					{
						DeleteDelta(vs);
						DeleteDeltaNote(vs);
					}
					if (_param.Settings.CanExecuteUpdate)
					{
						SxSys.command(SystemSettings.UpdateCommand, true);
					}
				}
				//テキストのセグメントリスト
				static IEnumerable<SxEntSeg> GetTextSegs(SxVS vs)
					=> vs.getSegList(0, 0, true, true)
					.Where(x => x.getGeom().type == SxGeom.TYPE_NOTE);
				//図面名変更
				void ChangeDrawNumber(SxVS vs)
				{
					//セグメントがあるかどうか
					var segs = GetTextSegs(vs);
					if (!segs.Any()) return;
					//
					try
					{
						foreach (var textSeg in segs)
						{
							if (textSeg.getGeom() is SxGeomText geomText)
							{
								foreach (var line in geomText.txt)
								{
									foreach (var pattern in _keywords.DrawNumberRegexes)
									{
										var txtLine = line;
										txtLine = Regex.Replace(line, pattern, _param.PrefixName);
									}
								}
							}
						}
					}
					catch (Exception e)
					{
						record.IsSuccess = false;
						record.Remark = GetRemark(record.Remark, ErrorCategory.DrawingNumber, e);
						RenameLogger.WriteLog(new LogItem
						{
							Exception = e,
							Level = LogLevel.Error,
							Message = GetExchangeError(ErrorCategory.DrawingNumber)
						});
					}
				}
				//日付変更
				void ChangeDate(SxVS vs)
				{
					var segs = GetTextSegs(vs);
					if (!segs.Any()) return;
					//
					try
					{
						foreach (var textSeg in segs)
						{
							if (textSeg.getGeom() is SxGeomText geomText)
							{
								if (geomText.text_line_num == 1)
								{
									foreach (var pattern in _keywords.DateRegexes)
									{
										if (!Regex.IsMatch(geomText.txt[0], pattern)) continue;
										//
										var date = DateTime.Today;
										geomText.txt[0] = $"{date.Year}/{date.Month}/{date.Day}";
										return;
									}
								}
							}
						}
					}
					catch (Exception e)
					{
						record.IsSuccess = false;
						record.Remark = GetRemark(record.Remark, ErrorCategory.Date, e);
						//ログ
						RenameLogger.WriteLog(new LogItem
						{
							Exception = e,
							Level = LogLevel.Error,
							Message = GetExchangeError(ErrorCategory.Date)
						});
					}
				}
				//署名変更
				void ChangeSignature(SxVS vs)
				{
					var segs = GetTextSegs(vs);
					if (!segs.Any()) return;
					try
					{
						foreach (var textSeg in segs)
						{
							if (textSeg.getGeom() is SxGeomText textGeom)
							{
								if (textGeom.text_line_num == 1)
								{
									foreach (var pattern in _keywords.Signatures)
									{
										if (Regex.IsMatch(textGeom.txt[0], pattern))
										{
											textGeom.txt[0] = _param.Signature;
											break;
										}
									}
								}
							}
						}
					}
					catch (Exception e)
					{
						record.IsSuccess = false;
						record.Remark = GetRemark(record.Remark, ErrorCategory.Signature, e);
						RenameLogger.WriteLog(new LogItem
						{
							Exception = e,
							Level = LogLevel.Error,
							Message = GetExchangeError(ErrorCategory.Signature)
						});
					}
				}
				//デルタマーク削除
				void DeleteDelta(SxVS vs)
				{
					var segs = vs.getSegList(0, 0, false, true, true, true)
						.Where(x => x.getGeom().type == SxGeom.TYPE_DELTA);
					if (!segs.Any()) return;
					//
					foreach (var seg in segs)
					{
						try
						{
							var deleteSeg = seg;
							deleteSeg.delete();
						}
						catch (Exception e)
						{
							record.IsSuccess = false;
							record.Remark = GetRemark(record.Remark, ErrorCategory.DelaNote, e);
							//ログ
							RenameLogger.WriteLog(new LogItem
							{
								Exception = e,
								Level = LogLevel.Error,
								Message = GetExchangeError(ErrorCategory.Delta)
							});
						}
					}
				}
				//訂正注記削除
				void DeleteDeltaNote(SxVS vs)
				{
					var segs = GetTextSegs(vs);
					if (!segs.Any()) return;
					//
					foreach (var seg in segs)
					{
						try
						{
							foreach (var pattern1 in _keywords.DeltaNoteRegexes)
							{
								foreach (var pattern2 in _keywords.Signatures)
								{
									//pattern1+pattern2で終わる
									var pattern = $"{pattern1}{pattern2}$";
									if (seg.getGeom() is SxGeomText geomText)
									{
										if (geomText.txt.Any(x => Regex.IsMatch(x, pattern)))
										{
											var deleteSeg = seg;
											deleteSeg.delete();
										}
									}
								}
							}
						}
						catch (Exception e)
						{
							record.IsSuccess = false;
							record.Remark = GetRemark(record.Remark, ErrorCategory.DelaNote, e);
							//ログ
							RenameLogger.WriteLog(new LogItem
							{
								Exception = e,
								Level = LogLevel.Error,
								Message = GetExchangeError(ErrorCategory.DelaNote)
							});
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
			sb.AppendLine(e.Message);
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
				 var newFile = this.GetNewPath2((string) file);
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

		private string GetNewPath2(string sourcePath)
		{
			var relativePath = sourcePath.Remove(0, _param.SourcePath.Length);
			//
			return $"{_param.DestinationPath}{relativePath}";
		}
	}
}
