using System;
using System.Collections.Generic;
using System.Text;

namespace Invertika_Editor.Classes.QuestEditor
{
	public class QDMessage: IQuestDataClass
	{
		public List<string> Messages
		{
			get;
			private set;
		}

		public QDMessage(List<string> messages)
		{
			Messages=messages;
		}
	}
}
