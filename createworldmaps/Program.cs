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

			//Kartenerzeugen
			CreateWorldmapHTML(pathOutput+"worldmap.html", xmin, xmax, ymin, ymax, 100, false);
			CreateWorldmapHTML(pathOutput+"worldmap-print.html", xmin, xmax, ymin, ymax, 100, true);
			CreateWorldmapHTML(pathOutput+"worldmap-big.html", xmin, xmax, ymin, ymax, 1400, false);

			CreateWorldmapMediaWiki(pathOutput+"worldmap.mediawiki", xmin, xmax, ymin, ymax, maps);
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

			swWorldMap.WriteLine("{| border=\"1\" cellpadding=\"2\" cellspacing=\"2\"");
			swWorldMap.WriteLine("! width=\"100px\" align=\"center\" | X/Y");

			while(MapX<=xmax)
			{
				swWorldMap.WriteLine("! width=\"100px\" align=\"center\" | {0}", MapX);
				MapX++;
			}

			MapX=xmin;

			while(MapY>=ymin)
			{
				swWorldMap.WriteLine("|-");
				swWorldMap.WriteLine("! align=\"center\" | {0}", MapY);

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

					swWorldMap.WriteLine("| http://data.invertika.org/worldmap/{0}-100.png [http://data.invertika.org/worldmap/{0}-1400.png Küste] ({1})", Map.GetOuterWorldMapFilenameWithoutExtension(MapX, MapY, 0), mapID);

					MapX++;
				}

				MapY--;
			}

			swWorldMap.WriteLine("|}");

			swWorldMap.Close();
		}
	}
}
