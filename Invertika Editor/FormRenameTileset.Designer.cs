namespace Invertika_Editor
{
	partial class FormRenameTileset
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
			this.lbCurrentTilesets=new System.Windows.Forms.ListBox();
			this.label1=new System.Windows.Forms.Label();
			this.tbNewName=new System.Windows.Forms.TextBox();
			this.pbMain=new System.Windows.Forms.ProgressBar();
			this.btnCalc=new System.Windows.Forms.Button();
			this.backgroundWorker=new System.ComponentModel.BackgroundWorker();
			this.SuspendLayout();
			// 
			// lbCurrentTilesets
			// 
			this.lbCurrentTilesets.Dock=System.Windows.Forms.DockStyle.Top;
			this.lbCurrentTilesets.FormattingEnabled=true;
			this.lbCurrentTilesets.Location=new System.Drawing.Point(0, 0);
			this.lbCurrentTilesets.Name="lbCurrentTilesets";
			this.lbCurrentTilesets.Size=new System.Drawing.Size(363, 238);
			this.lbCurrentTilesets.TabIndex=0;
			// 
			// label1
			// 
			this.label1.AutoSize=true;
			this.label1.Location=new System.Drawing.Point(-3, 241);
			this.label1.Name="label1";
			this.label1.Size=new System.Drawing.Size(70, 13);
			this.label1.TabIndex=1;
			this.label1.Text="Neuer Name:";
			// 
			// tbNewName
			// 
			this.tbNewName.Location=new System.Drawing.Point(0, 257);
			this.tbNewName.Name="tbNewName";
			this.tbNewName.Size=new System.Drawing.Size(363, 20);
			this.tbNewName.TabIndex=2;
			// 
			// pbMain
			// 
			this.pbMain.Location=new System.Drawing.Point(0, 283);
			this.pbMain.Name="pbMain";
			this.pbMain.Size=new System.Drawing.Size(363, 23);
			this.pbMain.TabIndex=3;
			// 
			// btnCalc
			// 
			this.btnCalc.Location=new System.Drawing.Point(0, 312);
			this.btnCalc.Name="btnCalc";
			this.btnCalc.Size=new System.Drawing.Size(363, 23);
			this.btnCalc.TabIndex=4;
			this.btnCalc.Text="Umbenennen";
			this.btnCalc.UseVisualStyleBackColor=true;
			this.btnCalc.Click+=new System.EventHandler(this.btnCalc_Click);
			// 
			// backgroundWorker
			// 
			this.backgroundWorker.WorkerReportsProgress=true;
			this.backgroundWorker.DoWork+=new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
			this.backgroundWorker.RunWorkerCompleted+=new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
			this.backgroundWorker.ProgressChanged+=new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
			// 
			// FormRenameTileset
			// 
			this.AutoScaleDimensions=new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize=new System.Drawing.Size(363, 340);
			this.Controls.Add(this.btnCalc);
			this.Controls.Add(this.pbMain);
			this.Controls.Add(this.tbNewName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lbCurrentTilesets);
			this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name="FormRenameTileset";
			this.StartPosition=System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text="Tileset umbenennen";
			this.Load+=new System.EventHandler(this.FormRenameTileset_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox lbCurrentTilesets;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbNewName;
		private System.Windows.Forms.ProgressBar pbMain;
		private System.Windows.Forms.Button btnCalc;
		private System.ComponentModel.BackgroundWorker backgroundWorker;
	}
}