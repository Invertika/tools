//  
//  Invertika.cs
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

namespace Invertika
{
	public class Invertika
	{
		//private void SaveFeatureMapMonsterSpreading(string filename, gtImage img, TMX map)
		//{
		//    //Farben
		//    Color green=Color.FromArgb(128, 0, 255, 0);
		//    Color yellow=Color.FromArgb(128, 255, 255, 0);
		//    Color red=Color.FromArgb(128, 255, 0, 0);
		//    Color blue=Color.FromArgb(128, 0, 0, 255);

		//    //Images
		//    gtImage tmpImage=img.GetImage();
		//    gtImage tmpDraw=new gtImage(tmpImage.Width, tmpImage.Height, tmpImage.ChannelFormat);

		//    //Ermittlung der Durchschnittswerte
		//    string fnMonsterXml=Globals.folder_clientdata+"monsters.xml";
		//    List<Monster> monsters=Monster.GetMonstersFromMonsterXml(fnMonsterXml);
		//    monsters.Sort();

		//    Int64 minFightingStrength=999999999999;
		//    Int64 maxFightingStrength=-999999999999;

		//    Int64 medianFightingStrength=0;

		//    Dictionary<int, Int64> MonsterIDsAndFightingStrength=new Dictionary<int, Int64>();

		//    foreach(Monster monster in monsters)
		//    {
		//        if(monster.ID==1) continue; //Killermade ignorieren
		//        if(monster.ID==31) continue; //Seraphim Nex ignorieren
		//        if(monster.ID>9999) continue; //Experimentelle Monster ignorieren
		//        Int64 fightingStrength=monster.FightingStrength;

		//        if(fightingStrength<minFightingStrength) minFightingStrength=fightingStrength;
		//        if(fightingStrength>maxFightingStrength) maxFightingStrength=fightingStrength;

		//        MonsterIDsAndFightingStrength.Add(monster.ID, fightingStrength);
		//    }

		//    medianFightingStrength=(maxFightingStrength+minFightingStrength)/2;

		//    //Monster der Karte ermitteln
		//    List<MonsterSpawn> mSpawns=Globals.GetMonsterSpawnFromMap(map);

		//    if(mSpawns.Count>0)
		//    {
		//        Int64 fss=0;

		//        foreach(MonsterSpawn spawn in mSpawns)
		//        {
		//            if(spawn.MonsterID==1) continue; //Killermade ignorieren
		//            if(spawn.MonsterID==31) continue; //Seraphim Nex ignorieren
		//            if(spawn.MonsterID>=10000) continue; //Pflanzen etc ignorieren
		//            fss+=MonsterIDsAndFightingStrength[spawn.MonsterID];
		//        }

		//        fss=fss/mSpawns.Count;

		//        //Einfärben je nach Stärke
		//        Int64 vSmarterGreen=(medianFightingStrength+minFightingStrength)/2;
		//        Int64 vSmarterYellow=(maxFightingStrength+medianFightingStrength)/2;

		//        if(fss<vSmarterGreen)
		//        {
		//            tmpDraw.Fill(green);
		//        }
		//        else if(fss<vSmarterYellow)
		//        {
		//            tmpDraw.Fill(yellow);
		//        }
		//        else
		//        {
		//            tmpDraw.Fill(red);
		//        }
		//    }
		//    else //Keine Monster auf der Karte vorhanden
		//    {
		//        tmpDraw.Fill(blue);
		//    }

		//    //Drawen
		//    tmpImage.Draw(0, 0, tmpDraw, true);
		//    tmpImage.SaveToFile(filename);
		//}
	}
}
