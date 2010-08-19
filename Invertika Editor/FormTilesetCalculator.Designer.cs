namespace Invertika_Editor
{
	partial class FormTilesetCalculator
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
			this.components=new System.ComponentModel.Container();
			this.imageList1=new System.Windows.Forms.ImageList(this.components);
			this.nudTileHeight=new System.Windows.Forms.NumericUpDown();
			this.label1=new System.Windows.Forms.Label();
			this.lblOutput=new System.Windows.Forms.Label();
			this.btnCalc=new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.nudTileHeight)).BeginInit();
			this.SuspendLayout();
			// 
			// imageList1
			// 
			this.imageList1.ColorDepth=System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageList1.ImageSize=new System.Drawing.Size(16, 16);
			this.imageList1.TransparentColor=System.Drawing.Color.Transparent;
			// 
			// nudTileHeight
			// 
			this.nudTileHeight.Location=new System.Drawing.Point(12, 25);
			this.nudTileHeight.Maximum=new decimal(new int[] {
            1024,
            0,
            0,
            0});
			this.nudTileHeight.Minimum=new decimal(new int[] {
            32,
            0,
            0,
            0});
			this.nudTileHeight.Name="nudTileHeight";
			this.nudTileHeight.Size=new System.Drawing.Size(161, 20);
			this.nudTileHeight.TabIndex=1;
			this.nudTileHeight.Value=new decimal(new int[] {
            32,
            0,
            0,
            0});
			this.nudTileHeight.ValueChanged+=new System.EventHandler(this.nudTileHeight_ValueChanged);
			// 
			// label1
			// 
			this.label1.AutoSize=true;
			this.label1.Location=new System.Drawing.Point(9, 9);
			this.label1.Name="label1";
			this.label1.Size=new System.Drawing.Size(109, 13);
			this.label1.TabIndex=2;
			this.label1.Text="Kachelhöhe (in Pixel):";
			// 
			// lblOutput
			// 
			this.lblOutput.AutoSize=true;
			this.lblOutput.Location=new System.Drawing.Point(9, 48);
			this.lblOutput.Name="lblOutput";
			this.lblOutput.Size=new System.Drawing.Size(155, 13);
			this.lblOutput.TabIndex=3;
			this.lblOutput.Text="Tilesetgröße: 1024 x 1024 Pixel";
			// 
			// btnCalc
			// 
			this.btnCalc.Location=new System.Drawing.Point(179, 25);
			this.btnCalc.Name="btnCalc";
			this.btnCalc.Size=new System.Drawing.Size(101, 20);
			this.btnCalc.TabIndex=4;
			this.btnCalc.Text="Berechnen";
			this.btnCalc.UseVisualStyleBackColor=true;
			this.btnCalc.Click+=new System.EventHandler(this.btnCalc_Click);
			// 
			// FormTilesetCalculator
			// 
			this.AutoScaleDimensions=new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize=new System.Drawing.Size(286, 68);
			this.Controls.Add(this.btnCalc);
			this.Controls.Add(this.lblOutput);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.nudTileHeight);
			this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name="FormTilesetCalculator";
			this.Text="Tilesetrechner";
			this.TopMost=true;
			((System.ComponentModel.ISupportInitialize)(this.nudTileHeight)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.NumericUpDown nudTileHeight;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblOutput;
		private System.Windows.Forms.Button btnCalc;
	}
}