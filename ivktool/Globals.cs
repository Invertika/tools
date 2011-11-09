using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCL;

namespace ivktool
{
	public class Globals
	{
		public static XmlData Options;
		public static string OptionsXmlFilename=FileSystem.ApplicationPath+"ivktool.xml";

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
	}
}
