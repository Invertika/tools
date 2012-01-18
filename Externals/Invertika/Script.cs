//  
//  Script.cs
//  
//  Author:
//       seeseekey <seeseekey@googlemail.com>
// 
//  Copyright (c) 2011, 2012 by seeseekey
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

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

			sw.WriteLine("-- Externe Map Skripting Datei");
			sw.WriteLine("-- In dieser Datei stehen die entsprechenden externen NPCs, Trigger und anderer Dinge.");
			sw.WriteLine("--");
			sw.WriteLine("-- © 2008-2011 by The Invertika Development Team");
			sw.WriteLine("--");
			sw.WriteLine("-- This file is part of Invertika. Invertika is free software; you can redistribute ");
			sw.WriteLine("-- it and/or modify it under the terms of the GNU General  Public License as published ");
			sw.WriteLine("-- by the Free Software Foundation; either version 3 of the License, or any later version.");
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
