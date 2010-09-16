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
using CSCL.Graphic;
using CSCL.Bots.Mediawiki;
using System.Text.RegularExpressions;
using Invertika_Editor.Classes;
using System.Net;
using CSCL.Helpers;

namespace Invertika_Editor
{
	public partial class FormMain : Form
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

				ret+=String.Format("| align=\"center\" | [[Image:item-{0}.png]] {{{{Anker|{0}}}}}\n", item.ID);
				ret+=String.Format("| align=\"center\" | {0}\n", item.ID);
				ret+=String.Format("| [[item-{0}|{1}]]\n", item.ID, item.Name);
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

				ret+=String.Format("| align=\"center\" | [[Image:monster-{0}.png]] {{{{Anker|{0}}}}}\n", monster.ID);
				ret+=String.Format("| align=\"center\" | {0}\n", monster.ID);
				ret+=String.Format("| [[monster-{0}|{1}]]\n", monster.ID, monster.Name);
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
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

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
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(folderBrowserDialog.ShowDialog()==DialogResult.OK)
			{
				string fnMonstersXml=Globals.folder_clientdata+"monsters.xml";
				List<Monster> monsters=Monster.GetMonstersFromMonsterXml(fnMonstersXml);

				foreach(Monster monster in monsters)
				{
					if(monster.ID>9999) continue; //Testmonster ignorieren

					if(monster.Sprite!=null)
					{
						if(monster.Sprite!="")
						{
							string[] splited=monster.Sprite.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
							string spritePath=Globals.folder_clientdata_graphics_sprites+splited[0];

							Sprite tmp=Sprite.GetSpriteFromXml(spritePath);

							Imageset set=tmp.Imagesets[0];
							string[] splited2=set.Src.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
							string srcname=splited2[0];
							gtImage setImage=gtImage.FromFile(Globals.folder_clientdata+srcname);
							gtImage monsterImage=setImage.GetSubImage(0, 0, (uint)set.Width, (uint)set.Height);

							string monsterDst=folderBrowserDialog.SelectedPath+FileSystem.PathDelimiter+"Monster-"+monster.ID+".png";
							monsterImage.SaveToPNGGDI(monsterDst);
						}
					}
				}

				MessageBox.Show("Monster Bilder wurden erfolgreich kopiert.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private string CheckTilesets()
		{
			List<string> files=FileSystem.GetFiles(Globals.folder_clientdata_graphics_tiles, false, "*.png");

			string msg="";

			bool found=false;

			int conformWidth=1024;
			int conformHeight=1024;

			foreach(string i in files)
			{
				if(i.IndexOf("xold_")!=-1) continue; //xold Tilesets werden nicht geprüft

				gtImage tmp=gtImage.FromFile(i);

				string[] splited=FileSystem.GetFilenameWithoutExt(i).Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
				int tileheight=Convert.ToInt32(splited[splited.Length-1]);
				conformHeight=Helper.GetValidTilesetHeight(tileheight);

				if(tmp.Width!=conformWidth||tmp.Height!=conformHeight)
				{
					msg+=String.Format("{0} - Größe entspricht nicht den Richtlinien - X: {1} / Y: {2} -> X: {3} / Y: {4}\n", FileSystem.GetFilename(i), tmp.Width, tmp.Height, conformWidth, conformHeight);
					found=true;
				}
			}

			if(found==false)
			{
				msg="Es wurden keine Fehler gefunden.";
			}

			return msg;
		}

		private void tilesetsÜberprüfenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			string msg=CheckTilesets();
			FormOutputBox.ShowOutputBox("Tilesets welche nicht mit den Richtlinien übereinstimmen", msg);
		}

		private void tilesetsZusammenrechnenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Diese Funktion rechtet 4 512x512 Tilesets zu einem 1024x1024 Tileset zusammen. Sie werden nun nach 4 Dateien gefragt und anschließend nach dem Speicherort.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);

			openFileDialog.Filter="PNG Dateien (*.png)|*.png";
			openFileDialog.FileName="";

			string fn1, fn2, fn3, fn4, fnSave;

			if(openFileDialog.ShowDialog()==DialogResult.OK) //file 1
			{
				fn1=openFileDialog.FileName;

				if(openFileDialog.ShowDialog()==DialogResult.OK) //file 2
				{
					fn2=openFileDialog.FileName;

					if(openFileDialog.ShowDialog()==DialogResult.OK) //file 3
					{
						fn3=openFileDialog.FileName;

						if(openFileDialog.ShowDialog()==DialogResult.OK) //file 4
						{
							fn4=openFileDialog.FileName;

							saveFileDialog.Filter="PNG Dateien (*.png)|*.png";
							saveFileDialog.FileName="";

							if(saveFileDialog.ShowDialog()==DialogResult.OK)
							{
								fnSave=saveFileDialog.FileName;

								gtImage img1=gtImage.FromFile(fn1);
								gtImage img2=gtImage.FromFile(fn2);
								gtImage img3=gtImage.FromFile(fn3);
								gtImage img4=gtImage.FromFile(fn4);

								if(img1.Width!=512||img1.Height!=512
									||img2.Width!=512||img2.Height!=512
									||img3.Width!=512||img3.Height!=512
									||img3.Width!=512||img3.Height!=512)
								{
									//Werte stimmen nicht
									MessageBox.Show("Eines des Ausgangstilesets ist nicht 512x512 Pixel groß. Prozess wird abgebrochen.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
									return;
								}

								gtImage imgSave=new gtImage(1024, 1024, gtImage.Format.RGBA);
								imgSave.Draw(0, 0, img1);
								imgSave.Draw(512, 0, img2);
								imgSave.Draw(0, 512, img3);
								imgSave.Draw(512, 512, img4);

								imgSave.SaveToPNGGDI(fnSave);

								MessageBox.Show("Tileset wurde erfolgreich erzeugt.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
							}
						}
					}
				}
			}
		}

		private void tilesetrechnerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormTilesetCalculator InstFormTilesetCalculator=new FormTilesetCalculator();
			InstFormTilesetCalculator.Show();
		}

		private void vonDenMapsBenutzteTilesetsErmittelnToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			List<string> maps=FileSystem.GetFiles(Globals.folder_clientdata, true, "*.tmx");
			Dictionary<string, int> usedTilesets=new Dictionary<string, int>();
			//List<string> usedTilesets=new List<string>();

			foreach(string fn in maps)
			{
				TMX map=new TMX();
				map.Open(fn, false);

				foreach(CSCL.FileFormats.TMX.TMX.TilesetData fnTileset in map.Tilesets)
				{
					string cleanTileset=FileSystem.GetFilename(fnTileset.imgsource);

					if(usedTilesets.ContainsKey(cleanTileset)==false)
					{
						usedTilesets.Add(cleanTileset, 1);
					}
					else
					{
						int val=usedTilesets[cleanTileset];
						val++;
						usedTilesets[cleanTileset]=val;
					}
				}
			}

