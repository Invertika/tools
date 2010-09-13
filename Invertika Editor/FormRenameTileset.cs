using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CSCL.FileFormats.TMX;
using CSCL;

namespace Invertika_Editor
{
	public partial class FormRenameTileset : Form
	{
		int FortschrittMax;
		int FortschrittValue;

		string oldTileset;
		string newTileset;

		public FormRenameTileset()
		{
			InitializeComponent();
		}

		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			//Maps laden
			List<string> mapfiles=FileSystem.GetFiles(Globals.folder_clientdata, true, "*.tmx");
			FortschrittMax=mapfiles.Count;

			foreach(string i in mapfiles)
			{
				bool changed=false;

				TMX maptmx=new TMX();
				maptmx.Open(i);

				//Tiles transformieren
				foreach(TMX.TilesetData ld in maptmx.Tilesets)
				{
					string newSource=ld.imgsource.Replace(oldTileset, newTileset);

					if(ld.imgsource!=newSource)
					{
						ld.imgsource=newSource;
						changed=true;
					}
				}

				if(changed)
				{
					//Map speichern
					maptmx.Save(i);
				}

				FortschrittValue++;
				backgroundWorker.ReportProgress(0);
			}

			//Tileset umbennen
			string oldFn= Globals.folder_clientdata_graphics_tiles + oldTileset;
			string newFn= Globals.folder_clientdata_graphics_tiles + newTileset;

			FileSystem.RenameFile(oldFn, newFn);
		}

		private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			pbMain.Maximum=FortschrittMax;
			pbMain.Value=FortschrittValue;
		}

		private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			pbMain.Value=pbMain.Maximum;

			MessageBox.Show("Vorgang abgeschlossen!", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);

			Close();
		}

		private void btnCalc_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			btnCalc.Enabled=false;
			tbNewName.Enabled=false;
			lbCurrentTilesets.Enabled=false;

			oldTileset=lbCurrentTilesets.SelectedItem.ToString();
			newTileset=tbNewName.Text;

			backgroundWorker.RunWorkerAsync();
		}

		private void FormRenameTileset_Load(object sender, EventArgs e)
		{
			List<string> tiles=FileSystem.GetFiles(Globals.folder_clientdata_graphics_tiles, true, "*.png");

			foreach(string i in tiles)
			{
				lbCurrentTilesets.Items.Add(FileSystem.GetFilename(i));
			}
		}
	}
}
