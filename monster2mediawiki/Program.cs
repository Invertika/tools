using System;
using System.Collections.Generic;
using System.Text;
using CSCL;
using System.Xml;
using System.IO;
using CSCL.Games.Manasource;

namespace monsters2mediawiki
{
	class Program
	{
		static void Main(string[] args)
		{
			Parameters cmdLine=Parameters.InterpretCommandLine(args);

			//TODO System Dateisystemzeichen auswerten

			//Parameter auswerten
			string fnMonsterXml=cmdLine.GetString("file000").TrimEnd('\\')+'\\';
			string fnMonsterMediaWiki=FileSystem.GetPath(fnMonsterXml) + FileSystem.GetFilenameWithoutExt(fnMonsterXml)+".mediawiki";

			if(FileSystem.Exists(fnMonsterXml))
			{
				StreamWriter sw=new StreamWriter(fnMonsterMediaWiki);

				try
				{
					sw.WriteLine("{| border=\"1\" cellspacing=\"0\" cellpadding=\"5\" width=\"100%\" align=\"center\"\n"
					+"! style=\"background:#efdead;\" | Bild\n"
					+"! style=\"background:#efdead;\" | ID\n"
					+"! style=\"background:#efdead;\" | Name\n"
					+"! style=\"background:#efdead;\" | HP\n"
					+"! style=\"background:#efdead;\" | Agressiv\n"
					+"! style=\"background:#efdead;\" | Angriff\n"
					+"! style=\"background:#efdead;\" | Verteidigung (physisch)\n"
					+"! style=\"background:#efdead;\" | Verteidigung (magisch)\n"
					+"! style=\"background:#efdead;\" | Erfahrung\n"

					//+"! style=\"background:#efdead;\" | Drops"
					+"|-\n");

					List<Monster> monsters=Monster.GetMonstersFromMonsterXml(fnMonsterXml);
					monsters.Sort();

					foreach(Monster monster in monsters)
					{
						if(item.ID>9999) continue; //Experimentele Monster ignorieren
						
						sw.WriteLine("| align=\"center\" | [[Image:monster-{0}.png]]", monster.ID);
						sw.WriteLine("| align=\"center\" | {0}", monster.ID);
						sw.WriteLine("| {0}", monster.Name);
						sw.WriteLine("| align=\"center\" | {0}", monster.Attributes.HP);
						
						if(monster.Behavior!=null)
						{
							if(monster.Behavior.Aggressive) sw.WriteLine("| align=\"center\" | Ja");
							else sw.WriteLine("| align=\"center\" | Nein");
						}
						else
						{
							sw.WriteLine("| align=\"center\" | nicht definiert");
						}
						
						sw.WriteLine("| align=\"center\" | {0}-{1}", monster.Attributes.AttackMin-monster.Attributes.AttackDelta, monster.Attributes.AttackMin+monster.Attributes.AttackDelta);
						sw.WriteLine("| align=\"center\" | {0}%", monster.Attributes.PhysicalDefence);
						sw.WriteLine("| align=\"center\" | {0}%", monster.Attributes.MagicalDefence);
						sw.WriteLine("| align=\"center\" | {0}", monster.Exp);

						//sw.WriteLine("| Bone (2.1%)<br>Skull (3%)<br>Dark Crystal (10%)<br>Warlord Helmet (0.03%)<br>Warlord Plate (0.02%)<br>Leather Gloves (0.35%)");
						//sw.WriteLine("| ''<span style=\"color:#ad1818\">Aggro</span>''");
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
