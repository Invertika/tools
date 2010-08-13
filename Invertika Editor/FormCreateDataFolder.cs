using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CSCL;
using ICSharpCode.SharpZipLib.Zip;

namespace Invertika_Editor
{
	public partial class FormCreateDataFolder:Form
	{
		public FormCreateDataFolder()
		{
			InitializeComponent();
		}

		private void btnTargetBrowse_Click(object sender, EventArgs e)
		{
			folderBrowserDialog.SelectedPath=tbTargetPath.Text;

			if(folderBrowserDialog.ShowDialog()==DialogResult.OK)
			{
				tbTargetPath.Text=folderBrowserDialog.SelectedPath;
				Globals.Options.WriteElement("xml.Options.Paths.CreateDataFolder.TargetFolder", folderBrowserDialog.SelectedPath);
			}
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

			string source=Globals.folder_root;

			string target=FileSystem.GetPathWithPathDelimiter(tbTargetPath.Text);

			List<string> ExcludesDirs=new List<string>();
			ExcludesDirs.Add(".svn");
			ExcludesDirs.Add("maps_templates");

			List<string> ExcludeFiles=new List<string>();
			ExcludeFiles.Add("CMakeLists.txt");

			if(FileSystem.ExistsDirectory(target))
			{
				FileSystem.RemoveDirectory(target, true);
			}

			FileSystem.CreateDirectory(target, true);

			#region tmwserv
			string serverPath=target+"tmwserv"+FileSystem.PathDelimiter;

			FileSystem.CreateDirectory(serverPath);
			FileSystem.CreateDirectory(serverPath+"data"+FileSystem.PathDelimiter+"maps"+FileSystem.PathDelimiter, true);
			FileSystem.CreateDirectory(serverPath+"data"+FileSystem.PathDelimiter+"scripts"+FileSystem.PathDelimiter, true);
			FileSystem.CreateDirectory(serverPath+"data"+FileSystem.PathDelimiter+"scripts"+FileSystem.PathDelimiter+"libs"+FileSystem.PathDelimiter, true);

			//Kopieren
			FileSystem.CopyFiles(source, serverPath, "*.sh");
			FileSystem.CopyFiles(source, serverPath, "*.xml");
			FileSystem.CopyFiles(Globals.folder_clientdata , serverPath+"data"+FileSystem.PathDelimiter, "*.xml");
			FileSystem.CopyFiles(Globals.folder_clientdata, serverPath+"data"+FileSystem.PathDelimiter, "*.xsd");
			FileSystem.CopyFiles(Globals.folder_clientdata, serverPath+"data"+FileSystem.PathDelimiter, "*.xsl");
			FileSystem.CopyFiles(Globals.folder_clientdata_maps, serverPath+"data"+FileSystem.PathDelimiter+"maps"+FileSystem.PathDelimiter, "*.tmx");

			FileSystem.CopyDirectory(Globals.folder_serverdata_scripts, serverPath+"data"+FileSystem.PathDelimiter+"scripts"+FileSystem.PathDelimiter, true, ExcludesDirs);
			FileSystem.CopyFiles(Globals.folder_serverdata, serverPath+"data"+FileSystem.PathDelimiter, "*.xml");
			#endregion

			#region tmwclient-full
			string clientPath=target+"tmwclient-full"+FileSystem.PathDelimiter;

			FileSystem.CreateDirectory(clientPath);
			FileSystem.CreateDirectory(clientPath+"data"+FileSystem.PathDelimiter+"fonts"+FileSystem.PathDelimiter, true);
			FileSystem.CreateDirectory(clientPath+"data"+FileSystem.PathDelimiter+"graphics"+FileSystem.PathDelimiter+"gui"+FileSystem.PathDelimiter, true);
			FileSystem.CreateDirectory(clientPath+"data"+FileSystem.PathDelimiter+"graphics"+FileSystem.PathDelimiter+"images"+FileSystem.PathDelimiter, true);
			FileSystem.CreateDirectory(clientPath+"data"+FileSystem.PathDelimiter+"graphics"+FileSystem.PathDelimiter+"items"+FileSystem.PathDelimiter, true);
			FileSystem.CreateDirectory(clientPath+"data"+FileSystem.PathDelimiter+"graphics"+FileSystem.PathDelimiter+"sprites"+FileSystem.PathDelimiter, true);
			FileSystem.CreateDirectory(clientPath+"data"+FileSystem.PathDelimiter+"graphics"+FileSystem.PathDelimiter+"tiles"+FileSystem.PathDelimiter, true);
			FileSystem.CreateDirectory(clientPath+"data"+FileSystem.PathDelimiter+"help", true);
			FileSystem.CreateDirectory(clientPath+"data"+FileSystem.PathDelimiter+"icons", true);
			FileSystem.CreateDirectory(clientPath+"data"+FileSystem.PathDelimiter+"maps", true);
			FileSystem.CreateDirectory(clientPath+"data"+FileSystem.PathDelimiter+"music", true);
			FileSystem.CreateDirectory(clientPath+"data"+FileSystem.PathDelimiter+"sfx", true);

			//Kopieren
			FileSystem.CopyFile(source+"AUTHORS-MANA", clientPath+"AUTHORS-MANA");
			FileSystem.CopyFile(source+"AUTHORS-INVERTIKA", clientPath+"AUTHORS-INVERTIKA");
			FileSystem.CopyFile(source+"COPYING", clientPath+"COPYING");
			FileSystem.CopyFile(Globals.folder_client+"Invertika.url", clientPath+"Invertika.url");

			FileSystem.CopyFiles(Globals.folder_client_data, clientPath+"data"+FileSystem.PathDelimiter, "*.*", ExcludeFiles);
			FileSystem.CopyFiles(Globals.folder_clientdata, clientPath+"data"+FileSystem.PathDelimiter, "*.xml");

			FileSystem.CopyFiles(Globals.folder_client_data_fonts, clientPath+"data"+FileSystem.PathDelimiter+"fonts"+FileSystem.PathDelimiter, "*.ttf");
			FileSystem.CopyDirectory(Globals.folder_client_data_graphics, clientPath+"data"+FileSystem.PathDelimiter+"graphics"+FileSystem.PathDelimiter, true, ExcludesDirs, ExcludeFiles, true);
			FileSystem.CopyDirectory(Globals.folder_clientdata_graphics, clientPath+"data"+FileSystem.PathDelimiter+"graphics"+FileSystem.PathDelimiter, true, ExcludesDirs, ExcludeFiles, true);

			FileSystem.CopyFiles(Globals.folder_client_data_help, clientPath+"data"+FileSystem.PathDelimiter+"help"+FileSystem.PathDelimiter, "*.*", ExcludeFiles);
			FileSystem.CopyFiles(Globals.folder_client_data_icons, clientPath+"data"+FileSystem.PathDelimiter+"icons"+FileSystem.PathDelimiter, "*.*", ExcludeFiles);
			FileSystem.CopyFiles(Globals.folder_clientdata_maps, clientPath+"data"+FileSystem.PathDelimiter+"maps"+FileSystem.PathDelimiter, "*.tmx");
			FileSystem.CopyDirectory(Globals.folder_clientdata_music, clientPath+"data"+FileSystem.PathDelimiter+"music"+FileSystem.PathDelimiter, true, ExcludesDirs, ExcludeFiles);
			FileSystem.CopyDirectory(Globals.folder_clientdata_sfx, clientPath+"data"+FileSystem.PathDelimiter+"sfx"+FileSystem.PathDelimiter, true, ExcludesDirs, ExcludeFiles);
			#endregion

			#region tmwclient-minimal
			clientPath=target+"tmwclient-minimal"+FileSystem.PathDelimiter;

			FileSystem.CreateDirectory(clientPath+"data"+FileSystem.PathDelimiter+"music", true);
			
			FileSystem.CreateDirectory(clientPath);
			FileSystem.CopyDirectory(source+"client"+FileSystem.PathDelimiter+"data"+FileSystem.PathDelimiter, clientPath+"data"+FileSystem.PathDelimiter, true, ExcludesDirs, ExcludeFiles);
			FileSystem.CopyFile(source+"AUTHORS-MANA", clientPath+"AUTHORS-MANA");
			FileSystem.CopyFile(source+"AUTHORS-INVERTIKA", clientPath+"AUTHORS-INVERTIKA");
			FileSystem.CopyFile(source+"COPYING", clientPath+"COPYING");
			FileSystem.CopyFile(Globals.folder_client+"Invertika.url", clientPath+"Invertika.url");
			FileSystem.CopyFile(Globals.folder_clientdata_music+"godness.ogg", clientPath+"data"+FileSystem.PathDelimiter+"music"+FileSystem.PathDelimiter+"godness.ogg");
			#endregion

			#region tmwdata
			clientPath=target+"tmwdata"+FileSystem.PathDelimiter;

			FileSystem.CreateDirectory(clientPath);
			FileSystem.CreateDirectory(clientPath+"data"+FileSystem.PathDelimiter, true);
			FileSystem.CopyDirectory(Globals.folder_clientdata, clientPath+"data"+FileSystem.PathDelimiter, true, ExcludesDirs);
			#endregion

			#region tmwdata zero update
			//Dev Verzeichniss
			clientPath=clientPath+"data"+FileSystem.PathDelimiter;
			List<string> filesNew=FileSystem.GetFiles(clientPath, true);

			//Zip erstellen
			ZipFile z=ZipFile.Create(target+FileSystem.PathDelimiter+"update-zero.zip");

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

			FileStream fs=new FileStream(target+FileSystem.PathDelimiter+"update-zero.zip", FileMode.Open, FileAccess.Read);
			BinaryReader br=new BinaryReader(fs);

			byte[] textToHash=br.ReadBytes((int)fs.Length);

			adler.Reset();
			adler.Update(textToHash);
			string adler32=String.Format("{0:x}", adler.Value);

			StreamWriter sw=new StreamWriter(target+FileSystem.PathDelimiter+"adler32.txt");
			sw.WriteLine(adler32);
			sw.Close();
			#endregion
		}

		private void bgwCreateDataFolders_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			MessageBox.Show("Data Verzeichnisse erstellt!");
		}

		private void FormCreateDataFolder_Load(object sender, EventArgs e)
		{
			tbTargetPath.Text=Globals.Options.GetElementAsString("xml.Options.Paths.CreateDataFolder.TargetFolder");
		}
	}
}
