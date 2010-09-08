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
			InstFormOutputBox.richTextBox1.Lines=message;

			InstFormOutputBox.Show();
		}

		public static void ShowOutputBox(string title, List<string> message)
		{
			FormOutputBox InstFormOutputBox=new FormOutputBox();

			InstFormOutputBox.Text=title;
			InstFormOutputBox.richTextBox1.Lines=message.ToArray();

			InstFormOutputBox.Show();
		}

		public static void ShowOutputBox(string title, string message)
		{
			FormOutputBox InstFormOutputBox=new FormOutputBox();

			InstFormOutputBox.Text=title;
			InstFormOutputBox.richTextBox1.Text=message;

			InstFormOutputBox.Show();
		}

		public FormOutputBox()
		{
			InitializeComponent();
		}
	}
}
