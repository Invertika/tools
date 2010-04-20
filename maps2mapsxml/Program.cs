using System;
using System.Collections.Generic;
using System.Text;
using CSCL;
using CSCL.Games.Manasource;

namespace maps2mapsxml
{
	class Program
	{
		static void Main(string[] args)
		{
			Parameters cmdLine=Parameters.InterpretCommandLine(args);

			//Parameter auswerten
			string error="";
			if(!cmdLine.Contains("file000")) { error="Kein maps.xml Dateiname angegeben!"; }
			else if(!cmdLine.Contains("file001")) { error="Kein Maps Pfad angegeben!"; }

			if(error!="")
			{
				Console.WriteLine(error);
				Console.WriteLine("");
				Console.WriteLine("Benutzung: maps2mapsxml.exe <maps.xml> <Maps Pfad>");
				Console.WriteLine(@"Beispiel: maps2mapsxml.exe D:\invertika.googlecode.com\server-data\maps.xml D:\invertika.googlecode.com\client-data\maps");
				return;
			}

			string fnMapsXml=cmdLine.GetString("file000");
			string pathMaps=FileSystem.GetPathWithPathDelimiter(cmdLine.GetString("file001"));

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
		}
	}
}
