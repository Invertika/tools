﻿//  
//  LuaDocParser.cs
//  
//  Author:
//       seeseekey <seeseekey@googlemail.com>
// 
//  Copyright (c) 2011, 2012 by seeseekey
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Invertika.LuaDoc
{
	public class LuaDocParser
	{
		Dictionary<string, List<string>> FunctionAndComments;
		string modulename="";

		public LuaDocParser(string filename)
		{
			FunctionAndComments=new Dictionary<string, List<string>>();

			string[] lines=File.ReadAllLines(filename);

			bool newFunction=false;
			List<string> comments=new List<string>();

			foreach(string line in lines)
			{
				string tmp=line.Trim();
				if(tmp=="") continue;

				if(tmp.StartsWith("module"))
				{
					//module("banker", package.seeall)
					tmp=tmp.Replace("module(\"", "");
					tmp=tmp.Replace("\", package.seeall)", "");
					modulename=tmp;
					comments=new List<string>();
					continue;
				}

				if(line.StartsWith("--"))
				{
					newFunction=true;
					comments.Add(tmp.Trim('-').Trim());
				}
				else
				{
					if(newFunction)
					{
						if(tmp.StartsWith("function"))
						{
							FunctionAndComments.Add(tmp, comments);
						}
						newFunction=false;
						comments=new List<string>();
					}
				}
			}
		}

		public LucDocReturn ExportLuaDocToMediaWiki()
		{
			List<string> fileContent=new List<string>();

			foreach(string key in FunctionAndComments.Keys)
			{
				fileContent.Add(String.Format("==={0}===", key));

				Dictionary<string, string> Parameters=new Dictionary<string, string>();
				List<string> values=FunctionAndComments[key];

				//Funktionen
				foreach(string value in values)
				{
					if(value.StartsWith("@param"))
					{
						char[] splitChars=new char[1];
						splitChars[0]=' ';
						string[] splited=value.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);

						if(splited.Length==2)
						{
							Parameters.Add(splited[1], "");
						}
						else if(splited.Length>2)
						{
							string desc="";

							for(int i=2; i<splited.Length; i++)
							{
								desc+=" "+splited[i];
							}

							Parameters.Add(splited[1], desc.Trim());
						}
					}
					else
					{
						fileContent.Add(value.Replace("function", "").Trim());
					}
				}

				//Funktionsparameter
				if(Parameters.Count>0)
				{
					fileContent.Add("");

					fileContent.Add("{{ParamTableStart}}");

					foreach(string k in Parameters.Keys)
					{
						fileContent.Add(String.Format("{{{{ParamTableRow| {0}  | | JA  |  | {1}}}}}", k, Parameters[k]));
					}

					fileContent.Add("{{ParamTableEnd}}");
				}

				fileContent.Add("");
			}

			LuaDocType docType=LuaDocType.Unknown;
			string Name="";

			if(modulename!="")
			{
				docType=LuaDocType.Module;
				Name=modulename;
			}

			LucDocReturn ldr=new LucDocReturn(fileContent, docType, Name);
			return ldr;
		}
	}
}