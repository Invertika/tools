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
using Invertika;

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

		bool SendProgress(double percent)
		{
			if(backgroundWorker==null) return true;
			backgroundWorker.ReportProgress((int)percent);
			return !backgroundWorker.CancellationPending;
		}

		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			Imaging.CreateMapsFromBitmap(tbBitmap.Text, Globals.folder_clientdata_mapstemplates, tbTargetPath.Text, (int)nudXmin.Value, (int)nudYmax.Value, SendProgress);
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
