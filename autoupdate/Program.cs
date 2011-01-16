using System;
using System.Collections.Generic;
using System.Text;
using CSCL;
using CSCL.Helpers;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using CSCL.Network.Ftp;

namespace autoupdate
{
	class Program
	{
		static void Main(string[] args)
		{
			#region Init
			if(args.Length!=1)
			{
				Console.WriteLine("Argument fehlt:");
				Console.WriteLine("z.B. mono autoupdate.exe autoupdate.xml");
				return;
			}

			XmlData config;

			try
			{
				config=new XmlData(args[0]);
			}
			catch(Exception e)
			{
				//throw e;
				Console.WriteLine("Konfiguration konnte nicht gelesen werden.");
				Console.WriteLine(e.ToString());
				return;
			}

			string workfolder_original=Directory.GetCurrentDirectory();

			string ftp_data_server=config.GetElementAsString("xml.ftp.data.server");
			string ftp_data_user=config.GetElementAsString("xml.ftp.data.user");
			string ftp_data_password=config.GetElementAsString("xml.ftp.data.password");

			string ftp_update_server=config.GetElementAsString("xml.ftp.update.server");
			string ftp_update_user=config.GetElementAsString("xml.ftp.update.user");
			string ftp_update_password=config.GetElementAsString("xml.ftp.update.password");

			bool activate_data=Convert.ToBoolean(config.GetElementAsString("xml.activate.data"));
			bool activate_update=Convert.ToBoolean(config.GetElementAsString("xml.activate.update"));

			string path_temp_folder=FileSystem.GetPathWithPathDelimiter(config.GetElementAsString("xml.path.temp"));

			string path_repostiory_trunk=FileSystem.GetPathWithPathDelimiter(config.GetElementAsString("xml.path.repository.trunk"));
			string path_repostiory_server=path_repostiory_trunk+"server/";
			string path_repostiory_serverdata=path_repostiory_trunk+"server-data/";
			string path_repostiory_serverdata_scripts=path_repostiory_trunk+"server-data/scripts/";
			string path_repostiory_clientdata=path_repostiory_trunk+"client-data/";
			string path_repostiory_clientdata_maps=path_repostiory_trunk+"client-data/maps/";

			string path_server_root=FileSystem.GetPathWithPathDelimiter(config.GetElementAsString("xml.path.server.root"));
			string path_server_data=path_server_root+"data/";
			string path_server_data_scripts=path_server_data+"scripts/";
			string path_server_data_maps=path_server_data+"maps/";
			string path_server_start_script=path_server_root+"start-server.sh";
			string path_server_stop_script=path_server_root+"stop-server.sh";

			List<string> ExcludesDirs=new List<string>();
			ExcludesDirs.Add(".svn");
			ExcludesDirs.Add("maps_templates");

			List<string> ExcludeFiles=new List<string>();
			ExcludeFiles.Add("CMakeLists.txt");
			#endregion

			#region Repository updaten
			Console.WriteLine("Update Repository...");
			Directory.SetCurrentDirectory(path_repostiory_trunk);
			ProcessHelpers.StartProcess("svn update", "", true);
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

			FileSystem.CopyDirectory(path_repostiory_serverdata, path_server_data, true, ExcludesDirs, ExcludeFiles);

			FileSystem.CopyFiles(path_repostiory_clientdata, path_server_data, "*.xml", ExcludeFiles, true);
			FileSystem.CopyFiles(path_repostiory_clientdata_maps, path_server_data_maps, "*.tmx", ExcludeFiles, true);
			#endregion

			#region Clientdaten
			Console.WriteLine("Erzeuge Verzeichnis mit Clientdaten...");
			string clientPath=path_temp_folder+"clientdata"+FileSystem.PathDelimiter;

			FileSystem.CreateDirectory(clientPath, true);
			FileSystem.CreateDirectory(clientPath+"data"+FileSystem.PathDelimiter, true);
			FileSystem.CopyDirectory(path_repostiory_clientdata, clientPath+"data"+FileSystem.PathDelimiter, true, ExcludesDirs);

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
				sw.WriteLine("");
				sw.WriteLine("##0 Entwicklerserver des Invertika Projektes");
				sw.WriteLine("Jeden Tag wird ein neues Image aus");
				sw.WriteLine("dem SVN Repository erstellt.");
				sw.WriteLine("##0");
				sw.WriteLine("##0 Status: in Betrieb");
				sw.WriteLine("##0 Autoupdate vom: {0}", DateTime.Now.ToShortTimeString());
				sw.WriteLine("");
				sw.WriteLine("##2 Das Invertika Development Team");
				sw.WriteLine("##0");
				sw.Close();

				//Upload
				Console.WriteLine("Beginne FTP Upload der Update Dateien...");
				FTPConnection Client=new FTPConnection();

				Client.ServerAddress=ftp_update_server;
				Client.UserName=ftp_update_user;
				Client.Password=ftp_update_password;

				Console.WriteLine("Verbinde mich mit FTP {0} mittels des Nutzers {1}.", ftp_update_server, ftp_update_user);

				Client.Connect();

				string[] currentFTPFiles=Client.GetFiles("");

				Console.WriteLine("Lösche bestehende Updatedateien auf dem FTP Server...");
				foreach(string i in currentFTPFiles)
				{
					if(i.IndexOf("update")!=-1)
					{
						Client.DeleteFile(i);
					}
				}

				Console.WriteLine("Lade Updatedatei hoch...");
				Client.UploadFile(zipFilename, FileSystem.GetFilename(zipFilename));
				Client.UploadFile(resFile, FileSystem.GetFilename(resFile));
				Client.UploadFile(newsFile, FileSystem.GetFilename(newsFile));

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
				FTPConnection ClientData=new FTPConnection();

				ClientData.ServerAddress=ftp_data_server;
				ClientData.UserName=ftp_data_user;
				ClientData.Password=ftp_data_password;

				Console.WriteLine("Verbinde mich mit FTP {0} mittels des Nutzers {1}.", ftp_data_server, ftp_data_user);

				ClientData.Connect();

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
					ClientData.UploadFile(ftpfile, relativeName);
				}

				ClientData.Close();
			}
			#endregion

			#region Ende
			Console.WriteLine("Autoupdate beenden");
			#endregion
		}
	}
}
