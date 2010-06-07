using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CSCL;
using CSCL.Crypto;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace Invertika_Editor
{
	public partial class FormCreateClientUpdate:Form
	{
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
				if(i.IndexOf(FileSystem.PathDelimiter+".svn"+FileSystem.PathDelimiter)==-1) ret.Add(i);
			}

			return ret;
		}

		public FormCreateClientUpdate()
		{
			InitializeComponent();
		}

		private void btnUpdateDataLastClientBrowse_Click(object sender, EventArgs e)
		{
			folderBrowserDialog.SelectedPath=tbUpdateDataLastClient.Text;

			if(folderBrowserDialog.ShowDialog()==DialogResult.OK)
			{
				tbUpdateDataLastClient.Text=folderBrowserDialog.SelectedPath;
			}
		}

		private void btnUpdateTargetfolderBrowse_Click(object sender, EventArgs e)
		{
			folderBrowserDialog.SelectedPath=tbUpdateTargetfolder.Text;

			if(folderBrowserDialog.ShowDialog()==DialogResult.OK)
			{
				tbUpdateTargetfolder.Text=folderBrowserDialog.SelectedPath;
			}
		}

		private void btnStartUpdate_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			string FolderDev=Globals.folder_root;
			string FolderLastClient=tbUpdateDataLastClient.Text;
			string FolderTarget=tbUpdateTargetfolder.Text;

			if(FileSystem.ExistsDirectory(FolderTarget))
			{
				FileSystem.RemoveDirectory(FolderTarget, true);
			}

			FileSystem.CreateDirectory(FolderTarget, true);

			//Dev Verzeichniss
			List<string> filesDev=new List<string>();

			filesDev.AddRange(GetFilesWithoutSVN(Globals.folder_client, false, "*.xml"));
			filesDev.AddRange(GetFilesWithoutSVN(Globals.folder_clientdata, false, "*.xml"));

			filesDev.AddRange(GetFilesWithoutSVN(Globals.folder_client_data_fonts));
			filesDev.AddRange(GetFilesWithoutSVN(Globals.folder_client_data_graphics));
			filesDev.AddRange(GetFilesWithoutSVN(Globals.folder_clientdata_graphics));

			filesDev.AddRange(GetFilesWithoutSVN(Globals.folder_client_data_help));
			filesDev.AddRange(GetFilesWithoutSVN(Globals.folder_client_data_icons));
			filesDev.AddRange(GetFilesWithoutSVN(Globals.folder_clientdata_maps));
			filesDev.AddRange(GetFilesWithoutSVN(Globals.folder_clientdata_music));
			filesDev.AddRange(GetFilesWithoutSVN(Globals.folder_clientdata_sfx));

			//Last Client
			List<string> filesLastClient=GetFilesWithoutSVN(FolderLastClient);

			List<string> filesNew=new List<string>();

			foreach(string i in filesDev)
			{
				string devRelativ=FileSystem.GetRelativePath(i, FolderDev+FileSystem.PathDelimiter);
				devRelativ=devRelativ.Replace("client"+FileSystem.PathDelimiter+"data"+FileSystem.PathDelimiter, "");
				devRelativ=devRelativ.Replace("client-data"+FileSystem.PathDelimiter, "");
				string devNewClient=FolderLastClient+FileSystem.PathDelimiter+devRelativ;

				if(FileSystem.GetFilename(devRelativ).ToLower()=="cmakelists.txt") continue;

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
				devRelativ2=devRelativ2.Replace("client-data"+FileSystem.PathDelimiter, "");
				devRelativ2=devRelativ2.Replace("client"+FileSystem.PathDelimiter+"data"+FileSystem.PathDelimiter, "");
				string path2=FileSystem.GetPath(devRelativ2, true);

				FileSystem.CreateDirectory(FolderTarget+FileSystem.PathDelimiter+path2, true);

				FileSystem.CopyFile(i, FolderTarget+FileSystem.PathDelimiter+devRelativ2);
			}

			List<string> filestarget=FileSystem.GetFiles(FolderTarget, true);

			//Zip erstellen
			ZipFile z=ZipFile.Create(FolderTarget+FileSystem.PathDelimiter+"update-0.zip");

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

			FileStream fs=new FileStream(FolderTarget+FileSystem.PathDelimiter+"update-0.zip", FileMode.Open, FileAccess.Read);
			BinaryReader br=new BinaryReader(fs);

			byte[] textToHash=br.ReadBytes((int)fs.Length);

			adler.Reset();
			adler.Update(textToHash);
			string adler32=String.Format("{0:x}", adler.Value);

			StreamWriter sw=new StreamWriter(FolderTarget+FileSystem.PathDelimiter+"adler32.txt");
			sw.WriteLine(adler32);
			sw.Close();

			MessageBox.Show("Vorgang abgeschlossen!", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);

			Close();
		}
	}
}
