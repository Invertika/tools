using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CSCL.Games.Manasource;

namespace Invertika
{
	public static class Worldmap
	{
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

	}
}
