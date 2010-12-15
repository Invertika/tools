namespace Invertika_Editor
{
	partial class FormNPCGenerator
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
			System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager(typeof(FormNPCGenerator));
			this.label23=new System.Windows.Forms.Label();
			this.rtbScriptOutput=new System.Windows.Forms.RichTextBox();
			this.label21=new System.Windows.Forms.Label();
			this.rtbSentences=new System.Windows.Forms.RichTextBox();
			this.label20=new System.Windows.Forms.Label();
			this.tbNPCName=new System.Windows.Forms.TextBox();
			this.label16=new System.Windows.Forms.Label();
			this.label17=new System.Windows.Forms.Label();
			this.nudPosX=new System.Windows.Forms.NumericUpDown();
			this.label18=new System.Windows.Forms.Label();
			this.nudPosY=new System.Windows.Forms.NumericUpDown();
			this.label19=new System.Windows.Forms.Label();
			this.nudNPCID=new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.nudPosX)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudPosY)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudNPCID)).BeginInit();
			this.SuspendLayout();
			// 
			// label23
			// 
			this.label23.AutoSize=true;
			this.label23.Font=new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label23.ForeColor=System.Drawing.SystemColors.ActiveCaption;
			this.label23.Location=new System.Drawing.Point(0, 430);
			this.label23.Name="label23";
			this.label23.Size=new System.Drawing.Size(123, 13);
			this.label23.TabIndex=25;
			this.label23.Text="http://invertika.org/NPC";
			this.label23.Click+=new System.EventHandler(this.label23_Click);
			// 
			// rtbScriptOutput
			// 
			this.rtbScriptOutput.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.rtbScriptOutput.Location=new System.Drawing.Point(3, 247);
			this.rtbScriptOutput.Name="rtbScriptOutput";
			this.rtbScriptOutput.ReadOnly=true;
			this.rtbScriptOutput.Size=new System.Drawing.Size(1251, 180);
			this.rtbScriptOutput.TabIndex=24;
			this.rtbScriptOutput.Text="";
			// 
			// label21
			// 
			this.label21.AutoSize=true;
			this.label21.Location=new System.Drawing.Point(0, 231);
			this.label21.Name="label21";
			this.label21.Size=new System.Drawing.Size(78, 13);
			this.label21.TabIndex=23;
			this.label21.Text="Skriptausgabe:";
			// 
			// rtbSentences
			// 
			this.rtbSentences.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.rtbSentences.Location=new System.Drawing.Point(3, 61);
			this.rtbSentences.Name="rtbSentences";
			this.rtbSentences.Size=new System.Drawing.Size(1251, 164);
			this.rtbSentences.TabIndex=22;
			this.rtbSentences.Text="";
			this.rtbSentences.TextChanged+=new System.EventHandler(this.rtbSentences_TextChanged);
			// 
			// label20
			// 
			this.label20.AutoSize=true;
			this.label20.Location=new System.Drawing.Point(0, 45);
			this.label20.Name="label20";
			this.label20.Size=new System.Drawing.Size(218, 13);
			this.label20.TabIndex=21;
			this.label20.Text="Zu sagende Sätze (Mit einem Enter trennen):";
			// 
			// tbNPCName
			// 
			this.tbNPCName.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.tbNPCName.Location=new System.Drawing.Point(3, 22);
			this.tbNPCName.Name="tbNPCName";
			this.tbNPCName.Size=new System.Drawing.Size(859, 20);
			this.tbNPCName.TabIndex=13;
			this.tbNPCName.TextChanged+=new System.EventHandler(this.tbNPCName_TextChanged);
			// 
			// label16
			// 
			this.label16.AutoSize=true;
			this.label16.Location=new System.Drawing.Point(0, 6);
			this.label16.Name="label16";
			this.label16.Size=new System.Drawing.Size(38, 13);
			this.label16.TabIndex=14;
			this.label16.Text="Name:";
			// 
			// label17
			// 
			this.label17.Anchor=((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Right)));
			this.label17.AutoSize=true;
			this.label17.Location=new System.Drawing.Point(997, 5);
			this.label17.Name="label17";
			this.label17.Size=new System.Drawing.Size(127, 13);
			this.label17.TabIndex=15;
			this.label17.Text="Position X (in Tilegrößen):";
			// 
			// nudPosX
			// 
			this.nudPosX.Anchor=((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Right)));
			this.nudPosX.Location=new System.Drawing.Point(999, 22);
			this.nudPosX.Maximum=new decimal(new int[] {
            200,
            0,
            0,
            0});
			this.nudPosX.Name="nudPosX";
			this.nudPosX.Size=new System.Drawing.Size(125, 20);
			this.nudPosX.TabIndex=16;
			this.nudPosX.Value=new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.nudPosX.ValueChanged+=new System.EventHandler(this.nudPosX_ValueChanged);
			// 
			// label18
			// 
			this.label18.Anchor=((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Right)));
			this.label18.AutoSize=true;
			this.label18.Location=new System.Drawing.Point(1128, 5);
			this.label18.Name="label18";
			this.label18.Size=new System.Drawing.Size(127, 13);
			this.label18.TabIndex=17;
			this.label18.Text="Position Y (in Tilegrößen):";
			// 
			// nudPosY
			// 
			this.nudPosY.Anchor=((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Right)));
			this.nudPosY.Location=new System.Drawing.Point(1130, 22);
			this.nudPosY.Maximum=new decimal(new int[] {
            200,
            0,
            0,
            0});
			this.nudPosY.Name="nudPosY";
			this.nudPosY.Size=new System.Drawing.Size(125, 20);
			this.nudPosY.TabIndex=18;
			this.nudPosY.Value=new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.nudPosY.ValueChanged+=new System.EventHandler(this.nudPosY_ValueChanged);
			// 
			// label19
			// 
			this.label19.Anchor=((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Right)));
			this.label19.AutoSize=true;
			this.label19.Location=new System.Drawing.Point(866, 5);
			this.label19.Name="label19";
			this.label19.Size=new System.Drawing.Size(46, 13);
			this.label19.TabIndex=19;
			this.label19.Text="NPC ID:";
			// 
			// nudNPCID
			// 
			this.nudNPCID.Anchor=((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Right)));
			this.nudNPCID.Location=new System.Drawing.Point(868, 22);
			this.nudNPCID.Maximum=new decimal(new int[] {
            200,
            0,
            0,
            0});
			this.nudNPCID.Name="nudNPCID";
			this.nudNPCID.Size=new System.Drawing.Size(125, 20);
			this.nudNPCID.TabIndex=20;
			this.nudNPCID.Value=new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.nudNPCID.ValueChanged+=new System.EventHandler(this.nudNPCID_ValueChanged);
			// 
			// FormNPCGenerator
			// 
			this.AutoScaleDimensions=new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize=new System.Drawing.Size(1266, 453);
			this.Controls.Add(this.label23);
			this.Controls.Add(this.rtbScriptOutput);
			this.Controls.Add(this.label21);
			this.Controls.Add(this.rtbSentences);
			this.Controls.Add(this.label20);
			this.Controls.Add(this.nudNPCID);
			this.Controls.Add(this.label19);
			this.Controls.Add(this.nudPosY);
			this.Controls.Add(this.label18);
			this.Controls.Add(this.nudPosX);
			this.Controls.Add(this.label17);
			this.Controls.Add(this.label16);
			this.Controls.Add(this.tbNPCName);
			this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon=((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name="FormNPCGenerator";
			this.StartPosition=System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text="NPC Generator";
			((System.ComponentModel.ISupportInitialize)(this.nudPosX)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudPosY)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudNPCID)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.RichTextBox rtbScriptOutput;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.RichTextBox rtbSentences;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.TextBox tbNPCName;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.NumericUpDown nudPosX;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.NumericUpDown nudPosY;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.NumericUpDown nudNPCID;

	}
}