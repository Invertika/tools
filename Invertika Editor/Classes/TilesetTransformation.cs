using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Invertika_Editor.Classes
{
	public class TilesetTransformation
	{
		public string OldTileset { get; private set; }
		public string NewTileset { get; private set; }
		
		public int TileWidth { get; private set; }
		public int TileHeight { get; private set; }

		public Dictionary<int, int> TransformationTable { get; private set; }

		public TilesetTransformation(string filename)
		{
			TransformationTable=new Dictionary<int, int>();

			StreamReader sr=new StreamReader(filename);
			OldTileset=sr.ReadLine().Trim();
			NewTileset=sr.ReadLine().Trim();

			TileWidth=Convert.ToInt32(sr.ReadLine().Trim());
			TileHeight=Convert.ToInt32(sr.ReadLine().Trim());

			while(!sr.EndOfStream)
			{
				string line=sr.ReadLine().Trim();
				string[] splited=line.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

				TransformationTable.Add(Convert.ToInt32(splited[0]), Convert.ToInt32(splited[1]));
			}

			sr.Close();
		}
	}
}
