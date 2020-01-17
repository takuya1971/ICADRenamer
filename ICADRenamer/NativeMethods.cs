/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using System.Runtime.InteropServices;

namespace ICADRenamer
{
	/// <summary>
	/// WinAPIの参照を表すクラス
	/// </summary>
	public static class NativeMethods
	{
		/// <summary>
		/// WinAPI コンソールの新規配置を実行する
		/// </summary>
		/// <returns></returns>
		[DllImport("kernel32.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool AllocConsole();

		/// <summary>
		/// WinAPI 親コンソールへのアタッチを実行する
		/// </summary>
		/// <param name="dwProcessId">プロセスID。親プロセスはuint.Maxvalueで取得可</param>
		/// <returns></returns>
		[DllImport("kernel32.dll")]
		public static extern bool AttachConsole(uint dwProcessId);

		/// <summary>
		/// WinAPI コンソールの開放
		/// </summary>
		/// <returns></returns>
		[DllImport("kernel32.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool FreeConsole();
	}
}
