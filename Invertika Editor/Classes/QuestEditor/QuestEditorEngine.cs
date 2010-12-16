using System;
using System.Collections.Generic;
using System.Text;
using CSCL;

namespace Invertika_Editor.Classes.QuestEditor
{
	public class QuestEditorEngine
	{
		public Treenode<KeyValuePair<string, IQuestDataClass>> QuestRoot
		{
			get;
			private set;
		}

		public QuestEditorEngine()
		{
			QuestRoot=new Treenode<KeyValuePair<string, IQuestDataClass>>();
			QuestRoot.Value=new KeyValuePair<string, IQuestDataClass>("@root", null);
		}

		public void AddMessage(List<string> messages, Treenode<KeyValuePair<string, IQuestDataClass>> node)
		{
			Treenode<KeyValuePair<string, IQuestDataClass>> message=new Treenode<KeyValuePair<string, IQuestDataClass>>();
			message.Value=new KeyValuePair<string, IQuestDataClass>("@message", (IQuestDataClass)new QDMessage(messages));
			node.Childs.Add(message);
		}

		//public void Save(string filename)
		//{
		//}

		//public void Load(string filename)
		//{
		//}

		private void InterateQuestTreeAndExportToLua(Treenode<KeyValuePair<string, IQuestDataClass>> node, List<string> lua, int spaces)
		{
			foreach(Treenode<KeyValuePair<string, IQuestDataClass>> child in node.Childs)
			{
				InterateQuestTreeAndExportToLua(child, lua, spaces+2);
			}

			switch(node.Value.Key)
			{
				case "@message":
					{
						QDMessage qdata=(QDMessage)node.Value.Value;

						lua.Add("return invertika.get_random_element("+qdata.Messages[0]+",");

						for(int i=1; i<qdata.Messages.Count; i++)
						{
							lua.Add("\""+qdata.Messages[i]+"\",");
						}

						lua[lua.Count-1]=lua[lua.Count-1].TrimEnd(',')+")";


						break;
					}
				default:
					{
						throw new NotImplementedException();
					}
			}
		}

		/// <summary>
		/// Exportiert die Interne Struktur in eine LUA Funktion
		/// </summary>
		/// <returns></returns>
		public List<string> ExportToLua()
		{
			List<string> ret=new List<string>();

			int spaces=0;
			ret.Add("function get_wache_say()");
			
			spaces+=2;
			InterateQuestTreeAndExportToLua(QuestRoot, ret, spaces);

			ret.Add("end");

			return ret;
		}
	}
}
