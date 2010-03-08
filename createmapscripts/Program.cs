using System;
using System.Collections.Generic;
using System.Text;
using CSCL;
using System.IO;
using CSCL.Games.Manasource;
using System.Xml;

namespace createmapscripts
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
			else if(!cmdLine.Contains("file002")) { error="Kein Zielpfad angegeben!"; }

			if(error!="")
			{
				Console.WriteLine(error);
				Console.WriteLine("");
				Console.WriteLine("Benutzung: maps2mapsxml.exe <maps.xml> <Maps Pfad>");
				Console.WriteLine(@"Beispiel: maps2mapsxml.exe D:\invertika.googlecode.com\server-data\maps.xml D:\invertika.googlecode.com\client-data\maps D:\Output");
				return;
			}

			string fnMapsXml=cmdLine.GetString("file000");
			string pathMaps=FileSystem.GetPathWithBackslash(cmdLine.GetString("file001"));
			string pathOutput=FileSystem.GetPathWithBackslash(cmdLine.GetString("file002"));

			//Maps laden
			List<Map> maps=Map.GetMapsFromMapsXml(fnMapsXml);

			foreach(Map i in maps)
			{
				string fnMap=String.Format("{0}{1}.tmx", pathMaps, i.Name);

				bool ExistDef=false;

				if(FileSystem.Exists(fnMap))
				{
					XmlData mapAsXml=new XmlData(fnMap);

					List<XmlNode> nodes=mapAsXml.GetElements("map.objectgroup.object");

					foreach(XmlNode node in nodes)
					{
						string name=mapAsXml.GetAttributeAsString(node, "name");

						if(name.ToLower()=="external map events")
						{
							ExistDef=true;
							break;
						}
					}

					if(ExistDef==false)
					{
						//Definition eintragen
						List<XmlNode> mapnodes=mapAsXml.GetElements("map");
						XmlNode mapnode=null;

						foreach(XmlNode xmlnode in mapnodes)
						{
							if(xmlnode.Name.ToLower()=="map")
							{
								mapnode=xmlnode;
							}
						}
							
							//mapnodes[0];

						XmlNode objectgroup=mapAsXml.AddElement(mapnode, "objectgroup", "");
						mapAsXml.AddAttribute(objectgroup, "name", "Object");
						mapAsXml.AddAttribute(objectgroup, "width", 70);
						mapAsXml.AddAttribute(objectgroup, "height", 70);
						mapAsXml.AddAttribute(objectgroup, "x", 70);
						mapAsXml.AddAttribute(objectgroup, "y", 70);

						XmlNode @object=mapAsXml.AddElement(objectgroup, "object", "");
						mapAsXml.AddAttribute(@object, "name", "External Map Events");
						mapAsXml.AddAttribute(@object, "type", "SCRIPT");
						mapAsXml.AddAttribute(@object, "x", 0);
						mapAsXml.AddAttribute(@object, "y", 0);

						XmlNode properties=mapAsXml.AddElement(@object, "properties", "");
						XmlNode property=mapAsXml.AddElement(properties, "property", "");
						mapAsXml.AddAttribute(property, "name", "FILENAME");

						string fnLuaScript=String.Format("scripts/maps/{0}.lua", i.Name);
						mapAsXml.AddAttribute(property, "value", fnLuaScript);

						mapAsXml.Save();
					}

					//Lua Script schreiben
					string fnLuaOutput=String.Format("{0}{1}.lua", pathOutput, i.Name);

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
	
								CreateMapScriptFile(fnLuaOutput, MapUp, MapRight, MapDown, MapLeft, true);
								break;
							}
							case "uw":
							{
								CreateMapScriptFile(fnLuaOutput, i.ID, i.ID, i.ID, i.ID, true);
								break;
							}
							case "iw":
							{
								CreateMapScriptFile(fnLuaOutput);
								break;
							}
					}
				}
				else
				{
					Console.WriteLine("Datei {0} existiert nicht!", fnMap);
				}
			}
		}

		static void CreateMapScriptFile(string filename)
		{
			CreateMapScriptFile(filename, 0, 0, 0, 0, false);
		}

		static void CreateMapScriptFile(string filename, int MapUp, int MapRight, int MapDown, int MapLeft, bool usewarp)
		{
			StreamWriter sw=new StreamWriter(filename);
			
			sw.WriteLine("----------------------------------------------------------------------------------");
			sw.WriteLine("-- Map File                                                                     --");
			sw.WriteLine("--                                                                              --");
			sw.WriteLine("-- In dieser Datei stehen die entsprechenden externen NPC's, Trigger und        --");
			sw.WriteLine("-- anderer Dinge.                                                               --");
			sw.WriteLine("--                                                                              --");
			sw.WriteLine("----------------------------------------------------------------------------------");
			sw.WriteLine("--  Copyright 2010 The Invertika Development Team                               --");
			sw.WriteLine("--                                                                              --");
			sw.WriteLine("--  This file is part of Invertika.                                             --");
			sw.WriteLine("--                                                                              --");
			sw.WriteLine("--  Invertika is free software; you can redistribute it and/or modify it        --");
			sw.WriteLine("--  under the terms of the GNU General  Public License as published by the Free --");
			sw.WriteLine("--  Software Foundation; either version 2 of the License, or any later version. --");
			sw.WriteLine("----------------------------------------------------------------------------------");
			sw.WriteLine("\n");
			sw.WriteLine("require \"data/scripts/libs/npclib\"");
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
	}
}