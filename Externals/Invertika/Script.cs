using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Invertika
{
	public static class Script
	{
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
	}
}
