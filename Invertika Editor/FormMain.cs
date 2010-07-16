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
using System.Xml;

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
			if(!ExitsConfig) Globals.Options.AddRoot("xml");

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

		private void mapsInDieMapsxmlToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string fnMapsXml=Globals.folder_serverdata+"maps.xml";
			string pathMaps=Globals.folder_clientdata_maps;

			//Maps laden
			List<Map> maps=Map.GetMapsFromMapsXml(fnMapsXml);

			List<string> mapfiles=FileSystem.GetFiles(pathMaps, true, "*.tmx");

			foreach(string i in mapfiles)
			{
				string mapname=FileSystem.GetFilenameWithoutExt(i);

				bool MapIsNew=true;

				foreach(Map j in maps)
				{
					if(j.Name.ToLower()==mapname)
					{
						MapIsNew=false;
						break;
					}
				}

				if(MapIsNew)
				{
					char[] splitChars= { '-' };
					string[] Splited=mapname.Split(splitChars);

					int lastID=0;

					switch(Splited[0].ToLower())
					{
						case "ow": // <!-- Outer World IDs > 0 -->
							{
								foreach(Map map in maps)
								{
									if(map.MapType.ToLower()!="ow") continue;
									if(lastID<map.ID) lastID=map.ID;
								}

								if(lastID<=0||lastID>=19000) throw new Exception("Wertebereich ungültig!");

								break;
							}
						case "uw": // <!-- Undefined World IDs > 19000 -->
							{
								foreach(Map map in maps)
								{
									if(map.MapType.ToLower()!="uw") continue;
									if(lastID<map.ID) lastID=map.ID;
								}

								if(lastID<=19000||lastID>=20000) throw new Exception("Wertebereich ungültig!");

								break;
							}
						case "iw": //<!-- Inner World IDs > 20000 -->
							{
								foreach(Map map in maps)
								{
									if(map.MapType.ToLower()!="iw") continue;
									if(lastID<map.ID) lastID=map.ID;
								}

								if(lastID<=20000||lastID>=40000) throw new Exception("Wertebereich ungültig!");

								break;
							}
					}

					lastID++;

					maps.Add(new Map(lastID, mapname));
				}
			}

			//maps.xml speichern
			Map.SaveToMapsXml(fnMapsXml, maps);

			MessageBox.Show("Alle fehlenden Maps wurden in die maps.xml eingetragen!", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void weltkartenErzeugenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(folderBrowserDialog.ShowDialog()==DialogResult.OK)
			{
				string fnMapsXml=Globals.folder_serverdata+"maps.xml";
				string pathOutput=FileSystem.GetPathWithPathDelimiter(folderBrowserDialog.SelectedPath);

				//xmin, xmax, ymin und ymax ermitteln
				List<Map> maps=Map.GetMapsFromMapsXml(fnMapsXml);

				int xmin=0;
				int xmax=0;
				int ymin=0;
				int ymax=0;

				foreach(Map i in maps)
				{
					if(i.MapType.ToLower()!="ow") continue;

					if(i.X<xmin) xmin=i.X;
					if(i.X>xmax) xmax=i.X;
					if(i.Y<ymin) ymin=i.Y;
					if(i.Y>ymax) ymax=i.Y;
				}

				FileSystem.CreateDirectory(pathOutput, true);

				//Kartenerzeugen
				Globals.CreateWorldmapHTML(pathOutput+"weltkarte.html", xmin, xmax, ymin, ymax, 100, false);
				Globals.CreateWorldmapHTML(pathOutput+"weltkarte-small.html", xmin, xmax, ymin, ymax, 50, false);
				Globals.CreateWorldmapHTML(pathOutput+"weltkarte-print.html", xmin, xmax, ymin, ymax, 100, true);
				Globals.CreateWorldmapHTML(pathOutput+"weltkarte-big.html", xmin, xmax, ymin, ymax, 1400, false);

				Globals.CreateWorldmapMediaWiki(pathOutput+"weltkarte.mediawiki", xmin, xmax, ymin, ymax, maps);

				Globals.CreateMySQLScript(pathOutput+"weltkarte.sql", maps);
				return;
			}
		}

		private void mapskripteErzeugenUndEintragenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(folderBrowserDialog.ShowDialog()==DialogResult.OK)
			{
				string fnMapsXml=Globals.folder_serverdata+"maps.xml";
				string pathMaps=Globals.folder_clientdata_maps;
				string pathOutput=FileSystem.GetPathWithPathDelimiter(folderBrowserDialog.SelectedPath);

				//Maps laden
				List<Map> maps=Map.GetMapsFromMapsXml(fnMapsXml);

				foreach(Map i in maps)
				{
					string fnMap=String.Format("{0}{1}.tmx", pathMaps, i.Name);

					bool ExistDef=false;

					if(FileSystem.Exists(fnMap))
					{
						XmlData mapAsXml=new XmlData(fnMap);

						List<XmlNode> nodes=mapAsXml.GetElements("map.objectgroup.object");

						foreach(XmlNode node in nodes)
						{
							string name=mapAsXml.GetAttributeAsString(node, "name");

							if(name.ToLower()=="external map events")
							{
								ExistDef=true;
								break;
							}
						}

						if(ExistDef==false)
						{
							//Definition eintragen
							List<XmlNode> mapnodes=mapAsXml.GetElements("map");
							XmlNode mapnode=null;

							foreach(XmlNode xmlnode in mapnodes)
							{
								if(xmlnode.Name.ToLower()=="map")
								{
									mapnode=xmlnode;
								}
							}

							//mapnodes[0];

							XmlNode objectgroup=mapAsXml.AddElement(mapnode, "objectgroup", "");
							mapAsXml.AddAttribute(objectgroup, "name", "Object");
							mapAsXml.AddAttribute(objectgroup, "width", 70);
							mapAsXml.AddAttribute(objectgroup, "height", 70);
							mapAsXml.AddAttribute(objectgroup, "x", 70);
							mapAsXml.AddAttribute(objectgroup, "y", 70);

							XmlNode @object=mapAsXml.AddElement(objectgroup, "object", "");
							mapAsXml.AddAttribute(@object, "name", "External Map Events");
							mapAsXml.AddAttribute(@object, "type", "SCRIPT");
							mapAsXml.AddAttribute(@object, "x", 0);
							mapAsXml.AddAttribute(@object, "y", 0);

							XmlNode properties=mapAsXml.AddElement(@object, "properties", "");
							XmlNode property=mapAsXml.AddElement(properties, "property", "");
							mapAsXml.AddAttribute(property, "name", "FILENAME");

							string fnLuaScript=String.Format("scripts/maps/{0}.lua", i.Name);
							mapAsXml.AddAttribute(property, "value", fnLuaScript);

							mapAsXml.Save();
						}

						//Lua Script schreiben
						string fnLuaOutput=String.Format("{0}{1}.lua", pathOutput, i.Name);

						switch(i.MapType.ToLower())
						{
							case "ow":
								{
									Map tmpMap;

									string tmpName=Map.IncreaseArcofMap(i.Name, XYZ.Y);
									int MapUp=0;
									try
									{
										tmpMap=Map.GetMapFromName(maps, tmpName);
										MapUp=tmpMap.ID;
									}
									catch
									{
									}

									tmpName=Map.IncreaseArcofMap(i.Name, XYZ.X);
									int MapRight=0;
									try
									{
										tmpMap=Map.GetMapFromName(maps, tmpName);
										MapRight=tmpMap.ID;
									}
									catch
									{
									}

									tmpName=Map.DecreaseArcofMap(i.Name, XYZ.Y);
									int MapDown=0;
									try
									{
										tmpMap=Map.GetMapFromName(maps, tmpName);
										MapDown=tmpMap.ID;
									}
									catch
									{
									}

									tmpName=Map.DecreaseArcofMap(i.Name, XYZ.X);
									int MapLeft=0;
									try
									{
										tmpMap=Map.GetMapFromName(maps, tmpName);
										MapLeft=tmpMap.ID;
									}
									catch
									{
									}

									Globals.CreateMapScriptFile(fnLuaOutput, MapUp, MapRight, MapDown, MapLeft, true);
									break;
								}
							case "uw":
								{
									Globals.CreateMapScriptFile(fnLuaOutput, i.ID, i.ID, i.ID, i.ID, true);
									break;
								}
							case "iw":
								{
									Globals.CreateMapScriptFile(fnLuaOutput);
									break;
								}
						}
					}
					else
					{
						Console.WriteLine("Datei {0} existiert nicht!", fnMap);
					}
				}
			}
		}

		private void mapsAusEinerBitmapErzeugenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormCreateMapsFromBitmap InstFormCreateMapsFromBitmap=new FormCreateMapsFromBitmap();
			InstFormCreateMapsFromBitmap.Show();
		}

		public string GetItemsAsMediaWiki()
		{
			string ret="";
			string fnItemsXml=Globals.folder_clientdata+"items.xml";

			ret+="{| border=\"1\" cellspacing=\"0\" cellpadding=\"5\" width=\"100%\" align=\"center\"\n"
			+"! style=\"background:#efdead;\" | Bild\n"
			+"! style=\"background:#efdead;\" | ID\n"
			+"! style=\"background:#efdead;\" | Name\n"
			+"! style=\"background:#efdead;\" | Beschreibung\n"
			+"! style=\"background:#efdead;\" | HP\n"
			+"! style=\"background:#efdead;\" | Gewicht\n"
			+"! style=\"background:#efdead;\" | Verteidigung\n"
			+"! style=\"background:#efdead;\" | Maximale Anzahl pro Slot\n"
			+"|-\n";

			List<Item> items=Item.GetItemsFromItemsXml(fnItemsXml);
			items.Sort();

			bool weapons=true;
			bool ruestungen=true;
			bool powerups=true;
			bool misc=true;

			foreach(Item item in items)
			{
				if(item.ID<0) continue; //Unötige Items (Hairsets etc) ignorieren

				if(item.ID>=10001&&item.ID<=20000)
				{
					if(weapons)
					{
						ret+="| style=\"background:#efdead;\" colspan=\"8\" align=\"center\"|Waffen\n";
						ret+="|-\n";
						weapons=false;
					}
				}

				if(item.ID>=20001&&item.ID<=30000)
				{
					if(ruestungen)
					{
						ret+="| style=\"background:#efdead;\" colspan=\"8\" align=\"center\"|Rüstungen und Kleidung\n";
						ret+="|-\n";
						ruestungen=false;
					}
				}

				if(item.ID>=30001&&item.ID<=40000)
				{
					if(powerups)
					{
						ret+="| style=\"background:#efdead;\" colspan=\"8\" align=\"center\"|Power Ups\n";
						ret+="|-\n";
						powerups=false;
					}
				}

				if(item.ID>=40001&&item.ID<=50000)
				{
					if(misc)
					{
						ret+="| style=\"background:#efdead;\" colspan=\"8\" align=\"center\"|Sonstiges\n";
						ret+="|-\n";
						misc=false;
					}
				}

				ret+=String.Format("| align=\"center\" | [[Image:item-{0}.png]]\n", item.ID);
				ret+=String.Format("| align=\"center\" | {0}\n", item.ID);
				ret+=String.Format("| {0}\n", item.Name);
				ret+=String.Format("| align=\"center\" | {0}\n", item.Description);
				ret+=String.Format("| align=\"center\" | {0}\n", item.HP);
				ret+=String.Format("| align=\"center\" | {0}\n", item.Weight);
				ret+=String.Format("| align=\"center\" | {0}%\n", item.Defense);
				ret+=String.Format("| align=\"center\" | {0}\n", item.MaxPerSlot);
				ret+=String.Format("|-\n");
			}

			ret+="|}\n";
			return ret;
		}

		private void inDateiToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			saveFileDialog.Filter="MediaWiki Dateien (*.mediamiki)|*.mediamiki";

			if(saveFileDialog.ShowDialog()==DialogResult.OK)
			{
				try
				{
					string ret=GetItemsAsMediaWiki();
					File.WriteAllText(saveFileDialog.FileName, ret);
					MessageBox.Show("Wikidatei erfolgreich geschrieben.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				catch(Exception ex)
				{
					string msg=String.Format("Fehler beim Erzeugen der Umsetzung zu MediaWiki Syntax:\n\nStacktrace:\n{0}", ex.StackTrace.ToString());
					MessageBox.Show(msg, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void inZwischenablageToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			string ret=GetItemsAsMediaWiki();
			Clipboard.SetText(ret);

			MessageBox.Show("Wikidatei erfolgreich in Zwischenablage geschreiben.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		public string GetMonstersAsMediaWiki()
		{
			string ret="";
			//Parameter auswerten
				string fnMonsterXml=Globals.folder_clientdata+"monsters.xml";


					ret+="{| border=\"1\" cellspacing=\"0\" cellpadding=\"5\" width=\"100%\" align=\"center\"\n"
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
					+"|-\n";

					List<Monster> monsters=Monster.GetMonstersFromMonsterXml(fnMonsterXml);
					monsters.Sort();

					foreach(Monster monster in monsters)
					{
						if(monster.ID>9999) continue; //Experimentelle Monster ignorieren

						ret+=String.Format("| align=\"center\" | [[Image:monster-{0}.png]]\n", monster.ID);
						ret+=String.Format("| align=\"center\" | {0}\n", monster.ID);
						ret+=String.Format("| {0}\n", monster.Name);
						ret+=String.Format("| align=\"center\" | {0}\n", monster.Attributes.HP);

						if(monster.Behavior!=null)
						{
							if(monster.Behavior.Aggressive) ret+=String.Format("| align=\"center\" | Ja\n");
							else ret+=String.Format("| align=\"center\" | Nein\n");
						}
						else
						{
							ret+=String.Format("| align=\"center\" | nicht definiert");
						}

						ret+=String.Format("| align=\"center\" | {0}-{1}\n", monster.Attributes.AttackMin-monster.Attributes.AttackDelta, monster.Attributes.AttackMin+monster.Attributes.AttackDelta);
						ret+=String.Format("| align=\"center\" | {0}%\n", monster.Attributes.PhysicalDefence);
						ret+=String.Format("| align=\"center\" | {0}%\n", monster.Attributes.MagicalDefence);
						ret+=String.Format("| align=\"center\" | {0}\n", monster.Exp);

						//String.Format("| Bone (2.1%)<br>Skull (3%)<br>Dark Crystal (10%)<br>Warlord Helmet (0.03%)<br>Warlord Plate (0.02%)<br>Leather Gloves (0.35%)");
						//String.Format("| ''<span style=\"color:#ad1818\">Aggro</span>''");
						ret+=String.Format("|-\n");
					}

					ret+="|}\n";

			return ret;
		}

		private void inDateiToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			saveFileDialog.Filter="MediaWiki Dateien (*.mediamiki)|*.mediamiki";

			if(saveFileDialog.ShowDialog()==DialogResult.OK)
			{
				try
				{
					string ret=GetMonstersAsMediaWiki();
					File.WriteAllText(saveFileDialog.FileName, ret);
					MessageBox.Show("Wikidatei erfolgreich geschrieben.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				catch(Exception ex)
				{
					string msg=String.Format("Fehler beim Erzeugen der Umsetzung zu MediaWiki Syntax:\n\nStacktrace:\n{0}", ex.StackTrace.ToString());
					MessageBox.Show(msg, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void inZwischenablageToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			string ret=GetMonstersAsMediaWiki();
			Clipboard.SetText(ret);

			MessageBox.Show("Wikidatei erfolgreich in Zwischenablage geschreiben.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void xMLÖffnenToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void adler32EinerDateiBerechnenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			openFileDialog.Multiselect=false;
			openFileDialog.FileName="";
			openFileDialog.Filter="Alle Dateien (*.*)|*.*";

			if(openFileDialog.ShowDialog()==DialogResult.OK)
			{
				//adler 32
				ICSharpCode.SharpZipLib.Checksums.Adler32 adler=new ICSharpCode.SharpZipLib.Checksums.Adler32();

				FileStream fs=new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
				BinaryReader br=new BinaryReader(fs);

				byte[] textToHash=br.ReadBytes((int)fs.Length);

				adler.Reset();
				adler.Update(textToHash);
				string adler32=String.Format("{0:x}", adler.Value);

				MessageBox.Show("Der Adler32 lautet: "+adler32, "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);  
			}
		}

		private void bOMAusSkriptenUndMapsEntfernenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			List<string> files=new List<string>();
			files.AddRange(FileSystem.GetFiles(Globals.folder_root, true, "*.xml"));
			files.AddRange(FileSystem.GetFiles(Globals.folder_root, true, "*.lua"));
			files.AddRange(FileSystem.GetFiles(Globals.folder_root, true, "*.tmx"));

			foreach(string i in files)
			{
				//BOM entfernen
				string text=File.ReadAllText(i);
				StreamWriter streamWriter=new StreamWriter(i, false, new UTF8Encoding(false));
				streamWriter.Write(text);
				streamWriter.Close();
			}

			MessageBox.Show("Alle BOMs wurden entfernt.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void itemsxmlBilderToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(folderBrowserDialog.ShowDialog()==DialogResult.OK)
			{
				string fnItemsXml=Globals.folder_clientdata+"items.xml";

				List<Item> items=Item.GetItemsFromItemsXml(fnItemsXml);

				foreach(Item item in items)
				{
					if(item.ID<0) continue; //Unötige Items (Hairsets etc) ignorieren
					string itemImageName=item.Image.Split('|')[0].Trim();
					string itemfnSrc=Globals.folder_clientdata_graphics_items+itemImageName;
					string itemfnDst=folderBrowserDialog.SelectedPath+FileSystem.PathDelimiter+"Item-"+item.ID+".png";
					//File.Copy(itemfnSrc, itemfnDst, true);
					FileSystem.CopyFile(itemfnSrc, itemfnDst, true);
				}

				MessageBox.Show("Item Bilder wurden erfolgreich kopiert.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void monsterxmlBilderToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//if(folderBrowserDialog.ShowDialog()==DialogResult.OK)
			//{
			//    string fnItemsXml=Globals.folder_clientdata+"monster.xml";

			//    List<Monster> items=Monster.GetMonstersFromMonsterXml(fnItemsXml);

			//    foreach(Monster item in items)
			//    {
			//        if(item.ID<0) continue; //Unötige Items (Hairsets etc) ignorieren
			//        string itemfnSrc=Globals.folder_clientdata_graphics_items+item.Image;
			//        string itemfnDst=folderBrowserDialog.SelectedPath+FileSystem.PathDelimiter+"Monster-"+item.ID+".png";
			//        FileSystem.CopyFile(itemfnSrc, itemfnDst, true);
			//    }

			//    MessageBox.Show("Item Bilder wurden erfolgreich kopiert.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
			//}
		}
	}
}