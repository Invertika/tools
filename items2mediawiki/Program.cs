using System;
using System.Collections.Generic;
using System.Text;
using CSCL;
using System.IO;
using CSCL.Games.Manasource;

namespace items2mediawiki
{
	class Program
	{
		static void Main(string[] args)
		{
			Parameters cmdLine=Parameters.InterpretCommandLine(args);

			//TODO System Dateisystemzeichen auswerten

			//Parameter auswerten
			string fnItemsXml=cmdLine.GetString("file000").TrimEnd('\\')+'\\';
			fnItemsXml=@"D:\#\Eigende Dateien\Dev\invertika.sourceforge.net\client-data\items.xml";
			string fnItemsMediaWiki=FileSystem.GetPath(fnItemsXml)+FileSystem.GetFilenameWithoutExt(fnItemsXml)+".mediawiki";

			if(FileSystem.Exists(fnItemsXml))
			{
				StreamWriter sw=new StreamWriter(fnItemsMediaWiki);

				try
				{
					sw.WriteLine("{| border=\"1\" cellspacing=\"0\" cellpadding=\"5\" width=\"100%\" align=\"center\"\n"
					+"! style=\"background:#efdead;\" | Bild\n"
					+"! style=\"background:#efdead;\" | ID\n"
					+"! style=\"background:#efdead;\" | Name\n"
					+"! style=\"background:#efdead;\" | Beschreibung\n"
					+"! style=\"background:#efdead;\" | HP\n"
					+"! style=\"background:#efdead;\" | Gewicht\n"
					+"! style=\"background:#efdead;\" | Verteidigung\n"
					+"! style=\"background:#efdead;\" | Maximale Anzahl pro Slot\n"
					+"|-\n");

					List<Item> items=Item.GetItemsFromItemsXml(fnItemsXml);
					items.Sort();

					foreach(Item item in items)
					{
						if(item.ID<0) continue; //Unötige Items (Hairsets etc) ignorieren

						sw.WriteLine("| align=\"center\" | [[Image:item-{0}.png]]", item.ID);
						sw.WriteLine("| align=\"center\" | {0}", item.ID);
						sw.WriteLine("| {0}", item.Name);
						sw.WriteLine("| align=\"center\" | {0}", item.Description);
						sw.WriteLine("| align=\"center\" | {0}", item.HP);
						sw.WriteLine("| align=\"center\" | {0}", item.Weight);
						sw.WriteLine("| align=\"center\" | {0}%", item.Defense);
						sw.WriteLine("| align=\"center\" | {0}", item.MaxPerSlot);
						sw.WriteLine("|-");
					}

					sw.WriteLine("|}");
				}
				catch(Exception e)
				{
					Console.WriteLine("Fehler beim Erzeugen der Umsetzung zu MediaWiki Syntax:\n\nStacktrace:\n{0}", e.StackTrace.ToString());
					return;
				}
				finally
				{
					sw.Close();
				}

				Console.WriteLine("Wikidatei erfolgreich geschrieben.");
			}
			else
			{
				Console.WriteLine("Nutzung: monster2mediawiki.exe [ <monster file> ]");
				Console.WriteLine("Beispiel: monster2mediawiki.exe monster.xml");
			}
		}
	}
}
