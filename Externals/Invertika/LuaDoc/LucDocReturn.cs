using System;
using System.Collections.Generic;
using System.Text;

namespace Invertika.LuaDoc
{
	public class LucDocReturn
	{
		public List<string> Functions { get; private set; }
		public LuaDocType DocType { get; private set; }
		public string Name { get; private set; }

		public LucDocReturn(List<string> content, LuaDocType docType, string name)
		{
			Functions=content;
			DocType=docType;
			Name=name;
		}
	}
}
