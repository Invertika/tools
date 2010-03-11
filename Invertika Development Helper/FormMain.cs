using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CSCL;
using System.IO;
using CSCL.FileFormats.TMX;
using CSCL.Graphic;
using System.Reflection;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using CSCL.Crypto;
using CSCL.Games.Manasource;
using CSCL.Network.Ftp;

namespace Invertika_Development_Helper
{
	public partial class FormMain:Form
	{
		public int FortschrittMax;
		public int FortschrittValue;
		bool RefreshAllowed=false;

		public FormMain()
		{
			InitializeComponent();
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			if(FileSystem.ExistsDirectory(Globals.OptionsDirectory)==false)
			{
				FileSystem.CreateDirectory(Globals.OptionsDirectory, true);
			}

			//Optionen
			bool ExitsConfig=FileSystem.ExistsFile(Globals.OptionsXmlFilename);
			Globals.Options=new XmlData(Globals.OptionsXmlFilename);

			if(ExitsConfig)
			{
				tbFTPFolder.Text=Globals.Options.GetElementAsString("xml.Options.FTPFolder");
				tbFTPPasswort.Text=Globals.Options.GetElementAsString("xml.Options.FTPPasswort");
				tbFTPServer.Text=Globals.Options.GetElementAsString("xml.Options.FTPServer");
				tbFTPUser.Text=Globals.Options.GetElementAsString("xml.Options.FTPUser");

				tbMapImagesDataFolder.Text=Globals.Options.GetElementAsString("xml.Options.MapImagesDataFolder");

				tbUpdateDataDev.Text=Globals.Options.GetElementAsString("xml.Options.UpdateDataDev");
				tbUpdateDataLastClient.Text=Globals.Options.GetElementAsString("xml.Options.UpdateDataLastClient");
				tbUpdateTargetfolder.Text=Globals.Options.GetElementAsString("xml.Options.UpdateTargetfolder");

				tbSourcePath.Text=Globals.Options.GetElementAsString("xml.Options.DataSourcePath");
				tbTargetPath.Text=Globals.Options.GetElementAsString("xml.Options.DataTargetPath");
			}

			RefreshAllowed=true;

			// Setzt die Versionsnummer anhand der Assembly Version
			Assembly MainAssembly=Assembly.GetExecutingAssembly();
			Text+=MainAssembly.GetName().Version.ToString();
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			RefreshDataInGlobalOptions();

			Globals.Options.Save();
		}

		private void RefreshDataInGlobalOptions()
		{
			if(RefreshAllowed)
			{
				Globals.Options.WriteElement("xml.Options.FTPFolder", tbFTPFolder.Text);
				Globals.Options.WriteElement("xml.Options.FTPPasswort", tbFTPPasswort.Text);
				Globals.Options.WriteElement("xml.Options.FTPServer", tbFTPServer.Text);
				Globals.Options.WriteElement("xml.Options.FTPUser", tbFTPUser.Text);

				Globals.Options.WriteElement("xml.Options.MapImagesDataFolder", tbMapImagesDataFolder.Text);

				Globals.Options.WriteElement("xml.Options.UpdateDataDev", tbUpdateDataDev.Text);
				Globals.Options.WriteElement("xml.Options.UpdateDataLastClient", tbUpdateDataLastClient.Text);
				Globals.Options.WriteElement("xml.Options.UpdateTargetfolder", tbUpdateTargetfolder.Text);

				Globals.Options.WriteElement("xml.Options.DataSourcePath", tbSourcePath.Text);
				Globals.Options.WriteElement("xml.Options.DataTargetPath", tbTargetPath.Text);
			}
		}

