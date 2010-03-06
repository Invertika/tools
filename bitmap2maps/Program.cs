using System;
using System.Collections.Generic;
using System.Text;
using CSCL;
using CSCL.Graphic;
using System.IO;
using System.Drawing;
using CSCL.Games.Manasource;

namespace bitmap2maps
{
	class Program
	{
		static void Main(string[] args)
		{
			Parameters cmdLine=Parameters.InterpretCommandLine(args);

			//Parameter auswerten
			string error="";
			if(!cmdLine.Contains("file000")) { error="Kein Bitmap Dateiname angegeben!"; }
			else if(!cmdLine.Contains("file001")) { error="Kein Maps Templatepfad angegeben!"; }
			else if(!cmdLine.Contains("file002")) { error="Kein Zielpfad angegeben!"; }
			else if(!cmdLine.Contains("xmin")) { error="Kein xmax angegeben!"; }
			else if(!cmdLine.Contains("ymax")) { error="Kein ymax angegeben!"; }

			if(error!="")
			{
				Console.WriteLine(error);
				Console.WriteLine("");
				Console.WriteLine("Benutzung: bitmap2maps.exe <Bitmap Dateiname> <Maps Templatepfad> <Zielpfad> [-xmin] [-ymin]");
				Console.WriteLine(@"Beispiel: bitmap2maps.exe D:\weltkarte.bmp D:\invertika.googlecode.com\client-data\maps_templates D:\Output -xmin:-25 -ymax:25");
				return;
			}

			string fnBitmap=cmdLine.GetString("file000");
			string pathMapsTemplates=FileSystem.GetPathWithBackslash(cmdLine.GetString("file001"));
			string pathOutput=FileSystem.GetPathWithBackslash(cmdLine.GetString("file002"));
			int MapX=Convert.ToInt32(cmdLine.GetString("xmin", ""));
			int MapY=Convert.ToInt32(cmdLine.GetString("ymax", ""));

			//Folder
			FileSystem.CreateDirectory(pathOutput, true);

			//Datei laden
			gtImage tmp=gtImage.FromFile(fnBitmap);

			//Vars
			uint kSize=180;

			uint y=0;
			uint x=0;

			string TemplateMap="";

			while(y<tmp.Height)
			{
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
	}
}
