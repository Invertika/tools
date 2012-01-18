//  
//  MonsterSpawn.cs
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

namespace Invertika.Classes
{
	public class MonsterSpawn
	{
		public int MaxBeings { get; private set; }
		public int MonsterID { get; private set; }
		public double SpawnRate { get; private set; }

		public MonsterSpawn(int maxBeings, int monsterID, double spawnRate)
		{
			MaxBeings=maxBeings;
			MonsterID=monsterID;
			SpawnRate=spawnRate;
		}
	}
}
