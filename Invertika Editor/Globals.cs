using System;
using System.Collections.Generic;
using System.Text;
using CSCL;
using System.IO;
using CSCL.Games.Manasource;
using Invertika_Editor.Classes;
using CSCL.FileFormats.TMX;

namespace Invertika_Editor
{
	public static class Globals
	{
		public static XmlData Options;
		public static string OptionsDirectory=FileSystem.ApplicationDataDirectory+".invertika.org\\Invertika Editor\\";
		public static string OptionsXmlFilename=OptionsDirectory+"Invertika Editor.xml";

		#region Eigenschaften
		public static string folder_root
		{
			get
			{
				return FileSystem.GetPathWithPathDelimiter(Globals.Options.GetElementAsString("xml.Options.Paths.Repository.Trunk"));
			}
		}

		public static string folder_client
		{
			get
			{
				return folder_root+"client"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_client_data
		{
			get
			{
				return folder_client+"data"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_client_data_fonts
		{
			get
			{
				return folder_client_data+"fonts"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_client_data_graphics
		{
			get
			{
				return folder_client_data+"graphics"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_client_data_help
		{
			get
			{
				return folder_client_data+"help"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_client_data_icons
		{
			get
			{
				return folder_client_data+"icons"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_serverdata
		{
			get
			{
				return Globals.folder_root+"server-data"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_serverdata_scripts
		{
			get
			{
				return folder_serverdata+"scripts"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_serverdata_scripts_libs
		{
			get
			{
				return folder_serverdata_scripts+"libs"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_serverdata_scripts_maps
		{
			get
			{
				return folder_serverdata_scripts+"maps"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_clientdata
		{
			get
			{
				return Globals.folder_root+"client-data"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_clientdata_maps
		{
			get
			{
				return folder_clientdata+"maps"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_clientdata_graphics_items
		{
			get
			{
				return folder_clientdata_graphics+"items"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_clientdata_graphics_sprites
		{
			get
			{
				return folder_clientdata_graphics+"sprites"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_clientdata_graphics_particles
		{
			get
			{
				return folder_clientdata_graphics+"particles"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_clientdata_mapstemplates
		{
			get
			{
				return folder_clientdata+"maps_templates"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_clientdata_music
		{
			get
			{
				return folder_clientdata+"music"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_clientdata_sfx
		{
			get
			{
				return folder_clientdata+"sfx"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_clientdata_graphics
		{
			get
			{
				return folder_clientdata+"graphics"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_clientdata_graphics_minimaps
		{
			get
			{
				return folder_clientdata_graphics+"minimaps"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_clientdata_graphics_tiles
		{
			get
			{
				return folder_clientdata_graphics+"tiles"+FileSystem.PathDelimiter;
			}
		}
		#endregion

		#region Methoden
		public static void CreateWorldmapHTML(string filename, int xmin, int xmax, int ymin, int ymax, int vfactor, bool printmap)
		{
			StreamWriter swWorldMap=new StreamWriter(filename);

			swWorldMap.WriteLine("<html>");
			swWorldMap.WriteLine(" <head>");
			swWorldMap.WriteLine("  <title>Weltkarte</title>");
			swWorldMap.WriteLine(" </head>");
			swWorldMap.WriteLine(" <body>");

			if(printmap) swWorldMap.WriteLine("  <table border=\"1\" cellspacing=\"0\" cellpadding=\"0\">");
			else swWorldMap.WriteLine("  <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");

			int MapX=xmin;
			int MapY=ymax;

			while(MapY>=ymin)
			{
				swWorldMap.WriteLine("   <tr>");

				MapX=xmin;

				while(MapX<=xmax)
				{
					swWorldMap.WriteLine("    <td>");
					swWorldMap.WriteLine("     <img src=\"http://data.invertika.org/worldmap/{0}-{1}.png\">", Map.GetOuterWorldMapFilenameWithoutExtension(MapX, MapY, 0), vfactor);
					swWorldMap.WriteLine("    </td>");

					MapX++;
				}

				swWorldMap.WriteLine("   </tr>");

				MapY--;
			}

			swWorldMap.WriteLine("  </table>");
			swWorldMap.WriteLine(" </body>");
			swWorldMap.WriteLine("</html>");

			swWorldMap.Close();
		}

		public static void CreateWorldmapMediaWiki(string filename, int xmin, int xmax, int ymin, int ymax, List<Map> maps)
		{
			int MapX=xmin;
			int MapY=ymax;

			StreamWriter swWorldMap=new StreamWriter(filename);

			swWorldMap.WriteLine("{|  border=\"1\" cellpadding=\"2\" cellspacing=\"2\" style=\"table-layout:fixed;width:100px\"");
			swWorldMap.WriteLine("! width=\"25\" valign=\"top\" |X/Y");

			while(MapX<=xmax)
			{
				swWorldMap.WriteLine("! width=\"100\" valign=\"top\" |{0}", MapX);
				MapX++;
			}

			MapX=xmin;

			while(MapY>=ymin)
			{
				swWorldMap.WriteLine("|-");
				swWorldMap.WriteLine("! {0}", MapY);

				MapX=xmin;

				while(MapX<=xmax)
				{
					string mapname=Map.GetOuterWorldMapFilenameWithoutExtension(MapX, MapY, 0);
					int mapID=0;

					foreach(Map i in maps)
					{
						if(i.Name==mapname)
						{
							mapID=i.ID;
							break;
						}
					}

					swWorldMap.WriteLine("| valign=\"top\" | http://data.invertika.org/worldmap/{0}-100.png [[{0}|Ozean]] [http://data.invertika.org/worldmap/{0}-1400.png (+)] ({1})", Map.GetOuterWorldMapFilenameWithoutExtension(MapX, MapY, 0), mapID);

					MapX++;
				}

				MapY--;
			}

			swWorldMap.WriteLine("|}");

			swWorldMap.Close();
		}

		public static void CreateMySQLScript(string filename, List<Map> maps)
		{
			StreamWriter sw=new StreamWriter(filename);

			//Tabellen Erzeugen
			string CreateSQL="CREATE TABLE IF NOT EXISTS `wmInformation` ("
				+"`IndexID` int(11) NOT NULL AUTO_INCREMENT,"
				+"`MapID` text character set utf8 collate utf8_unicode_ci NOT NULL,"
				+"`FileName` text character set utf8 collate utf8_unicode_ci NOT NULL,"
				+"`Title` text character set utf8 collate utf8_unicode_ci NOT NULL,"
				+"PRIMARY KEY  (`IndexID`)"
				+") ENGINE=MyISAM DEFAULT CHARSET=utf8;";

			sw.WriteLine(CreateSQL);
			sw.WriteLine();

			//Daten reinschreiben

			foreach(Map i in maps)
			{
				if(i.MapType=="ow")
				{
					string InsertSQL=String.Format("INSERT INTO `wmInformation` (`MapID`, `FileName`, `Title`) VALUES ({0}, '{1}', 'Ozean');", i.ID, i.Name);
					sw.WriteLine(InsertSQL);
				}
			}


			//READY.
			sw.Close();
		}

		public static void CreateMapScriptFile(string filename)
		{
			CreateMapScriptFile(filename, 0, 0, 0, 0, false);
		}

		public static void CreateMapScriptFile(string filename, int MapUp, int MapRight, int MapDown, int MapLeft, bool usewarp)
		{
			StreamWriter sw=new StreamWriter(filename);

			sw.WriteLine("----------------------------------------------------------------------------------");
			sw.WriteLine("-- Map File                                                                     --");
			sw.WriteLine("--                                                                              --");
			sw.WriteLine("-- In dieser Datei stehen die entsprechenden externen NPC's, Trigger und        --");
			sw.WriteLine("-- anderer Dinge.                                                               --");
			sw.WriteLine("--                                                                              --");
			sw.WriteLine("----------------------------------------------------------------------------------");
			sw.WriteLine("--  Copyright 2008-2010 The Invertika Development Team                          --");
			sw.WriteLine("--                                                                              --");
			sw.WriteLine("--  This file is part of Invertika.                                             --");
			sw.WriteLine("--                                                                              --");
			sw.WriteLine("--  Invertika is free software; you can redistribute it and/or modify it        --");
			sw.WriteLine("--  under the terms of the GNU General  Public License as published by the Free --");
			sw.WriteLine("--  Software Foundation; either version 2 of the License, or any later version. --");
			sw.WriteLine("----------------------------------------------------------------------------------");
			sw.WriteLine("\n");
			sw.WriteLine("require \"scripts/lua/npclib\"");
			sw.WriteLine("\n");

			if(usewarp)
			{
				sw.WriteLine("dofile(\"data/scripts/ivklibs/warp.lua\")");
				sw.WriteLine("");
			}

			sw.WriteLine("atinit(function()");

			if(usewarp)
			{
				sw.WriteLine(" create_inter_map_warp_trigger({0}, {1}, {2}, {3}) --- Intermap warp", MapUp, MapRight, MapDown, MapLeft);
			}

			sw.WriteLine("end)");

			sw.Close();
		}

		public static List<MonsterSpawn> GetMonsterSpawnFromMap(string filename)
		{
			TMX map=new TMX();
			map.Open(filename, false);
			return GetMonsterSpawnFromMap(map);
		}

		public static List<MonsterSpawn> GetMonsterSpawnFromMap(TMX map)
		{
			List<MonsterSpawn> ret=new List<MonsterSpawn>();

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
		#endregion
	}
}
