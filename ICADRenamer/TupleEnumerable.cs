/*	ICAD Renamer
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using System;
using System.Collections.Generic;

namespace ICADRenamer
{

	/// <summary>
	///   <para>列挙型タプルクラス 。
	///   <a href="https://gist.github.com/ufcpp/2b3e1a5821169f6b21ded175ad05c752">
	///   index付きforeach</a> からの引用。</para>
	/// </summary>
	public static partial class TupleEnumerable
	{

		/// <summary>インデックス付リストの集合を取得する</summary>
		/// <typeparam name="T">任意の型</typeparam>
		/// <param name="source">ソースと</param>
		/// <returns>  タプルのコレクション</returns>
		/// <exception cref="ArgumentNullException">source</exception>
		public static IEnumerable<(T item, int index)> Indexed<T>(this IEnumerable<T> source)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));

			IEnumerable<(T item, int index)> impl()
			{
				var i = 0;
				foreach (var item in source)
				{
					yield return (item, i);
					++i;
				}
			}
			return impl();
		}
	}
}
