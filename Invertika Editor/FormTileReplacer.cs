using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Invertika_Editor
{
	public partial class FormTileReplacer : Form
	{
		public string Filename { get; set; }
		public int TileID { get; private set; }

		public FormTileReplacer()
		{
			InitializeComponent();
		}

		private void FormTileReplacer_Load(object sender, EventArgs e)
		{
			pbImage.Load(Filename);
		}

		private void pbImage_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			int xInTiles=e.X/32;
			int yInTiles=e.Y/32;

			int tileID=yInTiles*32+xInTiles;
			TileID=tileID;
			
			DialogResult=DialogResult.OK;
		}
	}
}
