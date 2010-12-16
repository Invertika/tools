using System;
using System.Collections.Generic;
using System.Text;

namespace Invertika_Editor.Classes.QuestEditor
{
	public class QDIf : IQuestDataClass
	{
		public List<string> Messages
		{
			get;
			private set;
		}

		public QDIf(List<string> messages)
		{
			Messages=messages;
		}
	}
}
