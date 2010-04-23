using System;
using System.Collections.Generic;
using System.Text;
using CSCL;

namespace Invertika_Editor
{
	public static class Globals
	{
		public static XmlData Options;
		public static string OptionsDirectory=FileSystem.ApplicationDataDirectory+".invertika.org\\Invertika Editor\\";
		public static string OptionsXmlFilename=OptionsDirectory+"Invertika Editor.xml";

		public static string folder_client
		{
			get
			{
				return FileSystem.GetPathWithPathDelimiter(Globals.Options.GetElementAsString("xml.Options.Paths.Repository.Trunk"))+"client"+FileSystem.PathDelimiter;
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
				return FileSystem.GetPathWithPathDelimiter(Globals.Options.GetElementAsString("xml.Options.Paths.Repository.Trunk"))+"server-data"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_serverdata_scripts
		{
			get
			{
				return folder_serverdata+"scripts"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_clientdata
		{
			get
			{
				return FileSystem.GetPathWithPathDelimiter(Globals.Options.GetElementAsString("xml.Options.Paths.Repository.Trunk"))+"client-data"+FileSystem.PathDelimiter;
			}
		}

		public static string folder_clientdata_maps
		{
			get
			{
				return folder_clientdata+"maps"+FileSystem.PathDelimiter;
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
	}
}
