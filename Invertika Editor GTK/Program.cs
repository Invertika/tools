using System;
using Gtk;

namespace InvertikaEditor
{
	class Program
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			FormMain win = new FormMain ();
			win.Show ();
			Application.Run ();
		}
	}
}

