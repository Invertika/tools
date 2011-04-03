using System;
namespace InvertikaEditor
{
	public partial class FormMain : Gtk.Window
	{
		public FormMain() : base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			this.Maximize();
		}
		
		protected virtual void OnBeendenActionActivated(object sender, System.EventArgs e)
		{
			this.Destroy();
		}
		
		protected virtual void OnOptionenAction1Activated(object sender, System.EventArgs e)
		{
		}
	}
}

