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