		private void bgwCreateMapImages_DoWork(object sender, DoWorkEventArgs e)
		{
			string pathMapImages=tbMapImagesDataFolder.Text;
			List<string> files=FileSystem.GetFiles(pathMapImages+"\\maps\\", true, "ow-*.tmx");

			FortschrittMax=files.Count;
			FortschrittValue=0;

			bgwCreateMapImages.ReportProgress(0);

			string temp=FileSystem.TempPath;

			#region Bilderordner löschen
			List<string> filesToClear=new List<string>();
			filesToClear.AddRange(FileSystem.GetFiles(temp, false, "*-50*"));
			filesToClear.AddRange(FileSystem.GetFiles(temp, false, "*-100*"));
			filesToClear.AddRange(FileSystem.GetFiles(temp, false, "*-800*"));
			filesToClear.AddRange(FileSystem.GetFiles(temp, false, "*-1400*"));

			FileSystem.RemoveFiles(filesToClear);
			#endregion

			#region Bilder berechnen
			foreach(string i in files)
			{
				FortschrittValue++;
				bgwCreateMapImages.ReportProgress(0);

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

				gtImage pic1400=pic.Resize(1400, 1400);
				gtImage pic800=pic.Resize(800, 800);
				gtImage pic100=pic.Resize(100, 100);
				gtImage pic50=pic.Resize(50, 50);

				string fn=FileSystem.GetFilenameWithoutExt(i);

				string fn1400=temp+fn+"-1400.png";
				string fn800=temp+fn+"-800.png";
				string fn100=temp+fn+"-100.png";
				string fn50=temp+fn+"-50.png";
				string fnMinimap=pathMapImages+"\\graphics\\minimaps\\"+fn+".png";

				pic1400.SaveToFile(fn1400);
				pic800.SaveToFile(fn800);
				pic100.SaveToFile(fn100);
				pic100.SaveToFile(fnMinimap);
				pic50.SaveToFile(fn50);
			}
			#endregion

			#region Bilder per FTP hochladen
			List<string> filesToUpload=new List<string>();
			filesToUpload.AddRange(FileSystem.GetFiles(temp, false, "*-50*"));
			filesToUpload.AddRange(FileSystem.GetFiles(temp, false, "*-100*"));
			filesToUpload.AddRange(FileSystem.GetFiles(temp, false, "*-800*"));
			filesToUpload.AddRange(FileSystem.GetFiles(temp, false, "*-1400*"));

			FTPConnection Client=new FTPConnection();

			Client.ServerAddress=Globals.Options.GetElementAsString("xml.Options.FTPServer");
			Client.UserName=Globals.Options.GetElementAsString("xml.Options.FTPUser");
			Client.Password=Globals.Options.GetElementAsString("xml.Options.FTPPasswort");

			try
			{
				Client.Connect();
			}
			catch(Exception exception)
			{
				MessageBox.Show(exception.Message);
				return;
			}

			FortschrittMax=filesToUpload.Count;
			FortschrittValue=0;

			foreach(string i in filesToUpload)
			{
				Client.UploadFile(i, FileSystem.GetFilename(i));
				FortschrittValue++;
				bgwCreateMapImages.ReportProgress(0);
			}

			Client.Close();
			#endregion

			bgwCreateMapImages.ReportProgress(100);
		}

		private void bgwCreateMapImages_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if(e.ProgressPercentage==100)
			{
				pbCreateMapImages.Value=pbCreateMapImages.Maximum;
				btnStartCreateMapImages.Enabled=true;

				MessageBox.Show("Die Karten Thumbnails wurden erzeugt!");

				pbCreateMapImages.Value=0;
			}

			pbCreateMapImages.Maximum=FortschrittMax;
			pbCreateMapImages.Value=FortschrittValue;
		}

		private void btnBrowseMapImagesDataFolder_Click(object sender, EventArgs e)
		{
			fbdMain.SelectedPath=tbMapImagesDataFolder.Text;

			if(fbdMain.ShowDialog()==DialogResult.OK)
			{
				tbMapImagesDataFolder.Text=fbdMain.SelectedPath;
			}
		}

		private void tbFTPServer_TextChanged(object sender, EventArgs e)
		{
			RefreshDataInGlobalOptions();
		}

		private void btnStartCreateMapImages_Click(object sender, EventArgs e)
		{
			btnStartCreateMapImages.Enabled=false;
			bgwCreateMapImages.RunWorkerAsync();
		}

		private void btnUpdateDataDevBrowse_Click(object sender, EventArgs e)
		{
			fbdMain.SelectedPath=tbUpdateDataDev.Text;

			if(fbdMain.ShowDialog()==DialogResult.OK)
			{
				tbUpdateDataDev.Text=fbdMain.SelectedPath;
			}
		}

		private void btnUpdateDataLastClientBrowse_Click(object sender, EventArgs e)
		{
			fbdMain.SelectedPath=tbUpdateDataLastClient.Text;

			if(fbdMain.ShowDialog()==DialogResult.OK)
			{
				tbUpdateDataLastClient.Text=fbdMain.SelectedPath;
			}
		}

