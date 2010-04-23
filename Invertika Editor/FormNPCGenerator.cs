using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Invertika_Editor
{
	public partial class FormNPCGenerator:Form
	{
		public FormNPCGenerator()
		{
			InitializeComponent();
		}

		private void RefreshScriptOutput()
		{
			string output=String.Format(" create_npc(\"{0}\", {1}, {2} * TILESIZE + 16, {3} * TILESIZE + 16, {4}_talk, nil) --- {0}", tbNPCName.Text, nudNPCID.Value, nudPosX.Value, nudPosY.Value, tbNPCName.Text.ToLower());
			output+="\n\n";

			output+=String.Format("function {0}_talk(npc, ch)", tbNPCName.Text.ToLower());
			output+="\n";
			output+="	do_message(npc, ch, invertika.get_random_element(";

			foreach(string line in rtbSentences.Lines)
			{
				if(output[output.Length-1]=='(')
				{
					output+=String.Format("\"{0}\",\n", line);
				}
				else
				{
					output+=String.Format("	  \"{0}\",\n", line);
				}
			}

			output=output.TrimEnd('\n');
			output=output.TrimEnd(',');

			output+="))";

			output+="\n";
			output+="	do_npc_close(npc, ch)";
			output+="\n";
			output+="end";

			rtbScriptOutput.Text=output;
		}

		private void tbNPCName_TextChanged(object sender, EventArgs e)
		{
			RefreshScriptOutput();
		}

		private void nudNPCID_ValueChanged(object sender, EventArgs e)
		{
			RefreshScriptOutput();
		}

		private void nudPosX_ValueChanged(object sender, EventArgs e)
		{
			RefreshScriptOutput();
		}

		private void nudPosY_ValueChanged(object sender, EventArgs e)
		{
			RefreshScriptOutput();
		}

		private void rtbSentences_TextChanged(object sender, EventArgs e)
		{
			RefreshScriptOutput();
		}

		private void label23_Click(object sender, EventArgs e)
		{
			// Webbrowser mit Doamin aufrufen
			System.Diagnostics.Process p=new System.Diagnostics.Process();
			p.StartInfo.RedirectStandardOutput=false;
			p.StartInfo.FileName="http://wiki.invertika.org/NPC_Entwicklung";
			p.StartInfo.UseShellExecute=true;
			p.Start();
		}
	}
}
