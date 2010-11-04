namespace Invertika_Editor
{
	partial class FormOutputBox
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components=null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing&&(components!=null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.statusStrip=new System.Windows.Forms.StatusStrip();
			this.tssEntry=new System.Windows.Forms.ToolStripStatusLabel();
			this.rtbOutput=new System.Windows.Forms.RichTextBox();
			this.statusStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssEntry});
			this.statusStrip.Location=new System.Drawing.Point(0, 369);
			this.statusStrip.Name="statusStrip";
			this.statusStrip.Size=new System.Drawing.Size(864, 22);
			this.statusStrip.TabIndex=1;
			this.statusStrip.Text="statusStrip1";
			// 
			// tssEntry
			// 
			this.tssEntry.Name="tssEntry";
			this.tssEntry.Size=new System.Drawing.Size(60, 17);
			this.tssEntry.Text="Einträge: 0";
			// 
			// rtbOutput
			// 
			this.rtbOutput.Dock=System.Windows.Forms.DockStyle.Fill;
			this.rtbOutput.Location=new System.Drawing.Point(0, 0);
			this.rtbOutput.Name="rtbOutput";
			this.rtbOutput.Size=new System.Drawing.Size(864, 369);
			this.rtbOutput.TabIndex=2;
			this.rtbOutput.Text="";
			this.rtbOutput.TextChanged+=new System.EventHandler(this.rtbOutput_TextChanged);
			// 
			// FormOutputBox
			// 
			this.AutoScaleDimensions=new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize=new System.Drawing.Size(864, 391);
			this.Controls.Add(this.rtbOutput);
			this.Controls.Add(this.statusStrip);
			this.Name="FormOutputBox";
			this.StartPosition=System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text="Ausgabe:";
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripStatusLabel tssEntry;
		private System.Windows.Forms.RichTextBox rtbOutput;
	}
}