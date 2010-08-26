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

		private void xMLÖffnenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Diese Funktion ist noch nicht implementiert.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

		private void tilesetsÜberprüfenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			List<string> files=FileSystem.GetFiles(Globals.folder_clientdata_graphics_tiles, false, "*.png");

			string msg="Tilesets welche nicht mit den Richtlinien übereinstimmen:\n";
			msg+="\n";

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

			MessageBox.Show(msg, "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
			List<string> usedTilesets=new List<string>();

			foreach(string fn in maps)
			{
				TMX map=new TMX();
				map.Open(fn, false);

				foreach(CSCL.FileFormats.TMX.TMX.TilesetData fnTileset in map.Tilesets)
				{
					string cleanTileset=FileSystem.GetFilename(fnTileset.imgsource);
					if(usedTilesets.IndexOf(cleanTileset)==-1) usedTilesets.Add(cleanTileset);
				}
			}

			string msg="Von den Maps benutzte Tilesets:\n";
			msg+="\n";

			usedTilesets.Sort();

			foreach(string i in usedTilesets)
			{
				msg+=i+"\n";
			}

			MessageBox.Show(msg, "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void mapsÜberprüfenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			List<string> maps=FileSystem.GetFiles(Globals.folder_clientdata, true, "*.tmx");
			List<string> usedTilesets=new List<string>();

			foreach(string fn in maps)
			{
				TMX map=new TMX();
				map.Open(fn, false);

				foreach(CSCL.FileFormats.TMX.TMX.TilesetData fnTileset in map.Tilesets)
				{
					string cleanTileset=Globals.folder_clientdata+fnTileset.imgsource.Replace("../graphics", "graphics");
					if(usedTilesets.IndexOf(cleanTileset)==-1) usedTilesets.Add(cleanTileset);
				}
			}

			string msg="Fehler in den Maps:\n";
			msg+="\n";

			usedTilesets.Sort();

			foreach(string i in usedTilesets)
			{
				if(!FileSystem.ExistsFile(i))
				{
					msg+="Tileset existiert nicht: "+i+"\n";
				}
			}

			MessageBox.Show(msg, "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

					lines.Add(item.ToMediaWikiInfobox());
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
					lines.Add("Folgende Monster droppen das Item:");
					lines.Add("* ");
					lines.Add("* ");
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

			ret.Add("==Vorkommen==");
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
						string infoData=StringHelpers.ByteArrayToString(padData);
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

					lines.Add(monster.ToMediaWikiInfobox());
					lines.Add("");
					lines.Add("Dieses Monster besitzt noch keine Beschreibung.");
					lines.Add("");

					lines.AddRange(GetMonsterVorkommen(monster.ID, MonsterMapList));

					lines.Add("");

					lines.Add("==Items==");
					lines.Add("Folgende Items werden von dem Monster gedropt:");
					lines.Add("");
					lines.Add("{| border=\"1\" cellspacing=\"0\" cellpadding=\"5\" width=\"100%\" align=\"center\"");
					lines.Add("! style=\"background:#efdead;\" | Item");
					lines.Add("! style=\"background:#efdead;\" | Drop Wahrscheinlichkeit");
					lines.Add("|-");

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
							lines.Add(String.Format("| align=\"center\" | {0}", "Unbekanntes Item"));
						}
						else
						{
							lines.Add(String.Format("| align=\"center\" | [[item-{0}|{1}]]", dropItem.ID, dropItem.Name));
						}


						lines.Add(String.Format("| align=\"center\" | {0} %", drop.Percent));
						lines.Add("|-");
					}

					if(monster.Drops.Count==0)
					{
						lines.Add(String.Format("| align=\"center\" | {0}", "keine Drops"));
						lines.Add(String.Format("| align=\"center\" | {0} %", 0));
						lines.Add("|-");
					}

					lines.Add("|}");

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

		private void itemsÜberprüfenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			string fnItemsXml=Globals.folder_clientdata+"items.xml";
			List<Item> items=Item.GetItemsFromItemsXml(fnItemsXml);

			string msg="Fehler in der items.xml:\n";
			msg+="\n";

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
							msg+=String.Format("Sprite XML Datei ({0}) für Item {1} ({2})) existiert nicht.\n", spritePath, item.Name, item.ID);
						}
					}
				}
			}

			MessageBox.Show(msg, "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void monsterÜberprüfenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(Globals.folder_root=="")
			{
				MessageBox.Show("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			string fnMonstersXml=Globals.folder_clientdata+"monsters.xml";
			List<Monster> monsters=Monster.GetMonstersFromMonsterXml(fnMonstersXml);

			string msg="Fehler in der monsters.xml:\n";
			msg+="\n";

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
							msg+=String.Format("Sprite XML Datei ({0}) für Monster {1} ({2})) existiert nicht.\n", spritePath, monster.Name, monster.ID);
						}
					}
				}
			}

			MessageBox.Show(msg, "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
				int idxBeginInfobox=text.IndexOf("{{", 0);
				int idxEndInfobox=text.IndexOf("}}", 0);

				string infobox=text.Substring(idxBeginInfobox, idxEndInfobox-idxBeginInfobox+2);
				text=text.Replace(infobox, "");

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
						text=item.ToMediaWikiInfobox()+text;

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
				int idxBeginInfobox=text.IndexOf("{{", 0);
				int idxEndInfobox=text.IndexOf("}}", 0);

				string infobox=text.Substring(idxBeginInfobox, idxEndInfobox-idxBeginInfobox+2);
				text=text.Replace(infobox, "");

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

				if(monsterIndex==-1) continue;

				string fnMonstersXml=Globals.folder_clientdata+"monsters.xml";

				List<Monster> monsters=Monster.GetMonstersFromMonsterXml(fnMonstersXml);

				foreach(Monster monster in monsters)
				{
					if(monster.ID==monsterIndex)
					{
						text=monster.ToMediaWikiInfobox()+text;

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
			ExportItemsInfoboxToMediawikiAPI();

			//Monster
			ExportMonstersInfoboxToMediawikiAPI();
			ExportMonstersVorkommenToMediawikiAPI();

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

			string ret="Monsterverteilung in den Maps\n\n";

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

			MessageBox.Show(ret, "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void xMLSpeichernToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Diese Funktion ist noch nicht implementiert.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void tilesetsUmbennenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Diese Funktion ist noch nicht implementiert.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
				int idxBeginInfobox=text.IndexOf("{{", 0);
				int idxEndInfobox=text.IndexOf("}}", 0);

				string infobox=text.Substring(idxBeginInfobox, idxEndInfobox-idxBeginInfobox+2);

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
				idxBeginInfobox=text.IndexOf("==Vorkommen==", 0);
				idxEndInfobox=text.IndexOf("==Items==", 0);

				string vorkommen=text.Substring(idxBeginInfobox+13, idxEndInfobox-idxBeginInfobox-14);
				text=text.Replace(vorkommen, "");

				splited=vorkommen.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

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

				if(monsterIndex==-1) continue;		

				string fnMonstersXml=Globals.folder_clientdata+"monsters.xml";
				List<Monster> monsters=Monster.GetMonstersFromMonsterXml(fnMonstersXml);

				foreach(Monster monster in monsters)
				{
					if(monster.ID==monsterIndex)
					{
						List<string> mv=GetMonsterVorkommen(monster.ID, MonsterMapList);
						string mvRolled="";

						foreach(string mventry in mv)
						{
							mvRolled+=mventry+"\n";
						}

						text=text.Replace("==Vorkommen==", mvRolled);

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
	}
}