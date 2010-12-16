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
			Messages.Clear();
			Messages.AddRange(rtbMessages.Lines);

			DialogResult=System.Windows.Forms.DialogResult.OK;
		}

		private void FormQuestDataMessage_Load(object sender, EventArgs e)
		{
			rtbMessages.Lines=Messages.ToArray();
		}
	}
}
