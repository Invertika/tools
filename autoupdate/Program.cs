﻿//  
//  Programm.cs
//  
//  Author:
//       seeseekey <seeseekey@googlemail.com>
// 
//  Copyright (c) 2011, 2012 by seeseekey
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Text;
using CSCL;
using CSCL.Helpers;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using CSCL.Network.FTP;
using CSCL.Network.IRC;
using System.Threading;
using CSCL.Network.FTP.Client;
using System.Net;

namespace autoupdate
{
	class Program
	{
		static IrcClient irc=new IrcClient(); //IRC Client zum Bescheid sagen
		
		static void StartIRCListen()
		{
			irc.Listen();
		}

		static string[] FunkyWords
		{
			get
			{
				List<string> ret=new List<string>();

				ret.Add("Funky.");
				ret.Add("Yeah.");
				ret.Add("Shiny.");
				ret.Add("Schon abgefahren.");
				ret.Add("Stylish.");
				ret.Add("Common.");
				ret.Add("Nothing. Absolute Nothing.");
				ret.Add("BOOM.");
				ret.Add("BAAM.");
				ret.Add("Try it again, Sam.");
				ret.Add("Commit me.");
				ret.Add("Yes Sir.");
				ret.Add("Yes Mam.");
				ret.Add("Next turn.");
				ret.Add("Great.");
				ret.Add("Splash.");
				ret.Add("Wow.");
				ret.Add("Yep.");
				ret.Add("I need five minutes.");
				ret.Add("Condition Red.");
				ret.Add("Cowabunga.");
				ret.Add("Eat my shorts arrays.");
				ret.Add("Next.");
				ret.Add("Gimme more.");
				ret.Add("Not really?.");
				ret.Add("Woooooooosh.");
				ret.Add("Oops i did it again.");
				ret.Add("Wall Wall Wall, Door.");
				ret.Add("Recticulating Splines.");
				ret.Add("Sharp C.");
				ret.Add("Cookies.");
				ret.Add("Pi has an end.");
				ret.Add("Shure.");
				ret.Add("Downdate.");
				ret.Add("Do you hear it?");
				ret.Add("Save five percent.");
				ret.Add("Freaky.");
				ret.Add("Surprise, surprise.");
				ret.Add("Dragon power.");
				ret.Add("Hadouken");
				ret.Add("Kamehameha");
				ret.Add("Alone, Alone.");
				ret.Add("Ice bear fever.");
				ret.Add("Rocket belt.");
				ret.Add("Powered by EVA.");
				ret.Add("Ready to take off.");
				ret.Add("YMMD.");
				ret.Add("Hallelujah.");
				ret.Add("I think, it's good.");
				ret.Add("Something is breaking.");
				ret.Add("Seven Second Nova.");
				ret.Add("Okay.");
				ret.Add("Corelate reality.");
				ret.Add("Verify universe.");
				ret.Add("Vector math.");
				ret.Add("Piano time.");
				ret.Add("Good to know.");
				ret.Add("Cat content.");
				ret.Add("Yes.");
				ret.Add("Not.");
				ret.Add("I'am exclusive or.");
				ret.Add("Fat bread.");
				ret.Add("Should be better written in C#.");
				ret.Add("Back to Future?");
				ret.Add("Restore");
				ret.Add("READY.");
				ret.Add("Woof.");
				ret.Add("Worf, here is Data.");
				ret.Add("Energy.");
				ret.Add("3, 2, 1 - Error.");
				ret.Add("Singularity.");
				ret.Add("It lives.");
				ret.Add("Interesting.");
				ret.Add("Nice to meet you.");
				ret.Add("Kumbaya. Not.");
				ret.Add("Good luck.");
	
				return ret.ToArray();
			}
		}

