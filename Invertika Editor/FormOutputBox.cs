using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Invertika_Editor
{
	public partial class FormOutputBox : Form
	{
		public static void ShowOutputBox(string title, string[] message)
		{
			FormOutputBox InstFormOutputBox=new FormOutputBox();

			InstFormOutputBox.Text=title;
			InstFormOutputBox.rtbOutput.Lines=message;

			InstFormOutputBox.rtbOutput.Text=InstFormOutputBox.rtbOutput.Lines.ToString().TrimEnd();

			InstFormOutputBox.tssEntry.Text="Einträge: "+InstFormOutputBox.rtbOutput.Lines.Length.ToString();

			InstFormOutputBox.Show();
		}

		public static void ShowOutputBox(string title, List<string> message)
		{
			FormOutputBox InstFormOutputBox=new FormOutputBox();

			InstFormOutputBox.Text=title;
			InstFormOutputBox.rtbOutput.Lines=message.ToArray();
			InstFormOutputBox.rtbOutput.Text=InstFormOutputBox.rtbOutput.Text.TrimEnd();

			InstFormOutputBox.tssEntry.Text="Einträge: "+InstFormOutputBox.rtbOutput.Lines.Length.ToString();

			InstFormOutputBox.Show();
		}

		public static void ShowOutputBox(string title, string message)
		{
			FormOutputBox InstFormOutputBox=new FormOutputBox();

			InstFormOutputBox.Text=title;
			InstFormOutputBox.rtbOutput.Text=message.TrimEnd();

			InstFormOutputBox.tssEntry.Text="Einträge: "+InstFormOutputBox.rtbOutput.Lines.Length.ToString();

			InstFormOutputBox.Show();
		}

		public FormOutputBox()
		{
			InitializeComponent();
		}

		private void rtbOutput_TextChanged(object sender, EventArgs e)
		{
			tssEntry.Text="Einträge: "+rtbOutput.Lines.Length.ToString();
		}
	}
}