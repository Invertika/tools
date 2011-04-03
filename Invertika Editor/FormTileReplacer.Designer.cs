namespace Invertika_Editor
{
	partial class FormTileReplacer
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
			this.pbImage = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
			this.SuspendLayout();
			// 
			// pbImage
			// 
			this.pbImage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pbImage.Location = new System.Drawing.Point(0, 0);
			this.pbImage.Margin = new System.Windows.Forms.Padding(0);
			this.pbImage.Name = "pbImage";
			this.pbImage.Size = new System.Drawing.Size(1018, 1002);
			this.pbImage.TabIndex = 0;
			this.pbImage.TabStop = false;
			this.pbImage.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pbImage_MouseDoubleClick);
			// 
			// FormTileReplacer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1018, 1002);
			this.Controls.Add(this.pbImage);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormTileReplacer";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Tile auswählen";
			this.Load += new System.EventHandler(this.FormTileReplacer_Load);
			((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox pbImage;
	}
}