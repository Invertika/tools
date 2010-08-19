using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Invertika_Editor
{
	public partial class FormTilesetCalculator : Form
	{
		public FormTilesetCalculator()
		{
			InitializeComponent();
		}

		private void nudTileHeight_ValueChanged(object sender, EventArgs e)
		{
			int value=(int)nudTileHeight.Value;
			int tmp=1024/value;
			int height=tmp*value;

			string output=String.Format("Tilesetgröße: 1024 x {0} Pixel", height);
			lblOutput.Text=output;
		}

		private void btnCalc_Click(object sender, EventArgs e)
		{
			nudTileHeight_ValueChanged(null, null);
		}
	}
}
