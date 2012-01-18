//  
//  Imaging.cs
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
using CSCL.Graphic;
using CSCL.FileFormats.TMX;
using System.Drawing;
using CSCL.Games.Manasource;
using Invertika.Classes;
using CSCL;

namespace Invertika
{
	public static class Imaging
	{
		#region Weltkarte
		/// <summary>
		/// Speichert eine Karte welche je nach Monsterstärke eingefärbt wird
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="img"></param>
		/// <param name="map"></param>
		public static void SaveFeatureMapMonsterSpreading(string clientdata, string filename, gtImage img, TMX map)
		{
			//Farben
			Color green=Color.FromArgb(128, 0, 255, 0);
			Color yellow=Color.FromArgb(128, 255, 255, 0);
			Color red=Color.FromArgb(128, 255, 0, 0);
			Color blue=Color.FromArgb(128, 0, 0, 255);

			//Images
			gtImage tmpImage=img.GetImage();
			gtImage tmpDraw=new gtImage(tmpImage.Width, tmpImage.Height, tmpImage.ChannelFormat);

			//Ermittlung der Durchschnittswerte
			string fnMonsterXml=clientdata+"monsters.xml";
			List<Monster> monsters=Monster.GetMonstersFromMonsterXml(fnMonsterXml);
			monsters.Sort();

			Int64 minFightingStrength=999999999999;
			Int64 maxFightingStrength=-999999999999;

			Int64 medianFightingStrength=0;

			Dictionary<int, Int64> MonsterIDsAndFightingStrength=new Dictionary<int, Int64>();

			foreach(Monster monster in monsters)
			{
				if(monster.ID==1) continue; //Killermade ignorieren
				if(monster.ID==31) continue; //Seraphim Nex ignorieren
				if(monster.ID>9999) continue; //Experimentelle Monster ignorieren
				Int64 fightingStrength=monster.FightingStrength;

				if(fightingStrength<minFightingStrength) minFightingStrength=fightingStrength;
				if(fightingStrength>maxFightingStrength) maxFightingStrength=fightingStrength;

				MonsterIDsAndFightingStrength.Add(monster.ID, fightingStrength);
			}

			medianFightingStrength=(maxFightingStrength+minFightingStrength)/2;

			//Monster der Karte ermitteln
			List<MonsterSpawn> mSpawns=Monsters.GetMonsterSpawnFromMap(map);

			if(mSpawns.Count>0)
			{
				Int64 fss=0;

				foreach(MonsterSpawn spawn in mSpawns)
				{
					if(spawn.MonsterID==1) continue; //Killermade ignorieren
					if(spawn.MonsterID==31) continue; //Seraphim Nex ignorieren
					if(spawn.MonsterID>=10000) continue; //Pflanzen etc ignorieren
					fss+=MonsterIDsAndFightingStrength[spawn.MonsterID];
				}

				fss=fss/mSpawns.Count;

				//Einfärben je nach Stärke
				Int64 vSmarterGreen=(medianFightingStrength+minFightingStrength)/2;
				Int64 vSmarterYellow=(maxFightingStrength+medianFightingStrength)/2;

				if(fss<vSmarterGreen)
				{
					tmpDraw.Fill(green);
				}
				else if(fss<vSmarterYellow)
				{
					tmpDraw.Fill(yellow);
				}
				else
				{
					tmpDraw.Fill(red);
				}
			}
			else //Keine Monster auf der Karte vorhanden
			{
				tmpDraw.Fill(blue);
			}

			//Drawen
			tmpImage.Draw(0, 0, tmpDraw, true);
			tmpImage.SaveToFile(filename);
		}
		#endregion

		#region Kartenerzeugung
		/// <summary>
		/// Erzeugt aus einer Bitmap Außenkarten
		/// </summary>
		public static void CreateMapsFromBitmap(string filenameBitmap, string mapstemplates, string targetFolder, int xMin, int yMax, Progress progress)
		{
			if(progress!=null) if(!progress(0)) { return; }

			string fnBitmap=filenameBitmap;
			string pathMapsTemplates=mapstemplates;
			//string pathMapsTemplates=Globals.folder_clientdata_mapstemplates;
			string pathOutput=targetFolder;
			//int MapX=(int)(decimal)nudXmin.Value;
			//int MapY=(int)(decimal)nudYmax.Value;
			int MapX=xMin;
			int MapY=yMax;

			//Folder
			FileSystem.CreateDirectory(pathOutput, true);

			//Datei laden
			gtImage tmp=gtImage.FromFile(fnBitmap);

			//Vars
			uint kSize=100;

			uint y=0;
			uint x=0;

			string TemplateMap="";

			//FortschrittMax=(int)tmp.Height;
			//FortschrittValue=0;
			progress(0);
			//if(i%fivePercent==0) if(pi!=null) if(!pi(i*100.0/header.NumberOfPointRecords)) return null;

			while(y<tmp.Height)
			{
				progress(y/tmp.Height);
				//FortschrittValue=(int)y;
				//backgroundWorker.ReportProgress(0);

				Console.Write(".");
				x=0;
				MapX=-25;

				while(x<tmp.Width)
				{
					gtImage sub=tmp.GetSubImage(x, y, kSize, kSize);
					Color col=sub.GetMedianColor();

					#region Farbquantisierung
					int tol=50;

					//Meer
					if(ColCheck(col, 118, 134, 165, tol))
					{
						TemplateMap="ow-meer.tmx";
					}

					//Wüste
					if(ColCheck(col, 250, 203, 144, tol))
					{
						TemplateMap="ow-wueste.tmx";
					}

					//Eis
					if(ColCheck(col, 254, 254, 254, tol+50))
					{
						TemplateMap="ow-eis.tmx";
					}

					//Wald
					if(ColCheck(col, 33, 119, 31, tol))
					{
						TemplateMap="ow-wald.tmx";
					}

					//Erde
					if(ColCheck(col, 152, 108, 64, tol))
					{
						TemplateMap="ow-erde.tmx";
					}
					#endregion

					FileSystem.CopyFile(pathMapsTemplates+TemplateMap, pathOutput+Map.GetOuterWorldMapFilenameWithoutExtension(MapX, MapY, 0)+".tmx");

					x+=kSize;
					MapX++;
				}

				y+=kSize;
				MapY--;
			}

			if(progress!=null) progress(100);
		}

		static bool ColCheck(Color col, int r, int g, int b, int tol)
		{
			if(ColTol(col.R, r, tol))
			{
				if(ColTol(col.G, g, tol))
				{
					if(ColTol(col.B, b, tol))
					{
						return true;
					}
				}
			}

			return false;
		}

		static bool ColTol(int c, int value, int tol)
		{
			if(value-tol<c&&value+tol>c) return true;
			return false;
		}
		#endregion
	}
}