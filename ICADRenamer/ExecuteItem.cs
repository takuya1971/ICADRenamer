/* ICAD Renaler
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICADRenamer
{
	/// <summary>
	/// 実行区分
	/// </summary>
	public enum ExecuteItem
	{
		/// <summary>
		/// パーツ名変更+図番変更を実行
		/// </summary>
		All,
		/// <summary>
		/// パーツ名変更を実行
		/// </summary>
		RenameParts,
		/// <summary>
		/// 図番変更を実行
		/// </summary>
		DrawingTitiles
	}
}