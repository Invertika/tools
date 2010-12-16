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

			string padLeft="";
			padLeft=padLeft.PadLeft(spaces, ' ');

			switch(node.Value.Key)
			{
				case "@message":
					{
						QDMessage qdata=(QDMessage)node.Value.Value;

						lua.Add(padLeft+"do_message(npc, ch, invertika.get_random_element(\""+qdata.Messages[0]+"\",");

						string internalPad=padLeft;
						internalPad=internalPad.PadLeft(35, ' ');

						for(int i=1; i<qdata.Messages.Count; i++)
						{
							lua.Add(internalPad+"\""+qdata.Messages[i]+"\",");
						}

						lua[lua.Count-1]=lua[lua.Count-1].TrimEnd(',')+"))";


						break;
					}
				case "@root":
					{
						//Tags welche beim export ignoriert werden
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
		public List<string> ExportToLua(string npcName, int npcID, int posX, int posY)
		{
			List<string> ret=new List<string>();

			string functionName=String.Format("{0}_talk", npcName.ToLower());

			ret.Add(String.Format(" create_npc(\"{0}\", {1}, {2} * TILESIZE + 16, {3} * TILESIZE + 16, {4}, nil) --- {0}", npcName, npcID, posX, posY, functionName));
			ret.Add("");
			ret.Add("");

			ret.Add(String.Format("function {0}()", functionName));
			
			InterateQuestTreeAndExportToLua(QuestRoot, ret, 2);

			ret.Add("  do_npc_close(npc, ch)");
			ret.Add("end");

			return ret;
		}
	}
}
