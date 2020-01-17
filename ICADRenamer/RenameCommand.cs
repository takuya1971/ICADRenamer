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
		private const string _ProjectCodePattern = @"^M\d\d\d\d";

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
		public List<CsvRecordItem> RecordItems { get; private set; }

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
			if (_process == null)
			{
				var pCount = Process.GetProcessesByName(_iCADName).Length;
				if (pCount == 0)
				{
					_process = new Process();
					_process.StartInfo.FileName = @$"{Environment.GetEnvironmentVariable("ICADDIR")}\bin\icadx4j.exe";
					var f = _process.Start();
					_process.WaitForInputIdle();

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
				}
			}
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
				try
				{
					//ファイルモデル
					var fModel = new SxFileModel(file);
					//モデル
					var model = fModel.open(false);
					_process.WaitForInputIdle();
					//
					model.setActive();
					//パーツの処理
					RenameParts(ref model);
					//モデルが既にあればスキップ
					if (model.getInf().path != GetNewDir(
						Path.GetDirectoryName(file)))
					{
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
				}
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
				var geom = (SxGeomText)seg.getGeom();
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
			var geom = (SxGeomText)seg.getGeom();
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
			var pos = _param.SourcePath.Length + 1;
			var relativeDir = sourceDir.Remove(0, pos);
			var newDir = Path.Combine(_param.DestinationPath, relativeDir);
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
			var geom = (SxGeomText)seg.getGeom();
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
		private string GetNewName(string name)
			=> Regex.Replace(name, _ProjectCodePattern, _param.PrefixName);

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
			var geom = (SxGeomText)seg.getGeom();
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
			var name = GetNewName(inf.name);
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
					x => Regex.IsMatch(x.getInfDetail().name, _ProjectCodePattern)).Indexed();
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
					part.setActive();
					//パーツ詳細情報
					var partInf = part.getInfDetail();
					//未解決パーツならスキップ
					if (partInf.is_unloaded) continue;
					//新しいM番の名前を取得
					var partName = GetNewName(partInf.name);
					//パーツ名変更
					part.setName(partName, partInf.comment, "", true);
					//外部パーツの処理
					if (partInf.is_external)
					{
						//アクセス権の取得
						part.setAccess(false);
						//パーツの保存
						part.saveAs(GetNewDir(partInf.path), partName, partInf.comment);
						_process.WaitForInputIdle();
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
						Name=WF.getInf().name,
						Counter=i,
					});
			}
		}
	}
}
