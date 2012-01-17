//  
//  Program.cs
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
using System.Linq;
using System.Text;
using CSCL.Graphic;
using CSCL.FileFormats.TMX;
using System.IO;
using CSCL;
using CSCL.Network.FTP.Client;
using System.Net;
using CSCL.Crypto;
using System.Drawing;
using CSCL.Games.Manasource;
using System.Xml;
using CSCL.Helpers;
using ICSharpCode.SharpZipLib.Zip;
using CSCL.Bots.Mediawiki;
using Invertika;
using Invertika.Classes;
using CSCL.Exceptions;
using Invertika.LuaDoc;
using System.Threading;

namespace ivktool
{
	class Program
	{
		static void DisplayHelp()
		{
			Console.WriteLine("ivktool 1.9.8");
			Console.WriteLine("(c) 2008-2011 by the Invertika Developer Team (http://invertika.org)");
			Console.WriteLine("");
			Console.WriteLine("Nutzung: ivktool -aktion -parameter");
			Console.WriteLine("  z.B. ivktool -worldmap");
			Console.WriteLine("");
			Console.WriteLine("  -calcAdler32 <file(s)>");
			Console.WriteLine("  -check");
			Console.WriteLine("  -checkNPCsOnWiki");
			Console.WriteLine("  -createClientUpdate <pathLastFullClient> <pathUpdate>");
			Console.WriteLine("  -createCollisionsOnMaps");
			Console.WriteLine("  -createDataFolder <path(s)>");
			Console.WriteLine("  -createExampleConfig");
			Console.WriteLine("  -createInnerMap <mapFilename> <usedTemplateMap>");
			Console.WriteLine("  -createMapScriptsAndUpdateMaps");
			Console.WriteLine("  -createWorldmapDatabaseSQLFile <filename(s)>");
			Console.WriteLine("  -exportItemImages <path(s)>");
			Console.WriteLine("  -exportMonsterImages <path(s)>");
			Console.WriteLine("  -exportNPCSprites <path(s)>");
			Console.WriteLine("  -getMonstersOnMap");
			Console.WriteLine("  -getTilesetsFromMapsUsed");
			Console.WriteLine("  -removeBlankTilesFromMaps");
			Console.WriteLine("  -removeBomFromFiles");
			Console.WriteLine("  -removeNonExistingTilesetsFromMaps");
			Console.WriteLine("  -renameTileset <oldName> <newName>");
			Console.WriteLine("  -renameTilesetNameInMapsToTilesetFilename");
			Console.WriteLine("  -renderTMX <file(s)> -output:<path> -zoom:<percent>");
			Console.WriteLine("  -transformTileInMaps -srcTileset:<name> -dstTileset:<name> -srcTile:<id> -dstTile:<id>");
			Console.WriteLine("  -update [-onlyVisible] [-clearCache]");
			Console.WriteLine("  -updateMapsInMapsXml");
			Console.WriteLine("  -updateMinimaps [-all] [-clearCache]");
			Console.WriteLine("  -updateMediaWiki");
			Console.WriteLine("  -updateLuaInMediaWiki");
			Console.WriteLine("  -updateWorldmap [-all] [-clearCache]");
			Console.WriteLine("  -updateWorldmapDatabaseSQLFile <filename(s)>");
		}

		#region CreateConfig
		static void CreateExampleConfig()
		{
			string configName=FileSystem.ApplicationPath+"ivktool.xml";

			if(FileSystem.ExistsFile(configName))
			{
				Console.WriteLine("Es existiert bereits eine Konfigurationsdatei!");
				return;
			}

			StreamWriter sw=new StreamWriter(configName);

			sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>");
			sw.WriteLine("<xml>");
			sw.WriteLine("  <Options>");
			sw.WriteLine("    <Paths>");
			sw.WriteLine("      <Repository>");
			sw.WriteLine("        <Trunk>/home/seeseekey/Invertika/Repository/trunk</Trunk>");
			sw.WriteLine("      </Repository>");
			sw.WriteLine("    </Paths>");
			sw.WriteLine("    <FTP>");
			sw.WriteLine("      <Worldmap>");
			sw.WriteLine("        <Folder>");
			sw.WriteLine("        </Folder>");
			sw.WriteLine("        <Password>geheim</Password>");
			sw.WriteLine("        <Server>invertika.org</Server>");
			sw.WriteLine("        <User>nutzer</User>");
			sw.WriteLine("      </Worldmap>");
			sw.WriteLine("    </FTP>");
			sw.WriteLine("    <Mediawiki>");
			sw.WriteLine("      <URL>http://wiki.invertika.org</URL>");
			sw.WriteLine("      <Username>nutzer</Username>");
			sw.WriteLine("      <Passwort>geheim</Passwort>");
			sw.WriteLine("    </Mediawiki>");
			sw.WriteLine("  </Options>");
			sw.WriteLine("</xml>");

			sw.Close();
		}
		#endregion

		#region Clientupdate erstellen
		static List<string> GetFilesWithoutGit(string Path)
		{
			return GetFilesWithoutGit(Path, true, "*.*");
		}

		static List<string> GetFilesWithoutGit(string Path, bool rekursiv, string filter)
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
				if(i.IndexOf(FileSystem.PathDelimiter+".git"+FileSystem.PathDelimiter)==-1) ret.Add(i);
			}

