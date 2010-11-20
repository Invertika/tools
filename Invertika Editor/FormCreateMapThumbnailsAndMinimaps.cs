﻿using System;
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
using CSCL.Games.Manasource;
using Invertika_Editor.Classes;

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

		private void SaveMonsterSpreading(string filename, gtImage img, TMX map)
		{
			//Farben
			//Color green=Color.FromArgb(64, 0, 0, 255);
			//Color yellow=Color.FromArgb(64, 255, 255, 0);
			//Color red=Color.FromArgb(64, 255, 0, 0);
			//Color blue=Color.FromArgb(64, 0, 0, 255);

			Color green=Color.FromArgb(128, 0, 255, 0);
			Color yellow=Color.FromArgb(128, 255, 255, 0);
			Color red=Color.FromArgb(128, 255, 0, 0);
			Color blue=Color.FromArgb(128, 0, 0, 255);

			//Images
			gtImage tmpImage=img.GetImage();
			gtImage tmpDraw=new gtImage(tmpImage.Width, tmpImage.Height, tmpImage.ChannelFormat);

			//Ermittlung der Durchschnittswerte
			string fnMonsterXml=Globals.folder_clientdata+"monsters.xml";
			List<Monster> monsters=Monster.GetMonstersFromMonsterXml(fnMonsterXml);
			monsters.Sort();

			int minFightingStrength=999999;
			int maxFightingStrength=-999999;

			int medianFightingStrength=0;

			Dictionary<int, int> MonsterIDsAndFightingStrength=new Dictionary<int, int>();

			foreach(Monster monster in monsters)
			{
				if(monster.ID==1) continue; //Killermade ignorieren
				if(monster.ID>9999) continue; //Experimentelle Monster ignorieren
				int fightingStrength=monster.FightingStrength;

				if(fightingStrength<minFightingStrength) minFightingStrength=fightingStrength;
				if(fightingStrength>maxFightingStrength) maxFightingStrength=fightingStrength;

				MonsterIDsAndFightingStrength.Add(monster.ID, fightingStrength);
			}

			medianFightingStrength=(maxFightingStrength+minFightingStrength)/2;

			//Monster der Karte ermitteln
			List<MonsterSpawn> mSpawns=Globals.GetMonsterSpawnFromMap(map);

			if(mSpawns.Count>0)
			{
				int fss=0;

				foreach(MonsterSpawn spawn in mSpawns)
				{
					fss+=MonsterIDsAndFightingStrength[spawn.MonsterID];
				}

				fss=fss/mSpawns.Count;

				//Einfärben je nach Stärke
				int vSmarterGreen=(medianFightingStrength+minFightingStrength)/2;
				int vSmarterYellow=(maxFightingStrength+medianFightingStrength)/2;

				if(fss<vSmarterGreen)
				{
					tmpDraw.Fill(green);
				}
				else if(fss<vSmarterYellow)
				{
					tmpDraw.Fill(yellow);
				}
				else
				{
					tmpDraw.Fill(red);
				}
			}
			else //Keine Monster auf der Karte vorhanden
			{
				tmpDraw.Fill(blue);
			}

			//Drawen
			tmpImage.Draw(0, 0, tmpDraw, true);
			tmpImage.SaveToFile(filename);
		}

		private void bgwCreateMapThumbnailsAndMinimaps_DoWork(object sender, DoWorkEventArgs e)
		{
			List<string> files=FileSystem.GetFiles(Globals.folder_clientdata_maps, true, "*.tmx");

			FortschrittMax=files.Count;
			FortschrittValue=0;

			bgwCreateMapThumbnailsAndMinimaps.ReportProgress(0);

			string temp=FileSystem.TempPath + "Invertika Editor\\";
			string tempFmMonsterSpreading=FileSystem.TempPath+"Invertika Editor\\fm-monster-spreading\\";

			#region Bilderordner löschen und neu anlegen
			FileSystem.RemoveDirectory(temp, true);
			FileSystem.CreateDirectory(temp, true);
			FileSystem.CreateDirectory(tempFmMonsterSpreading, true);
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

				//xmlHash=""; //DEBUG

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

					//Featuremap Monster Spreading
					string fnMonsterSpreading=tempFmMonsterSpreading+fn+"-"+imageSize+".png";
					SaveMonsterSpreading(fnMonsterSpreading, pic, file);

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
			filesToUpload.AddRange(FileSystem.GetFiles(temp, true, "*.png"));

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

			//Ordner für die Feature Maps
			if(!Client.Exists("fm-monster-spreading/ow-o0000-o0000-o0000-800.png"))
			{
				Client.CreateDirectory("fm-monster-spreading");
			}

			foreach(string i in filesToUpload)
			{
				string uploadf=FileSystem.GetRelativePath(i, temp);
				uploadf=uploadf.Replace('\\', '/');
				Client.UploadFile(i, uploadf);
				FortschrittValue++;
				bgwCreateMapThumbnailsAndMinimaps.ReportProgress(0);
			}

			Client.Close();
			#endregion

			Globals.Options.Save();
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
