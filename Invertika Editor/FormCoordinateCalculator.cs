using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CSCL.Games.Manasource;

namespace Invertika_Editor
{
	public partial class FormCoordinateCalculator:Form
	{
		public FormCoordinateCalculator()
		{
			InitializeComponent();
		}

		private void nudXTile_ValueChanged(object sender, EventArgs e)
		{
			nudXPixel.Value=Helper.GetPixelCoord((int)nudXTile.Value);
			tbXYPixel.Text=nudXPixel.Text+" x "+nudYPixel.Text;
		}

		private void nudYTile_ValueChanged(object sender, EventArgs e)
		{
			nudYPixel.Value=Helper.GetPixelCoord((int)nudYTile.Value);
			tbXYPixel.Text=nudXPixel.Text+" x "+nudYPixel.Text;
		}

		private void nudXPixel_ValueChanged(object sender, EventArgs e)
		{
			nudXTile.Value=Helper.GetTileCoord((int)nudXPixel.Value);
		}

		private void btnCopy_Click(object sender, EventArgs e)
		{
			rtbComments.Text+="\n"+tbXYPixel.Text;
		}

		private void nudYPixel_ValueChanged(object sender, EventArgs e)
		{
			nudYTile.Value=Helper.GetTileCoord((int)nudYPixel.Value);
		}
	}
}
