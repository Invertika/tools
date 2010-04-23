namespace Invertika_Editor
{
	partial class FormCoordinateCalculator
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
			System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager(typeof(FormCoordinateCalculator));
			this.nudXPixel=new System.Windows.Forms.NumericUpDown();
			this.nudYPixel=new System.Windows.Forms.NumericUpDown();
			this.btnCopy=new System.Windows.Forms.Button();
			this.tbXYPixel=new System.Windows.Forms.TextBox();
			this.rtbComments=new System.Windows.Forms.RichTextBox();
			this.label5=new System.Windows.Forms.Label();
			this.label3=new System.Windows.Forms.Label();
			this.label4=new System.Windows.Forms.Label();
			this.nudYTile=new System.Windows.Forms.NumericUpDown();
			this.nudXTile=new System.Windows.Forms.NumericUpDown();
			this.label2=new System.Windows.Forms.Label();
			this.label1=new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.nudXPixel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudYPixel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudYTile)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudXTile)).BeginInit();
			this.SuspendLayout();
			// 
			// nudXPixel
			// 
			this.nudXPixel.Location=new System.Drawing.Point(10, 75);
			this.nudXPixel.Maximum=new decimal(new int[] {
            100000,
            0,
            0,
            0});
			this.nudXPixel.Name="nudXPixel";
			this.nudXPixel.Size=new System.Drawing.Size(120, 20);
			this.nudXPixel.TabIndex=37;
			this.nudXPixel.ValueChanged+=new System.EventHandler(this.nudXPixel_ValueChanged);
			// 
			// nudYPixel
			// 
			this.nudYPixel.Location=new System.Drawing.Point(152, 75);
			this.nudYPixel.Maximum=new decimal(new int[] {
            100000,
            0,
            0,
            0});
			this.nudYPixel.Name="nudYPixel";
			this.nudYPixel.Size=new System.Drawing.Size(120, 20);
			this.nudYPixel.TabIndex=36;
			this.nudYPixel.ValueChanged+=new System.EventHandler(this.nudYPixel_ValueChanged);
			// 
			// btnCopy
			// 
			this.btnCopy.Location=new System.Drawing.Point(197, 127);
			this.btnCopy.Name="btnCopy";
			this.btnCopy.Size=new System.Drawing.Size(75, 23);
			this.btnCopy.TabIndex=35;
			this.btnCopy.Text="Kopieren ->";
			this.btnCopy.UseVisualStyleBackColor=true;
			this.btnCopy.Click+=new System.EventHandler(this.btnCopy_Click);
			// 
			// tbXYPixel
			// 
			this.tbXYPixel.Location=new System.Drawing.Point(10, 101);
			this.tbXYPixel.Name="tbXYPixel";
			this.tbXYPixel.ReadOnly=true;
			this.tbXYPixel.Size=new System.Drawing.Size(262, 20);
			this.tbXYPixel.TabIndex=34;
			// 
			// rtbComments
			// 
			this.rtbComments.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.rtbComments.Location=new System.Drawing.Point(294, 30);
			this.rtbComments.Name="rtbComments";
			this.rtbComments.Size=new System.Drawing.Size(656, 144);
			this.rtbComments.TabIndex=33;
			this.rtbComments.Text="";
			// 
			// label5
			// 
			this.label5.AutoSize=true;
			this.label5.Location=new System.Drawing.Point(291, 8);
			this.label5.Name="label5";
			this.label5.Size=new System.Drawing.Size(69, 13);
			this.label5.TabIndex=32;
			this.label5.Text="Kommentare:";
			// 
			// label3
			// 
			this.label3.AutoSize=true;
			this.label3.Location=new System.Drawing.Point(149, 59);
			this.label3.Name="label3";
			this.label3.Size=new System.Drawing.Size(48, 13);
			this.label3.TabIndex=31;
			this.label3.Text="Y (Pixel):";
			// 
			// label4
			// 
			this.label4.AutoSize=true;
			this.label4.Location=new System.Drawing.Point(7, 59);
			this.label4.Name="label4";
			this.label4.Size=new System.Drawing.Size(48, 13);
			this.label4.TabIndex=30;
			this.label4.Text="X (Pixel):";
			// 
			// nudYTile
			// 
			this.nudYTile.Location=new System.Drawing.Point(152, 30);
			this.nudYTile.Maximum=new decimal(new int[] {
            100000,
            0,
            0,
            0});
			this.nudYTile.Name="nudYTile";
			this.nudYTile.Size=new System.Drawing.Size(120, 20);
			this.nudYTile.TabIndex=29;
			this.nudYTile.ValueChanged+=new System.EventHandler(this.nudYTile_ValueChanged);
			// 
			// nudXTile
			// 
			this.nudXTile.Location=new System.Drawing.Point(10, 30);
			this.nudXTile.Maximum=new decimal(new int[] {
            100000,
            0,
            0,
            0});
			this.nudXTile.Name="nudXTile";
			this.nudXTile.Size=new System.Drawing.Size(120, 20);
			this.nudXTile.TabIndex=28;
			this.nudXTile.ValueChanged+=new System.EventHandler(this.nudXTile_ValueChanged);
			// 
			// label2
			// 
			this.label2.AutoSize=true;
			this.label2.Location=new System.Drawing.Point(149, 8);
			this.label2.Name="label2";
			this.label2.Size=new System.Drawing.Size(43, 13);
			this.label2.TabIndex=27;
			this.label2.Text="Y (Tile):";
			// 
			// label1
			// 
			this.label1.AutoSize=true;
			this.label1.Location=new System.Drawing.Point(7, 8);
			this.label1.Name="label1";
			this.label1.Size=new System.Drawing.Size(43, 13);
			this.label1.TabIndex=26;
			this.label1.Text="X (Tile):";
			// 
			// FormCoordinateCalculator
			// 
			this.AutoScaleDimensions=new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize=new System.Drawing.Size(954, 180);
			this.Controls.Add(this.nudXPixel);
			this.Controls.Add(this.nudYPixel);
			this.Controls.Add(this.btnCopy);
			this.Controls.Add(this.tbXYPixel);
			this.Controls.Add(this.rtbComments);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.nudYTile);
			this.Controls.Add(this.nudXTile);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon=((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name="FormCoordinateCalculator";
			this.StartPosition=System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text="Koordinatenrechner";
			((System.ComponentModel.ISupportInitialize)(this.nudXPixel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudYPixel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudYTile)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudXTile)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.NumericUpDown nudXPixel;
		private System.Windows.Forms.NumericUpDown nudYPixel;
		private System.Windows.Forms.Button btnCopy;
		private System.Windows.Forms.TextBox tbXYPixel;
		private System.Windows.Forms.RichTextBox rtbComments;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown nudYTile;
		private System.Windows.Forms.NumericUpDown nudXTile;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
	}
}