		static void Main(string[] args)
		{
			#region Init
			if(args.Length!=1)
			{
				Console.WriteLine("Argument fehlt:");
				Console.WriteLine("z.B. mono autoupdate.exe autoupdate.xml");
				return;
			}

			if(!FileSystem.ExistsFile(args[0]))
			{
				Console.WriteLine("Angegebene Datei existiert nicht.");
				return;
			}

			XmlData config;

			try
			{
				config=new XmlData(args[0]);
			}
			catch(Exception e)
			{
				Console.WriteLine("Konfiguration konnte nicht gelesen werden.");
				Console.WriteLine(e.ToString());
				return;
			}

			Console.WriteLine("Autoupdate 1.2.1 wurde gestartet...");

			string workfolder_original=Directory.GetCurrentDirectory();

			string misc_servername=config.GetElementAsString("xml.misc.servername");

			string ftp_data_server=config.GetElementAsString("xml.ftp.data.server");
			string ftp_data_user=config.GetElementAsString("xml.ftp.data.user");
			string ftp_data_password=config.GetElementAsString("xml.ftp.data.password");

			bool irc_active=false;
			string irc_network="";
			string irc_channel="";

			if(config.GetElementAsString("xml.irc.active")!="")
			{
				irc_active=Convert.ToBoolean(config.GetElementAsString("xml.irc.active"));
				irc_network=config.GetElementAsString("xml.irc.network");
				irc_channel=config.GetElementAsString("xml.irc.channel");
			}

			string ftp_update_server=config.GetElementAsString("xml.ftp.update.server");
			string ftp_update_user=config.GetElementAsString("xml.ftp.update.user");
			string ftp_update_password=config.GetElementAsString("xml.ftp.update.password");

			bool activate_data=Convert.ToBoolean(config.GetElementAsString("xml.activate.data"));
			bool activate_update=Convert.ToBoolean(config.GetElementAsString("xml.activate.update"));

			string path_temp_folder=FileSystem.GetPathWithPathDelimiter(config.GetElementAsString("xml.path.temp"));

			string path_repostiory_trunk=FileSystem.GetPathWithPathDelimiter(config.GetElementAsString("xml.path.repository.trunk"));
			string path_repostiory_server=path_repostiory_trunk+"server/";
			string path_repostiory_data=path_repostiory_trunk+"data/";
			string path_repostiory_data_scripts=path_repostiory_data+"/scripts/";
			string path_repostiory_data_maps=path_repostiory_data+"/maps/";

			string path_server_root=FileSystem.GetPathWithPathDelimiter(config.GetElementAsString("xml.path.server.root"));
			string path_server_data=path_server_root+"data/";
			string path_server_data_scripts=path_server_data+"scripts/";
			string path_server_data_maps=path_server_data+"maps/";
			string path_server_start_script=path_server_root+"start-server.sh";
			string path_server_stop_script=path_server_root+"stop-server.sh";

			List<string> ExcludesDirsClient=new List<string>();
			ExcludesDirsClient.Add("maps_templates");
			ExcludesDirsClient.Add("maps_rules");
			ExcludesDirsClient.Add("scripts");
			ExcludesDirsClient.Add(".git");

			List<string> ExcludesDirsServer=new List<string>();
			ExcludesDirsServer.Add("maps_templates");
			ExcludesDirsServer.Add("maps_rules");
			ExcludesDirsServer.Add("graphics");
			ExcludesDirsServer.Add("music");
			ExcludesDirsServer.Add("sfx");
			ExcludesDirsServer.Add(".git");

			List<string> ExcludeFiles=new List<string>();
			ExcludeFiles.Add("CMakeLists.txt");
			#endregion

			#region IRC Message absetzen
			if(irc_active)
			{
				Console.WriteLine("Sende IRC Nachricht...");

				irc.SendDelay=200;
				irc.AutoRetry=true;
				irc.ActiveChannelSyncing=true;

				string[] serverlist=new string[] { irc_network };
				int port=6667;

				irc.Connect(serverlist, port);
				irc.Login("Autoupdate", "Autoupdate", 0, "AutoupdateIRC");
				irc.RfcJoin("#invertika");

				Random rnd=new Random();
				string funkyWord=FunkyWords[rnd.Next(FunkyWords.Length)];
				irc.SendMessage(SendType.Message, irc_channel, String.Format("Autoupdate wurde auf dem Server {0} gestartet. {1}", misc_servername, funkyWord));

				new Thread(new ThreadStart(StartIRCListen)).Start();
			}
			#endregion

			#region Repository updaten
			Console.WriteLine("Update Repository...");
			Directory.SetCurrentDirectory(path_repostiory_data);
			ProcessHelpers.StartProcess("git", "pull", true);
			#endregion

			#region Server stoppen und Serverdaten löschen
			Console.WriteLine("Stoppe Server...");
			Directory.SetCurrentDirectory(path_server_root);
			ProcessHelpers.StartProcess(path_server_stop_script, "", false);

			Console.WriteLine("Lösche Serverdaten...");
			if(FileSystem.ExistsDirectory(path_server_data))
			{
				FileSystem.RemoveDirectory(path_server_data, true, true);
			}

			Console.WriteLine("Lösche temporäres Verzeichnis...");
			if(FileSystem.ExistsDirectory(path_temp_folder))
			{
				FileSystem.RemoveDirectory(path_temp_folder, true, true);
			}
			#endregion

			#region Neue Serverdaten kopieren
			Directory.SetCurrentDirectory(path_server_root);
			Console.WriteLine("Kopiere neue Serverdaten...");

			FileSystem.CreateDirectory(path_server_data_maps, true);

			FileSystem.CopyDirectory(path_repostiory_data, path_server_data, true, ExcludesDirsServer, ExcludeFiles);
			#endregion

			#region Clientdaten
			Console.WriteLine("Erzeuge Verzeichnis mit Clientdaten...");
			string clientPath=path_temp_folder+"clientdata"+FileSystem.PathDelimiter;

			FileSystem.CreateDirectory(clientPath, true);
			FileSystem.CreateDirectory(clientPath+"data"+FileSystem.PathDelimiter, true);
			FileSystem.CopyDirectory(path_repostiory_data, clientPath+"data"+FileSystem.PathDelimiter, true, ExcludesDirsClient);

			List<string> clientDataFiles=FileSystem.GetFiles(clientPath, true);
			#endregion

			#region Clientdaten Update erzeugen und hochladen
			if(activate_update)
			{
				Console.WriteLine("Erstelle Zip Datei für Update...");
				clientPath=clientPath+"data"+FileSystem.PathDelimiter;

				//Zip erstellen
				string zipFilename=path_temp_folder+"update-"+Various.GetTimeID()+".zip";
				ZipFile z=ZipFile.Create(zipFilename);

				z.BeginUpdate();

				int fivePercent=clientDataFiles.Count/20;
				int countZipFiles=0;

				foreach(string i in clientDataFiles)
				{
					countZipFiles++;

					if(FileSystem.GetExtension(i).ToLower()=="ogg")
					{
						Console.WriteLine("Datei {0} aus dem Update ausgeschlossen.", FileSystem.GetFilename(i));
						continue;
					}

					string rel=FileSystem.GetRelativePath(i, clientPath, true);
					z.Add(i, rel);

					if(countZipFiles%fivePercent==0)
					{
						Console.Write(".");
					}
				}

				z.CommitUpdate();
				z.Close();

				//adler 32
				ICSharpCode.SharpZipLib.Checksums.Adler32 adler=new ICSharpCode.SharpZipLib.Checksums.Adler32();

				FileStream fs=new FileStream(zipFilename, FileMode.Open, FileAccess.Read);
				BinaryReader br=new BinaryReader(fs);

				byte[] textToHash=br.ReadBytes((int)fs.Length);

				adler.Reset();
				adler.Update(textToHash);
				string adler32=String.Format("{0:x}", adler.Value);

				//Ressources
				string resFile=path_temp_folder+FileSystem.PathDelimiter+"resources2.txt";
				StreamWriter sw=new StreamWriter(resFile);
				sw.WriteLine("{0} {1}", FileSystem.GetFilename(zipFilename), adler32);
				sw.Close();

				//Newsfile
				string newsFile=path_temp_folder+FileSystem.PathDelimiter+"news.txt";
				sw=new StreamWriter(newsFile);
				
				sw.WriteLine("##3 Serenity");
				sw.WriteLine("##0");
				sw.WriteLine("##0 Entwicklerserver des Invertika Projektes");
				sw.WriteLine("##0 Automatisches Update wird nach jedem");
				sw.WriteLine("##0 Commit im Repository vorgenommen.");
				sw.WriteLine("##0");
				sw.WriteLine("##0 Status: in Betrieb");
				sw.WriteLine("##0 Autoupdate vom {0}, {1} Uhr.", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
				sw.WriteLine("##0");
				sw.WriteLine("##2 Das Invertika Development Team");
				sw.WriteLine("##0");
				sw.Close();

				//Upload
				Console.WriteLine("Beginne FTP Upload der Update Dateien...");
				FTPSClient Client=new FTPSClient();

				NetworkCredential networkCredential=new NetworkCredential();
				networkCredential.Domain=ftp_update_server;
				networkCredential.UserName=ftp_update_user;
				networkCredential.Password=ftp_update_password;

				Console.WriteLine("Verbinde mich mit FTP {0} mittels des Nutzers {1}.", ftp_update_server, ftp_update_user);

				Client.Connect(networkCredential.Domain, networkCredential, ESSLSupportMode.ClearText);

				List<string> currentFTPFiles=Client.GetDirectoryFiles(""); //TODO muss getestet werden
					

				Console.WriteLine("Lösche bestehende Updatedateien auf dem FTP Server...");
				foreach(string i in currentFTPFiles)
				{
					if(i.IndexOf("update")!=-1)
					{
						Client.DeleteFile(i);
					}
				}

				Console.WriteLine("Lade Updatedatei hoch...");
				Client.PutFile(zipFilename, FileSystem.GetFilename(zipFilename));
				Client.PutFile(resFile, FileSystem.GetFilename(resFile));
				Client.PutFile(newsFile, FileSystem.GetFilename(newsFile));

				Client.Close();
			}
			#endregion

			#region Server wieder starten
			Console.WriteLine("Starte Server neu...");
			Directory.SetCurrentDirectory(path_server_root);
			ProcessHelpers.StartProcess(path_server_start_script, "", false);
			#endregion

			#region Clientdaten Data erzeugen und hochladen
			if(activate_data)
			{
				//Upload
				Console.WriteLine("Beginne FTP Upload der Data Dateien...");
				FTPSClient ClientData=new FTPSClient();

				NetworkCredential networkCredential=new NetworkCredential();
				networkCredential.Domain=ftp_data_server;
				networkCredential.UserName=ftp_data_user;
				networkCredential.Password=ftp_data_password;

				Console.WriteLine("Verbinde mich mit FTP {0} mittels des Nutzers {1}.", ftp_data_server, ftp_data_user);

				ClientData.Connect(networkCredential.Domain, networkCredential, ESSLSupportMode.ClearText);

				Console.WriteLine("Lade Data Dateien hoch...");

				foreach(string ftpfile in clientDataFiles)
				{
					string relativeName=FileSystem.GetRelativePath(ftpfile, clientPath);
					string dirToCreate=FileSystem.GetPath(relativeName, true);

					if(dirToCreate!="")
					{
						string[] folders=dirToCreate.Split(FileSystem.PathDelimiter);
						string dirTemp="";

						foreach(string i in folders)
						{
							if(i.Trim()=="") continue;
							if(i=="/") continue;

							dirTemp+=i+FileSystem.PathDelimiter;

							try
							{
								ClientData.CreateDirectory(dirTemp);
							}
							catch
							{
							}
						}
					}

					Console.WriteLine("Datei {0} wird hochgeladen...", relativeName);
					ClientData.PutFile(ftpfile, relativeName);
				}

				ClientData.Close();
			}
			#endregion

			#region IRC Message absetzen und aus Channel verschwinden
			if(irc_active)
			{
				Console.WriteLine("Sende IRC Nachricht...");
				irc.SendMessage(SendType.Message, irc_channel, String.Format("Autoupdate wurde auf dem Server {0} beendet und manaserv wieder gestartet.", misc_servername));
				Thread.Sleep(15000);
				irc.Disconnect();
			}
			#endregion

			#region Ende
			Console.WriteLine("Autoupdate beenden");
			#endregion
		}
	}
}