			return ret;
		}

		static void CreateClientUpdate(string folderLastFullClient, string folderUpdateTarget)
		{
			if(Globals.folder_root=="")
			{
				Console.WriteLine("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.");
				return;
			}

			string FolderDev=Globals.folder_root;
			string FolderLastClient=folderLastFullClient;
			string FolderTarget=folderUpdateTarget;

			if(FileSystem.ExistsDirectory(FolderTarget))
			{
				FileSystem.RemoveDirectory(FolderTarget, true);
			}

			FileSystem.CreateDirectory(FolderTarget, true);

			//Dev Verzeichniss
			List<string> filesDev=new List<string>();

			filesDev.AddRange(GetFilesWithoutGit(Globals.folder_client, false, "*.xml"));
			filesDev.AddRange(GetFilesWithoutGit(Globals.folder_data, false, "*.xml"));

			filesDev.AddRange(GetFilesWithoutGit(Globals.folder_client_data_fonts));
			filesDev.AddRange(GetFilesWithoutGit(Globals.folder_client_data_graphics));
			filesDev.AddRange(GetFilesWithoutGit(Globals.folder_data_graphics));

			filesDev.AddRange(GetFilesWithoutGit(Globals.folder_client_data_help));
			filesDev.AddRange(GetFilesWithoutGit(Globals.folder_client_data_icons));
			filesDev.AddRange(GetFilesWithoutGit(Globals.folder_data_maps));
			filesDev.AddRange(GetFilesWithoutGit(Globals.folder_data_music));
			filesDev.AddRange(GetFilesWithoutGit(Globals.folder_data_sfx));

			//Last Client
			List<string> filesNew=new List<string>();

			foreach(string i in filesDev)
			{
				string devRelativ=FileSystem.GetRelativePath(i, FolderDev+FileSystem.PathDelimiter);
				devRelativ=devRelativ.Replace("client"+FileSystem.PathDelimiter+"data"+FileSystem.PathDelimiter, "");
				devRelativ=devRelativ.Replace("client-data"+FileSystem.PathDelimiter, "");
				string devNewClient=FolderLastClient+FileSystem.PathDelimiter+devRelativ;

				if(FileSystem.GetFilename(devRelativ).ToLower()=="cmakelists.txt") continue;
				if(FileSystem.GetFilename(devRelativ).ToLower()=="branding.xml") continue;

				//if(FileSystem.GetFilename(devRelativ)==FileSystem.GetFilename(i))
				//{
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
				//}

				filesNew.Add(i);
			}

			//Ziel
			foreach(string i in filesNew)
			{
				string devRelativ2=FileSystem.GetRelativePath(i, FolderDev+FileSystem.PathDelimiter);
				devRelativ2=devRelativ2.Replace("client-data"+FileSystem.PathDelimiter, "");
				devRelativ2=devRelativ2.Replace("client"+FileSystem.PathDelimiter+"data"+FileSystem.PathDelimiter, "");
				string path2=FileSystem.GetPath(devRelativ2, true);

				FileSystem.CreateDirectory(FolderTarget+FileSystem.PathDelimiter+path2, true);

				FileSystem.CopyFile(i, FolderTarget+FileSystem.PathDelimiter+devRelativ2);
			}

			List<string> filestarget=FileSystem.GetFiles(FolderTarget, true);

			//Zip erstellen
			Console.WriteLine("Erstelle Zip Datei...");
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

			Console.WriteLine("Vorgang abgeschlossen!");
		}
		#endregion

		#region Create map script and update maps
		static void CreateMapScriptsAndUpdateMaps()
		{
			string fnMapsXml=Globals.folder_data+"maps.xml";
			string pathMaps=Globals.folder_data_maps;
			string pathOutput=Globals.folder_data_scripts_maps;

			//Maps laden
			List<Map> maps=Map.GetMapsFromMapsXml(fnMapsXml);

			foreach(Map i in maps)
			{
				string fnMap=String.Format("{0}{1}.tmx", pathMaps, i.Name);

				if(FileSystem.Exists(fnMap))
				{
					WriteExternalMapEvents(fnMap);

					string fnLuaOutput=String.Format("{0}{1}.lua", pathOutput, i.Name);

					if(!FileSystem.ExistsFile(fnLuaOutput))
					{
						//Lua Script schreiben
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

									Script.CreateMapScriptFile(fnLuaOutput, MapUp, MapRight, MapDown, MapLeft, true);
									break;
								}
							case "uw":
								{
									Script.CreateMapScriptFile(fnLuaOutput, i.ID, i.ID, i.ID, i.ID, true);
									break;
								}
							case "iw":
								{
									Script.CreateMapScriptFile(fnLuaOutput);
									break;
								}
						}
					}
				}
				else
				{
					Console.WriteLine("Datei {0} existiert nicht!", fnMap);
				}
			}

			Console.WriteLine("Vorgang abgeschlossen!");
		}
		#endregion

		#region CalcAdler32
		static void CalcAdler32(string filename)
		{
			//adler 32
			ICSharpCode.SharpZipLib.Checksums.Adler32 adler=new ICSharpCode.SharpZipLib.Checksums.Adler32();

			FileStream fs=new FileStream(filename, FileMode.Open, FileAccess.Read);
			BinaryReader br=new BinaryReader(fs);

			byte[] textToHash=br.ReadBytes((int)fs.Length);

			adler.Reset();
			adler.Update(textToHash);
			string adler32=String.Format("{0:x}", adler.Value);

			Console.WriteLine("Der Adler32 lautet: {0}", adler32);
		}
		#endregion

		#region Check
		static void CheckNPCsOnWiki()
		{
			if(Globals.folder_root=="")
			{
				Console.WriteLine("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.");
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL")=="")
			{
				Console.WriteLine("Bitte geben sie eine Mediawiki URL in den Optionen an.");
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username")=="")
			{
				Console.WriteLine("Bitte geben sie einen Mediawiki Nutzernamen in den Optionen an.");
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort")=="")
			{
				Console.WriteLine("Bitte geben sie einen Mediawiki Passwort in den Optionen an.");
				return;
			}

			//MediaWiki
			string url=Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL");
			string username=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username");
			string password=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort");

			Site wiki=new Site(url, username, password);

			PageList pl=new PageList(wiki);
			pl.FillAllFromCategory("NPC");

			List<string> npcsInWiki=new List<string>();
			foreach(Page page in pl)
			{
				npcsInWiki.Add(page.title.ToLower());
			}
			
			//Bestimmte NPC Namen ausschließen
			npcsInWiki.Add("...");
			npcsInWiki.Add("grabstein");
			npcsInWiki.Add("roulette");
			npcsInWiki.Add("schild");
			npcsInWiki.Add("slotmaschine");
			npcsInWiki.Add("würfeltisch");			

			//Mapskript
			List<string> mapscripts=FileSystem.GetFiles(Globals.folder_data_scripts_maps, false, "*.lua");

			foreach(string mapscript in mapscripts)
			{
				if(FileSystem.GetFilename(mapscript)=="uw-sandbox-o0000-o0000-o0000.lua") continue;
				
				string[] lines=File.ReadAllLines(mapscript);

				foreach(string line in lines)
				{
					//    create_npc("Isskel", 36, 17 * TILESIZE + 16, 21 * TILESIZE + 16, isskel_talk, nil) --- Isskel
					if(line.Trim().StartsWith("create_npc"))
					{
						string npcname=line.Trim();
						npcname=npcname.Replace("create_npc(\"", "");
						npcname=npcname.Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries)[0];
						
						npcname=npcname.Replace('_', ' ');
						
						if(npcsInWiki.IndexOf(npcname.ToLower())==-1)
						{
							Console.WriteLine("NPC {0} in der Skriptdatei {1} ist nicht im Wiki dokumentiert.", npcname, FileSystem.GetFilename(mapscript));
						}
					}
				}
			}
		}

		static string CheckItems()
		{
			Console.WriteLine("Überprüfe Items...");

			string fnItemsXml=Globals.folder_data+"items.xml";
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
						string imagePath=Globals.folder_data_graphics_items+splited[0];

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
						string spritePath=Globals.folder_data_graphics_sprites+splited[0];

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
								string setPath=Globals.folder_data+splited2[0];

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

		static string CheckMaps()
		{
			Console.WriteLine("Überprüfe Maps...");

			string msg="";
			bool found=false;

			List<string> maps=FileSystem.GetFiles(Globals.folder_data_maps, true, "*.tmx");
			List<string> usedTilesets=new List<string>();

			if(maps==null)
			{
				Console.WriteLine("Es wurden keine Maps gefunden.");
				Console.WriteLine("Eventuell ist der Pfad des Repositories in der Konfigurationdatei falsch.");
				return "";
			}

			foreach(string fnCurrent in maps)
			{
				Console.WriteLine("Map {0} wird überprüft...", FileSystem.GetFilename(fnCurrent));

				bool ground=false, fringe=false, over=false, collision=false, @object=false;
				int countGround=0, countFringe=0, countOver=0, countCollision=0;

				TMX map=null;

				try
				{
					map=new TMX(fnCurrent, false);
				}
				catch(NotSupportedCompressionException)
				{
					msg+=String.Format("Unbekannte Kompressionsart (warscheinlich zlib) in Map {0} vorhanden.\n", fnCurrent);
					continue;
				}
				catch
				{
					msg+=String.Format("Map {0} konnte nicht geöffnet werden.\n", fnCurrent);
					continue;
				}

				string fn=FileSystem.GetRelativePath(fnCurrent, Globals.folder_data);

				foreach(CSCL.FileFormats.TMX.TMX.TilesetData fnTileset in map.Tilesets)
				{
					string cleanTileset=Globals.folder_data+fnTileset.imgsource.Replace("../graphics", "graphics");
					if(usedTilesets.IndexOf(cleanTileset)==-1) usedTilesets.Add(cleanTileset);
				}

				//Check ob Layer gleiche Größe wie die Map haben
				foreach(TMX.LayerData ld in map.Layers)
				{
					if(ld.width!=map.Width) msg+=String.Format("Layerbreite des Layers {0} ungleich Mapbreite in Map {1}.\n", ld.name, fn);
					if(ld.height!=map.Height) msg+=String.Format("Layerhöhe des Layers {0} ungleich Maphöhe in Map {1}.\n", ld.name, fn);
				}

				foreach(TMX.LayerData ld in map.Layers)
				{
					switch(ld.name)
					{
						case "Ground":
							{
								countGround++;
								ground=true;
								break;
							}
						case "Fringe":
							{
								countFringe++;
								fringe=true;
								break;
							}
						case "Over":
							{
								countOver++;
								over=true;
								break;
							}
						case "Collision":
							{
								countCollision++;
								collision=true;
								break;
							}
						default:
							{
								found=true;
								msg+=String.Format("Unbekannter Layer ({0}) in Map {1} vorhanden.\n", ld.name, fn);
								break;
							}
					}
				}

				bool newEntry=false;

				if(!ground) { found=true; newEntry=true; msg+=String.Format("Ground Layer in Map {0} nicht vorhanden.\n", fn); }
				if(!fringe) { found=true; newEntry=true; msg+=String.Format("Fringe Layer in Map {0} nicht vorhanden.\n", fn); }
				if(!over) { found=true; newEntry=true; msg+=String.Format("Over Layer in Map {0} nicht vorhanden.\n", fn); }
				if(!collision) { found=true; newEntry=true; msg+=String.Format("Collision Layer in Map {0} nicht vorhanden.\n", fn); }

				if(countFringe>1) { found=true; newEntry=true; msg+=String.Format("Fringe Layer in Map {0} öfter als ein Mal vorhanden.\n", fn); }
				if(countCollision>1) { found=true; newEntry=true; msg+=String.Format("Collision Layer in Map {0} öfter als ein Mal vorhanden.\n", fn); }

				int externalMapEventsCount=0;

				if(map.ObjectLayers.Count==0)
				{
					msg+=String.Format("Kein Objektlayer in der Map {0} vorhanden.\n", fn);
				}

				if(map.ObjectLayers.Count>1)
				{
					msg+=String.Format("Mehr als ein Objektlayer in der Map {0} vorhanden.\n", fn);
				}

				foreach(Objectgroup og in map.ObjectLayers)
				{
					if(og.Name=="Object")
					{
						@object=true;

						foreach(CSCL.FileFormats.TMX.Object obj in og.Objects)
						{
							//Namen überprüfen
							if(obj.Name==null||obj.Name=="")
							{
								found=true;
								newEntry=true;
								msg+=String.Format("Objektname für ein Objekt in der Map {0} nicht gesetzt.\n", fn);
							}

							//Warp Überprüfung
							if(obj.Type=="WARP")
							{
								//Prüfen ob Warp auf der Karte liegt
								if(!(obj.X/32>=0&&obj.X/32<=map.Width&&obj.Y/32>=0&&obj.Y/32<=map.Height))
								{
									found=true;
									newEntry=true;
									msg+=String.Format("WARP ({0}) in Map {1} liegt nicht in der Karte.\n", obj.Name, fn);
								}

								string dest_map="";
								int dest_x=0;
								int dest_y=0;

								foreach(Property prop in obj.Properties)
								{
									try
									{
										switch(prop.Name)
										{
											case "DEST_MAP":
												{
													dest_map=prop.Value;
													break;
												}
											case "DEST_X":
												{
													dest_x=Convert.ToInt32(prop.Value);
													break;
												}
											case "DEST_Y":
												{
													dest_y=Convert.ToInt32(prop.Value);
													break;
												}
										}
									}
									catch
									{
										found=true;
										newEntry=true;
										msg+=String.Format("Eigenschaft ({0}) in Objekt {1} auf der Map {2} ist nicht gesetzt.\n", prop.Name, obj.Name, fn);
									}
								}

								int dest_x_pixel=dest_x;
								int dest_y_pixel=dest_y;

								dest_x=dest_x/32;
								dest_y=dest_y/32;

								string warpmapname=Globals.folder_data_maps+dest_map+".tmx";
								if(FileSystem.ExistsFile(warpmapname))
								{
									TMX warpMap=new TMX(warpmapname);

									if(!(dest_x>=0&&dest_x<=warpMap.Width&&dest_y>=0&&dest_y<=warpMap.Height)) //Warp in der Map enthalten
									{
										found=true;
										newEntry=true;
										msg+=String.Format("WARP ({0}) auf Map ({1}) in Map {2} zeigt auf nicht vorhandenen Bereich.\n", obj.Name, dest_map+".tmx", fn);
									}
									else
									{
										//Plausbilitätsprüfung
										foreach(Objectgroup ogWarp in warpMap.ObjectLayers)
										{
											foreach(CSCL.FileFormats.TMX.Object objWarp in ogWarp.Objects)
											{
												if(ogWarp.Name=="Object")
												{
													//Warp Überprüfung
													if(objWarp.Type=="WARP")
													{
														bool DestIsInWarp=false;

														if((dest_x_pixel>=objWarp.X)&&(dest_x_pixel<=objWarp.X+objWarp.Width))
														{
															//X liegt drin
															if((dest_y_pixel>=objWarp.Y)&&(dest_y_pixel<=objWarp.Y+objWarp.Height))
															{
																//Y liegt drin
																DestIsInWarp=true;
															}
														}

														if(DestIsInWarp)
														{
															found=true;
															newEntry=true;
															msg+=String.Format("WARP ({0}) auf Map ({1}) in Map {2} zeigt auf weiteren Warp.\n", obj.Name, dest_map+".tmx", fn);
														}
													}
												}
											}
										}
									}
								}
								else
								{
									found=true;
									newEntry=true;
									msg+=String.Format("Per WARP ({0}) Referenzierte Map ({1}) in Map {2} existiert nicht.\n", obj.Name, dest_map+".tmx", fn);
								}
							}
							else if(obj.Type=="SCRIPT") //Skripte überprüfen
							{
								if(obj.Name=="External Map Events")
								{
									externalMapEventsCount++;

									if(externalMapEventsCount>1)
									{
										found=true;
										newEntry=true;
										msg+=String.Format("Mehrere External Map Events Objekte in der Map ({0}) gefunden.\n", fn);
									}

									string scriptfilename="";

									foreach(Property prop in obj.Properties)
									{
										switch(prop.Name)
										{
											case "FILENAME":
												{
													scriptfilename=prop.Value;
													break;
												}
										}
									}

									if(FileSystem.GetFilenameWithoutExt(fn)!=FileSystem.GetFilenameWithoutExt(scriptfilename))
									{
										found=true;
										newEntry=true;
										msg+=String.Format("Dateiname der Skriptdatei in der Map ({0}) entspricht nicht dem Mapnamen.\n", fn);
									}
								}
								else
								{
									found=true;
									newEntry=true;
									msg+=String.Format("Unbekanntes SCRIPT Objekt in der Map ({0}) gefunden.\n", fn);
								}
							}
						}
					}
					else
					{
						found=true;
						newEntry=true;
						msg+=String.Format("Unbekannter Objektlayer ({0}) in Map {1} vorhanden.\n", og.Name, fn);
					}
				}

				if(externalMapEventsCount==0)
				{
					found=true;
					newEntry=true;
					msg+=String.Format("Es wurde kein \"External Map Events\" in der Map ({0}) gefunden.\n", fn);
				}

				if(!@object) { found=true; msg+=String.Format("Object Layer in Map {0} nicht vorhanden.\n", fn); }

				if(newEntry) msg+="\n";
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

			//Maps XML checken
			string fnMapsXml=Globals.folder_data+"maps.xml";
			List<Map> mapsXml=Map.GetMapsFromMapsXml(fnMapsXml);

			foreach(Map i in mapsXml)
			{
				string fnMap=Globals.folder_data_maps+i.Name+".tmx";

				if(!FileSystem.Exists(fnMap))
				{
					found=true;

					msg+="Map (in maps.xml) existiert nicht: "+i.Name+"\n";
				}
			}

			msg=msg.TrimEnd('\n');

			//nichts gefunden
			if(found==false)
			{
				msg="Keine Fehler gefunden.\n";
			}

			return msg;
		}

		static string CheckMonster()
		{
			Console.WriteLine("Überprüfe Monster...");

			string fnItemsXml=Globals.folder_data+"items.xml";
			List<Item> items=Item.GetItemsFromItemsXml(fnItemsXml);

			string fnMonstersXml=Globals.folder_data+"monsters.xml";
			List<Monster> monsters=Monster.GetMonstersFromMonsterXml(fnMonstersXml);
			monsters.Sort();

			string msg="";

			bool found=false;

			foreach(Monster monster in monsters)
			{
				//Drops testen
				foreach(Drop drop in monster.Drops)
				{
					Item dropitem=null;

					foreach(Item i in items)
					{
						if(drop.Item==i.ID)
						{
							dropitem=i;
							break;
						}
					}

					if(dropitem==null)
					{
						found=true;
						msg+=String.Format("Drop ({0}) für Monster {1} ({2})) existiert nicht.\n", drop.Item, monster.Name, monster.ID);
					}
				}

				//Sounds testen
				foreach(Sound sound in monster.Sounds)
				{
					if(sound!=null)
					{
						if(sound.Filename!=null)
						{
							if(sound.Filename!="")
							{
								string imagePath=Globals.folder_data_sfx+sound.Filename;

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
						string spritePath=Globals.folder_data_graphics_sprites+splited[0];

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
								string setPath=Globals.folder_data+splited2[0];

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

		static string CheckNPCs()
		{
			Console.WriteLine("Überprüfe NPCs...");

			bool found=false;
			string msg="";

			string fnNpcsXml=Globals.folder_data+"npcs.xml";
			List<Npc> npcs=Npc.GetNpcsFromXml(fnNpcsXml);

			foreach(Npc npc in npcs)
			{
				if(npc.SpriteFilename!=null)
				{
					//Sprite prüfen
					string[] splited2=npc.SpriteFilename.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
					string setPath=Globals.folder_data_graphics_sprites+splited2[0];

					if(!FileSystem.Exists(setPath))
					{
						string relpath=FileSystem.GetRelativePath(setPath, Globals.folder_data);

						found=true;
						msg+=String.Format("Sprite Datei ({0}) für NPC (ID: {1}) existiert nicht.\n", relpath, npc.ID);
					}
				}

				if(npc.ParticleFxFilename!=null)
				{
					//Sprite prüfen
					string[] splited2=npc.ParticleFxFilename.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
					string setPath=Globals.folder_data+splited2[0];

					if(!FileSystem.Exists(setPath))
					{
						string relpath=FileSystem.GetRelativePath(setPath, Globals.folder_data);

						found=true;
						msg+=String.Format("ParticleFX Datei ({0}) für NPC (ID: {1}) existiert nicht.\n", relpath, npc.ID);
					}
				}
			}

			if(found==false)
			{
				msg="Es wurden keine Fehler gefunden.";
			}

			return msg;
		}

		static string CheckSpritesOnFileLayer()
		{
			Console.WriteLine("Überprüfe Sprites auf Dateibasis...");

			List<string> Files=FileSystem.GetFiles(Globals.folder_data_graphics_sprites, true, "*.png");

			if(Files==null)
			{
				Console.WriteLine("Es wurden keine Sprites gefunden.");
				Console.WriteLine("Eventuell ist der Pfad des Repositories in der Konfigurationdatei falsch.");
				return "";
			}

			Files.AddRange(FileSystem.GetFiles(Globals.folder_data_graphics_sprites, true, "*.xml"));

			Dictionary<string, int> fileCount=new Dictionary<string, int>();

			foreach(string i in Files)
			{
				string path=FileSystem.GetRelativePath(FileSystem.GetPath(i), Globals.folder_data_graphics_sprites);
				string fn=FileSystem.GetFilenameWithoutExt(i);
				string key=path+fn;

				if(fileCount.ContainsKey(key))
				{
					fileCount[key]++;
				}
				else
				{
					fileCount.Add(key, 1);
				}
			}

			List<string> tmpList=new List<string>();

			foreach(string key in fileCount.Keys)
			{
				int val=fileCount[key];
				if(val!=2)
				{
					tmpList.Add(String.Format("Datei {0} existiert {1} mal.", key, val));
				}
			}

			tmpList.Sort();

			string msg="";

			if(tmpList.Count>0)
			{
				foreach(string i in tmpList)
				{
					msg+=i+"\n";
				}
			}
			else
			{
				msg="Keine Fehler gefunden.\n";
			}

			return msg;
		}

		static string CheckSprites()
		{
			Console.WriteLine("Überprüfe Sprites...");

			bool found=false;
			string msg="";

			List<string> spriteFiles=FileSystem.GetFiles(Globals.folder_data_graphics_sprites, true, "*.xml");

			if(spriteFiles==null)
			{
				Console.WriteLine("Es wurden keine Sprites gefunden.");
				Console.WriteLine("Eventuell ist der Pfad des Repositories in der Konfigurationdatei falsch.");
				return "";
			}

			//Sprite öffnen und testen
			foreach(string i in spriteFiles)
			{
				Sprite tmpSprite=Sprite.GetSpriteFromXml(i);

				foreach(Imageset set in tmpSprite.Imagesets)
				{
					string[] splited2=set.Src.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
					string setPath=Globals.folder_data+splited2[0];

					if(!FileSystem.Exists(setPath))
					{
						found=true;

						string relpathPNG=FileSystem.GetRelativePath(setPath, Globals.folder_data);
						string relpathXML=FileSystem.GetRelativePath(i, Globals.folder_data);

						msg+=String.Format("Sprite PNG Datei ({0}) für XML Datei {1} existiert nicht.\n", relpathPNG, relpathXML);
					}
				}
			}

			if(found==false)
			{
				msg="Es wurden keine Fehler gefunden.";
			}

			return msg;
		}

		static string CheckTilesets()
		{
			Console.WriteLine("Überprüfe Tilesets...");

			List<string> files=FileSystem.GetFiles(Globals.folder_data_graphics_tiles, false, "*.png");

			string msg="";

			bool found=false;

			int conformWidth=1024;
			int conformHeight=1024;

			if(files==null)
			{
				Console.WriteLine("Es wurden keine Tilesets gefunden.");
				Console.WriteLine("Eventuell ist der Pfad des Repositories in der Konfigurationdatei falsch.");
				return "";
			}

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

		static void Check()
		{
			if(Globals.folder_root=="")
			{
				Console.WriteLine("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.");
				return;
			}
			
			if(!FileSystem.ExistsDirectory(Globals.folder_root))
			{
				Console.WriteLine("Der Pfad Invertika Repository existiert nicht.");
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
			msg+="NPCs:\n";
			msg+=CheckNPCs();
			msg+="\n\n";
			msg+="Sprites:\n";
			msg+=CheckSprites();
			msg+="\n\n";
			msg+="Tilesets:\n";
			msg+=CheckTilesets();
			msg+="\n\n";
			msg+="Spriteüberprüfung auf Dateiebene:\n";
			msg+=CheckSpritesOnFileLayer();
			
			Console.WriteLine("");
			Console.WriteLine("Ergebnisse der Überprüfung:");
			Console.WriteLine("");
			Console.WriteLine(msg);
		}
		#endregion

		#region Datenordner erstellen
		static void CreateDataFolder(string dst)
		{
			string source=Globals.folder_root;

			string target=FileSystem.GetPathWithPathDelimiter(dst);

			List<string> ExcludesDirs=new List<string>();
			ExcludesDirs.Add(".git");
			ExcludesDirs.Add("maps_templates");
			ExcludesDirs.Add("maps_rules");

			List<string> ExcludeFiles=new List<string>();
			ExcludeFiles.Add("CMakeLists.txt");
			ExcludeFiles.Add(".gitignore");

			if(FileSystem.ExistsDirectory(target))
			{
				FileSystem.RemoveDirectory(target, true);
			}

			FileSystem.CreateDirectory(target, true);

			Console.WriteLine("Erzeuge Serverdaten...");
			#region manaserv
			string serverPath=target+"manaserv"+FileSystem.PathDelimiter;

			FileSystem.CreateDirectory(serverPath);
			FileSystem.CreateDirectory(serverPath+"data"+FileSystem.PathDelimiter+"maps"+FileSystem.PathDelimiter, true);
			FileSystem.CreateDirectory(serverPath+"data"+FileSystem.PathDelimiter+"scripts"+FileSystem.PathDelimiter, true);
			FileSystem.CreateDirectory(serverPath+"data"+FileSystem.PathDelimiter+"scripts"+FileSystem.PathDelimiter+"libs"+FileSystem.PathDelimiter, true);

			//Kopieren
			FileSystem.CopyFiles(source, serverPath, "*.sh");
			FileSystem.CopyFiles(source, serverPath, "*.xml");
			FileSystem.CopyFiles(Globals.folder_data, serverPath+"data"+FileSystem.PathDelimiter, "*.xml");
			FileSystem.CopyFiles(Globals.folder_data, serverPath+"data"+FileSystem.PathDelimiter, "*.xsd");
			FileSystem.CopyFiles(Globals.folder_data, serverPath+"data"+FileSystem.PathDelimiter, "*.xsl");
			FileSystem.CopyFiles(Globals.folder_data_maps, serverPath+"data"+FileSystem.PathDelimiter+"maps"+FileSystem.PathDelimiter, "*.tmx");

			FileSystem.CopyDirectory(Globals.folder_data_scripts, serverPath+"data"+FileSystem.PathDelimiter+"scripts"+FileSystem.PathDelimiter, true, ExcludesDirs);
			FileSystem.CopyFiles(Globals.folder_data, serverPath+"data"+FileSystem.PathDelimiter, "*.xml");
			#endregion

			Console.WriteLine("Erzeuge Clientdaten...");
			#region manaclient-full
			string clientPath=target+"manaclient-full"+FileSystem.PathDelimiter;

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
			FileSystem.CopyFiles(Globals.folder_data, clientPath+"data"+FileSystem.PathDelimiter, "*.xml");

			FileSystem.CopyFiles(Globals.folder_client_data_fonts, clientPath+"data"+FileSystem.PathDelimiter+"fonts"+FileSystem.PathDelimiter, "*.ttf");
			FileSystem.CopyDirectory(Globals.folder_client_data_graphics, clientPath+"data"+FileSystem.PathDelimiter+"graphics"+FileSystem.PathDelimiter, true, ExcludesDirs, ExcludeFiles, true);
			FileSystem.CopyDirectory(Globals.folder_data_graphics, clientPath+"data"+FileSystem.PathDelimiter+"graphics"+FileSystem.PathDelimiter, true, ExcludesDirs, ExcludeFiles, true);

			FileSystem.CopyFiles(Globals.folder_client_data_help, clientPath+"data"+FileSystem.PathDelimiter+"help"+FileSystem.PathDelimiter, "*.*", ExcludeFiles);
			FileSystem.CopyFiles(Globals.folder_client_data_icons, clientPath+"data"+FileSystem.PathDelimiter+"icons"+FileSystem.PathDelimiter, "*.*", ExcludeFiles);
			FileSystem.CopyFiles(Globals.folder_data_maps, clientPath+"data"+FileSystem.PathDelimiter+"maps"+FileSystem.PathDelimiter, "*.tmx");
			FileSystem.CopyDirectory(Globals.folder_data_music, clientPath+"data"+FileSystem.PathDelimiter+"music"+FileSystem.PathDelimiter, true, ExcludesDirs, ExcludeFiles);
			FileSystem.CopyDirectory(Globals.folder_data_sfx, clientPath+"data"+FileSystem.PathDelimiter+"sfx"+FileSystem.PathDelimiter, true, ExcludesDirs, ExcludeFiles);
			#endregion

			Console.WriteLine("Erzeuge minimale Clientdaten...");
			#region manaclient-minimal
			clientPath=target+"manaclient-minimal"+FileSystem.PathDelimiter;

			FileSystem.CreateDirectory(clientPath+"data"+FileSystem.PathDelimiter+"music", true);

			FileSystem.CreateDirectory(clientPath);
			FileSystem.CopyDirectory(source+"client"+FileSystem.PathDelimiter+"data"+FileSystem.PathDelimiter, clientPath+"data"+FileSystem.PathDelimiter, true, ExcludesDirs, ExcludeFiles);
			FileSystem.CopyFile(source+"AUTHORS-MANA", clientPath+"AUTHORS-MANA");
			FileSystem.CopyFile(source+"AUTHORS-INVERTIKA", clientPath+"AUTHORS-INVERTIKA");
			FileSystem.CopyFile(source+"COPYING", clientPath+"COPYING");
			FileSystem.CopyFile(Globals.folder_client+"Invertika.url", clientPath+"Invertika.url");
			FileSystem.CopyFile(Globals.folder_data_music+"godness.ogg", clientPath+"data"+FileSystem.PathDelimiter+"music"+FileSystem.PathDelimiter+"godness.ogg");
			#endregion

			Console.WriteLine("Erzeuge Datenordner...");
			#region manadata
			clientPath=target+"manadata"+FileSystem.PathDelimiter;

			FileSystem.CreateDirectory(clientPath);
			FileSystem.CreateDirectory(clientPath+"data"+FileSystem.PathDelimiter, true);
			
			List<string> manaDataExcludeDirs=new List<string>(ExcludesDirs);
			manaDataExcludeDirs.Add("scripts");
			
			FileSystem.CopyDirectory(Globals.folder_data, clientPath+"data"+FileSystem.PathDelimiter, true, manaDataExcludeDirs, ExcludeFiles);
			#endregion

			Console.WriteLine("Erzeuge Nullupdate...");
			#region manadata zero update
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
		#endregion

		#region GetMonstersOnMap
		static void GetMonstersOnMap()
		{
			if(Globals.folder_root=="")
			{
				Console.WriteLine("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.");
				return;
			}

			Console.WriteLine("Ermittle alle Monster auf den Maps...");

			Dictionary<int, List<string>> MonsterMapList=GetAllMonsterSpawnsFromMaps();

			string fnMonstersXml=Globals.folder_data+"monsters.xml";
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

			Console.WriteLine(ret);
		}
		#endregion

		#region MediaWiki aktualisieren
		static Dictionary<int, List<string>> GetAllMonsterSpawnsFromMaps()
		{
			Dictionary<int, List<string>> ret=new Dictionary<int, List<string>>();

			List<string> maps=FileSystem.GetFiles(Globals.folder_data, true, "*.tmx");

			if(maps==null)
			{
				Console.WriteLine("Es wurden keine Maps gefunden.");
				Console.WriteLine("Eventuell ist der Pfad des Repositories in der Konfigurationdatei falsch.");
				return new Dictionary<int, List<string>>();
			}

			foreach(string fn in maps)
			{
				List<MonsterSpawn> spawns=Monsters.GetMonsterSpawnFromMap(fn);

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

		static List<string> GetMonsterAppearance(int id, Dictionary<int, List<string>> monsterMapList)
		{
			List<string> ret=new List<string>();

			ret.Add("Das Monster kommt auf folgenden Karten vor:");

			if(monsterMapList.ContainsKey(id))
			{
				List<string> maps=monsterMapList[id];

				foreach(string map in maps)
				{
					WebClient client=new WebClient();
					string infourl=String.Format("http://weltkarte.invertika.org/info.php?onlytext=1&fn={0}", map);
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

		static string GetPlantAsMediaWiki()
		{
			string ret="";
			//Parameter auswerten
			string fnMonsterXml=Globals.folder_data+"monsters.xml";
			string fnItemsXml=Globals.folder_data+"items.xml";

			List<Item> items=Item.GetItemsFromItemsXml(fnItemsXml);

			ret+="{| border=\"1\" cellspacing=\"0\" cellpadding=\"5\" width=\"100%\" align=\"center\" class=\"wikitable sortable\"\n"
			+"! style=\"background:#efdead;\" | Bild\n"
			+"! style=\"background:#efdead;\" | ID\n"
			+"! style=\"background:#efdead;\" | Name\n"
			+"! style=\"background:#efdead;\" | HP\n"
			+"! style=\"background:#efdead;\" | Agressiv\n"
			+"! style=\"background:#efdead;\" | Angriff\n"
			+"! style=\"background:#efdead;\" | Verteidigung (physisch)\n"
			+"! style=\"background:#efdead;\" | Verteidigung (magisch)\n"
			+"! style=\"background:#efdead;\" | Mutation\n"
			+"! style=\"background:#efdead;\" | Händlerdropwert (in Aki)\n"
			+"! style=\"background:#efdead;\" | Kampfstärke\n"
			+"! style=\"background:#efdead;\" | Erfahrung\n"
				//+"! style=\"background:#efdead;\" | Drops"
			+"|-\n";

			List<Monster> monsters=Monster.GetMonstersFromMonsterXml(fnMonsterXml);
			monsters.Sort();

			foreach(Monster monster in monsters)
			{
				if(monster.ID<10000) continue; //Monster, etc. ignorieren
				if(monster.ID>=20000) continue; //Experimentelle Monster etc. ignorieren

				ret+=String.Format("| align=\"center\" | [[Image:Monster-{0}.png]] {{{{Anker|{0}}}}}\n", monster.ID);
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

				if(monster.Attributes.AttackDelta==0)
				{
					ret+=String.Format("| align=\"center\" | {0}\n", monster.Attributes.AttackMin);
				}
				else
				{
					ret+=String.Format("| align=\"center\" | {0}-{1}\n", monster.Attributes.AttackMin, monster.Attributes.AttackMin+monster.Attributes.AttackDelta);
				}

				ret+=String.Format("| align=\"center\" | {0}\n", monster.Attributes.PhysicalDefence);
				ret+=String.Format("| align=\"center\" | {0}\n", monster.Attributes.MagicalDefence);
				ret+=String.Format("| align=\"center\" | {0}%\n", monster.Attributes.Mutation);

				ret+=String.Format("| align=\"center\" | {0}\n", monster.GetSaleDropMoneyValue(items));

				ret+=String.Format("| align=\"center\" | {0}\n", monster.FightingStrength);
				ret+=String.Format("| align=\"center\" | {0}\n", monster.Exp);

				ret+=String.Format("|-\n");
			}

			ret+="|}\n";

			return ret;
		}

		static string GetItemsAsMediaWiki()
		{
			string ret="";
			string fnItemsXml=Globals.folder_data+"items.xml";

			ret+="{| border=\"1\" cellspacing=\"0\" cellpadding=\"5\" width=\"100%\" align=\"center\" class=\"wikitable sortable\"\n"
			+"! style=\"background:#efdead;\" | Bild\n"
			+"! style=\"background:#efdead;\" | ID\n"
			+"! style=\"background:#efdead;\" | Name\n"
			+"! style=\"background:#efdead;\" | Beschreibung\n"
			+"! style=\"background:#efdead;\" | HP\n"
			+"! style=\"background:#efdead;\" | Gewicht\n"
			+"! style=\"background:#efdead;\" | Verteidigung\n"
			+"! style=\"background:#efdead;\" | Verkaufspreis (in Aki)\n"
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
						ret+="| style=\"background:#efdead;\" colspan=\"9\" align=\"center\"|Waffen\n";
						ret+="|-\n";
						weapons=false;
					}
				}

				if(item.ID>=20001&&item.ID<=30000)
				{
					if(ruestungen)
					{
						ret+="| style=\"background:#efdead;\" colspan=\"9\" align=\"center\"|Rüstungen und Kleidung\n";
						ret+="|-\n";
						ruestungen=false;
					}
				}

				if(item.ID>=30001&&item.ID<=40000)
				{
					if(powerups)
					{
						ret+="| style=\"background:#efdead;\" colspan=\"9\" align=\"center\"|Power Ups\n";
						ret+="|-\n";
						powerups=false;
					}
				}

				if(item.ID>=40001&&item.ID<=50000)
				{
					if(misc)
					{
						ret+="| style=\"background:#efdead;\" colspan=\"9\" align=\"center\"|Sonstiges\n";
						ret+="|-\n";
						misc=false;
					}
				}

				ret+=String.Format("| align=\"center\" | [[Image:Item-{0}.png]] {{{{Anker|{0}}}}}\n", item.ID);
				ret+=String.Format("| align=\"center\" | {0}\n", item.ID);
				ret+=String.Format("| [[item-{0}|{1}]]\n", item.ID, item.Name);
				ret+=String.Format("| align=\"center\" | {0}\n", item.Description);
				ret+=String.Format("| align=\"center\" | {0}\n", item.HP);
				ret+=String.Format("| align=\"center\" | {0}\n", item.Weight);
				ret+=String.Format("| align=\"center\" | {0}\n", item.Defense);
				ret+=String.Format("| align=\"center\" | {0}\n", item.Value);
				ret+=String.Format("| align=\"center\" | {0}\n", item.MaxPerSlot);
				ret+=String.Format("|-\n");
			}

			ret+="|}\n";
			return ret;
		}

		static string GetMonstersAsMediaWiki()
		{
			string ret="";
			//Parameter auswerten
			string fnMonsterXml=Globals.folder_data+"monsters.xml";
			string fnItemsXml=Globals.folder_data+"items.xml";

			List<Item> items=Item.GetItemsFromItemsXml(fnItemsXml);

			ret+="{| border=\"1\" cellspacing=\"0\" cellpadding=\"5\" width=\"100%\" align=\"center\" class=\"wikitable sortable\"\n"
			+"! style=\"background:#efdead;\" | Bild\n"
			+"! style=\"background:#efdead;\" | ID\n"
			+"! style=\"background:#efdead;\" | Name\n"
			+"! style=\"background:#efdead;\" | HP\n"
			+"! style=\"background:#efdead;\" | Agressiv\n"
			+"! style=\"background:#efdead;\" | Angriff\n"
			+"! style=\"background:#efdead;\" | Verteidigung (physisch)\n"
			+"! style=\"background:#efdead;\" | Verteidigung (magisch)\n"
			+"! style=\"background:#efdead;\" | Mutation\n"
			+"! style=\"background:#efdead;\" | Geschwindigkeit (in Tiles pro Sekunde)\n"
			+"! style=\"background:#efdead;\" | Händlerdropwert (in Aki)\n"
			+"! style=\"background:#efdead;\" | Kampfstärke\n"
			+"! style=\"background:#efdead;\" | Erfahrung\n"
				//+"! style=\"background:#efdead;\" | Drops"
			+"|-\n";

			List<Monster> monsters=Monster.GetMonstersFromMonsterXml(fnMonsterXml);
			monsters.Sort();

			foreach(Monster monster in monsters)
			{
				if(monster.ID>=10000) continue; //Experimentelle Monster, Pflanzen etc. ignorieren

				ret+=String.Format("| align=\"center\" | [[Image:Monster-{0}.png]] {{{{Anker|{0}}}}}\n", monster.ID);
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

				if(monster.Attributes.AttackDelta==0)
				{
					ret+=String.Format("| align=\"center\" | {0}\n", monster.Attributes.AttackMin);
				}
				else
				{
					ret+=String.Format("| align=\"center\" | {0}-{1}\n", monster.Attributes.AttackMin, monster.Attributes.AttackMin+monster.Attributes.AttackDelta);
				}

				ret+=String.Format("| align=\"center\" | {0}\n", monster.Attributes.PhysicalDefence);
				ret+=String.Format("| align=\"center\" | {0}\n", monster.Attributes.MagicalDefence);
				ret+=String.Format("| align=\"center\" | {0}%\n", monster.Attributes.Mutation);
				ret+=String.Format("| align=\"center\" | {0}\n", monster.Attributes.Speed);

				ret+=String.Format("| align=\"center\" | {0}\n", monster.GetSaleDropMoneyValue(items));

				ret+=String.Format("| align=\"center\" | {0}\n", monster.FightingStrength);
				ret+=String.Format("| align=\"center\" | {0}\n", monster.Exp);

				ret+=String.Format("|-\n");
			}

			ret+="|}\n";

			return ret;
		}

		static string GetNPCSpritesAsMediaWiki()
		{
			string ret="";

			//Parameter auswerten
			ret+="{| border=\"1\" cellspacing=\"0\" cellpadding=\"5\" width=\"100%\" align=\"center\" class=\"wikitable sortable\"\n"
			+"! style=\"background:#efdead;\" | Bild\n"
			+"! style=\"background:#efdead;\" | ID\n"
			+"! style=\"background:#efdead;\" | Sprite (Dateiname)\n"
			+"! style=\"background:#efdead;\" | Partikel FX (Dateiname)\n"
			+"|-\n";

			string fnNpcsXml=Globals.folder_data+"npcs.xml";
			List<Npc> npcs=Npc.GetNpcsFromXml(fnNpcsXml);

			foreach(Npc npc in npcs)
			{
				ret+=String.Format("| align=\"center\" | [[Image:NpcSprite-{0}.png]] {{{{Anker|{0}}}}}\n", npc.ID);
				ret+=String.Format("| align=\"center\" | {0}\n", npc.ID);
				ret+=String.Format("| align=\"center\" | {0}\n", npc.SpriteFilename);
				ret+=String.Format("| align=\"center\" | {0}\n", npc.ParticleFxFilename);

				ret+=String.Format("|-\n");
			}

			ret+="|}\n";

			return ret;
		}

		static void ExportNPCSpriteTableToMediaWiki()
		{
			string url=Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL");
			string username=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username");
			string password=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort");

			Site wiki=new Site(url, username, password);

			Page page=new Page(wiki, "Liste der NPC Sprites");
			page.LoadEx();

			string npcSpriteList=GetNPCSpritesAsMediaWiki();

			//NPC Sprites ermitteln
			string text=page.text;
			string start="{{Anker|AutomaticStartNPCSpriteList}}";
			string end="{Anker|AutomaticEndNPCSpriteList}}";
			int idxBeginInfobox=text.IndexOf(start, 0);
			int idxEndInfobox=text.IndexOf(end, 0);

			int lengthOfString=(idxEndInfobox-idxBeginInfobox)-start.Length-1;

			string monsterdrops=text.Substring(idxBeginInfobox+start.Length, lengthOfString);
			if(monsterdrops!="\n")
			{
				text=text.Replace(monsterdrops, "");
			}

			string replaceString="{{Anker|AutomaticStartNPCSpriteList}}\n"+npcSpriteList;
			text=text.Replace(start, replaceString);

			if(page.text!=text)
			{
				page.text=text;
				page.Save("Bot: Liste der NPC Sprites aktualisiert.", true);
			}
		}

		static void ExportMonsterTableToMediawikiAPI()
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
				page.Save("Bot: Liste der Monster aktualisiert.", true);
			}
		}

		static void ExportPlantsTableToMediawikiAPI()
		{
			string url=Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL");
			string username=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username");
			string password=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort");

			Site wiki=new Site(url, username, password);

			Page page=new Page(wiki, "Liste der Pflanzen");
			page.LoadEx();

			string monsterlist=GetPlantAsMediaWiki();

			//Monster Vorkommen ermitteln
			string text=page.text;
			string start="{{Anker|AutomaticStartPlantList}}";
			string end="{Anker|AutomaticEndPlantList}}";
			int idxBeginInfobox=text.IndexOf(start, 0);
			int idxEndInfobox=text.IndexOf(end, 0);

			int lengthOfString=(idxEndInfobox-idxBeginInfobox)-start.Length-1;

			string monsterdrops=text.Substring(idxBeginInfobox+start.Length, lengthOfString);
			if(monsterdrops!="\n")
			{
				text=text.Replace(monsterdrops, "");
			}

			string replaceString="{{Anker|AutomaticStartPlantList}}\n"+monsterlist;
			text=text.Replace(start, replaceString);

			if(page.text!=text)
			{
				page.text=text;
				page.Save("Bot: Liste der Pflanzen aktualisiert.", true);
			}
		}

		static void ExportItemMonstersDropsToMediawikiAPI()
		{
			string url=Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL");
			string username=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username");
			string password=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort");

			Site wiki=new Site(url, username, password);

			PageList pl=new PageList(wiki);
			pl.FillAllFromCategory("Item");
			pl.LoadEx();

			string fnMonsterXml=Globals.folder_data+"monsters.xml";

			List<Monster> monsters=Monster.GetMonstersFromMonsterXml(fnMonsterXml);

			foreach(Page page in pl)
			{
				string text=page.text;

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
					if(monster.ID>=10000) continue; //Pflanzen etc haben keine "Drops"

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

				if(page.text!=text)
				{
					page.text=text;
					page.Save("Bot: Item Drops aktualisiert.", true);
				}
			}
		}

		static void ExportItemsInfoboxToMediawikiAPI()
		{
			string url=Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL");
			string username=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username");
			string password=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort");

			Site wiki=new Site(url, username, password);

			PageList pl=new PageList(wiki);
			pl.FillAllFromCategory("Item");
			pl.LoadEx();

			foreach(Page page in pl)
			{
				string text=page.text;
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

				string fnItemsXml=Globals.folder_data+"items.xml";

				List<Item> items=Item.GetItemsFromItemsXml(fnItemsXml);

				foreach(Item item in items)
				{
					if(item.ID==itemIndex)
					{
						string replaceString="{{Anker|AutomaticStartInfobox}}"+item.ToMediaWikiInfobox();

						text=text.Replace(start, replaceString);

						if(page.text!=text)
						{
							page.text=text;
							page.Save("Bot: Infobox Item aktualisiert.", true);
						}

						break;
					}
				}
			}
		}

		static void ExportMonstersInfoboxToMediawikiAPI()
		{
			string fnItemsXml=Globals.folder_data+"items.xml";
			List<Item> items=Item.GetItemsFromItemsXml(fnItemsXml);

			string url=Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL");
			string username=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username");
			string password=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort");

			Site wiki=new Site(url, username, password);

			PageList pl=new PageList(wiki);
			pl.FillAllFromCategory("Monster");
			pl.FillAllFromCategory("Pflanze");
			pl.LoadEx();

			foreach(Page page in pl)
			{
				string text=page.text;

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
				string fnMonstersXml=Globals.folder_data+"monsters.xml";

				List<Monster> monsters=Monster.GetMonstersFromMonsterXml(fnMonstersXml);

				foreach(Monster monster in monsters)
				{
					if(monster.ID==monsterIndex)
					{
						string replaceString="{{Anker|AutomaticStartInfobox}}"+monster.ToMediaWikiInfobox(items);

						text=text.Replace(start, replaceString);

						if(page.text!=text)
						{
							page.text=text;
							page.Save("Bot: Infobox Monster aktualisiert.", true);
						}
						break;
					}
				}
			}
		}

		static void ExportItemTableToMediawikiAPI()
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
				page.Save("Bot: Liste der Items aktualisiert.", true);
			}
		}

		static void ExportMonstersDropsToMediawikiAPI()
		{
			string url=Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL");
			string username=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username");
			string password=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort");

			Site wiki=new Site(url, username, password);

			PageList pl=new PageList(wiki);
			pl.FillAllFromCategory("Monster");
			pl.FillAllFromCategory("Pflanze");
			pl.LoadEx();

			string fnMonsterXml=Globals.folder_data+"monsters.xml";
			string fnItemsXml=Globals.folder_data+"items.xml";

			List<Monster> monsters=Monster.GetMonstersFromMonsterXml(fnMonsterXml);
			List<Item> items=Item.GetItemsFromItemsXml(fnItemsXml);

			foreach(Page page in pl)
			{
				string text=page.text;

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

						if(page.text!=text)
						{
							page.text=text;
							page.Save("Bot: Drops Monster aktualisiert.", true);
						}
						break;
					}
				}
			}
		}

		static void ExportMonstersVorkommenToMediawikiAPI()
		{
			string url=Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL");
			string username=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username");
			string password=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort");

			Site wiki=new Site(url, username, password);

			PageList pl=new PageList(wiki);
			pl.FillAllFromCategory("Monster");
			pl.FillAllFromCategory("Pflanze");
			pl.LoadEx();

			Dictionary<int, List<string>> MonsterMapList=GetAllMonsterSpawnsFromMaps();

			foreach(Page page in pl)
			{
				string text=page.text;

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

				string fnMonstersXml=Globals.folder_data+"monsters.xml";
				List<Monster> monsters=Monster.GetMonstersFromMonsterXml(fnMonstersXml);

				foreach(Monster monster in monsters)
				{
					if(monster.ID==monsterIndex)
					{
						List<string> mv=GetMonsterAppearance(monster.ID, MonsterMapList);
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

						if(page.text!=text)
						{
							page.text=text;
							page.Save("Bot: Vorkommen Monster aktualisiert.", true);
						}
						break;
					}
				}
			}
		}

		static void UpdateMediaWiki()
		{
			if(Globals.folder_root=="")
			{
				Console.WriteLine("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.");
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL")=="")
			{
				Console.WriteLine("Bitte geben sie eine Mediawiki URL in den Optionen an.");
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username")=="")
			{
				Console.WriteLine("Bitte geben sie einen Mediawiki Nutzernamen in den Optionen an.");
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort")=="")
			{
				Console.WriteLine("Bitte geben sie einen Mediawiki Passwort in den Optionen an.");
				return;
			}

			//Pflanzen
			try
			{
				Console.WriteLine("Exportiere NPC Sprites...");
				ExportNPCSpriteTableToMediaWiki();

				Console.WriteLine("Exportiere Pflanzentabelle...");
				ExportPlantsTableToMediawikiAPI();

				//Items
				Console.WriteLine("Exportiere Itemtabellen...");
				ExportItemTableToMediawikiAPI();
				ExportItemMonstersDropsToMediawikiAPI();
				ExportItemsInfoboxToMediawikiAPI();

				//Monster
				Console.WriteLine("Exportiere Monstertabellen...");
				ExportMonsterTableToMediawikiAPI();
				ExportMonstersDropsToMediawikiAPI();
				ExportMonstersInfoboxToMediawikiAPI();
				ExportMonstersVorkommenToMediawikiAPI();
			}
			catch(Exception ex)
			{
				Console.WriteLine("Es ist ein Fehler beim Update mittels MediaWiki API aufgetreten!");
				Console.WriteLine(ex.Message);
				return;
			}

			Console.WriteLine("Alle Mediawiki Exporte durchgeführt.");
		}
		#endregion

		#region Minimaps berechnen
		static void UpdateMinimaps(bool all, bool clearCache)
		{
			List<string> files=FileSystem.GetFiles(Globals.folder_data_maps, true, "*.tmx");

			if(files==null)
			{
				Console.WriteLine("Es wurden keine Maps gefunden.");
				Console.WriteLine("Eventuell ist der Pfad des Repositories in der Konfigurationdatei falsch.");
				return;
			}

			#region Bilder berechnen
			foreach(string i in files)
			{
				//Nur sichtbare Weltkarte rendern.
				if(!all)
				{
					Map map=new Map(0, FileSystem.GetFilenameWithoutExt(i));

					//if(map.MapType=="iw") continue;
					//if(map.MapType=="uw") continue;

					if(map.MapType=="ow")
					{
						if(map.X>=-7&&map.X<=7&&map.Y>=-7&&map.Y<=7)
						{
							//nichts tun (sprich rendern)
						}
						else
						{
							continue;
						}
					}
				}

				Console.WriteLine("Überprüfe für Map {0} auf Aktualisierung...", FileSystem.GetFilename(i));

				GC.Collect(2);

				//Hashvergleich
				string text=File.ReadAllText(i);
				string textHash=Hash.SHA1.HashStringToSHA1(text);
				string xmlPath="xml.CalcMinimaps."+FileSystem.GetFilenameWithoutExt(i);

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
					Globals.Options.WriteElement("xml.CalcMinimaps."+FileSystem.GetFilenameWithoutExt(i), textHash);
				}
				else
				{
					Globals.Options.WriteElement(xmlPath, textHash);
				}

				//Hashvergleich
				if(clearCache==false)
				{
					if(textHash==xmlHash) continue;
				}

				//Karte berechnen
				Console.WriteLine("Berechne Minimap für Map {0}", FileSystem.GetFilename(i));

				TMX file=new TMX(i);

				gtImage pic=file.Render();

				int imageSizeOriginalWidth=(int)pic.Width;
				int imageSizeOriginalHeight=(int)pic.Height;
				double imageVerhaeltnis=imageSizeOriginalWidth/(double)imageSizeOriginalHeight;

				int imageSize=100;
				pic=pic.Resize(imageSize, (int)(imageSize/imageVerhaeltnis));

				string fn=FileSystem.GetFilenameWithoutExt(i);
				string fnMinimap=Globals.folder_data_graphics_minimaps+fn+".png";
				pic.SaveToFile(fnMinimap);
			}
			#endregion

			Globals.Options.Save();
		}
		#endregion

		#region RenderTMX
		static void RenderTMX(string tmx, string output, double zoom)
		{
#if! DEBUG
			try
			{
#endif
				if(!FileSystem.ExistsFile(tmx))
				{
					Console.WriteLine("Die Datei {0} existiert nicht!.", tmx);
					return;
				}

				TMX map=new TMX(tmx);
				gtImage img=map.Render();

				if(zoom!=100)
				{
					int newWidth=(int)(img.Width/100*zoom);
					int newHeight=(int)(img.Height/100*zoom);
					img=img.Resize(newWidth, newHeight);
				}

				string fn=output+FileSystem.GetFilenameWithoutExt(tmx)+".png";
				img.SaveToPNGGDI(fn);

				Console.WriteLine("Datei {0} wurde nach {1} gerendert", tmx, fn);
#if! DEBUG
			}
			catch(Exception exception)
			{
				Console.WriteLine("Es gab Probleme beim Parsen der Datei {0}.\n{1}", FileSystem.GetFilename(tmx), exception.ToString());
			}
#endif
		}
		#endregion

		#region Tileset umbenennen
		static void RenameTileset(string oldName, string newName)
		{
			//Maps laden
			List<string> mapfiles=FileSystem.GetFiles(Globals.folder_data, true, "*.tmx");

			foreach(string i in mapfiles)
			{
				bool changed=false;

				TMX maptmx=new TMX(i);

				//Tiles transformieren
				foreach(TMX.TilesetData ld in maptmx.Tilesets)
				{
					string newSource=ld.imgsource.Replace(oldName, newName);

					if(ld.imgsource!=newSource)
					{
						ld.imgsource=newSource;
						changed=true;
					}
				}

				if(changed)
				{
					Console.WriteLine("Bennene Tileset in Map {0} um.", FileSystem.GetFilename(i));
					//Map speichern
					maptmx.Save(i);
				}
			}

			//Tileset umbennen
			string oldFn=Globals.folder_data_graphics_tiles+oldName;
			string newFn=Globals.folder_data_graphics_tiles+newName;

			FileSystem.RenameFile(oldFn, newFn);
		}
		#endregion

		#region UpdateMapsInMapsXml
		static void UpdateMapsInMapsXml()
		{
			string fnMapsXml=Globals.folder_data+"maps.xml";
			string pathMaps=Globals.folder_data_maps;

			//Maps laden
			List<Map> maps=Map.GetMapsFromMapsXml(fnMapsXml);

			List<string> mapfiles=FileSystem.GetFiles(pathMaps, true, "*.tmx");

			if(mapfiles==null)
			{
				Console.WriteLine("Es wurden keine Maps gefunden.");
				Console.WriteLine("Eventuell ist der Pfad des Repositories in der Konfigurationdatei falsch.");
				return;
			}

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

			Console.WriteLine("Alle fehlenden Maps wurden in die maps.xml eingetragen!");
		}
		#endregion

		#region Weltkarte berechnen
		static int GetNextImageSize(int currentSize)
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

		static void SaveFeatureMapMusic(string filename, gtImage img, TMX map)
		{
			//Farben
			Color green=Color.FromArgb(128, 0, 255, 0);
			//Color yellow=Color.FromArgb(128, 255, 255, 0);
			Color red=Color.FromArgb(128, 255, 0, 0);
			//Color blue=Color.FromArgb(128, 0, 0, 255);

			//Images
			gtImage tmpImage=img.GetImage();
			gtImage tmpDraw=new gtImage(tmpImage.Width, tmpImage.Height, tmpImage.ChannelFormat);

			//Properties durchsuchen
			bool found=false;

			foreach(Property prop in map.Properties)
			{
				if(prop.Name=="music")
				{
					found=true;
					break;
				}
			}

			if(found) tmpDraw.Fill(green);
			else tmpDraw.Fill(red);

			//Drawen
			tmpImage.Draw(0, 0, tmpDraw, true);
			tmpImage.SaveToFile(filename);
		}

		static void UpdateWorldmap(bool all, bool clearCache)
		{
			List<string> files=FileSystem.GetFiles(Globals.folder_data_maps, true, "*.tmx");

			if(files==null)
			{
				Console.WriteLine("Es wurden keine Maps gefunden.");
				Console.WriteLine("Eventuell ist der Pfad des Repositories in der Konfigurationdatei falsch.");
				return;
			}

			string temp=FileSystem.TempPath+"Invertika Editor" + FileSystem.PathDelimiter;
			string tempFmMonsterSpreading=FileSystem.TempPath+"Invertika Editor"+FileSystem.PathDelimiter+"fm-monster-spreading"+FileSystem.PathDelimiter;
			string tempFmMusic=FileSystem.TempPath+"Invertika Editor"+FileSystem.PathDelimiter+"fm-music"+FileSystem.PathDelimiter;

			#region Bilderordner löschen und neu anlegen
			Console.WriteLine("Bereinige Bilderordner...");

			FileSystem.RemoveDirectory(temp, true);
			FileSystem.CreateDirectory(temp, true);
			FileSystem.CreateDirectory(tempFmMonsterSpreading, true);
			FileSystem.CreateDirectory(tempFmMusic, true);
			#endregion

			#region Bilder berechnen
			//int debug=0;
			
			foreach(string i in files)
			{
				Console.WriteLine("Überprüfe für Map {0} auf Aktualisierung...", FileSystem.GetFilename(i));

				GC.Collect(2);

				//Nur sichtbare Weltkarte rendern.
				if(!all)
				{
					Map map=new Map(0, FileSystem.GetFilenameWithoutExt(i));

					//if(map.MapType=="iw") continue;
					//if(map.MapType=="uw") continue;

					if(map.MapType=="ow")
					{
						if(map.X>=-7&&map.X<=7&&map.Y>=-7&&map.Y<=7)
						{
							//nichts tun (sprich rendern)
						}
						else
						{
							continue;
						}
					}
				}

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
				
				if(clearCache==false)
				{
					if(textHash==xmlHash) continue;
				}

				//Karte berechnen
				Console.WriteLine("Berechne Bilder für Map {0}", FileSystem.GetFilename(i));

				TMX file=new TMX(i);

				gtImage pic=file.Render();

				int imageSizeOriginalWidth=(int)pic.Width;
				int imageSizeOriginalHeight=(int)pic.Height;
				double imageVerhaeltnis=imageSizeOriginalWidth/(double)imageSizeOriginalHeight;

				//int imageSize=6400;
				int imageSize=800;
				pic=pic.Resize(imageSize, (int)(imageSize/imageVerhaeltnis));

				bool next=true;

				while(next)
				{
					string fn=FileSystem.GetFilenameWithoutExt(i);
					string fnNumeric=temp+fn+"-"+imageSize+".png";
					pic.SaveToFile(fnNumeric);

					//Featuremap Monster Spreading
					string fnMonsterSpreading=tempFmMonsterSpreading+fn+"-"+imageSize+".png";
					Imaging.SaveFeatureMapMonsterSpreading(Globals.folder_data, fnMonsterSpreading, pic, file);

					//Featuremap Music
					string fnMusic=tempFmMusic+fn+"-"+imageSize+".png";
					SaveFeatureMapMusic(fnMusic, pic, file);

					switch(imageSize)
					{
						//case 6400:
						//    {
						//        imageSize=1600;
						//        break;
						//    }
						//case 100:
						//    {
						//        //Minimap zusätzlich speichern
						//        string fnMinimap=Globals.folder_clientdata_graphics_minimaps+fn+".png";
						//        pic.SaveToFile(fnMinimap);
						//        break;
						//    }
						case 10:
							{
								next=false;
								break;
							}
					}

					imageSize=GetNextImageSize(imageSize);

					pic=pic.Resize(imageSize, (int)(imageSize/imageVerhaeltnis));
					GC.Collect(3);
				}
				
				//Debug
				//debug++;
				//if(debug>5) break;
			}
			#endregion

			#region Bilder per FTP hochladen
			List<string> filesToUpload=new List<string>();
			filesToUpload.AddRange(FileSystem.GetFiles(temp, true, "*.png"));
			
			Console.WriteLine("Lade {0} Dateien hoch...", filesToUpload.Count);

			FTPSClient Client=new FTPSClient();

			NetworkCredential networkCredential=new NetworkCredential();
			networkCredential.Domain=Globals.Options.GetElementAsString("xml.Options.FTP.Worldmap.Server");
			networkCredential.UserName=Globals.Options.GetElementAsString("xml.Options.FTP.Worldmap.User");
			networkCredential.Password=Globals.Options.GetElementAsString("xml.Options.FTP.Worldmap.Password");

			try
			{
				Client.Connect(networkCredential.Domain, networkCredential, ESSLSupportMode.ClearText);
			}
			catch(Exception exception)
			{
				Console.WriteLine("Fehler: {0}", exception.Message);
				return;
			}

			//Ordner für die Feature Maps
			if(!Client.Exists("fm-monster-spreading/ow-o0000-o0000-o0000-800.png")) //Monster Spreading
			{
				Client.CreateDirectory("fm-monster-spreading");
			}

			if(!Client.Exists("fm-music/ow-o0000-o0000-o0000-800.png")) //Music
			{
				Client.CreateDirectory("fm-music");
			}

			for(int i=0; i<filesToUpload.Count; i++)
			{
				string filename=filesToUpload[i];

				try
				{
					Console.WriteLine("Lade Bild {0} hoch...", FileSystem.GetFilename(filename));
					string uploadf=FileSystem.GetRelativePath(filename, temp);
					uploadf=uploadf.Replace('\\', '/');
					//Client.UploadFile(i, uploadf);
					Client.PutFile(filename, uploadf);
				}
				catch
				{
					Console.WriteLine("Fehler beim Upload von Bild {0}.", FileSystem.GetFilename(filename));
					Thread.Sleep(2000);
					Console.WriteLine("Stelle Verbindung wieder her...", FileSystem.GetFilename(filename));
					Client.Connect(networkCredential.Domain, networkCredential, ESSLSupportMode.ClearText);

					i--;
				}
			}

			Client.Close();
			#endregion

			Globals.Options.Save();
			Console.WriteLine("Weltkarte geupdatet");
		}
		#endregion

		#region RemoveBOMFromFile
		static void RemoveBOMFromFiles()
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

			Console.WriteLine("Alle BOMs wurden entfernt.");
		}
		#endregion

		#region Copy Images (Items, Monster)
		static void ExportItemImages(string target)
		{
			if(Globals.folder_root=="")
			{
				Console.WriteLine("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.");
				return;
			}

			string fnItemsXml=Globals.folder_data+"items.xml";

			List<Item> items=Item.GetItemsFromItemsXml(fnItemsXml);

			foreach(Item item in items)
			{
				if(item.ID<0) continue; //Unötige Items (Hairsets etc) ignorieren
				string itemImageName=item.Image.Split('|')[0].Trim();
				string itemfnSrc=Globals.folder_data_graphics_items+itemImageName;
				string itemfnDst=target+FileSystem.PathDelimiter+"Item-"+item.ID+".png";
				//File.Copy(itemfnSrc, itemfnDst, true);
				FileSystem.CopyFile(itemfnSrc, itemfnDst, true);
			}

			Console.WriteLine("Item Bilder wurden erfolgreich kopiert.");
		}

		static void ExportMonsterImages(string target)
		{
			if(Globals.folder_root=="")
			{
				Console.WriteLine("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.");
				return;
			}

			string fnMonstersXml=Globals.folder_data+"monsters.xml";
			List<Monster> monsters=Monster.GetMonstersFromMonsterXml(fnMonstersXml);

			foreach(Monster monster in monsters)
			{
				if(monster.ID>=50000) continue; //Testmonster ignorieren

				if(monster.Sprite!=null)
				{
					if(monster.Sprite!="")
					{
						string[] splited=monster.Sprite.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
						string spritePath=Globals.folder_data_graphics_sprites+splited[0];

						Sprite tmp=Sprite.GetSpriteFromXml(spritePath);

						Imageset set=tmp.Imagesets[0];
						string[] splited2=set.Src.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
						string srcname=splited2[0];
						gtImage setImage=gtImage.FromFile(Globals.folder_data+srcname);
						gtImage monsterImage=setImage.GetSubImage(0, 0, (uint)set.Width, (uint)set.Height);

						string monsterDst=target+FileSystem.PathDelimiter+"Monster-"+monster.ID+".png";
						monsterImage.SaveToPNGGDI(monsterDst);
					}
				}
			}

			Console.WriteLine("Monster Bilder wurden erfolgreich kopiert.");
		}

		static void ExportNPCSprites(string target)
		{
			if(Globals.folder_root=="")
			{
				Console.WriteLine("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.");
				return;
			}

			string fnNpcsXml=Globals.folder_data+"npcs.xml";
			List<Npc> npcs=Npc.GetNpcsFromXml(fnNpcsXml);

			foreach(Npc npc in npcs)
			{
				if(npc.SpriteFilename!=null)
				{
					if(npc.SpriteFilename!="")
					{
						string[] splited=npc.SpriteFilename.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
						string spritePath=Globals.folder_data_graphics_sprites+splited[0];

						Sprite tmp=Sprite.GetSpriteFromXml(spritePath);

						Imageset set=tmp.Imagesets[0];
						string[] splited2=set.Src.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
						string srcname=splited2[0];
						gtImage setImage=gtImage.FromFile(Globals.folder_data+srcname);
						gtImage monsterImage=setImage.GetSubImage(0, 0, (uint)set.Width, (uint)set.Height);

						string monsterDst=target+FileSystem.PathDelimiter+"npcsprite-"+npc.ID+".png";
						monsterImage.SaveToPNGGDI(monsterDst);
					}
				}
			}

			Console.WriteLine("NPC Sprites wurden erfolgreich kopiert.");
		}
		#endregion

		#region Tilesets
		static void GetTilesetsFromMapsUsed()
		{
			if(Globals.folder_root=="")
			{
				Console.WriteLine("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.");
				return;
			}

			List<string> maps=FileSystem.GetFiles(Globals.folder_data, true, "*.tmx");
			Dictionary<string, int> usedTilesets=new Dictionary<string, int>();

			foreach(string fn in maps)
			{
				Console.WriteLine("Überprüfe Map {0} auf Tilesets...", FileSystem.GetFilename(fn));

				TMX map=new TMX(fn, false);

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

			List<string> tilesets=new List<string>();

			foreach(string i in usedTilesets.Keys)
			{
				tilesets.Add(String.Format("{0} ({1} mal)", i, usedTilesets[i]));
			}

			tilesets.Sort();

			foreach(string i in tilesets)
			{
				Console.WriteLine(i);
			}
		}

		static void RemoveNonExistingTilesetsFromMaps()
		{
			if(Globals.folder_root=="")
			{
				Console.WriteLine("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.");
				return;
			}

			List<string> maps=FileSystem.GetFiles(Globals.folder_data, true, "*.tmx");
			int removedTilesets=0;

			foreach(string fnCurrent in maps)
			{
				TMX map=new TMX(fnCurrent, false);

				for(int i=0; i<map.Tilesets.Count; i++)
				{
					CSCL.FileFormats.TMX.TMX.TilesetData fnTileset=map.Tilesets[i];

					string cleanTileset=Globals.folder_data+fnTileset.imgsource.Replace("../graphics", "graphics");

					if(!FileSystem.ExistsFile(cleanTileset))
					{
						map.Tilesets.Remove(fnTileset);
						removedTilesets++;
					}
				}

				map.Save(fnCurrent);
			}

			Console.WriteLine("Es wurden "+removedTilesets+" fehlerhafte Tilesets korrigiert.");
		}

		static void RenameTilesetNameInMapsToTilesetFilename()
		{
			//Maps laden
			List<string> mapfiles=FileSystem.GetFiles(Globals.folder_data, true, "*.tmx");

			foreach(string i in mapfiles)
			{
				bool changed=false;

				TMX maptmx=new TMX(i);

				//Tiles transformieren
				foreach(TMX.TilesetData ld in maptmx.Tilesets)
				{
					string fnForTilesetName=FileSystem.GetFilenameWithoutExt(ld.imgsource);

					if(ld.name!=fnForTilesetName)
					{
						changed=true;
						ld.name=fnForTilesetName;
					}
				}

				if(changed)
				{
					//Map speichern
					maptmx.Save(i);
				}
			}
		}
		#endregion

		#region Mapping
		static void WriteExternalMapEvents(string fnMap)
		{
			TMX tmx=new TMX(fnMap);

			bool ExistDef=false;

			string fnLuaScript=String.Format("scripts/maps/{0}.lua", FileSystem.GetFilenameWithoutExt(fnMap));

			foreach(Objectgroup og in tmx.ObjectLayers)
			{
				foreach(CSCL.FileFormats.TMX.Object objk in og.Objects)
				{
					if(objk.Name.ToLower()=="external map events")
					{
						//name der Skriptdatei korrekt setzen
						foreach(Property prop in objk.Properties)
						{
							if(prop.Name.ToLower()=="filename")
							{
								prop.Value=fnLuaScript;
							}
						}

						ExistDef=true;
						break;
					}
				}
			}

			if(ExistDef==false)
			{
				Objectgroup objectGroup=null;

				if(tmx.ObjectLayers.Count==0)
				{
					tmx.ObjectLayers.Add(new Objectgroup("Object", 0, 0, 0, 0));
				}

				objectGroup=tmx.ObjectLayers[0];
				CSCL.FileFormats.TMX.Object mapObject=new CSCL.FileFormats.TMX.Object("External Map Events", "SCRIPT", 0, 0, 0, 0);
				mapObject.Properties.Add(new Property("FILENAME", fnLuaScript));
				objectGroup.Objects.Add(mapObject);
			}

			tmx.Save(fnMap);
		}

		static void CreateInnerMap(string mapFilename, string usedTemplateMap)
		{
			mapFilename=Globals.folder_data_maps+mapFilename;
			usedTemplateMap=Globals.folder_data_mapstemplates+usedTemplateMap;

			if(FileSystem.ExistsFile(mapFilename))
			{
				Console.WriteLine("Die Map {0} existiert bereits!", FileSystem.GetFilename(mapFilename));
				return;
			}

			FileSystem.CopyFile(usedTemplateMap, mapFilename);
			string luaFilename=String.Format("{0}{1}.lua", Globals.folder_data_scripts_maps, FileSystem.GetFilenameWithoutExt(mapFilename));
			Script.CreateMapScriptFile(luaFilename);

			//Map öffnen und External Map Events umschreiben
			WriteExternalMapEvents(mapFilename);

			//Maps.xml erweiteren
			string fnMapsXml=Globals.folder_data+"maps.xml";
			List<Map> maps=Map.GetMapsFromMapsXml(fnMapsXml);

			//LastID für iw Maps ermitteln
			int lastID=0;

			foreach(Map map in maps)
			{
				if(map.MapType=="iw")
				{
					if(lastID<map.ID) lastID=map.ID;
				}
			}

			maps.Add(new Map(++lastID, FileSystem.GetFilenameWithoutExt(mapFilename)));

			//maps.xml speichern
			Map.SaveToMapsXml(fnMapsXml, maps);
		}

		static void RemoveBlankTilesFromMaps()
		{
			//Maps laden
			List<string> mapfiles=FileSystem.GetFiles(Globals.folder_data, true, "*.tmx");

			foreach(string i in mapfiles)
			{
				bool changed=false;

				TMX maptmx=new TMX(i);

				//für jeden Layer
				foreach(TMX.LayerData ld in maptmx.Layers)
				{
					for(int y=0; y<ld.height; y++)
					{
						for(int x=0; x<ld.width; x++)
						{
							int TileNumber=ld.data[x, y];

							if(TileNumber==0) continue; //leeres Tile

							gtImage tile=maptmx.GetTile(TileNumber);
							Color median=tile.GetMedianColor();

							int summe=median.A+median.R+median.G+median.R;

							if(summe==0)
							{
								ld.data[x, y]=0;
								changed=true;
							}
						}
					}
				}

				if(changed)
				{
					//Map speichern
					maptmx.Save(i);
				}
			}

			Console.WriteLine("Leere Tiles wurden entfernt.");
		}

		static void CreateCollisionsOnMaps()
		{
			//Maps laden
			List<string> mapfiles=FileSystem.GetFiles(Globals.folder_data, true, "*.tmx");

			foreach(string filename in mapfiles)
			{
				TMX TestTMX=new TMX(filename);

				TMX.LayerData fringe=null;
				TMX.LayerData coll=null;

				foreach(TMX.LayerData layer in TestTMX.Layers)
				{
					switch(layer.name)
					{
						case "Fringe":
							{
								fringe=layer;
								break;
							}
						case "Collision":
							{
								coll=layer;
								break;
							}
					}
				}

				//CollID = 
				int CollID=coll.data[0, 0];
				bool FileChanged=false;

				for(int y=0; y<fringe.height; y++)
				{
					for(int x=0; x<fringe.width; x++)
					{
						int fieldData=fringe.data[x, y];
						if(fieldData==0) continue;

						TMX.TilesetData tInfo=TestTMX.GetTileset(fieldData);

						if(CollID==coll.data[x, y]) continue;

						switch(FileSystem.GetFilename(tInfo.imgsource))
						{
							case "wood1_32_96.png":
								{
									int realID=fieldData-tInfo.firstgid;

									switch(realID)
									{
										case 0: //Baum
										case 1: //Baum
										case 2: //Baum
										case 3: //Baum
										case 4: //Baum
										case 5: //Baum
											{
												FileChanged=true;
												coll.data[x, y]=CollID;
												break;
											}
									}

									break;
								}
							case "wood1_32_160.png":
								{
									int realID=fieldData-tInfo.firstgid;

									switch(realID)
									{
										case 1: //Baum
										case 4: //Baum
										case 7: //Baum
											{
												FileChanged=true;
												coll.data[x, y]=CollID;
												break;
											}
									}

									break;
								}
						}
					}
				}

				if(FileChanged) TestTMX.Save(filename);
			}

			Console.WriteLine("Vorgang beendet.");
		}
		#endregion

		#region Transformation
		//public partial class FormTileReplacer : Form
		//{
		//    public string Filename { get; set; }
		//    public int TileID { get; private set; }

		//    private void pbImage_MouseDoubleClick(object sender, MouseEventArgs e)
		//    {
		//        int xInTiles=e.X/32;
		//        int yInTiles=e.Y/32;

		//        int tileID=yInTiles*32+xInTiles;
		//        TileID=tileID;

		//        DialogResult=DialogResult.OK;
		//    }
		//}

		static void TransformTileInMaps(string srcTileset, string dstTileset, int src, int dst)
		{
			//Maps laden
			List<string> mapfiles=FileSystem.GetFiles(Globals.folder_data, true, "*.tmx");

			foreach(string i in mapfiles)
			{
				bool changed=false;

				TMX maptmx=new TMX(i);

				//schauen ob zieltileset vorhanden
				bool TargetTilesetExists=false;

				TMX.TilesetData destTileset=null;

				foreach(TMX.TilesetData td in maptmx.Tilesets)
				{
					if(td.imgsource.IndexOf(FileSystem.GetFilename(dstTileset))!=-1)
					{
						TargetTilesetExists=true;
						destTileset=td;
						break;
					}
				}

				//Tiles transformieren
				maptmx.RemoveGidsFromLayerData(); //RemoveGidFrom Tiles

				if(TargetTilesetExists==false)
				{
					//Tileset eintragen
					TMX.TilesetData tsData=new TMX.TilesetData();
					tsData.name=FileSystem.GetFilenameWithoutExt(dstTileset);
					tsData.imgsource="../graphics/tiles/"+FileSystem.GetFilename(dstTileset);
					tsData.tileheight=32;
					tsData.tilewidth=32;
					maptmx.Tilesets.Add(tsData);

					destTileset=tsData;
				}

				foreach(TMX.LayerData ld in maptmx.Layers)
				{
					for(int y=0; y<ld.height; y++)
					{
						for(int x=0; x<ld.width; x++)
						{
							TMX.TilesetData ts=ld.tilesetmap[x, y];
							int TileNumber=ld.data[x, y];

							if(ts.imgsource!=null)
							{
								if(ts.imgsource.IndexOf(FileSystem.GetFilename(srcTileset))!=-1)
								{
									if(TileNumber==src)
									{
										changed=true;
										ld.data[x, y]=dst;
										ld.tilesetmap[x, y]=destTileset;
									}
								}
							}
						}
					}
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

			Console.WriteLine("Tile wurde transformiert.");
		}
		#endregion

		#region Worldmap
		static void CreateWorldmapDatabaseSQLFile(string target)
		{
			if(Globals.folder_root=="")
			{
				Console.WriteLine("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.");
				return;
			}

			//Maps laden
			string fnMapsXml=Globals.folder_data+"maps.xml";
			List<Map> maps=Map.GetMapsFromMapsXml(fnMapsXml);

			//Ini
			List<string> sqlFile=new List<string>();
			sqlFile.Add("INSERT `wmInformation` (`MapID`, `FileName`, `Title`, `Music`) VALUES");

			//maps
			foreach(Map i in maps)
			{
				WebClient client=new WebClient();
				string infourl=String.Format("http://weltkarte.invertika.org/info.php?onlytext=1&fn={0}", i.Name);
				byte[] padData=client.DownloadData(new Uri(infourl));

				if(padData.Length==0)
				{
					sqlFile.Add(String.Format("('{0}', '{1}', '{2}', '{3}'),", i.ID, i.Name, "kein Name vergeben", "keine Musikdatei angegeben"));
				}
			}

			sqlFile[sqlFile.Count-1]=sqlFile[sqlFile.Count-1].TrimEnd(',')+";";

			File.WriteAllLines(target, sqlFile.ToArray());

			Console.WriteLine("Weltkarten DB SQL Datei geschrieben.");
		}

		static void UpdateWorldmapDatabaseSQLFile(string target)
		{
			if(Globals.folder_root=="")
			{
				Console.WriteLine("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.");
				return;
			}

			//Maps laden
			string fnMapsXml=Globals.folder_data+"maps.xml";
			List<Map> maps=Map.GetMapsFromMapsXml(fnMapsXml);

			//Ini
			List<string> sqlFile=new List<string>();

			//maps
			foreach(Map i in maps)
			{
				string fnMap=Globals.folder_data_maps+i.Name+".tmx";
				TMX tmx=new TMX(fnMap, false);

				Property p=tmx.GetProperty("music");
				if(p==null)
				{
					sqlFile.Add(String.Format("UPDATE wmInformation SET Music = '{0}' WHERE FileName LIKE \"%{1}%\";", "keine Musikdatei angegeben", i.Name));
				}
				else
				{
					sqlFile.Add(String.Format("UPDATE wmInformation SET Music = '{0}' WHERE FileName LIKE \"%{1}%\";", p.Value, i.Name));
				}
			}

			File.WriteAllLines(target, sqlFile.ToArray());

			Console.WriteLine("Weltkarten DB SQL Datei geschrieben.");
		}
		#endregion

		#region LUA MediaWiki
		static void UpdateLuaInMediaWiki()
		{
			if(Globals.folder_root=="")
			{
				Console.WriteLine("Bitte geben sie in den Optionen den Pfad zum Invertika Repository an.");
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL")=="")
			{
				Console.WriteLine("Bitte geben sie eine Mediawiki URL in den Optionen an.");
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username")=="")
			{
				Console.WriteLine("Bitte geben sie einen Mediawiki Nutzernamen in den Optionen an.");
				return;
			}

			if(Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort")=="")
			{
				Console.WriteLine("Bitte geben sie einen Mediawiki Passwort in den Optionen an.");
				return;
			}

			string url=Globals.Options.GetElementAsString("xml.Options.Mediawiki.URL");
			string username=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Username");
			string password=Globals.Options.GetElementAsString("xml.Options.Mediawiki.Passwort");

			Site wiki=new Site(url, username, password);

			List<string> luafiles=FileSystem.GetFiles(Globals.folder_data_scripts_libs, true, "*.lua");

			foreach(string file in luafiles)
			{
				LuaDocParser ldp=new LuaDocParser(file);
				LucDocReturn ret=ldp.ExportLuaDocToMediaWiki();

				switch(ret.DocType)
				{
					case LuaDocType.Module:
						{
							Page page=new Page(wiki, ret.Name+" (Lua Modul)");

							page.Load();

							string text=page.text;

							if(text=="")
							{
								List<string> lines=new List<string>();

								lines.Add("{{Status_Green}}");
								lines.Add("{{Automatic}}");

								lines.Add("");

								lines.Add("==Funktionen==");

								//Funktions
								lines.Add("{{Anker|AutomaticStartFunctions}}");
								lines.AddRange(ret.Functions);
								lines.Add("{{Anker|AutomaticEndFunctions}}");
								lines.Add("");
								lines.Add("[[Kategorie: Lua]]");
								lines.Add("[[Kategorie: Lua Modul]]");

								foreach(string ll in lines)
								{
									text+=ll+"\n";
								}

								if(page.text!=text)
								{
									page.text=text;
									page.Save("Sourcecode Dokumentation erstellt.", false);
								}
							}
							else //Entsprechende Bereiche ersetzen
							{
								string start="{{Anker|AutomaticStartFunctions}}";
								string end="{Anker|AutomaticEndFunctions}}";
								int idxBeginInfobox=text.IndexOf(start, 0);
								int idxEndInfobox=text.IndexOf(end, 0);

								int lengthOfString=(idxEndInfobox-idxBeginInfobox)-start.Length-1;
								string vorkommen=text.Substring(idxBeginInfobox+start.Length, lengthOfString);

								if(vorkommen!="\n")
								{
									text=text.Replace(vorkommen, "");
								}

								string replaceString="{{Anker|AutomaticStartFunctions}}\n";

								foreach(string ll in ret.Functions)
								{
									replaceString+=ll+"\n";
								}

								text=text.Replace(start, replaceString);

								if(page.text!=text)
								{
									page.text=text;
									page.Save("Sourcecode Dokumentation aktualisiert.", true);
								}
							}
							break;
						}
					default:
						{
							break;
						}
				}

			}

			Console.WriteLine("Lua Dokumentation aktualisiert.");
		}
		#endregion

		static List<string> GetFilesFromParameters(Parameters param)
		{
			List<string> ret=new List<string>();

			foreach(string i in param.GetNames())
			{
				if(i.StartsWith("file"))
				{
					ret.Add(param.GetString(i));
				}
			}

			return ret;
		}

		static void Main(string[] args)
		{		
			//Optionen
			bool ExitsConfig=FileSystem.ExistsFile(Globals.OptionsXmlFilename);
			
			if(ExitsConfig)
			{
				try
				{
					Console.WriteLine("Lade Konfiguration: {0}", Globals.OptionsXmlFilename);
					Globals.Options=new XmlData(Globals.OptionsXmlFilename);
				}
				catch(Exception ex)
				{
					Console.WriteLine("Fehler bei der Auswertung der Konfigurationsdatei:");
					Console.WriteLine(ex.Message);
					return;
				}
			}
			else
			{
				Console.WriteLine("Es existiert keine Konfigurationsdatei.");
			}

			//Parameter auswerten
			Parameters parameters=null;

			try
			{
				parameters=Parameters.InterpretCommandLine(args);
			}
			catch
			{
				Console.WriteLine("Parameter wurden nicht erkannt!");
				Console.WriteLine("");
				DisplayHelp();
				return;
			}

			Console.WriteLine("Starte Aktion am {0} um {1} Uhr...", DateTime.Now.Date.ToShortDateString(), DateTime.Now.ToShortTimeString());

			//Aktion starten
			if(parameters.GetBool("calcAdler32"))
			{
				List<string> files=GetFilesFromParameters(parameters);

				if(files.Count==0) Console.WriteLine("Kein Dateiname angegeben!");
				else
				{
					foreach(string file in files)
					{
						CalcAdler32(file);
					}
				}
			}
			else if(parameters.GetBool("check"))
			{
				Check();
			}
			else if(parameters.GetBool("checkNPCsOnWiki"))
			{
				CheckNPCsOnWiki();
			}
			else if(parameters.GetBool("createClientUpdate"))
			{
				List<string> files=GetFilesFromParameters(parameters);

				if(files.Count!=2) Console.WriteLine("Keine Pfade angegeben!");
				else
				{
					string folderLastFullClient=files[0];
					string folderUpdateTarget=files[1];

					CreateClientUpdate(folderLastFullClient, folderUpdateTarget);
				}
			}
			else if(parameters.GetBool("createCollisionsOnMaps"))
			{
				CreateCollisionsOnMaps();
			}
			else if(parameters.GetBool("createDataFolder"))
			{
				List<string> paths=GetFilesFromParameters(parameters);

				if(paths.Count==0) Console.WriteLine("Keine Pfad(e) angegeben!");
				else
				{
					foreach(string path in paths)
					{
						CreateDataFolder(path);
					}
				}
			}
			else if(parameters.GetBool("createExampleConfig"))
			{
				CreateExampleConfig();
			}
			else if(parameters.GetBool("createInnerMap"))
			{
				List<string> files=GetFilesFromParameters(parameters);

				if(files.Count!=2) Console.WriteLine("Parameter inkorrekt!");
				else
				{
					string mapFilename=files[0];
					string usedTemplateMap=files[1];

					CreateInnerMap(mapFilename, usedTemplateMap);
				}
			}
			else if(parameters.GetBool("createMapScriptsAndUpdateMaps"))
			{
				CreateMapScriptsAndUpdateMaps();
			}
			else if(parameters.GetBool("createWorldmapDatabaseSQLFile"))
			{
				List<string> files=GetFilesFromParameters(parameters);

				if(files.Count==0) Console.WriteLine("Keine Datei(en) angegeben!");
				else
				{
					foreach(string file in files)
					{
						CreateWorldmapDatabaseSQLFile(file);
					}
				}
			}
			else if(parameters.GetBool("exportItemImages"))
			{
				List<string> paths=GetFilesFromParameters(parameters);

				if(paths.Count==0) Console.WriteLine("Keine Pfad(e) angegeben!");
				else
				{
					foreach(string path in paths)
					{
						ExportItemImages(path);
					}
				}
			}
			else if(parameters.GetBool("exportMonsterImages"))
			{
				List<string> paths=GetFilesFromParameters(parameters);

				if(paths.Count==0) Console.WriteLine("Keine Pfad(e) angegeben!");
				else
				{
					foreach(string path in paths)
					{
						ExportMonsterImages(path);
					}
				}
			}
			else if(parameters.GetBool("exportNPCSprites"))
			{
				List<string> paths=GetFilesFromParameters(parameters);

				if(paths.Count==0) Console.WriteLine("Keine Pfad(e) angegeben!");
				else
				{
					foreach(string path in paths)
					{
						ExportNPCSprites(path);
					}
				}
			}
			else if(parameters.GetBool("getMonstersOnMap"))
			{
				GetMonstersOnMap();
			}
			else if(parameters.GetBool("getTilesetsFromMapsUsed"))
			{
				GetTilesetsFromMapsUsed();
			}
			else if(parameters.GetBool("removeBlankTilesFromMaps"))
			{
				RemoveBlankTilesFromMaps();
			}
			else if(parameters.GetBool("removeBomFromFiles"))
			{
				RemoveBOMFromFiles();
			}
			else if(parameters.GetBool("removeNonExistingTilesetsFromMaps"))
			{
				RemoveNonExistingTilesetsFromMaps();
			}
			else if(parameters.GetBool("renameTileset"))
			{
				List<string> tilesets=GetFilesFromParameters(parameters);

				if(tilesets.Count!=2) Console.WriteLine("Parameter inkorrekt angegeben.");
				else
				{
					string oldName=tilesets[0];
					string newName=tilesets[1];
					RenameTileset(oldName, newName);
				}
			}
			else if(parameters.GetBool("renameTilesetNameInMapsToTilesetFilename"))
			{
				RenameTilesetNameInMapsToTilesetFilename();
			}
			else if(parameters.GetBool("renderTMX"))
			{
				List<string> files=GetFilesFromParameters(parameters);

				string output=parameters.GetString("output", "");
				double zoom=parameters.GetDouble("zoom", 100);

				if(output=="") Console.WriteLine("Keinen Ausgabepfad angegeben!");
				else
				{
					foreach(string file in files)
					{
						string fileRepository=Globals.folder_data_maps+file;
						
						if(FileSystem.ExistsFile(fileRepository)) RenderTMX(fileRepository, output, zoom); //Rendern für den Fall das es im Repository vorhanden ist
						else RenderTMX(file, output, zoom); //Rendern für den Fall das eine nicht Repositorydatei gerendert werden soll
							
						GC.Collect(3);
					}
				}
			}
			else if(parameters.GetBool("transformTileInMaps"))
			{
				string srcTileset=parameters.GetString("srcTileset", "");
				string dstTileset=parameters.GetString("dstTileset", "");

				string srcTile=parameters.GetString("srcTile", "");
				string dstTile=parameters.GetString("dstTile", "");

				if(srcTileset==""||srcTileset=="") Console.WriteLine("Kein Tileset angegeben!");
				if(srcTile==""||dstTile=="") Console.WriteLine("Keine Tiles angegeben!");

				TransformTileInMaps(srcTileset, dstTileset, Convert.ToInt32(srcTile), Convert.ToInt32(dstTile));
			}
			else if(parameters.GetBool("update"))
			{
				bool all=parameters.GetBool("all", false);
				bool clearCache=parameters.GetBool("clearCache", false);

				UpdateMinimaps(all, clearCache);
				UpdateWorldmap(all, clearCache);
				UpdateMediaWiki();
			}
			else if(parameters.GetBool("updateMapsInMapsXml"))
			{
				UpdateMapsInMapsXml();
			}
			else if(parameters.GetBool("updateMinimaps"))
			{
				bool all=parameters.GetBool("all", false);
				bool clearCache=parameters.GetBool("clearCache", false);
				UpdateMinimaps(all, clearCache);
			}
			else if(parameters.GetBool("updateMediaWiki"))
			{
				UpdateMediaWiki();
			}
			else if(parameters.GetBool("updateLuaInMediaWiki"))
			{
				UpdateLuaInMediaWiki();
			}
			else if(parameters.GetBool("updateWorldmap"))
			{
				bool all=parameters.GetBool("all", false);
				bool clearCache=parameters.GetBool("clearCache", false);
				UpdateWorldmap(all, clearCache);
			}
			else if(parameters.GetBool("updateWorldmapDatabaseSQLFile"))
			{
				List<string> files=GetFilesFromParameters(parameters);

				if(files.Count==0) Console.WriteLine("Keine Datei(en) angegeben!");
				else
				{
					foreach(string file in files)
					{
						UpdateWorldmapDatabaseSQLFile(file);
					}
				}
			}
			else
			{
				DisplayHelp();
			}

			Console.WriteLine("Beende Aktion am {0} um {1} Uhr...", DateTime.Now.Date.ToShortDateString(), DateTime.Now.ToShortTimeString());
		}
	}
}
