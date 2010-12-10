namespace Invertika_Editor
{
	partial class FormCreateMapThumbnailsAndMinimaps
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
			System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager(typeof(FormCreateMapThumbnailsAndMinimaps));
			this.cbClearCache=new System.Windows.Forms.CheckBox();
			this.pbCreateMapImages=new System.Windows.Forms.ProgressBar();
			this.btnStartCreateMapThumbnailsAndMinimaps=new System.Windows.Forms.Button();
			this.bgwCreateMapThumbnailsAndMinimaps=new System.ComponentModel.BackgroundWorker();
			this.cbOnlyVisibleMaps=new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// cbClearCache
			// 
			this.cbClearCache.AutoSize=true;
			this.cbClearCache.Location=new System.Drawing.Point(12, 39);
			this.cbClearCache.Name="cbClearCache";
			this.cbClearCache.Size=new System.Drawing.Size(321, 17);
			this.cbClearCache.TabIndex=24;
			this.cbClearCache.Text="Zwischenspeicher löschen (alle Karten werden neu berechnet)";
			this.cbClearCache.UseVisualStyleBackColor=true;
			// 
			// pbCreateMapImages
			// 
			this.pbCreateMapImages.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.pbCreateMapImages.Location=new System.Drawing.Point(12, 12);
			this.pbCreateMapImages.Name="pbCreateMapImages";
			this.pbCreateMapImages.Size=new System.Drawing.Size(482, 21);
			this.pbCreateMapImages.TabIndex=23;
			// 
			// btnStartCreateMapThumbnailsAndMinimaps
			// 
			this.btnStartCreateMapThumbnailsAndMinimaps.Anchor=((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Right)));
			this.btnStartCreateMapThumbnailsAndMinimaps.Location=new System.Drawing.Point(500, 12);
			this.btnStartCreateMapThumbnailsAndMinimaps.Name="btnStartCreateMapThumbnailsAndMinimaps";
			this.btnStartCreateMapThumbnailsAndMinimaps.Size=new System.Drawing.Size(108, 21);
			this.btnStartCreateMapThumbnailsAndMinimaps.TabIndex=22;
			this.btnStartCreateMapThumbnailsAndMinimaps.Text="Start";
			this.btnStartCreateMapThumbnailsAndMinimaps.UseVisualStyleBackColor=true;
			this.btnStartCreateMapThumbnailsAndMinimaps.Click+=new System.EventHandler(this.btnStartCreateMapThumbnailsAndMinimaps_Click);
			// 
			// bgwCreateMapThumbnailsAndMinimaps
			// 
			this.bgwCreateMapThumbnailsAndMinimaps.WorkerReportsProgress=true;
			this.bgwCreateMapThumbnailsAndMinimaps.DoWork+=new System.ComponentModel.DoWorkEventHandler(this.bgwCreateMapThumbnailsAndMinimaps_DoWork);
			this.bgwCreateMapThumbnailsAndMinimaps.ProgressChanged+=new System.ComponentModel.ProgressChangedEventHandler(this.bgwCreateMapThumbnailsAndMinimaps_ProgressChanged);
			this.bgwCreateMapThumbnailsAndMinimaps.RunWorkerCompleted+=new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwCreateMapThumbnailsAndMinimaps_RunWorkerCompleted);
			// 
			// cbOnlyVisibleMaps
			// 
			this.cbOnlyVisibleMaps.AutoSize=true;
			this.cbOnlyVisibleMaps.Location=new System.Drawing.Point(12, 63);
			this.cbOnlyVisibleMaps.Name="cbOnlyVisibleMaps";
			this.cbOnlyVisibleMaps.Size=new System.Drawing.Size(177, 17);
			this.cbOnlyVisibleMaps.TabIndex=25;
			this.cbOnlyVisibleMaps.Text="Nur sichtbare Weltkarte rendern";
			this.cbOnlyVisibleMaps.UseVisualStyleBackColor=true;
			// 
			// FormCreateMapThumbnailsAndMinimaps
			// 
			this.AutoScaleDimensions=new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize=new System.Drawing.Size(611, 92);
			this.Controls.Add(this.cbOnlyVisibleMaps);
			this.Controls.Add(this.cbClearCache);
			this.Controls.Add(this.pbCreateMapImages);
			this.Controls.Add(this.btnStartCreateMapThumbnailsAndMinimaps);
			this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon=((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name="FormCreateMapThumbnailsAndMinimaps";
			this.StartPosition=System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text="Kartenthumbnails und Minimaps berechnen";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox cbClearCache;
		private System.Windows.Forms.ProgressBar pbCreateMapImages;
		private System.Windows.Forms.Button btnStartCreateMapThumbnailsAndMinimaps;
		private System.ComponentModel.BackgroundWorker bgwCreateMapThumbnailsAndMinimaps;
		private System.Windows.Forms.CheckBox cbOnlyVisibleMaps;
	}
}