		private void btnUpdateTargetfolderBrowse_Click(object sender, EventArgs e)
		{
			fbdMain.SelectedPath=tbUpdateTargetfolder.Text;

			if(fbdMain.ShowDialog()==DialogResult.OK)
			{
				tbUpdateTargetfolder.Text=fbdMain.SelectedPath;
			}
		}

		List<string> GetFilesWithoutSVN(string Path)
		{
			return GetFilesWithoutSVN(Path, true, "*.*");
		}

		List<string> GetFilesWithoutSVN(string Path, bool rekursiv, string filter)
		{
			List<string> tmpFiles=FileSystem.GetFiles(Path, rekursiv, filter);
			List<string> files=new List<string>();

			foreach(string file in tmpFiles)
			{
				if(file.ToLower().IndexOf("makefile.am")==-1)
				{
					files.Add(file);
				}
			}

			List<string> ret=new List<string>();

			foreach(string i in files)
			{
				if(i.IndexOf("\\.svn\\")==-1) ret.Add(i);
			}

			return ret;
		}

		private void btnStartUpdate_Click(object sender, EventArgs e)
		{
			tsbStatusLabel.Text="Vorgang wurde begonnen!";

			string FolderDev=tbUpdateDataDev.Text;
			string FolderLastClient=tbUpdateDataLastClient.Text;
			string FolderTarget=tbUpdateTargetfolder.Text;

			if(FileSystem.ExistsDirectory(FolderTarget))
			{
				FileSystem.RemoveDirectory(FolderTarget, true);
			}

			FileSystem.CreateDirectory(FolderTarget, true);

			//Dev Verzeichniss
			List<string> filesDev=new List<string>();

			filesDev.AddRange(GetFilesWithoutSVN(FolderDev+"\\client\\", false, "*.xml"));
			filesDev.AddRange(GetFilesWithoutSVN(FolderDev+"\\client-data\\", false, "*.xml"));

			filesDev.AddRange(GetFilesWithoutSVN(FolderDev+"\\client\\data\\fonts\\"));
			filesDev.AddRange(GetFilesWithoutSVN(FolderDev+"\\client\\data\\graphics\\"));
			filesDev.AddRange(GetFilesWithoutSVN(FolderDev+"\\client-data\\graphics\\"));

			filesDev.AddRange(GetFilesWithoutSVN(FolderDev+"\\client\\data\\help\\"));
			filesDev.AddRange(GetFilesWithoutSVN(FolderDev+"\\client\\data\\icons\\"));
			filesDev.AddRange(GetFilesWithoutSVN(FolderDev+"\\client-data\\maps\\"));
			filesDev.AddRange(GetFilesWithoutSVN(FolderDev+"\\client-data\\music\\"));
			filesDev.AddRange(GetFilesWithoutSVN(FolderDev+"\\client-data\\sfx\\"));

			//Last Client
			List<string> filesLastClient=GetFilesWithoutSVN(FolderLastClient);

			List<string> filesNew=new List<string>();

			foreach(string i in filesDev)
			{
				string devRelativ=FileSystem.GetRelativePath(i, FolderDev+'\\');
				devRelativ=devRelativ.Replace("client\\data\\", "");
				devRelativ=devRelativ.Replace("client-data\\", "");
				string devNewClient=FolderLastClient+'\\'+devRelativ;

				if(FileSystem.ExistsFile(devNewClient))
				{
					//Weitere Vergleiche
					long SizeDev=FileSystem.GetFilesize(i);
					long SizeLastClient=FileSystem.GetFilesize(devNewClient);

					if(SizeDev==SizeLastClient)
					{
						string hashDev=Hash.SHA1.HashFileToSHA1(i);
						string hashLastClient=Hash.SHA1.HashFileToSHA1(devNewClient);

						if(hashDev==hashLastClient) continue;
					}
				}

				filesNew.Add(i);
			}

			//Ziel
			//SCL.Helpers.ListHelpers.WriteListIntofile(filesNew, FolderTarget + "\\changedfiles.txt");

			foreach(string i in filesNew)
			{
				string devRelativ2=FileSystem.GetRelativePath(i, FolderDev+'\\');
				devRelativ2=devRelativ2.Replace("client-data\\", "");
				devRelativ2=devRelativ2.Replace("client\\data\\", "");
				string path2=FileSystem.GetPath(devRelativ2, true);

				FileSystem.CreateDirectory(FolderTarget+'\\'+path2, true);

				FileSystem.CopyFile(i, FolderTarget+'\\'+devRelativ2);
			}

			List<string> filestarget=FileSystem.GetFiles(FolderTarget, true);

			//Zip erstellen
			ZipFile z=ZipFile.Create(FolderTarget+"\\update-0.zip");

			z.BeginUpdate();

			foreach(string i in filestarget)
			{
				string rel=FileSystem.GetRelativePath(i, FolderTarget, true);
				z.Add(i, rel);
			}

			z.CommitUpdate();
			z.Close();

			//adler 32
			ICSharpCode.SharpZipLib.Checksums.Adler32 adler=new ICSharpCode.SharpZipLib.Checksums.Adler32();

			FileStream fs=new FileStream(FolderTarget+"\\update-0.zip", FileMode.Open, FileAccess.Read);
			BinaryReader br=new BinaryReader(fs);

			byte[] textToHash=br.ReadBytes((int)fs.Length);

			adler.Reset();
			adler.Update(textToHash);
			string adler32=String.Format("{0:x}", adler.Value);

			StreamWriter sw=new StreamWriter(FolderTarget+"\\adler32.txt");
			sw.WriteLine(adler32);
			sw.Close();

			tsbStatusLabel.Text="Vorgang abgeschlossen!";
		}

