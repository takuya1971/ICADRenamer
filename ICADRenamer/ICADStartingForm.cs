using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICADRenamer
{
	/// <summary>
	/// ICAD起動中に表示するフォームを表すクラス
	/// </summary>
	/// <seealso cref="System.Windows.Forms.Form" />
	public partial class ICADStartingForm : Form
	{
		/// <summary>
		///  <see cref="ICADStartingForm"/> classの初期化
		/// </summary>
		public ICADStartingForm()
		{
			InitializeComponent();
		}
	}
}
