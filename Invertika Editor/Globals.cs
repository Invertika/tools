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

		public static string folder_clientdata_mapsrules
		{
			get
			{
				return folder_clientdata+"maps_rules"+FileSystem.PathDelimiter;
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

		
		#endregion
	}
}