		private void btnSourceBrowse_Click(object sender, EventArgs e)
		{
			fbdMain.SelectedPath=tbSourcePath.Text;

			if(fbdMain.ShowDialog()==DialogResult.OK)
			{
				tbSourcePath.Text=fbdMain.SelectedPath;
			}
		}

		private void btnTargetBrowse_Click(object sender, EventArgs e)
		{
			fbdMain.SelectedPath=tbTargetPath.Text;

			if(fbdMain.ShowDialog()==DialogResult.OK)
			{
				tbTargetPath.Text=fbdMain.SelectedPath;
			}
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			if(tbSourcePath.Text=="")
			{
				MessageBox.Show("Kein Quellverzeichnis angegeben!");
				return;
			}

			if(tbTargetPath.Text=="")
			{
				MessageBox.Show("Kein Zielverzeichnis angegeben!");
				return;
			}

			bgwCreateDataFolders.RunWorkerAsync();
		}

		private void bgwCreateDataFolders_DoWork(object sender, DoWorkEventArgs e)
		{
			string source=tbSourcePath.Text;
			source=source.TrimEnd('\\')+'\\';
			string target=tbTargetPath.Text;
			target=target.TrimEnd('\\')+'\\';

			List<string> ExcludesDirs=new List<string>();
			ExcludesDirs.Add(".svn");
			ExcludesDirs.Add("maps_templates");

			if(FileSystem.ExistsDirectory(target))
			{
				FileSystem.RemoveDirectory(target, true);
			}

			FileSystem.CreateDirectory(target, true);

			#region tmwserv
			string serverPath=target+"tmwserv\\";

			FileSystem.CreateDirectory(serverPath);
			FileSystem.CreateDirectory(serverPath+"data\\maps\\", true);
			FileSystem.CreateDirectory(serverPath+"data\\scripts\\", true);
			FileSystem.CreateDirectory(serverPath+"data\\scripts\\libs\\", true);

			//Kopieren
			FileSystem.CopyFiles(source, serverPath, "*.sh");
			FileSystem.CopyFiles(source, serverPath, "*.xml");
			FileSystem.CopyFiles(source+"client-data\\", serverPath+"data\\", "*.xml");
			FileSystem.CopyFiles(source+"client-data\\", serverPath+"data\\", "*.xsd");
			FileSystem.CopyFiles(source+"client-data\\", serverPath+"data\\", "*.xsl");
			FileSystem.CopyFiles(source+"client-data\\maps\\", serverPath+"data\\maps\\", "*.tmx");

			FileSystem.CopyDirectory(source+"server-data\\scripts\\", serverPath+"data\\scripts\\", true, ExcludesDirs);
			FileSystem.CopyFiles(source+"server-data\\", serverPath+"data\\", "*.xml");
			#endregion

			#region tmwclient-full
			string clientPath=target+"tmwclient-full\\";

			FileSystem.CreateDirectory(clientPath);
			FileSystem.CreateDirectory(clientPath+"data\\fonts\\", true);
			FileSystem.CreateDirectory(clientPath+"data\\graphics\\gui\\", true);
			FileSystem.CreateDirectory(clientPath+"data\\graphics\\images\\", true);
			FileSystem.CreateDirectory(clientPath+"data\\graphics\\items\\", true);
			FileSystem.CreateDirectory(clientPath+"data\\graphics\\sprites\\", true);
			FileSystem.CreateDirectory(clientPath+"data\\graphics\\tiles\\", true);
			FileSystem.CreateDirectory(clientPath+"data\\help", true);
			FileSystem.CreateDirectory(clientPath+"data\\icons", true);
			FileSystem.CreateDirectory(clientPath+"data\\maps", true);
			FileSystem.CreateDirectory(clientPath+"data\\music", true);
			FileSystem.CreateDirectory(clientPath+"data\\sfx", true);

			//Kopieren
			FileSystem.CopyFile(source+"AUTHORS-MANA", clientPath+"AUTHORS-MANA");
			FileSystem.CopyFile(source+"AUTHORS-INVERTIKA", clientPath+"AUTHORS-INVERTIKA");
			FileSystem.CopyFile(source+"COPYING", clientPath+"COPYING");
			FileSystem.CopyFile(source+"client\\"+"Invertika.url", clientPath+"Invertika.url");

			FileSystem.CopyFiles(source+"client\\data\\", clientPath+"data\\", "*.*");
			FileSystem.CopyFiles(source+"client-data\\", clientPath+"data\\", "*.xml");

			FileSystem.CopyFiles(source+"client\\data\\fonts\\", clientPath+"data\\fonts\\", "*.ttf");
			FileSystem.CopyDirectory(source+"client\\data\\graphics\\", clientPath+"data\\graphics\\", true, ExcludesDirs);
			FileSystem.CopyDirectory(source+"client-data\\graphics\\", clientPath+"data\\graphics\\", true, ExcludesDirs);

			FileSystem.CopyFiles(source+"client\\data\\help\\", clientPath+"data\\help\\", "*.*");
			FileSystem.CopyFiles(source+"client\\data\\icons\\", clientPath+"data\\icons\\", "*.*");
			FileSystem.CopyFiles(source+"client-data\\maps\\", clientPath+"data\\maps\\", "*.tmx");
			FileSystem.CopyDirectory(source+"client-data\\music\\", clientPath+"data\\music\\", true, ExcludesDirs);
			FileSystem.CopyDirectory(source+"client-data\\sfx\\", clientPath+"data\\sfx\\", true, ExcludesDirs);
			#endregion

			#region tmwclient-minimal
			clientPath=target+"tmwclient-minimal\\";

			FileSystem.CreateDirectory(clientPath+"data\\music", true);

			FileSystem.CreateDirectory(clientPath);
			FileSystem.CopyDirectory(source+"client\\data\\", clientPath+"data\\", true, ExcludesDirs);
			FileSystem.CopyFile(source+"AUTHORS-MANA", clientPath+"AUTHORS-MANA");
			FileSystem.CopyFile(source+"AUTHORS-INVERTIKA", clientPath+"AUTHORS-INVERTIKA");
			FileSystem.CopyFile(source+"COPYING", clientPath+"COPYING");
			FileSystem.CopyFile(source+"client\\"+"Invertika.url", clientPath+"Invertika.url");
			FileSystem.CopyFile(source+"client-data\\music\\godness.ogg", clientPath+"data\\music\\godness.ogg");
			#endregion

			#region tmwdata
			clientPath=target+"tmwdata\\";

			FileSystem.CreateDirectory(clientPath);
			FileSystem.CreateDirectory(clientPath+"data\\", true);
			FileSystem.CopyDirectory(source+"client-data\\", clientPath+"data\\", true, ExcludesDirs);
			#endregion

			#region tmwdata zero update
			//Dev Verzeichniss
			clientPath=clientPath+"data\\";
			List<string> filesNew=FileSystem.GetFiles(clientPath, true);

			//Zip erstellen
			ZipFile z=ZipFile.Create(target+"\\update-zero.zip");

			z.BeginUpdate();

			foreach(string i in filesNew)
			{
				string rel=FileSystem.GetRelativePath(i, clientPath, true);
				z.Add(i, rel);
			}

			z.CommitUpdate();
			z.Close();

			//adler 32
			ICSharpCode.SharpZipLib.Checksums.Adler32 adler=new ICSharpCode.SharpZipLib.Checksums.Adler32();

			FileStream fs=new FileStream(target+"\\update-zero.zip", FileMode.Open, FileAccess.Read);
			BinaryReader br=new BinaryReader(fs);

			byte[] textToHash=br.ReadBytes((int)fs.Length);

			adler.Reset();
			adler.Update(textToHash);
			string adler32=String.Format("{0:x}", adler.Value);

			StreamWriter sw=new StreamWriter(target+"\\adler32.txt");
			sw.WriteLine(adler32);
			sw.Close();
			#endregion
		}

