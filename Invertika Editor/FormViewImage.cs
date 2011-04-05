using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Invertika_Editor
{
	public partial class FormViewImage : Form
	{
		public Bitmap ImageToView { get; set; }

		public FormViewImage()
		{
			InitializeComponent();
		}

		private void FormViewImage_Load(object sender, EventArgs e)
		{
			pictureBox.Image=ImageToView;
		}
	}
}
