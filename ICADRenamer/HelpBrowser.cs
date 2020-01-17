/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using ICADRenamer.Log;

using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ICADRenamer
{
	/// <summary>
	/// ヘルプブラウザを表すクラス
	/// </summary>
	/// <seealso cref="System.Windows.Forms.Form" />
	public partial class HelpBrowser : Form
	{

		/// <summary>
		///   <see cref="HelpBrowser"/> classの初期化
		/// </summary>
		public HelpBrowser() => InitializeComponent();

		/// <summary>
		/// 開くURLを保持するプロパティ
		/// </summary>
		public string Target { get; set; }

		/// <summary>
		/// ヘルプブラウザのクローズイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e"></param>
		private void HelpBrowser_FormClosed(object sender, FormClosedEventArgs e)
			=> RenameLogger.WriteLog(LogMessageKind.Operation, new List<(LogMessageCategory category, string message)>
			{
				(LogMessageCategory.SourceForm,Text),
				(LogMessageCategory.Message,$"{Text}が閉じました。")
			});

		/// <summary>
		/// フォームのロードイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		[Obsolete]
		private void HelpBrowser_Load(object sender, EventArgs e)
		{
			_webView.NavigateToLocal(Target);
			_undoButton.Enabled = false;
			_redoButton.Enabled = false;
		}

		/// <summary>
		/// ヘルプブラウザのShownイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void HelpBrowser_Shown(object sender, EventArgs e)
			=> RenameLogger.WriteLog(LogMessageKind.Operation, new List<(LogMessageCategory category, string message)>
			{
				(LogMessageCategory.SourceForm,Text),
				(LogMessageCategory.Message,$"{Text}を開きました。")
			});

		/// <summary>
		/// ホームボタンクリックイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>

		[Obsolete]
		private void HomeButton_Click(object sender, EventArgs e) => _webView.NavigateToLocal(Target);

		/// <summary>
		/// 進むボタンクリックイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void RedoButton_Click(object sender, EventArgs e) => _webView.GoForward();

		/// <summary>
		/// 戻るボタンクリックイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void UndoButton_Click(object sender, EventArgs e) => _webView.GoBack();

		/// <summary>
		/// 更新ボタンクリックイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void UpdateButton_Click(object sender, EventArgs e) => _webView.Update();

		/// <summary>
		/// ナビゲーション完了イベントイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void WebView_NavigationCompleted(object sender, WebViewControlNavigationCompletedEventArgs e)
		{
			_undoButton.Enabled = _webView.CanGoBack;
			_redoButton.Enabled = _webView.CanGoForward;
			//ログ
			RenameLogger.WriteLog(LogMessageKind.HelpPageNavigate, new List<(LogMessageCategory category, string message)>
			{
				(LogMessageCategory.SourceForm,Text),
				(LogMessageCategory.Message,"ページ移動しました。"),
				(LogMessageCategory.MoveTo,e.Uri.AbsoluteUri),
				(LogMessageCategory.Result,e.IsSuccess.ToString())
			});
		}
	}
}
