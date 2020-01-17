/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using System;
using System.Windows.Forms;

namespace ICADRenamer
{
	/// <summary>
	/// コンテキストメニューストリップを表すクラス
	/// </summary>
	/// <seealso cref="System.Windows.Forms.ContextMenuStrip" />
	public class RegexContextMenuStrip : ContextMenuStrip
	{
		/// <summary>
		///   <see cref="RegexContextMenuStrip"/> classの初期化
		/// </summary>
		public RegexContextMenuStrip()
		{
			Items.AddRange(new ToolStripMenuItem[]
			{
				AddNewItem,
				EditItem,
				DeleteItem,
			});
			AddNewItem.Click += AddNewItem_Click;
			EditItem.Click += EditItem_Click;
			DeleteItem.Click += DeleteItem_Click;
		}

		/// <summary>
		///  新規作成要求イベント時に動作するイベント
		/// </summary>
		public event EventHandler AddNewRequest;

		/// <summary>
		///  削除要求イベント時に動作するイベント
		/// </summary>
		public event EventHandler DeleteRequest;

		/// <summary>
		///  編集要求イベント時に動作するイベント
		/// </summary>
		public event EventHandler EditRequest;

		/// <summary>
		/// 新規作成ボタンを保持するプロパティ
		/// </summary>
		public ToolStripMenuItem AddNewItem { get; set; } = new ToolStripMenuItem
		{
			Name = "addNewItemMenuStrip",
			Text = "新規作成(&N)"
		};

		/// <summary>
		/// 削除ボタンを保持するプロパティ
		/// </summary>
		public ToolStripMenuItem DeleteItem { get; set; } = new ToolStripMenuItem
		{
			Name = "DeleteItemMenuStrip",
			Text = "削除(&D)"
		};

		/// <summary>
		/// 編集ボタンを保持するプロパティ
		/// </summary>
		public ToolStripMenuItem EditItem { get; set; } = new ToolStripMenuItem
		{
			Name = "EditItemMenuStrip",
			Text = "編集(&E)"
		};

		/// <summary>
		/// 新規作成ボタンクリックイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void AddNewItem_Click(object sender, EventArgs e) => AddNewRequest?.Invoke(this, new EventArgs());

		/// <summary>
		/// 削除用ボタンクリックイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void DeleteItem_Click(object sender, EventArgs e) => DeleteRequest?.Invoke(this, new EventArgs());

		/// <summary>
		/// 編集ボタンクリックイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void EditItem_Click(object sender, EventArgs e) => EditRequest?.Invoke(this, new EventArgs());
	}
}
