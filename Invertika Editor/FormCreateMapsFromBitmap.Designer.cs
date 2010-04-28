namespace Invertika_Editor
{
	partial class FormCreateMapsFromBitmap
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
			this.tbBitmap=new System.Windows.Forms.TextBox();
			this.tbTargetPath=new System.Windows.Forms.TextBox();
			this.label1=new System.Windows.Forms.Label();
			this.label2=new System.Windows.Forms.Label();
			this.label3=new System.Windows.Forms.Label();
			this.label4=new System.Windows.Forms.Label();
			this.btnBrowseBitmap=new System.Windows.Forms.Button();
			this.btnBrowseTargetPath=new System.Windows.Forms.Button();
			this.progressBar=new System.Windows.Forms.ProgressBar();
			this.nudXmin=new System.Windows.Forms.NumericUpDown();
			this.nudYmax=new System.Windows.Forms.NumericUpDown();
			this.btnStartProcess=new System.Windows.Forms.Button();
			this.backgroundWorker=new System.ComponentModel.BackgroundWorker();
			this.openFileDialog=new System.Windows.Forms.OpenFileDialog();
			this.folderBrowserDialog=new System.Windows.Forms.FolderBrowserDialog();
			((System.ComponentModel.ISupportInitialize)(this.nudXmin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudYmax)).BeginInit();
			this.SuspendLayout();
			// 
			// tbBitmap
			// 
			this.tbBitmap.Location=new System.Drawing.Point(12, 25);
			this.tbBitmap.Name="tbBitmap";
			this.tbBitmap.ReadOnly=true;
			this.tbBitmap.Size=new System.Drawing.Size(407, 20);
			this.tbBitmap.TabIndex=0;
			// 
			// tbTargetPath
			// 
			this.tbTargetPath.Location=new System.Drawing.Point(12, 64);
			this.tbTargetPath.Name="tbTargetPath";
			this.tbTargetPath.ReadOnly=true;
			this.tbTargetPath.Size=new System.Drawing.Size(407, 20);
			this.tbTargetPath.TabIndex=1;
			// 
			// label1
			// 
			this.label1.AutoSize=true;
			this.label1.Location=new System.Drawing.Point(12, 9);
			this.label1.Name="label1";
			this.label1.Size=new System.Drawing.Size(42, 13);
			this.label1.TabIndex=2;
			this.label1.Text="Bitmap:";
			// 
			// label2
			// 
			this.label2.AutoSize=true;
			this.label2.Location=new System.Drawing.Point(12, 48);
			this.label2.Name="label2";
			this.label2.Size=new System.Drawing.Size(48, 13);
			this.label2.TabIndex=3;
			this.label2.Text="Zielpfad:";
			// 
			// label3
			// 
			this.label3.AutoSize=true;
			this.label3.Location=new System.Drawing.Point(12, 87);
			this.label3.Name="label3";
			this.label3.Size=new System.Drawing.Size(31, 13);
			this.label3.TabIndex=4;
			this.label3.Text="x-min";
			// 
			// label4
			// 
			this.label4.AutoSize=true;
			this.label4.Location=new System.Drawing.Point(211, 87);
			this.label4.Name="label4";
			this.label4.Size=new System.Drawing.Size(34, 13);
			this.label4.TabIndex=5;
			this.label4.Text="y-max";
			// 
			// btnBrowseBitmap
			// 
			this.btnBrowseBitmap.Location=new System.Drawing.Point(425, 25);
			this.btnBrowseBitmap.Name="btnBrowseBitmap";
			this.btnBrowseBitmap.Size=new System.Drawing.Size(136, 20);
			this.btnBrowseBitmap.TabIndex=6;
			this.btnBrowseBitmap.Text="Durchsuchen...";
			this.btnBrowseBitmap.UseVisualStyleBackColor=true;
			this.btnBrowseBitmap.Click+=new System.EventHandler(this.btnBrowseBitmap_Click);
			// 
			// btnBrowseTargetPath
			// 
			this.btnBrowseTargetPath.Location=new System.Drawing.Point(425, 64);
			this.btnBrowseTargetPath.Name="btnBrowseTargetPath";
			this.btnBrowseTargetPath.Size=new System.Drawing.Size(136, 20);
			this.btnBrowseTargetPath.TabIndex=7;
			this.btnBrowseTargetPath.Text="Durchsuchen...";
			this.btnBrowseTargetPath.UseVisualStyleBackColor=true;
			this.btnBrowseTargetPath.Click+=new System.EventHandler(this.btnBrowseTargetPath_Click);
			// 
			// progressBar
			// 
			this.progressBar.Location=new System.Drawing.Point(15, 129);
			this.progressBar.Name="progressBar";
			this.progressBar.Size=new System.Drawing.Size(404, 23);
			this.progressBar.TabIndex=8;
			// 
			// nudXmin
			// 
			this.nudXmin.Location=new System.Drawing.Point(12, 103);
			this.nudXmin.Minimum=new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
			this.nudXmin.Name="nudXmin";
			this.nudXmin.Size=new System.Drawing.Size(196, 20);
			this.nudXmin.TabIndex=9;
			this.nudXmin.Value=new decimal(new int[] {
            25,
            0,
            0,
            -2147483648});
			// 
			// nudYmax
			// 
			this.nudYmax.Location=new System.Drawing.Point(214, 103);
			this.nudYmax.Minimum=new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
			this.nudYmax.Name="nudYmax";
			this.nudYmax.Size=new System.Drawing.Size(205, 20);
			this.nudYmax.TabIndex=10;
			this.nudYmax.Value=new decimal(new int[] {
            25,
            0,
            0,
            0});
			// 
			// btnStartProcess
			// 
			this.btnStartProcess.Location=new System.Drawing.Point(425, 129);
			this.btnStartProcess.Name="btnStartProcess";
			this.btnStartProcess.Size=new System.Drawing.Size(136, 23);
			this.btnStartProcess.TabIndex=11;
			this.btnStartProcess.Text="Prozess starten";
			this.btnStartProcess.UseVisualStyleBackColor=true;
			this.btnStartProcess.Click+=new System.EventHandler(this.btnStartProcess_Click);
			// 
			// backgroundWorker
			// 
			this.backgroundWorker.WorkerReportsProgress=true;
			this.backgroundWorker.DoWork+=new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
			this.backgroundWorker.RunWorkerCompleted+=new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
			this.backgroundWorker.ProgressChanged+=new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
			// 
			// openFileDialog
			// 
			this.openFileDialog.FileName="openFileDialog1";
			// 
			// FormCreateMapsFromBitmap
			// 
			this.AutoScaleDimensions=new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize=new System.Drawing.Size(568, 162);
			this.Controls.Add(this.btnStartProcess);
			this.Controls.Add(this.nudYmax);
			this.Controls.Add(this.nudXmin);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.btnBrowseTargetPath);
			this.Controls.Add(this.btnBrowseBitmap);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tbTargetPath);
			this.Controls.Add(this.tbBitmap);
			this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name="FormCreateMapsFromBitmap";
			this.StartPosition=System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text="Maps aus Bitmap erzeugen";
			((System.ComponentModel.ISupportInitialize)(this.nudXmin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudYmax)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbBitmap;
		private System.Windows.Forms.TextBox tbTargetPath;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnBrowseBitmap;
		private System.Windows.Forms.Button btnBrowseTargetPath;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.NumericUpDown nudXmin;
		private System.Windows.Forms.NumericUpDown nudYmax;
		private System.Windows.Forms.Button btnStartProcess;
		private System.ComponentModel.BackgroundWorker backgroundWorker;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
	}
}