		private void bgwCreateDataFolders_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			MessageBox.Show("Data Verzeichnisse erstellt!");
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

		private void nudYPixel_ValueChanged(object sender, EventArgs e)
		{
			nudYTile.Value=Helper.GetTileCoord((int)nudYPixel.Value);
		}

		private void btnCopy_Click(object sender, EventArgs e)
		{
			if(rtbComments.SelectionStart==0) rtbComments.Text+=tbXYPixel.Text;
			else rtbComments.Text+="\n"+tbXYPixel.Text;
		}

		private void tMXÖffnenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			openFileDialog.Multiselect=false;
			openFileDialog.FileName="";
			openFileDialog.Filter="TMX Dateien (*.tmx)|*.tmx";

			if(openFileDialog.ShowDialog()==DialogResult.OK)
			{
				try
				{
					TMX TestTMX=new TMX();
					TestTMX.Open(openFileDialog.FileName);
					TestTMX.Render();
					MessageBox.Show("Datei konnte ohne Probleme geparst werden.");
				}
				catch(Exception exception)
				{
					MessageBox.Show("Es gab Probleme beim Parsen der Datei.\n"+exception.ToString());
				}
			}
		}

		private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void RefreshScriptOutput()
		{
			string output=String.Format(" create_npc(\"{0}\", {1}, {2} * TILESIZE + 16, {3} * TILESIZE + 16, {4}_talk, nil) --- {0}", tbNPCName.Text, nudNPCID.Value, nudPosX.Value, nudPosY.Value, tbNPCName.Text.ToLower());
			output+="\n\n";

			output+=String.Format("function {0}_talk(npc, ch)", tbNPCName.Text.ToLower());
			output+="\n";
			output+="	do_message(npc, ch, invertika.get_random_element(";

			foreach(string line in rtbSentences.Lines)
			{
				if(output[output.Length-1]=='(')
				{
					output+=String.Format("\"{0}\",\n", line);
				}
				else
				{
					output+=String.Format("	  \"{0}\",\n", line);
				}
			}

			output=output.TrimEnd('\n');
			output=output.TrimEnd(',');

			output+="))";

			output+="\n";
			output+="	do_npc_close(npc, ch)";
			output+="\n";
			output+="end";

			rtbScriptOutput.Text=output;
		}

		private void tbNPCName_TextChanged(object sender, EventArgs e)
		{
			RefreshScriptOutput();
		}

		private void nudNPCID_ValueChanged(object sender, EventArgs e)
		{
			RefreshScriptOutput();
		}

		private void nudPosX_ValueChanged(object sender, EventArgs e)
		{
			RefreshScriptOutput();
		}

		private void nudPosY_ValueChanged(object sender, EventArgs e)
		{
			RefreshScriptOutput();
		}

		private void rtbSentences_TextChanged(object sender, EventArgs e)
		{
			RefreshScriptOutput();
		}

		private void label23_Click(object sender, EventArgs e)
		{
			// Webbrowser mit Doamin aufrufen
			System.Diagnostics.Process p=new System.Diagnostics.Process();
			p.StartInfo.RedirectStandardOutput=false;
			p.StartInfo.FileName="http://wiki.invertika.org/NPC_Entwicklung";
			p.StartInfo.UseShellExecute=true;
			p.Start();
		}
	}
}
