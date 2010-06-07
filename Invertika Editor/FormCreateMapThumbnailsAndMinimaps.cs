using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CSCL;
using System.IO;
using CSCL.Crypto;
using CSCL.FileFormats.TMX;
using CSCL.Graphic;
using CSCL.Network.Ftp;

namespace Invertika_Editor
{
	public partial class FormCreateMapThumbnailsAndMinimaps:Form
	{
		int FortschrittMax;
		int FortschrittValue;

		public FormCreateMapThumbnailsAndMinimaps()
		{
			InitializeComponent();
		}

		private int GetNextImageSize(int currentSize)
		{
			if(currentSize>50)
			{
				return currentSize/2;
			}
			else
			{
				return currentSize-10;
			}
		}

		private void bgwCreateMapThumbnailsAndMinimaps_DoWork(object sender, DoWorkEventArgs e)
		{
			List<string> files=FileSystem.GetFiles(Globals.folder_clientdata_maps, true, "ow-*.tmx");

			FortschrittMax=files.Count;
			FortschrittValue=0;

			bgwCreateMapThumbnailsAndMinimaps.ReportProgress(0);

			string temp=FileSystem.TempPath;

			#region Bilderordner löschen
			List<string> filesToClear=new List<string>();
			filesToClear.AddRange(FileSystem.GetFiles(temp, false, "*-800*"));
			filesToClear.AddRange(FileSystem.GetFiles(temp, false, "*-400*"));
			filesToClear.AddRange(FileSystem.GetFiles(temp, false, "*-200*"));
			filesToClear.AddRange(FileSystem.GetFiles(temp, false, "*-100*"));
			filesToClear.AddRange(FileSystem.GetFiles(temp, false, "*-50*"));
			filesToClear.AddRange(FileSystem.GetFiles(temp, false, "*-40*"));
			filesToClear.AddRange(FileSystem.GetFiles(temp, false, "*-30*"));
			filesToClear.AddRange(FileSystem.GetFiles(temp, false, "*-20*"));
			filesToClear.AddRange(FileSystem.GetFiles(temp, false, "*-10*"));

			FileSystem.RemoveFiles(filesToClear);
			#endregion

			#region Bilder berechnen
			foreach(string i in files)
			{
				FortschrittValue++;
				bgwCreateMapThumbnailsAndMinimaps.ReportProgress(0);

				GC.Collect(2);

				//Hashvergleich
				string text=File.ReadAllText(i);
				string textHash=Hash.SHA1.HashStringToSHA1(text);
				string xmlPath="xml.CalcMapImages."+FileSystem.GetFilenameWithoutExt(i);

				string xmlHash;
				try
				{
					xmlHash=Globals.Options.GetElementAsString(xmlPath);
				}
				catch
				{
					xmlHash="";
				}

				if(xmlHash=="")
				{
					Globals.Options.WriteElement("xml.CalcMapImages."+FileSystem.GetFilenameWithoutExt(i), textHash);
				}
				else
				{
					Globals.Options.WriteElement(xmlPath, textHash);
				}

				if(cbClearCache.Checked==false)
				{
					if(textHash==xmlHash) continue;
				}

				//Karte berechnen
				TMX file=new TMX();
				file.Open(i);

				gtImage pic=file.Render();

				int imageSizeOriginal=(int)pic.Width;
				int imageSize=800;
				pic=pic.Resize(imageSize, imageSize);

				bool next=true;

				while(next)
				{
					string fn=FileSystem.GetFilenameWithoutExt(i);
					string fnNumeric=temp+fn+"-"+imageSize+".png";
					pic.SaveToFile(fnNumeric);

					switch(imageSize)
					{
						case 100:
							{
								//Minimap zusätzlich speichern
								string fnMinimap=Globals.folder_clientdata_graphics_minimaps+fn+".png";
								pic.SaveToFile(fnMinimap);
								break;
							}
						case 10:
							{
								next=false;
								break;
							}
					}

					imageSize=GetNextImageSize(imageSize);

					pic=pic.Resize(imageSize, imageSize);
					GC.Collect(3);
				}
			}
			#endregion

			#region Bilder per FTP hochladen
			List<string> filesToUpload=new List<string>();
			filesToUpload.AddRange(FileSystem.GetFiles(temp, false, "*-800*"));
			filesToUpload.AddRange(FileSystem.GetFiles(temp, false, "*-400*"));
			filesToUpload.AddRange(FileSystem.GetFiles(temp, false, "*-200*"));
			filesToUpload.AddRange(FileSystem.GetFiles(temp, false, "*-100*"));
			filesToUpload.AddRange(FileSystem.GetFiles(temp, false, "*-50*"));
			filesToUpload.AddRange(FileSystem.GetFiles(temp, false, "*-40*"));
			filesToUpload.AddRange(FileSystem.GetFiles(temp, false, "*-30*"));
			filesToUpload.AddRange(FileSystem.GetFiles(temp, false, "*-20*"));
			filesToUpload.AddRange(FileSystem.GetFiles(temp, false, "*-10*"));

			FTPConnection Client=new FTPConnection();

			Client.ServerAddress=Globals.Options.GetElementAsString("xml.Options.FTP.Worldmap.Server");
			Client.UserName=Globals.Options.GetElementAsString("xml.Options.FTP.Worldmap.User");
			Client.Password=Globals.Options.GetElementAsString("xml.Options.FTP.Worldmap.Password");

			try
			{
				Client.Connect();
			}
			catch(Exception exception)
			{
				MessageBox.Show(exception.Message, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			FortschrittMax=filesToUpload.Count;
			FortschrittValue=0;

			foreach(string i in filesToUpload)
			{
				Client.UploadFile(i, FileSystem.GetFilename(i));
				FortschrittValue++;
				bgwCreateMapThumbnailsAndMinimaps.ReportProgress(0);
			}

			Client.Close();
			#endregion
		}

		private void bgwCreateMapThumbnailsAndMinimaps_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			pbCreateMapImages.Maximum=FortschrittMax;
			pbCreateMapImages.Value=FortschrittValue;
		}

		private void btnStartCreateMapThumbnailsAndMinimaps_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.FTP.Worldmap.Server")=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den FTP Server an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.FTP.Worldmap.User")=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den FTP Benutzer an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.FTP.Worldmap.Password")=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen das FTP Passwort an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			btnStartCreateMapThumbnailsAndMinimaps.Enabled=false;
			bgwCreateMapThumbnailsAndMinimaps.RunWorkerAsync();
		}

		private void bgwCreateMapThumbnailsAndMinimaps_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			pbCreateMapImages.Value=pbCreateMapImages.Maximum;
			btnStartCreateMapThumbnailsAndMinimaps.Enabled=true;

			MessageBox.Show("Vorgang abgeschlossen!", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);

			pbCreateMapImages.Value=0;

			Close();
		}
	}
}
