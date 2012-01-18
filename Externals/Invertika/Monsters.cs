//  
//  Monsters.cs
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
using CSCL.FileFormats.TMX;
using Invertika.Classes;

namespace Invertika
{
	public static class Monsters
	{
		/// <summary>
		/// Gibt die spawnenden Monster für eine Map zurück
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static List<MonsterSpawn> GetMonsterSpawnFromMap(string filename)
		{
			TMX map=new TMX(filename, false);
			return GetMonsterSpawnFromMap(map);
		}

		/// <summary>
		/// Gibt die spawnenden Monster für eine Map zurück
		/// </summary>
		/// <param name="map"></param>
		/// <returns></returns>
		public static List<MonsterSpawn> GetMonsterSpawnFromMap(TMX map)
		{
			List<MonsterSpawn> ret=new List<MonsterSpawn>();

			foreach(Objectgroup objgroup in map.ObjectLayers)
			{
				if(objgroup.Name.ToLower()=="object")
				{
					foreach(CSCL.FileFormats.TMX.Object obj in objgroup.Objects)
					{
						if(obj.Type!=null)
						{
							if(obj.Type.ToLower()=="spawn")
							{
								int MAX_BEINGS=-1;
								int MONSTER_ID=-1;
								double SPAWN_RATE=-1;

								foreach(Property prop in obj.Properties)
								{
									switch(prop.Name.ToLower())
									{
										case "monster_id":
											{
												MONSTER_ID=Convert.ToInt32(prop.Value);
												break;
											}
										case "max_beings":
											{
												MAX_BEINGS=Convert.ToInt32(prop.Value);
												break;
											}
										case "spawn_rate":
											{
												SPAWN_RATE=Convert.ToDouble(prop.Value);
												break;
											}
									}
								}

								ret.Add(new MonsterSpawn(MAX_BEINGS, MONSTER_ID, SPAWN_RATE));
							}
						}
					}
				}
			}

			return ret;
		}
	}
}
