using System;
using System.Collections.Generic;
using System.Text;
using CSCL;
using System.IO;
using CSCL.Games.Manasource;

namespace createworldmaps
{
	class Program
	{
		static void Main(string[] args)
		{
			Parameters cmdLine=Parameters.InterpretCommandLine(args);

			//Parameter auswerten
			string error="";
			if(!cmdLine.Contains("file000")) { error="Kein maps.xml Dateiname angegeben!"; }
			else if(!cmdLine.Contains("file001")) { error="Kein Zielpfad angegeben!"; }

			if(error!="")
			{
				Console.WriteLine(error);
				Console.WriteLine("");
				Console.WriteLine("Benutzung: createworldmaps.exe <maps.xml> <Zielpfad>");
				Console.WriteLine(@"Beispiel: createworldmaps.exe D:\invertika.googlecode.com\server-data\maps.xml D:\Output");
				return;
			}

			string fnMapsXml=cmdLine.GetString("file000");
			string pathOutput=FileSystem.GetPathWithBackslash(cmdLine.GetString("file001"));

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
			CreateWorldmapHTML(pathOutput+"weltkarte.html", xmin, xmax, ymin, ymax, 100, false);
			CreateWorldmapHTML(pathOutput+"weltkarte-small.html", xmin, xmax, ymin, ymax, 50, false);
			CreateWorldmapHTML(pathOutput+"weltkarte-print.html", xmin, xmax, ymin, ymax, 100, true);
			CreateWorldmapHTML(pathOutput+"weltkarte-big.html", xmin, xmax, ymin, ymax, 1400, false);

			CreateWorldmapMediaWiki(pathOutput+"weltkarte.mediawiki", xmin, xmax, ymin, ymax, maps);

			CreateMySQLScript(pathOutput+"weltkarte.sql", maps);
			return;
		}

		static void CreateWorldmapHTML(string filename, int xmin, int xmax, int ymin, int ymax, int vfactor, bool printmap)
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

		static void CreateWorldmapMediaWiki(string filename, int xmin, int xmax, int ymin, int ymax, List<Map> maps)
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

		static void CreateMySQLScript(string filename, List<Map> maps)
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
	}
}
