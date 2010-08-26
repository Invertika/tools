using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Invertika_Editor
{
	public partial class FormOptions:Form
	{
		public FormOptions()
		{
			InitializeComponent();
		}

		private void FormOptions_Load(object sender, EventArgs e)
		{
			//Pfade
			tbRepositoryTrunk.Text=Globals.folder_root;

			//FTP (Weltkarte)
			tbFTPWorldmapFolder.Text=Globals.Options.GetElementAsString("xml.Options.FTP.Worldmap.Folder");
			tbFTPWorldmapPasswort.Text=Globals.Options.GetElementAsString("xml.Options.FTP.Worldmap.Password");
			tbFTPWorldmapServer.Text=Globals.Options.GetElementAsString("xml.Options.FTP.Worldmap.Server");
			tbFTPWorldmapUser.Text=Globals.Options.GetElementAsString("xml.Options.FTP.Worldmap.User");

			//Wiki
			tbMediawikiURL.Text=Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL");
			tbMediawikiUsername.Text=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username");
			tbMediawikiPassword.Text=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort");
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			//Pfade
			Globals.Options.WriteElement("xml.Options.Paths.Repository.Trunk", tbRepositoryTrunk.Text);

			//FTP (Weltkarte)
			Globals.Options.WriteElement("xml.Options.FTP.Worldmap.Folder", tbFTPWorldmapFolder.Text);
			Globals.Options.WriteElement("xml.Options.FTP.Worldmap.Password", tbFTPWorldmapPasswort.Text);
			Globals.Options.WriteElement("xml.Options.FTP.Worldmap.Server", tbFTPWorldmapServer.Text);
			Globals.Options.WriteElement("xml.Options.FTP.Worldmap.User", tbFTPWorldmapUser.Text);

			//Wiki
			Globals.Options.WriteElement("xml.Options.Mediawiki.URL", tbMediawikiURL.Text);
			Globals.Options.WriteElement("xml.Options.Mediawiki.Username", tbMediawikiUsername.Text);
			Globals.Options.WriteElement("xml.Options.Mediawiki.Passwort", tbMediawikiPassword.Text);

			//Close
			Close();
		}

		private void btnRepositoryTrunkBrowse_Click(object sender, EventArgs e)
		{
			if(folderBrowserDialog.ShowDialog()==DialogResult.OK)
			{
				tbRepositoryTrunk.Text=folderBrowserDialog.SelectedPath;
			}
		}
	}
}
