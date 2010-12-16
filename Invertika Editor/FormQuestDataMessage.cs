using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Invertika_Editor
{
	public partial class FormQuestDataMessage : Form
	{
		public List<string> Messages;

		public FormQuestDataMessage()
		{
			InitializeComponent();
			Messages=new List<string>();
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			if(rbOnlyOneMessage.Checked)
			{
				Messages.Add(tbNachricht.Text);
			}
			else if(rbRandomMessage.Checked)
			{
				Messages.AddRange(rtbMessages.Lines);
			}

			DialogResult=System.Windows.Forms.DialogResult.OK;
		}
	}
}
