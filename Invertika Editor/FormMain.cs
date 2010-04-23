using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CSCL;
using System.Reflection;
using CSCL.FileFormats.TMX;
using System.IO;
using CSCL.Games.Manasource;

namespace Invertika_Editor
{
	public partial class FormMain:Form
	{
		public FormMain()
		{
			InitializeComponent();
		}

		private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void optionenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormOptions InstFormOptions=new FormOptions();
			InstFormOptions.ShowDialog();
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
				//tbMapImagesDataFolder.Text=Globals.Options.GetElementAsString("xml.Options.MapImagesDataFolder");

				//tbUpdateDataDev.Text=Globals.Options.GetElementAsString("xml.Options.UpdateDataDev");
				//tbUpdateDataLastClient.Text=Globals.Options.GetElementAsString("xml.Options.UpdateDataLastClient");
				//tbUpdateTargetfolder.Text=Globals.Options.GetElementAsString("xml.Options.UpdateTargetfolder");

				//tbSourcePath.Text=Globals.Options.GetElementAsString("xml.Options.DataSourcePath");
				//tbTargetPath.Text=Globals.Options.GetElementAsString("xml.Options.DataTargetPath");
			}

			//RefreshAllowed=true;

			// Setzt die Versionsnummer anhand der Assembly Version
			Assembly MainAssembly=Assembly.GetExecutingAssembly();
			Text+=" "+MainAssembly.GetName().Version.ToString();
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			Globals.Options.Save();
		}

		private void kartenthumbnailsUndMinimapsErzeugenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormCreateMapThumbnailsAndMinimaps InstFormCreateMapThumbnailsAndMinimaps=new FormCreateMapThumbnailsAndMinimaps();
			InstFormCreateMapThumbnailsAndMinimaps.Show();
		}

		private void clientUpdateErstellenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormCreateClientUpdate InstFormCreateClientUpdate=new FormCreateClientUpdate();
			InstFormCreateClientUpdate.Show();
		}

		private void datenOrdnerErstellenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormCreateDataFolder InstFormCreateDataFolder=new FormCreateDataFolder();
			InstFormCreateDataFolder.Show();
		}

		private void koordinatenrechnerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormCoordinateCalculator InstFormCoordinateCalculator=new FormCoordinateCalculator();
			InstFormCoordinateCalculator.Show();
		}

		private void nPCGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormNPCGenerator InstFormNPCGenerator=new FormNPCGenerator();
			InstFormNPCGenerator.Show();
		}

		private void tMXÖffnenUndRendernToolStripMenuItem_Click(object sender, EventArgs e)
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

		private void itemsxmlMediaWikiToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.Options.GetElementAsString("xml.Options.Paths.Repository.Trunk")=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			saveFileDialog.Filter="MediaWiki Dateien (*.mediamiki)|*.mediamiki";

			if(saveFileDialog.ShowDialog()==DialogResult.OK)
			{
				string fnItemsXml=Globals.folder_clientdata+"items.xml";
				string fnItemsMediaWiki=saveFileDialog.FileName;

				StreamWriter sw=new StreamWriter(fnItemsMediaWiki);

				try
				{
					sw.WriteLine("{| border=\"1\" cellspacing=\"0\" cellpadding=\"5\" width=\"100%\" align=\"center\"\n"
					+"! style=\"background:#efdead;\" | Bild\n"
					+"! style=\"background:#efdead;\" | ID\n"
					+"! style=\"background:#efdead;\" | Name\n"
					+"! style=\"background:#efdead;\" | Beschreibung\n"
					+"! style=\"background:#efdead;\" | HP\n"
					+"! style=\"background:#efdead;\" | Gewicht\n"
					+"! style=\"background:#efdead;\" | Verteidigung\n"
					+"! style=\"background:#efdead;\" | Maximale Anzahl pro Slot\n"
					+"|-\n");

					List<Item> items=Item.GetItemsFromItemsXml(fnItemsXml);
					items.Sort();

					foreach(Item item in items)
					{
						if(item.ID<0) continue; //Unötige Items (Hairsets etc) ignorieren

						sw.WriteLine("| align=\"center\" | [[Image:item-{0}.png]]", item.ID);
						sw.WriteLine("| align=\"center\" | {0}", item.ID);
						sw.WriteLine("| {0}", item.Name);
						sw.WriteLine("| align=\"center\" | {0}", item.Description);
						sw.WriteLine("| align=\"center\" | {0}", item.HP);
						sw.WriteLine("| align=\"center\" | {0}", item.Weight);
						sw.WriteLine("| align=\"center\" | {0}%", item.Defense);
						sw.WriteLine("| align=\"center\" | {0}", item.MaxPerSlot);
						sw.WriteLine("|-");
					}

					sw.WriteLine("|}");
				}
				catch(Exception ex)
				{
					string msg=String.Format("Fehler beim Erzeugen der Umsetzung zu MediaWiki Syntax:\n\nStacktrace:\n{0}", ex.StackTrace.ToString());
					MessageBox.Show(msg, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				finally
				{
					sw.Close();
				}

				MessageBox.Show("Wikidatei erfolgreich geschrieben.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void monsterxmlMediaWikiToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.Options.GetElementAsString("xml.Options.Paths.Repository.Trunk")=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			saveFileDialog.Filter="MediaWiki Dateien (*.mediawiki)|*.mediawiki";

			if(saveFileDialog.ShowDialog()==DialogResult.OK)
			{
				//Parameter auswerten
				string fnMonsterXml=Globals.folder_clientdata+"monsters.xml";

				if(fnMonsterXml=="")
				{
					Console.WriteLine("Kein Dateiname angegeben!");
					return;
				}

				string fnMonsterMediaWiki=saveFileDialog.FileName;

				StreamWriter sw=new StreamWriter(fnMonsterMediaWiki);

				try
				{
					sw.WriteLine("{| border=\"1\" cellspacing=\"0\" cellpadding=\"5\" width=\"100%\" align=\"center\"\n"
					+"! style=\"background:#efdead;\" | Bild\n"
					+"! style=\"background:#efdead;\" | ID\n"
					+"! style=\"background:#efdead;\" | Name\n"
					+"! style=\"background:#efdead;\" | HP\n"
					+"! style=\"background:#efdead;\" | Agressiv\n"
					+"! style=\"background:#efdead;\" | Angriff\n"
					+"! style=\"background:#efdead;\" | Verteidigung (physisch)\n"
					+"! style=\"background:#efdead;\" | Verteidigung (magisch)\n"
					+"! style=\"background:#efdead;\" | Erfahrung\n"

					//+"! style=\"background:#efdead;\" | Drops"
					+"|-\n");

					List<Monster> monsters=Monster.GetMonstersFromMonsterXml(fnMonsterXml);
					monsters.Sort();

					foreach(Monster monster in monsters)
					{
						if(monster.ID>9999) continue; //Experimentelle Monster ignorieren

						sw.WriteLine("| align=\"center\" | [[Image:monster-{0}.png]]", monster.ID);
						sw.WriteLine("| align=\"center\" | {0}", monster.ID);
						sw.WriteLine("| {0}", monster.Name);
						sw.WriteLine("| align=\"center\" | {0}", monster.Attributes.HP);

						if(monster.Behavior!=null)
						{
							if(monster.Behavior.Aggressive) sw.WriteLine("| align=\"center\" | Ja");
							else sw.WriteLine("| align=\"center\" | Nein");
						}
						else
						{
							sw.WriteLine("| align=\"center\" | nicht definiert");
						}

						sw.WriteLine("| align=\"center\" | {0}-{1}", monster.Attributes.AttackMin-monster.Attributes.AttackDelta, monster.Attributes.AttackMin+monster.Attributes.AttackDelta);
						sw.WriteLine("| align=\"center\" | {0}%", monster.Attributes.PhysicalDefence);
						sw.WriteLine("| align=\"center\" | {0}%", monster.Attributes.MagicalDefence);
						sw.WriteLine("| align=\"center\" | {0}", monster.Exp);

						//sw.WriteLine("| Bone (2.1%)<br>Skull (3%)<br>Dark Crystal (10%)<br>Warlord Helmet (0.03%)<br>Warlord Plate (0.02%)<br>Leather Gloves (0.35%)");
						//sw.WriteLine("| ''<span style=\"color:#ad1818\">Aggro</span>''");
						sw.WriteLine("|-");
					}

					sw.WriteLine("|}");
				}
				catch(Exception ex)
				{
					string msg=String.Format("Fehler beim Erzeugen der Umsetzung zu MediaWiki Syntax:\n\nStacktrace:\n{0}", ex.StackTrace.ToString());
					MessageBox.Show(msg, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				finally
				{
					sw.Close();
				}

				MessageBox.Show("Wikidatei erfolgreich geschrieben.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}
	}
}