			List<string> msg=new List<string>();

			foreach(string i in usedTilesets.Keys)
			{
				msg.Add(String.Format("{0} ({1} mal)", i, usedTilesets[i]));
			}

			msg.Sort();

			FormOutputBox.ShowOutputBox("Von den Maps benutzte Tilesets", msg);
		}

		private string CheckMaps()
		{
			string msg="";
			bool found=false;

			List<string> maps=FileSystem.GetFiles(Globals.folder_clientdata, true, "*.tmx");
			List<string> usedTilesets=new List<string>();

			foreach(string fn in maps)
			{
				bool ground=false, fringe=false, over=false, collision=false, @object=false;

				TMX map=new TMX();
				map.Open(fn, false);

				foreach(CSCL.FileFormats.TMX.TMX.TilesetData fnTileset in map.Tilesets)
				{
					string cleanTileset=Globals.folder_clientdata+fnTileset.imgsource.Replace("../graphics", "graphics");
					if(usedTilesets.IndexOf(cleanTileset)==-1) usedTilesets.Add(cleanTileset);
				}

				foreach(TMX.LayerData ld in map.Layers)
				{
					switch(ld.name)
					{
						case "Ground":
							{
								ground=true;
								break;
							}
						case "Fringe":
							{
								fringe=true;
								break;
							}
						case "Over":
							{
								over=true;
								break;
							}
						case "Collision":
							{
								collision=true;
								break;
							}
						default:
							{
								found=true;
								msg+=String.Format("Unbekannter Layer ({0}) in Map {1} vorhanden.", ld.name, fn);
								break;
							}
					}
				}

				if(!ground) { found=true; msg+=String.Format("Ground Layer in Map {1} nicht vorhanden.", fn); }
				if(!fringe) { found=true; msg+=String.Format("Fringe Layer in Map {1} nicht vorhanden.", fn); }
				if(!over) { found=true; msg+=String.Format("Over Layer in Map {1} nicht vorhanden.", fn); }
				if(!collision) { found=true; msg+=String.Format("Collision Layer in Map {1} nicht vorhanden.", fn); }

				foreach(Objectgroup og in map.ObjectLayers)
				{
					if(og.Name=="Object")
					{
						@object=true;
					}
					else
					{
						found=true;
						msg+=String.Format("Unbekannter Objektlayer ({0}) in Map {1} vorhanden.", og.Name, fn);
					}
				}

				if(!@object) { found=true; msg+=String.Format("Object Layer in Map {1} vorhanden.", fn); }
			}

			usedTilesets.Sort();

			foreach(string i in usedTilesets)
			{
				if(!FileSystem.ExistsFile(i))
				{
					found=true;
					msg+="Tileset existiert nicht: "+i+"\n";
				}
			}

			if(found==false)
			{
				msg="Keine Fehler gefunden.";
			}

			return msg;
		}

		private void mapsÜberprüfenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			string msg=CheckMaps();
			FormOutputBox.ShowOutputBox("Fehler in den Maps", msg);
		}

		private void itemsxmlMediawikiInfoboxenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(folderBrowserDialog.ShowDialog()==DialogResult.OK)
			{
				string fnItemsXml=Globals.folder_clientdata+"items.xml";

				List<Item> items=Item.GetItemsFromItemsXml(fnItemsXml);

				foreach(Item item in items)
				{
					if(item.ID<0) continue; //Unötige Items (Hairsets etc) ignorieren

					List<string> lines=new List<string>();

					lines.Add("{{Automatic}}"+item.ToMediaWikiInfobox());
					lines.Add("");
					lines.Add("Dieses Item besitzt noch keine Beschreibung.");
					lines.Add("");
					lines.Add("==Vorkommen==");
					lines.Add("===Maps===");
					lines.Add("Das Item kommt in folgenden Karten vor:");
					lines.Add("* ");
					lines.Add("* ");
					lines.Add("");
					lines.Add("===Monster===");
					lines.Add("{{Anker|AutomaticStartDrops}}");
					lines.Add("{{Anker|AutomaticEndDrops}}");
					lines.Add("");
					lines.Add("[[Kategorie:Item]]");

					string itemfnDst=folderBrowserDialog.SelectedPath+FileSystem.PathDelimiter+"Item-"+item.ID+".txt";
					File.WriteAllLines(itemfnDst, lines.ToArray());
				}

				MessageBox.Show("Item Seiten wurden erfolgreich geschrieben.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private List<string> GetMonsterVorkommen(int id, Dictionary<int, List<string>> monstermaplist)
		{
			List<string> ret=new List<string>();

			ret.Add("Das Monster kommt auf folgenden Karten vor:");

			if(monstermaplist.ContainsKey(id))
			{
				List<string> maps=monstermaplist[id];

				foreach(string map in maps)
				{
					WebClient client=new WebClient();
					string infourl=String.Format("http://weltkarte.invertika.org/mapinfo.php?onlytext=1&fn={0}", map);
					byte[] padData=client.DownloadData(new Uri(infourl));

					if(padData.Length!=0)
					{
						string infoData=StringHelpers.ByteArrayToStringUTF8(padData);
						string[] splited=infoData.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

						ret.Add(String.Format("* [[{0}|{1}]]", map, splited[0]));
					}
					else
					{
						ret.Add(String.Format("* [[{0}]]", map));
					}
				}
			}
			else
			{
				ret.Add("* keine Vorkommen bekannt");
			}

			return ret;
		}

		private void monsterxmlMediawikiInfoboxenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(folderBrowserDialog.ShowDialog()==DialogResult.OK)
			{
				string fnMonsterXml=Globals.folder_clientdata+"monsters.xml";
				string fnItemsXml=Globals.folder_clientdata+"items.xml";

				List<Monster> monsters=Monster.GetMonstersFromMonsterXml(fnMonsterXml);
				List<Item> items=Item.GetItemsFromItemsXml(fnItemsXml);

				Dictionary<int, List<string>> MonsterMapList=GetAllMonsterSpawnsFromMaps();

				foreach(Monster monster in monsters)
				{
					if(monster.ID>9999) continue; //Experimentelle Monster ignorieren

					List<string> lines=new List<string>();

					lines.Add("{{Automatic}}{{Anker|AutomaticStartInfobox}}"+monster.ToMediaWikiInfobox()+"{{Anker|AutomaticEndInfobox}}");
					lines.Add("");
					lines.Add("Dieses Monster besitzt noch keine Beschreibung.");
					lines.Add("");

					lines.Add("==Vorkommen==");
					lines.Add("{{Anker|AutomaticStartAppearance}}");
					lines.Add("{{Anker|AutomaticEndAppearance}} ");

					lines.Add("");

					lines.Add("==Items==");
					lines.Add("{{Anker|AutomaticStartDrops}}");
					lines.Add("{{Anker|AutomaticEndDrops}}");

					lines.Add("");

					lines.Add("==Strategie==");
					lines.Add("Für dieses Monster existiert noch keine Strategie.");
					lines.Add("");
					lines.Add("[[Kategorie:Monster]]");

					string itemfnDst=folderBrowserDialog.SelectedPath+FileSystem.PathDelimiter+"Monster-"+monster.ID+".txt";
					File.WriteAllLines(itemfnDst, lines.ToArray());
				}

				MessageBox.Show("Monster Seiten wurden erfolgreich geschrieben.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private string CheckItems()
		{
			string fnItemsXml=Globals.folder_clientdata+"items.xml";
			List<Item> items=Item.GetItemsFromItemsXml(fnItemsXml);
			items.Sort();

			string msg="";

			bool found=false;

			foreach(Item item in items)
			{
				if(item.Image!=null)
				{
					if(item.Image!="")
					{
						string[] splited=item.Image.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
						string imagePath=Globals.folder_clientdata_graphics_items+splited[0];

						if(!FileSystem.Exists(imagePath))
						{
							found=true;
							msg+=String.Format("Itembild ({0}) für Item {1} ({2})) existiert nicht.\n", imagePath, item.Name, item.ID);
						}
					}
				}

				if(item.Sprite!=null)
				{
					if(item.Sprite!="")
					{
						string[] splited=item.Sprite.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
						string spritePath=Globals.folder_clientdata_graphics_sprites+splited[0];

						if(!FileSystem.Exists(spritePath))
						{
							found=true;
							msg+=String.Format("Sprite XML Datei ({0}) für Item {1} ({2})) existiert nicht.\n", spritePath, item.Name, item.ID);
						}
						else
						{
							//Sprite öffnen und testen
							Sprite tmpSprite=Sprite.GetSpriteFromXml(spritePath);

							foreach(Imageset set in tmpSprite.Imagesets)
							{
								string[] splited2=set.Src.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
								string setPath=Globals.folder_clientdata+splited2[0];

								if(!FileSystem.Exists(setPath))
								{
									found=true;
									msg+=String.Format("Sprite PNG Datei ({0}) für Item {1} ({2})) existiert nicht.\n", setPath, item.Name, item.ID);
								}
							}
						}
					}
				}
			}

			if(found==false)
			{
				msg="Es wurden keine Fehler gefunden.";
			}

			return msg;
		}

		private void itemsÜberprüfenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			string msg=CheckItems();

			FormOutputBox.ShowOutputBox("Fehler in der items.xml", msg);
		}

		private string CheckMonster()
		{
			string fnMonstersXml=Globals.folder_clientdata+"monsters.xml";
			List<Monster> monsters=Monster.GetMonstersFromMonsterXml(fnMonstersXml);
			monsters.Sort();

			string msg="";

			bool found=false;

			foreach(Monster monster in monsters)
			{
				foreach(Sound sound in monster.Sounds)
				{
					if(sound!=null)
					{
						if(sound.Filename!=null)
						{
							if(sound.Filename!="")
							{
								string imagePath=Globals.folder_clientdata_sfx+sound.Filename;

								if(!FileSystem.Exists(imagePath))
								{
									found=true;
									msg+=String.Format("Sound ({0}) für Monster {1} ({2})) existiert nicht.\n", imagePath, monster.Name, monster.ID);
								}
							}
						}
					}
				}

				if(monster.Sprite!=null)
				{
					if(monster.Sprite!="")
					{
						string[] splited=monster.Sprite.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
						string spritePath=Globals.folder_clientdata_graphics_sprites+splited[0];

						if(!FileSystem.Exists(spritePath))
						{
							found=true;
							msg+=String.Format("Sprite XML Datei ({0}) für Monster {1} ({2})) existiert nicht.\n", spritePath, monster.Name, monster.ID);
						}
						else
						{
							//Sprite öffnen und testen
							Sprite tmpSprite=Sprite.GetSpriteFromXml(spritePath);

							foreach(Imageset set in tmpSprite.Imagesets)
							{
								string[] splited2=set.Src.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
								string setPath=Globals.folder_clientdata+splited2[0];

								if(!FileSystem.Exists(setPath))
								{
									found=true;
									msg+=String.Format("Sprite PNG Datei ({0}) für Monster {1} ({2})) existiert nicht.\n", setPath, monster.Name, monster.ID);
								}
							}
						}
					}
				}
			}

			if(found==false)
			{
				msg="Es wurden keine Fehler gefunden.";
			}

			return msg;
		}

		private void monsterÜberprüfenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			string msg=CheckMonster();
			FormOutputBox.ShowOutputBox("Fehler in der monsters.xml", msg);
		}

		private void ExportItemsInfoboxToMediawikiAPI()
		{
			string url=Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL");
			string username=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username");
			string password=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort");

			Site wiki=new Site(url, username, password);

			PageList pl=new PageList(wiki);
			pl.FillAllFromCategory("Item");
			pl.LoadEx();

			foreach(Page i in pl)
			{
				string text=i.text;
				string start="{{Anker|AutomaticStartInfobox}}";
				string end="{Anker|AutomaticEndInfobox}}";

				int idxBeginInfobox=text.IndexOf(start, 0);
				int idxEndInfobox=text.IndexOf(end, 0);

				int lengthOfString=(idxEndInfobox-idxBeginInfobox)-start.Length-1;
				string infobox=text.Substring(idxBeginInfobox+start.Length, lengthOfString);
				if(infobox!="\n")
				{
					text=text.Replace(infobox, "");
				}

				int itemIndex=-1;
				string[] splited=infobox.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

				foreach(string entry in splited)
				{
					string tmpEntry=entry.Replace(" ", "").ToLower();
					if(tmpEntry.IndexOf("id=")!=-1)
					{
						string[] splited2=tmpEntry.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
						itemIndex=Convert.ToInt32(splited2[1]);
						break;
					}
				}

				if(itemIndex==-1) continue;

				string fnItemsXml=Globals.folder_clientdata+"items.xml";

				List<Item> items=Item.GetItemsFromItemsXml(fnItemsXml);

				foreach(Item item in items)
				{
					if(item.ID==itemIndex)
					{
						string replaceString="{{Anker|AutomaticStartInfobox}}"+item.ToMediaWikiInfobox();

						text=text.Replace(start, replaceString);

						if(i.text!=text)
						{
							i.text=text;
						}
						break;
					}
				}
			}

			pl.SaveSmoothly(1, "Bot: Infobox Item aktualisiert.", true);
		}

		private void itemsxmlMediawikiInfoboxüberMediawkiAPIToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL")=="")
			{
				MessageBox.Show("Bitte geben sie eine Mediawiki URL in den Optionen an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username")=="")
			{
				MessageBox.Show("Bitte geben sie einen Mediawiki Nutzernamen in den Optionen an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort")=="")
			{
				MessageBox.Show("Bitte geben sie einen Mediawiki Passwort in den Optionen an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			ExportItemsInfoboxToMediawikiAPI();

			MessageBox.Show("Infoboxen für die Items in der Mediawiki aktualisiert.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void ExportMonstersInfoboxToMediawikiAPI()
		{
			string url=Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL");
			string username=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username");
			string password=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort");

			Site wiki=new Site(url, username, password);

			PageList pl=new PageList(wiki);
			pl.FillAllFromCategory("Monster");
			pl.LoadEx();

			foreach(Page i in pl)
			{
				string text=i.text;

				//Monster ID ermitteln
				string start="{{Anker|AutomaticStartInfobox}}";
				string end="{Anker|AutomaticEndInfobox}}";
				int idxBeginInfobox=text.IndexOf(start, 0);
				int idxEndInfobox=text.IndexOf(end, 0);

				int lengthOfString=(idxEndInfobox-idxBeginInfobox)-start.Length-1;
				string infobox=text.Substring(idxBeginInfobox+start.Length, lengthOfString);

				int monsterIndex=-1;
				string[] splited=infobox.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

				foreach(string entry in splited)
				{
					string tmpEntry=entry.Replace(" ", "").ToLower();
					if(tmpEntry.IndexOf("id=")!=-1)
					{
						string[] splited2=tmpEntry.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
						monsterIndex=Convert.ToInt32(splited2[1]);
						break;
					}
				}

				if(infobox!="\n")
				{
					text=text.Replace(infobox, "");
				}

				if(monsterIndex==-1) continue;

				//Infobox updaten
				string fnMonstersXml=Globals.folder_clientdata+"monsters.xml";

				List<Monster> monsters=Monster.GetMonstersFromMonsterXml(fnMonstersXml);

				foreach(Monster monster in monsters)
				{
					if(monster.ID==monsterIndex)
					{

						string replaceString="{{Anker|AutomaticStartInfobox}}"+monster.ToMediaWikiInfobox();

						text=text.Replace(start, replaceString);

						if(i.text!=text)
						{
							i.text=text;
						}
						break;
					}
				}
			}

			pl.SaveSmoothly(1, "Bot: Infobox Monster aktualisiert.", true);
		}

		private void monstersxmlMediaWikiInfoboxüberMediawikiAPIToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL")=="")
			{
				MessageBox.Show("Bitte geben sie eine Mediawiki URL in den Optionen an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username")=="")
			{
				MessageBox.Show("Bitte geben sie einen Mediawiki Nutzernamen in den Optionen an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort")=="")
			{
				MessageBox.Show("Bitte geben sie einen Mediawiki Passwort in den Optionen an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			ExportMonstersInfoboxToMediawikiAPI();

			MessageBox.Show("Infoboxen für die Monster in der Mediawiki aktualisiert.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void alleMediawkiExportenDurchführenüberMediawikiAPIToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL")=="")
			{
				MessageBox.Show("Bitte geben sie eine Mediawiki URL in den Optionen an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username")=="")
			{
				MessageBox.Show("Bitte geben sie einen Mediawiki Nutzernamen in den Optionen an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort")=="")
			{
				MessageBox.Show("Bitte geben sie einen Mediawiki Passwort in den Optionen an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			//Items
			ExportItemMonstersDropsToMediawikiAPI();
			ExportItemsInfoboxToMediawikiAPI();
			ExportItemTableToMediawikiAPI();

			//Monster
			ExportMonstersDropsToMediawikiAPI();
			ExportMonstersInfoboxToMediawikiAPI();
			ExportMonstersVorkommenToMediawikiAPI();
			ExportMonsterTableToMediawikiAPI();

			MessageBox.Show("Alle Mediawiki Exporte durchgeführt.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void monsterInMapEinfügenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Diese Funktion ist noch nicht implementiert.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private List<MonsterSpawn> GetMonsterSpawnFromMap(string filename)
		{
			List<MonsterSpawn> ret=new List<MonsterSpawn>();

			TMX map=new TMX();
			map.Open(filename, false);

			foreach(Objectgroup objgroup in map.ObjectLayers)
			{
				if(objgroup.Name.ToLower()=="object")
				{
					foreach(CSCL.FileFormats.TMX.Object obj in objgroup.Objects)
					{
						if(obj.Type!=null)
						{
							if(obj.Type.ToLower()=="spawn")
							{
								int MAX_BEINGS=-1;
								int MONSTER_ID=-1;
								double SPAWN_RATE=-1;

								foreach(Property prop in obj.Properties)
								{
									switch(prop.Name.ToLower())
									{
										case "monster_id":
											{
												MONSTER_ID=Convert.ToInt32(prop.Value);
												break;
											}
										case "max_beings":
											{
												MAX_BEINGS=Convert.ToInt32(prop.Value);
												break;
											}
										case "spawn_rate":
											{
												SPAWN_RATE=Convert.ToDouble(prop.Value);
												break;
											}
									}
								}

								ret.Add(new MonsterSpawn(MAX_BEINGS, MONSTER_ID, SPAWN_RATE));
							}
						}
					}
				}
			}

			return ret;
		}

		private Dictionary<int, List<string>> GetAllMonsterSpawnsFromMaps()
		{
			Dictionary<int, List<string>> ret=new Dictionary<int, List<string>>();

			List<string> maps=FileSystem.GetFiles(Globals.folder_clientdata, true, "*.tmx");

			foreach(string fn in maps)
			{
				List<MonsterSpawn> spawns=GetMonsterSpawnFromMap(fn);

				foreach(MonsterSpawn spawn in spawns)
				{
					if(ret.ContainsKey(spawn.MonsterID))
					{
						ret[spawn.MonsterID].Add(FileSystem.GetFilenameWithoutExt(fn));
					}
					else
					{
						List<string> tmpMapList=new List<string>();
						tmpMapList.Add(FileSystem.GetFilenameWithoutExt(fn));
						ret.Add(spawn.MonsterID, tmpMapList);
					}
				}
			}

			return ret;
		}

		private void monsterInDenMapsErmittelnToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			Dictionary<int, List<string>> MonsterMapList=GetAllMonsterSpawnsFromMaps();

			string fnMonstersXml=Globals.folder_clientdata+"monsters.xml";
			List<Monster> monsters=Monster.GetMonstersFromMonsterXml(fnMonstersXml);
			monsters.Sort();

			string ret="";

			foreach(KeyValuePair<int, List<string>> kvp in MonsterMapList)
			{
				foreach(Monster monster in monsters)
				{
					if(monster.ID==kvp.Key)
					{
						ret+=String.Format("Monster: {0}\n", monster.Name);
						break;
					}
				}

				ret+=String.Format("Monster ID: {0}\n", kvp.Key);

				foreach(string mapname in kvp.Value)
				{
					ret+=String.Format("Map: {0}\n", mapname);
				}

				ret+="\n";
			}

			FormOutputBox.ShowOutputBox("Monsterverteilung in den Maps", ret);
		}

		private void tilesetsUmbennenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			FormRenameTileset InstFormRenameTileset=new FormRenameTileset();
			InstFormRenameTileset.Show();
		}

		private void ExportMonstersVorkommenToMediawikiAPI()
		{
			string url=Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL");
			string username=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username");
			string password=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort");

			Site wiki=new Site(url, username, password);

			PageList pl=new PageList(wiki);
			pl.FillAllFromCategory("Monster");
			pl.LoadEx();

			Dictionary<int, List<string>> MonsterMapList=GetAllMonsterSpawnsFromMaps();

			foreach(Page i in pl)
			{
				string text=i.text;

				//Monster ID ermitteln
				string start="{{Anker|AutomaticStartInfobox}}";
				string end="{Anker|AutomaticEndInfobox}}";
				int idxBeginInfobox=text.IndexOf(start, 0);
				int idxEndInfobox=text.IndexOf(end, 0);

				int lengthOfString=(idxEndInfobox-idxBeginInfobox)-start.Length-1;
				string infobox=text.Substring(idxBeginInfobox+start.Length, lengthOfString);

				int monsterIndex=-1;
				string[] splited=infobox.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

				foreach(string entry in splited)
				{
					string tmpEntry=entry.Replace(" ", "").ToLower();
					if(tmpEntry.IndexOf("id=")!=-1)
					{
						string[] splited2=tmpEntry.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
						monsterIndex=Convert.ToInt32(splited2[1]);
						break;
					}
				}

				//Monster Vorkommen ermitteln
				start="{{Anker|AutomaticStartAppearance}}";
				end="{Anker|AutomaticEndAppearance}}";
				idxBeginInfobox=text.IndexOf(start, 0);
				idxEndInfobox=text.IndexOf(end, 0);

				lengthOfString=(idxEndInfobox-idxBeginInfobox)-start.Length-1;
				string vorkommen=text.Substring(idxBeginInfobox+start.Length, lengthOfString);
				if(vorkommen!="\n")
				{
					text=text.Replace(vorkommen, "");
				}

				if(monsterIndex==-1) continue;

				string fnMonstersXml=Globals.folder_clientdata+"monsters.xml";
				List<Monster> monsters=Monster.GetMonstersFromMonsterXml(fnMonstersXml);

				foreach(Monster monster in monsters)
				{
					if(monster.ID==monsterIndex)
					{
						List<string> mv=GetMonsterVorkommen(monster.ID, MonsterMapList);
						string mvRolled="{{Anker|AutomaticStartAppearance}}";

						foreach(string mventry in mv)
						{
							mvRolled+=mventry+"\n";
						}

						if(mv.Count==0)
						{
							mvRolled+="* das Monster kommt auf keiner Karte vor\n";
						}

						text=text.Replace(start, mvRolled);

						if(i.text!=text)
						{
							i.text=text;
						}
						break;
					}
				}
			}

			pl.SaveSmoothly(1, "Bot: Vorkommen Monster aktualisiert.", true);
		}

		private void vorkommenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL")=="")
			{
				MessageBox.Show("Bitte geben sie eine Mediawiki URL in den Optionen an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username")=="")
			{
				MessageBox.Show("Bitte geben sie einen Mediawiki Nutzernamen in den Optionen an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort")=="")
			{
				MessageBox.Show("Bitte geben sie einen Mediawiki Passwort in den Optionen an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			ExportMonstersVorkommenToMediawikiAPI();

			MessageBox.Show("Vorkommen für die Monster in der Mediawiki aktualisiert.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void ExportMonstersDropsToMediawikiAPI()
		{
			string url=Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL");
			string username=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username");
			string password=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort");

			Site wiki=new Site(url, username, password);

			PageList pl=new PageList(wiki);
			pl.FillAllFromCategory("Monster");
			pl.LoadEx();

			Dictionary<int, List<string>> MonsterMapList=GetAllMonsterSpawnsFromMaps();

			string fnMonsterXml=Globals.folder_clientdata+"monsters.xml";
			string fnItemsXml=Globals.folder_clientdata+"items.xml";

			List<Monster> monsters=Monster.GetMonstersFromMonsterXml(fnMonsterXml);
			List<Item> items=Item.GetItemsFromItemsXml(fnItemsXml);

			foreach(Page i in pl)
			{
				string text=i.text;

				//Monster ID ermitteln
				string start="{{Anker|AutomaticStartInfobox}}";
				string end="{Anker|AutomaticEndInfobox}}";
				int idxBeginInfobox=text.IndexOf(start, 0);
				int idxEndInfobox=text.IndexOf(end, 0);

				int lengthOfString=(idxEndInfobox-idxBeginInfobox)-start.Length-1;
				string infobox=text.Substring(idxBeginInfobox+start.Length, lengthOfString);

				int monsterIndex=-1;
				string[] splited=infobox.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

				foreach(string entry in splited)
				{
					string tmpEntry=entry.Replace(" ", "").ToLower();
					if(tmpEntry.IndexOf("id=")!=-1)
					{
						string[] splited2=tmpEntry.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
						monsterIndex=Convert.ToInt32(splited2[1]);
						break;
					}
				}

				//Monster Vorkommen ermitteln
				start="{{Anker|AutomaticStartDrops}}";
				end="{Anker|AutomaticEndDrops}}";
				idxBeginInfobox=text.IndexOf(start, 0);
				idxEndInfobox=text.IndexOf(end, 0);

				lengthOfString=(idxEndInfobox-idxBeginInfobox)-start.Length-1;
				string vorkommen=text.Substring(idxBeginInfobox+start.Length, lengthOfString);
				if(vorkommen!="\n")
				{
					text=text.Replace(vorkommen, "");
				}

				if(monsterIndex==-1) continue;

				foreach(Monster monster in monsters)
				{
					if(monster.ID==monsterIndex)
					{
						string droplines="Folgende Items werden von dem Monster gedropt:\n\n";
						droplines+="{{DropTableStart}}\n";

						foreach(Drop drop in monster.Drops)
						{
							Item dropItem=null;

							foreach(Item si in items)
							{
								if(si.ID==drop.Item)
								{
									dropItem=si;
									break;
								}
							}

							if(dropItem==null)
							{
								droplines+=String.Format("{{{{DropTableRow| {0} | 0 %}}}}\n", "Unbekanntes Item");
							}
							else
							{
								droplines+=String.Format("{{{{DropTableRow| [[item-{0}|{1}]] | {2} %}}}}\n", dropItem.ID, dropItem.Name, drop.Percent);
							}
						}

						if(monster.Drops.Count==0)
						{
							droplines="\n* keine Drops vorhanden\n";
						}
						else
						{
							droplines+="{{DropTableEnd}}\n";
						}

						string replaceString="{{Anker|AutomaticStartDrops}}"+droplines;
						text=text.Replace(start, replaceString);

						if(i.text!=text)
						{
							i.text=text;
						}
						break;
					}
				}
			}

			pl.SaveSmoothly(1, "Bot: Drops Monster aktualisiert.", true);
		}

		private void dropsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL")=="")
			{
				MessageBox.Show("Bitte geben sie eine Mediawiki URL in den Optionen an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username")=="")
			{
				MessageBox.Show("Bitte geben sie einen Mediawiki Nutzernamen in den Optionen an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort")=="")
			{
				MessageBox.Show("Bitte geben sie einen Mediawiki Passwort in den Optionen an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			ExportMonstersDropsToMediawikiAPI();

			MessageBox.Show("Drops für die Monster in der Mediawiki aktualisiert.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void ExportItemMonstersDropsToMediawikiAPI()
		{
			string url=Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL");
			string username=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username");
			string password=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort");

			Site wiki=new Site(url, username, password);

			PageList pl=new PageList(wiki);
			pl.FillAllFromCategory("Item");
			pl.LoadEx();

			Dictionary<int, List<string>> MonsterMapList=GetAllMonsterSpawnsFromMaps();

			string fnMonsterXml=Globals.folder_clientdata+"monsters.xml";
			string fnItemsXml=Globals.folder_clientdata+"items.xml";

			List<Monster> monsters=Monster.GetMonstersFromMonsterXml(fnMonsterXml);
			List<Item> items=Item.GetItemsFromItemsXml(fnItemsXml);

			foreach(Page i in pl)
			{
				string text=i.text;

				//Monster ID ermitteln
				string start="{{Anker|AutomaticStartInfobox}}";
				string end="{Anker|AutomaticEndInfobox}}";
				int idxBeginInfobox=text.IndexOf(start, 0);
				int idxEndInfobox=text.IndexOf(end, 0);

				int lengthOfString=(idxEndInfobox-idxBeginInfobox)-start.Length-1;
				string infobox=text.Substring(idxBeginInfobox+start.Length, lengthOfString);

				int itemIndex=-1;
				string[] splited=infobox.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

				foreach(string entry in splited)
				{
					string tmpEntry=entry.Replace(" ", "").ToLower();
					if(tmpEntry.IndexOf("id=")!=-1)
					{
						string[] splited2=tmpEntry.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
						itemIndex=Convert.ToInt32(splited2[1]);
						break;
					}
				}

				//Monster Vorkommen ermitteln
				start="{{Anker|AutomaticStartDrops}}";
				end="{Anker|AutomaticEndDrops}}";
				idxBeginInfobox=text.IndexOf(start, 0);
				idxEndInfobox=text.IndexOf(end, 0);

				lengthOfString=(idxEndInfobox-idxBeginInfobox)-start.Length-1;
				string monsterdrops=text.Substring(idxBeginInfobox+start.Length, lengthOfString);
				if(monsterdrops!="\n")
				{
					text=text.Replace(monsterdrops, "");
				}

				if(itemIndex==-1) continue;

				string droplines="Folgende Monster dropen das Item:\n\n";
				droplines+="{{DropTableStart}}\n";

				bool OneDrop=false;

				foreach(Monster monster in monsters)
				{
					foreach(Drop drop in monster.Drops)
					{
						if(drop.Item==itemIndex)
						{
							OneDrop=true;
							droplines+=String.Format("{{{{DropTableRow| [[monster-{0}|{1}]] | {2} %}}}}\n", monster.ID, monster.Name, drop.Percent);
						}
					}
				}

				if(OneDrop==false)
				{
					droplines="\n* kein Monster dropt dieses Item\n";
				}
				else
				{
					droplines+="{{DropTableEnd}}\n";
				}

				string replaceString="{{Anker|AutomaticStartDrops}}"+droplines;
				text=text.Replace(start, replaceString);

				if(i.text!=text)
				{
					i.text=text;
				}
			}

			pl.SaveSmoothly(1, "Bot: Item Drops aktualisiert.", true);
		}

		private void dropsToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL")=="")
			{
				MessageBox.Show("Bitte geben sie eine Mediawiki URL in den Optionen an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username")=="")
			{
				MessageBox.Show("Bitte geben sie einen Mediawiki Nutzernamen in den Optionen an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort")=="")
			{
				MessageBox.Show("Bitte geben sie einen Mediawiki Passwort in den Optionen an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			ExportItemMonstersDropsToMediawikiAPI();

			MessageBox.Show("Drops für die Items in der Mediawiki aktualisiert.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void mapsxmlWeltkartenDBSQLDateiToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			saveFileDialog.Filter="SQL Dateien (*.sql)|*.sql";

			if(saveFileDialog.ShowDialog()==DialogResult.OK)
			{
				//Maps laden
				string fnMapsXml=Globals.folder_serverdata+"maps.xml";
				List<Map> maps=Map.GetMapsFromMapsXml(fnMapsXml);

				//Ini
				List<string> sqlFile=new List<string>();
				sqlFile.Add("INSERT `wmInformation` (`MapID`, `FileName`, `Title`) VALUES");

				//maps
				foreach(Map i in maps)
				{
					WebClient client=new WebClient();
					string infourl=String.Format("http://weltkarte.invertika.org/mapinfo.php?onlytext=1&fn={0}", i.Name);
					byte[] padData=client.DownloadData(new Uri(infourl));

					if(padData.Length==0)
					{
						sqlFile.Add(String.Format("('{0}', '{1}', '{2}'),", i.ID, i.Name, "kein Name vergeben"));
					}
				}

				sqlFile[sqlFile.Count-1]=sqlFile[sqlFile.Count-1].TrimEnd(',')+";";

				File.WriteAllLines(saveFileDialog.FileName, sqlFile.ToArray());

				MessageBox.Show("Weltkarten DB SQL Datei geschrieben.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void tilesetInAllenMapsTransformierenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			openFileDialog.Filter="Tileset Transformation Dateien (*.tt)|*.tt";

			//Temp
			if(openFileDialog.ShowDialog()==DialogResult.OK)
			{
				TilesetTransformation tt=new TilesetTransformation(openFileDialog.FileName);

				//Maps laden
				List<string> mapfiles=FileSystem.GetFiles(Globals.folder_clientdata, true, "*.tmx");

				foreach(string i in mapfiles)
				{
					bool changed=false;

					TMX maptmx=new TMX();
					maptmx.Open(i);

					//Tiles transformieren
					maptmx.RemoveGidsFromLayerData(); //RemoveGidFrom Tiles

					foreach(TMX.LayerData ld in maptmx.Layers)
					{
						for(int y=0; y<ld.height; y++)
						{
							for(int x=0; x<ld.width; x++)
							{
								int TileNumber=ld.data[x, y];
								TMX.TilesetData ts=ld.tilesetmap[x, y];

								if(ts.imgsource!=null)
								{
									if(FileSystem.GetFilename(ts.imgsource)==tt.OldTileset)
									{
										changed=true;
										try
										{
											ld.data[x, y]=tt.TransformationTable[TileNumber];
										}
										catch
										{
											ld.data[x, y]=0;
										}
									}
								}
							}
						}
					}

					//Tileset umbennen
					TMX.TilesetData TilesetToReplace=null;
					TMX.TilesetData TilesetToRemove=null;

					foreach(TMX.TilesetData td in maptmx.Tilesets)
					{
						if(td.imgsource!=null)
						{
							if(FileSystem.GetFilename(td.imgsource)==tt.OldTileset)
							{
								//Schauen ob das Tileset bereits existiert
								foreach(TMX.TilesetData tileset in maptmx.Tilesets)
								{
									if(FileSystem.GetFilename(tileset.imgsource)==tt.NewTileset)
									{
										TilesetToRemove=td;
										TilesetToReplace=tileset;
										break;
									}
								}

								//Tileset umbennen
								changed=true;
								td.imgsource=td.imgsource.Replace(FileSystem.GetFilename(td.imgsource), tt.NewTileset);

								break;
							}
						}
					}

					if(TilesetToRemove!=null)
					{
						maptmx.ReplaceTilesetInTilesetMap(TilesetToRemove, TilesetToReplace);
						maptmx.Tilesets.Remove(TilesetToRemove);
					}

					if(changed)
					{
						//FirstGids neu vergeben
						maptmx.Tilesets.Sort();

						int firstgit=1;

						foreach(TMX.TilesetData gidChange in maptmx.Tilesets)
						{
							gidChange.firstgid=firstgit;
							firstgit+=2000; //Sicherheitsabstand
						}

						//AddGidToTiles
						maptmx.AddsGidsToLayerData();

						//Map speichern
						maptmx.Save(i);
					}
				}

				MessageBox.Show("Tileset wurde mittels der TT Datei ("+FileSystem.GetFilename(openFileDialog.FileName)+")transformiert.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void tTDateiFür5121024LinksObenErzeugenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			openFileDialog.Filter="PNG Dateien (*.png)|*.png";

			MessageBox.Show("Bitte altes Tileset auswählen.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);

			string tilesetOld="";
			string tilesetNew="";

			if(openFileDialog.ShowDialog()==DialogResult.OK)
			{
				tilesetOld=openFileDialog.FileName;

				MessageBox.Show("Bitte neues Tileset auswählen.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);

				if(openFileDialog.ShowDialog()==DialogResult.OK)
				{
					tilesetNew=openFileDialog.FileName;
					CreateTransformFile(tilesetNew, tilesetOld, 0);
				}
			}
		}

		void CreateTransformFile(string tilesetNew, string tilesetOld, int corrFactor)
		{
			tilesetNew=openFileDialog.FileName;

			TilesetInfo ti=Helper.GetTilesetInfo(tilesetOld);

			if(ti.TileWidth!=32)
			{
				MessageBox.Show("Zur Zeit werden nur Tilesets mit einer Tilebreite von 32 Pixel unterstützt.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			//Datei schreiben
			string ttFilename=Globals.folder_clientdata_graphics_tiles+FileSystem.GetFilenameWithoutExt(tilesetOld)+".tt";
			StreamWriter sw=new StreamWriter(ttFilename);

			sw.WriteLine(FileSystem.GetFilename(tilesetOld));
			sw.WriteLine(FileSystem.GetFilename(tilesetNew));

			sw.WriteLine(ti.TileWidth);
			sw.WriteLine(ti.TileHeight);

			double tilesPerRowOld=16.0;

			int tilesCount=256; //Höhe 32 //Kann eigentlich ruhig zu viel sein - wird dann bei der Transformation nicht bentutzt
			//if(ti.TileHeight==64) tilesCount=128; //Höhe 64

			for(int i=0; i<tilesCount; i++)
			{
				int oldTileNumber=i;
				int rowNumber=(int)(oldTileNumber/tilesPerRowOld);
				int newTileNumber=(rowNumber*16)+oldTileNumber;
				newTileNumber+=corrFactor; //korrektur

				sw.WriteLine("{0}:{1}", oldTileNumber, newTileNumber);
			}

			sw.Close();
		}

		private void tTDateiFür5121024RechtsObenErzeugenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			openFileDialog.Filter="PNG Dateien (*.png)|*.png";

			MessageBox.Show("Bitte altes Tileset auswählen.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);

			string tilesetOld="";
			string tilesetNew="";

			if(openFileDialog.ShowDialog()==DialogResult.OK)
			{
				tilesetOld=openFileDialog.FileName;

				MessageBox.Show("Bitte neues Tileset auswählen.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);

				if(openFileDialog.ShowDialog()==DialogResult.OK)
				{
					tilesetNew=openFileDialog.FileName;
					CreateTransformFile(tilesetNew, tilesetOld, 16);
				}
			}
		}

		private void höhlengeneratorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Diese Funktion ist noch nicht implementiert.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void tTDateiFür5121024RechtsUntenErzeugenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			openFileDialog.Filter="PNG Dateien (*.png)|*.png";

			MessageBox.Show("Bitte altes Tileset auswählen.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);

			string tilesetOld="";
			string tilesetNew="";

			if(openFileDialog.ShowDialog()==DialogResult.OK)
			{
				tilesetOld=openFileDialog.FileName;

				MessageBox.Show("Bitte neues Tileset auswählen.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);

				if(openFileDialog.ShowDialog()==DialogResult.OK)
				{
					tilesetNew=openFileDialog.FileName;
					CreateTransformFile(tilesetNew, tilesetOld, 768);
				}
			}
		}

		private void tTDateiFür5121024LinksUntenErzeugenToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			openFileDialog.Filter="PNG Dateien (*.png)|*.png";

			MessageBox.Show("Bitte altes Tileset auswählen.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);

			string tilesetOld="";
			string tilesetNew="";

			if(openFileDialog.ShowDialog()==DialogResult.OK)
			{
				tilesetOld=openFileDialog.FileName;

				MessageBox.Show("Bitte neues Tileset auswählen.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);

				if(openFileDialog.ShowDialog()==DialogResult.OK)
				{
					tilesetNew=openFileDialog.FileName;
					CreateTransformFile(tilesetNew, tilesetOld, 512);
				}
			}
		}

		private void ExportItemTableToMediawikiAPI()
		{
			string url=Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL");
			string username=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username");
			string password=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort");

			Site wiki=new Site(url, username, password);

			Page page=new Page(wiki, "Liste der Items");
			page.LoadEx();

			string itemlist=GetItemsAsMediaWiki();

			//Monster Vorkommen ermitteln
			string text=page.text;
			string start="{{Anker|AutomaticStartItemList}}";
			string end="{Anker|AutomaticEndItemList}}";
			int idxBeginInfobox=text.IndexOf(start, 0);
			int idxEndInfobox=text.IndexOf(end, 0);

			int lengthOfString=(idxEndInfobox-idxBeginInfobox)-start.Length-1;

			string monsterdrops=text.Substring(idxBeginInfobox+start.Length, lengthOfString);
			if(monsterdrops!="\n")
			{
				text=text.Replace(monsterdrops, "");
			}

			string replaceString="{{Anker|AutomaticStartItemList}}\n"+itemlist;
			text=text.Replace(start, replaceString);

			if(page.text!=text)
			{
				page.text=text;
			}

			page.Save("Bot: Liste der Items aktualisiert.", true);
		}

		private void tabelleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL")=="")
			{
				MessageBox.Show("Bitte geben sie eine Mediawiki URL in den Optionen an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username")=="")
			{
				MessageBox.Show("Bitte geben sie einen Mediawiki Nutzernamen in den Optionen an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort")=="")
			{
				MessageBox.Show("Bitte geben sie einen Mediawiki Passwort in den Optionen an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			ExportItemTableToMediawikiAPI();

			MessageBox.Show("Liste der Items in der Mediawiki aktualisiert.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void ExportMonsterTableToMediawikiAPI()
		{
			string url=Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL");
			string username=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username");
			string password=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort");

			Site wiki=new Site(url, username, password);

			Page page=new Page(wiki, "Liste der Monster");
			page.LoadEx();

			string monsterlist=GetMonstersAsMediaWiki();

			//Monster Vorkommen ermitteln
			string text=page.text;
			string start="{{Anker|AutomaticStartMonsterList}}";
			string end="{Anker|AutomaticEndMonsterList}}";
			int idxBeginInfobox=text.IndexOf(start, 0);
			int idxEndInfobox=text.IndexOf(end, 0);

			int lengthOfString=(idxEndInfobox-idxBeginInfobox)-start.Length-1;

			string monsterdrops=text.Substring(idxBeginInfobox+start.Length, lengthOfString);
			if(monsterdrops!="\n")
			{
				text=text.Replace(monsterdrops, "");
			}

			string replaceString="{{Anker|AutomaticStartMonsterList}}\n"+monsterlist;
			text=text.Replace(start, replaceString);

			if(page.text!=text)
			{
				page.text=text;
			}

			page.Save("Bot: Liste der Monster aktualisiert.", true);
		}

		private void tabelleToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL")=="")
			{
				MessageBox.Show("Bitte geben sie eine Mediawiki URL in den Optionen an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username")=="")
			{
				MessageBox.Show("Bitte geben sie einen Mediawiki Nutzernamen in den Optionen an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort")=="")
			{
				MessageBox.Show("Bitte geben sie einen Mediawiki Passwort in den Optionen an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			ExportMonsterTableToMediawikiAPI();

			MessageBox.Show("Liste der Monster in der Mediawiki aktualisiert.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void kompletteÜberprüfungToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			string msg="Items:\n";
			msg+=CheckItems();
			msg+="\n\n";
			msg+="Maps:\n";
			msg+=CheckMaps();
			msg+="\n\n";
			msg+="Monster:\n";
			msg+=CheckMonster();
			msg+="\n\n";
			msg+="Tilesets:\n";
			msg+=CheckTilesets();

			FormOutputBox.ShowOutputBox("Komplette Überprüfung", msg);
		}

		private void tTDatei32x64Für5121024LinksObenErzeugenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			openFileDialog.Filter="PNG Dateien (*.png)|*.png";

			MessageBox.Show("Bitte altes Tileset auswählen.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);

			string tilesetOld="";
			string tilesetNew="";

			if(openFileDialog.ShowDialog()==DialogResult.OK)
			{
				tilesetOld=openFileDialog.FileName;

				MessageBox.Show("Bitte neues Tileset auswählen.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);

				if(openFileDialog.ShowDialog()==DialogResult.OK)
				{
					tilesetNew=openFileDialog.FileName;

					TilesetInfo ti=Helper.GetTilesetInfo(tilesetOld);

					if(ti.TileWidth!=32||ti.TileHeight!=32)
					{
						MessageBox.Show("Zur Zeit werden nur Tilesets mit einer Tilegröße von 32 x 32 Pixel unterstützt.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
						return;
					}

					//Datei schreiben
					string ttFilename=Globals.folder_clientdata_graphics_tiles+FileSystem.GetFilenameWithoutExt(tilesetOld)+".tt";
					StreamWriter sw=new StreamWriter(ttFilename);

					sw.WriteLine(FileSystem.GetFilename(tilesetOld));
					sw.WriteLine(FileSystem.GetFilename(tilesetNew));

					sw.WriteLine(ti.TileWidth);
					sw.WriteLine(ti.TileHeight);

					double tilesPerRowOld=16.0;

					for(int i=0; i<256; i++)
					{
						int oldTileNumber=i;
						int rowNumber=(int)(oldTileNumber/tilesPerRowOld);
						int newTileNumber=(rowNumber*16)+oldTileNumber;

						sw.WriteLine("{0}:{1}", oldTileNumber, newTileNumber);
					}

					sw.Close();
				}
			}
		}
	}
}