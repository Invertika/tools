using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CSCL;

namespace Invertika_Editor.Controls
{
	public partial class QuestEditorMessageEditor : Form
	{
		public Parameters Data
		{
			get;
			set;
		}

		public QuestEditorMessageEditor()
		{
			InitializeComponent();
		}

		private void QuestEditorMessageEditor_Load(object sender, EventArgs e)
		{
			List<string> value=Data.GetStringList("Message/Values");

			if(value==null)
			{
				value=new List<string>();
				value.Add("");
			}

			if(Data.GetBool("Message/MultibleValues")==true)
			{
				rbRandomValues.Checked=true;
				rtbRandomValues.Lines=value.ToArray();
			}
			else
			{
				rbOneValue.Checked=true;
				tbOneValue.Text=value[0];
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if(rbRandomValues.Checked)
			{
				Data.Add("Message/MultibleValues", true);
				Data.Add("Message/Values", new List<string>(rtbRandomValues.Lines));
			}
			else
			{
				Data.Add("Message/MultibleValues", false);
				List<string> ret=new List<string>();
				ret.Add(tbOneValue.Text);
				Data.Add("Message/Values", ret);
			}

			Close();
		}
	}
}
