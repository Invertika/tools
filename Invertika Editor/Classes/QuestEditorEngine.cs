using System;
using System.Collections.Generic;
using System.Text;
using CSCL;

namespace Invertika_Editor.Classes
{
	public class QuestEditorEngine
	{
		Parameters questStructure;

		public Parameters QuestRoot
		{
			get;
			private set;
		}

		public QuestEditorEngine()
		{
			questStructure=new Parameters();
			QuestRoot=questStructure.Add("ROOT", "ROOT");
		}

		public Parameters GetParameters(string path)
		{
			return questStructure.GetParameters(path);
		}

		public void AddMessage(List<string> messages, string path)
		{
			GetParameters(path).Add("MESSAGE", messages);
		}

		public void Save(string filename)
		{
		}

		public void Load(string filename)
		{
		}

		/// <summary>
		/// Exportiert die Interne Struktur in eine LUA Funktion
		/// </summary>
		/// <returns></returns>
		public List<string> ExportToLua()
		{
			List<string> ret=new List<string>();

			int spaces=0;
			Parameters currentRoot=null;
			ret.Add("function get_wache_say()");
			spaces+=2;

			//QuestRoot Kinder druchgehen

			string parametersName="";

			switch(parametersName)
			{
				case "MESSAGE":
					{
						List<string> values=QuestRoot.GetStringList("");

						ret.Add("return invertika.get_random_element("+values[0]+",");

						for(int i=1; i<values.Count; i++)
						{
							ret.Add("\""+values[i]+"\",");
						}

						values[values.Count-1]=values[values.Count-1].TrimEnd(',')+")";


						break;
					}
			}


			ret.Add("end");

			return ret;
		}
	}
}
