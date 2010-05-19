using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CSCL;
using CSCL.Graphic;
using CSCL.Games.Manasource;

namespace Invertika_Editor
{
	public partial class FormCreateMapsFromBitmap:Form
	{
		int FortschrittMax;
		int FortschrittValue;

		public FormCreateMapsFromBitmap()
		{
			InitializeComponent();
		}

		private void btnStartProcess_Click(object sender, EventArgs e)
		{
			//Pfade checken

			btnStartProcess.Enabled=false;

			backgroundWorker.RunWorkerAsync();
		}

		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			string fnBitmap=tbBitmap.Text;
			string pathMapsTemplates=Globals.folder_clientdata_mapstemplates;
			string pathOutput=tbTargetPath.Text;
			int MapX=(int)(decimal)nudXmin.Value;
			int MapY=(int)(decimal)nudYmax.Value;

			//Folder
			FileSystem.CreateDirectory(pathOutput, true);

			//Datei laden
			gtImage tmp=gtImage.FromFile(fnBitmap);

			//Vars
			uint kSize=100;

			uint y=0;
			uint x=0;

			string TemplateMap="";

			FortschrittMax=(int)tmp.Height;
			FortschrittValue=0;

			while(y<tmp.Height)
			{
				FortschrittValue=(int)y;
				backgroundWorker.ReportProgress(0);

				Console.Write(".");
				x=0;
				MapX=-25;

				while(x<tmp.Width)
				{
					gtImage sub=tmp.GetSubImage(x, y, kSize, kSize);
					Color col=sub.GetMedianColor();

					#region Farbquantisierung
					int tol=50;

					//Meer
					if(ColCheck(col, 118, 134, 165, tol))
					{
						TemplateMap="ow-meer.tmx";
					}

					//Wüste
					if(ColCheck(col, 250, 203, 144, tol))
					{
						TemplateMap="ow-wueste.tmx";
					}

					//Eis
					if(ColCheck(col, 254, 254, 254, tol+50))
					{
						TemplateMap="ow-eis.tmx";
					}

					//Wald
					if(ColCheck(col, 33, 119, 31, tol))
					{
						TemplateMap="ow-wald.tmx";
					}

					//Erde
					if(ColCheck(col, 152, 108, 64, tol))
					{
						TemplateMap="ow-erde.tmx";
					}
					#endregion

					FileSystem.CopyFile(pathMapsTemplates+TemplateMap, pathOutput+Map.GetOuterWorldMapFilenameWithoutExtension(MapX, MapY, 0)+".tmx");

					x+=kSize;
					MapX++;
				}

				y+=kSize;
				MapY--;
			}
		}

		private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			progressBar.Maximum=FortschrittMax;
			progressBar.Value=FortschrittValue;
		}

		private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			btnStartProcess.Enabled=true;
			MessageBox.Show("Vorgang abgeschlossen!", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		static bool ColCheck(Color col, int r, int g, int b, int tol)
		{
			if(ColTol(col.R, r, tol))
			{
				if(ColTol(col.G, g, tol))
				{
					if(ColTol(col.B, b, tol))
					{
						return true;
					}
				}
			}

			return false;
		}

		static bool ColTol(int c, int value, int tol)
		{
			if(value-tol<c&&value+tol>c) return true;
			return false;
		}

		private void btnBrowseBitmap_Click(object sender, EventArgs e)
		{
			openFileDialog.Multiselect=false;
			openFileDialog.FileName="";
			openFileDialog.Filter="JPEG Dateien (*.jpg)|*.jpg|PNG Dateien (*.png)|*.png|BMP Dateien (*.bmp)|*.bmp";

			if(openFileDialog.ShowDialog()==DialogResult.OK)
			{
				tbBitmap.Text=openFileDialog.FileName;
			}
		}

		private void btnBrowseTargetPath_Click(object sender, EventArgs e)
		{
			if(folderBrowserDialog.ShowDialog()==DialogResult.OK)
			{
				tbTargetPath.Text=folderBrowserDialog.SelectedPath;
			}
		}
	}
}
