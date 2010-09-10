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

			InstFormOutputBox.tssEntry.Text="Einträge: "+InstFormOutputBox.rtbOutput.Lines.Length.ToString();

			InstFormOutputBox.Show();
		}

		public static void ShowOutputBox(string title, List<string> message)
		{
			FormOutputBox InstFormOutputBox=new FormOutputBox();

			InstFormOutputBox.Text=title;
			InstFormOutputBox.rtbOutput.Lines=message.ToArray();

			InstFormOutputBox.tssEntry.Text="Einträge: "+InstFormOutputBox.rtbOutput.Lines.Length.ToString();

			InstFormOutputBox.Show();
		}

		public static void ShowOutputBox(string title, string message)
		{
			FormOutputBox InstFormOutputBox=new FormOutputBox();

			InstFormOutputBox.Text=title;
			InstFormOutputBox.rtbOutput.Text=message;

			InstFormOutputBox.tssEntry.Text="Einträge: "+InstFormOutputBox.rtbOutput.Lines.Length.ToString();

			InstFormOutputBox.Show();
		}

		public FormOutputBox()
		{
			InitializeComponent();
		}
	}
}
