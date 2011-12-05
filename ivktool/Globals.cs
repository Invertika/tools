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
				if(Globals.Options==null)
				{
					return "";
				}
				else
				{
					return FileSystem.GetPathWithPathDelimiter(Globals.Options.GetElementAsString("xml.Options.Paths.Repository.Trunk"));
				}
			}
		}

		#region Repository client
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
		#endregion

		#region Repository data
		public static string folder_data
		{
			get
			{
				return Globals.folder_root+"data"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_data_scripts
		{
			get
			{
				return folder_data+"scripts"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_data_scripts_libs
		{
			get
			{
				return folder_data_scripts+"libs"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_data_scripts_maps
		{
			get
			{
				return folder_data_scripts+"maps"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_data_maps
		{
			get
			{
				return folder_data+"maps"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_data_graphics_items
		{
			get
			{
				return folder_data_graphics+"items"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_data_graphics_sprites
		{
			get
			{
				return folder_data_graphics+"sprites"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_data_graphics_particles
		{
			get
			{
				return folder_data_graphics+"particles"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_data_mapstemplates
		{
			get
			{
				return folder_data+"maps_templates"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_data_mapsrules
		{
			get
			{
				return folder_data+"maps_rules"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_data_music
		{
			get
			{
				return folder_data+"music"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_data_sfx
		{
			get
			{
				return folder_data+"sfx"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_data_graphics
		{
			get
			{
				return folder_data+"graphics"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_data_graphics_minimaps
		{
			get
			{
				return folder_data_graphics+"minimaps"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_data_graphics_tiles
		{
			get
			{
				return folder_data_graphics+"tiles"+FileSystem.PathDelimiter;
			}
		}
		#endregion
		#endregion
	}
}
