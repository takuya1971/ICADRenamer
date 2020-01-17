/* ICAD Renaler
	Copyright (c) 2020 T. Kinoshita. All Rights Reserved.
*/
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace ICADRenamer.CSV
{
	/// <summary>
	///	true, falseを成功・失敗に変換するを表すクラス
	/// </summary>
	/// <seealso cref="CsvHelper.TypeConversion.BooleanConverter" />
	internal class BooleanJapaneseConverter : BooleanConverter
	{
		/// <summary>
		/// falseの時の文字列を保持するフィールド
		/// </summary>
		const string _falseValue = "失敗";

		/// <summary>
		/// trueの時の文字列を保持するフィールド
		/// </summary>
		const string _trueValue = "成功";

		/// <summary>
		/// 文字列からの変換
		/// </summary>
		/// <param name="text">The string to convert to an object.</param>
		/// <param name="row">The <see cref="T:CsvHelper.IReaderRow" /> for the current record.</param>
		/// <param name="memberMapData">The <see cref="T:CsvHelper.Configuration.MemberMapData" /> for the member being created.</param>
		/// <returns>
		/// The object created from the string.
		/// </returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1304:CultureInfo を指定します", Justification = "<保留中>")]
		public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) => (text.ToLower()) switch
		{
			_trueValue => true,
			_falseValue => false,
			_ => base.ConvertFromString(text, row, memberMapData),
		};

		/// <summary>
		/// Converts the object to a string.
		/// </summary>
		/// <param name="value">The object to convert to a string.</param>
		/// <param name="row">The <see cref="T:CsvHelper.IWriterRow" /> for the current record.</param>
		/// <param name="memberMapData">The <see cref="T:CsvHelper.Configuration.MemberMapData" /> for the member being written.</param>
		/// <returns>
		/// The string representation of the object.
		/// </returns>
		public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
		{
			if (value is bool boolvalue)
			{
				return boolvalue ? _trueValue : _falseValue;
			}
			return base.ConvertToString(value, row, memberMapData);
		}
	}
}
