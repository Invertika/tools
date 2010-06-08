namespace Invertika_Editor
{
	partial class FormCreateDataFolder
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
			System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager(typeof(FormCreateDataFolder));
			this.label6=new System.Windows.Forms.Label();
			this.pbCreateDataFolders=new System.Windows.Forms.ProgressBar();
			this.btnStart=new System.Windows.Forms.Button();
			this.btnTargetBrowse=new System.Windows.Forms.Button();
			this.tbTargetPath=new System.Windows.Forms.TextBox();
			this.bgwCreateDataFolders=new System.ComponentModel.BackgroundWorker();
			this.folderBrowserDialog=new System.Windows.Forms.FolderBrowserDialog();
			this.SuspendLayout();
			// 
			// label6
			// 
			this.label6.AutoSize=true;
			this.label6.Location=new System.Drawing.Point(12, 8);
			this.label6.Name="label6";
			this.label6.Size=new System.Drawing.Size(80, 13);
			this.label6.TabIndex=22;
			this.label6.Text="Zielverzeichnis:";
			// 
			// pbCreateDataFolders
			// 
			this.pbCreateDataFolders.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.pbCreateDataFolders.Location=new System.Drawing.Point(12, 50);
			this.pbCreateDataFolders.Name="pbCreateDataFolders";
			this.pbCreateDataFolders.Size=new System.Drawing.Size(617, 23);
			this.pbCreateDataFolders.TabIndex=21;
			// 
			// btnStart
			// 
			this.btnStart.Anchor=((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Right)));
			this.btnStart.Location=new System.Drawing.Point(635, 50);
			this.btnStart.Name="btnStart";
			this.btnStart.Size=new System.Drawing.Size(103, 23);
			this.btnStart.TabIndex=20;
			this.btnStart.Text="Start";
			this.btnStart.UseVisualStyleBackColor=true;
			this.btnStart.Click+=new System.EventHandler(this.btnStart_Click);
			// 
			// btnTargetBrowse
			// 
			this.btnTargetBrowse.Anchor=((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Right)));
			this.btnTargetBrowse.Location=new System.Drawing.Point(635, 23);
			this.btnTargetBrowse.Name="btnTargetBrowse";
			this.btnTargetBrowse.Size=new System.Drawing.Size(103, 20);
			this.btnTargetBrowse.TabIndex=19;
			this.btnTargetBrowse.Text="Durchsuchen...";
			this.btnTargetBrowse.UseVisualStyleBackColor=true;
			this.btnTargetBrowse.Click+=new System.EventHandler(this.btnTargetBrowse_Click);
			// 
			// tbTargetPath
			// 
			this.tbTargetPath.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.tbTargetPath.Location=new System.Drawing.Point(12, 24);
			this.tbTargetPath.Name="tbTargetPath";
			this.tbTargetPath.ReadOnly=true;
			this.tbTargetPath.Size=new System.Drawing.Size(617, 20);
			this.tbTargetPath.TabIndex=18;
			// 
			// bgwCreateDataFolders
			// 
			this.bgwCreateDataFolders.WorkerReportsProgress=true;
			this.bgwCreateDataFolders.DoWork+=new System.ComponentModel.DoWorkEventHandler(this.bgwCreateDataFolders_DoWork);
			this.bgwCreateDataFolders.RunWorkerCompleted+=new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwCreateDataFolders_RunWorkerCompleted);
			// 
			// FormCreateDataFolder
			// 
			this.AutoScaleDimensions=new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize=new System.Drawing.Size(750, 85);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.pbCreateDataFolders);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.btnTargetBrowse);
			this.Controls.Add(this.tbTargetPath);
			this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon=((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name="FormCreateDataFolder";
			this.StartPosition=System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text="Daten Ordner erstellen";
			this.Load+=new System.EventHandler(this.FormCreateDataFolder_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ProgressBar pbCreateDataFolders;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnTargetBrowse;
		private System.Windows.Forms.TextBox tbTargetPath;
		private System.ComponentModel.BackgroundWorker bgwCreateDataFolders;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
	}
}