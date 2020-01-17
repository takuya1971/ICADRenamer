/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICADRenamer.Log;
using NLog;

namespace ICADRenamer
{
	/// <summary>
	/// バージョン情報を表すクラス
	/// </summary>
	/// <seealso cref="System.Windows.Forms.Form" />
	partial class AboutBox : Form
	{
		/// <summary>
		///   <see cref="AboutBox"/> classの初期化
		/// </summary>
		public AboutBox()
		{
			InitializeComponent();
			this.Text = String.Format("{0} のバージョン情報", AssemblyTitle);
			this.labelProductName.Text = AssemblyProduct;
			this.labelVersion.Text = String.Format("バージョン {0}", AssemblyVersion);
			this.labelCopyright.Text = AssemblyCopyright;
			this.labelCompanyName.Text = AssemblyCompany;
			this.textBoxDescription.Text = AssemblyDescription;
		}

		#region アセンブリ属性アクセサー

		public static string AssemblyCompany
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCompanyAttribute) attributes[0]).Company;
			}
		}

		public static string AssemblyCopyright
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCopyrightAttribute) attributes[0]).Copyright;
			}
		}

		public static string AssemblyDescription => SystemMethods.GetLisence();

		public static string AssemblyProduct
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyProductAttribute) attributes[0]).Product;
			}
		}

		public static string AssemblyTitle
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				if (attributes.Length > 0)
				{
					AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute) attributes[0];
					if (titleAttribute.Title.Length > 0)
					{
						return titleAttribute.Title;
					}
				}
				return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}

		public static string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();
		#endregion

		/// <summary>
		/// フォームのクローズイベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void AboutBox_FormClosed(object sender, FormClosedEventArgs e)
			=> RenameLogger.WriteLog(LogMessageKind.Operation, new List<(LogMessageCategory category, string message)>
			{
				(LogMessageCategory.SourceForm,Text),
				(LogMessageCategory.Message,$"{Text}を閉じました。")
			});

		/// <summary>
		/// 情報ボックス表示イベントを実行する
		/// </summary>
		/// <param name="sender">イベント呼び出し元オブジェクト</param>
		/// <param name="e">e</param>
		private void AboutBox_Shown(object sender, EventArgs e)
					=> RenameLogger.WriteLog(LogMessageKind.Operation, new List<(LogMessageCategory category, string message)>
		{
			(LogMessageCategory.SourceForm,Text),
			(LogMessageCategory.Message,$"{Text}を開きました。")
		});
	}
}
