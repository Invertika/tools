namespace Invertika_Editor
{
	partial class FormCreateClientUpdate
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
			System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager(typeof(FormCreateClientUpdate));
			this.btnUpdateTargetfolderBrowse=new System.Windows.Forms.Button();
			this.tbUpdateTargetfolder=new System.Windows.Forms.TextBox();
			this.btnStartUpdate=new System.Windows.Forms.Button();
			this.btnUpdateDataLastClientBrowse=new System.Windows.Forms.Button();
			this.tbUpdateDataLastClient=new System.Windows.Forms.TextBox();
			this.label14=new System.Windows.Forms.Label();
			this.label15=new System.Windows.Forms.Label();
			this.folderBrowserDialog=new System.Windows.Forms.FolderBrowserDialog();
			this.SuspendLayout();
			// 
			// btnUpdateTargetfolderBrowse
			// 
			this.btnUpdateTargetfolderBrowse.Anchor=((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Right)));
			this.btnUpdateTargetfolderBrowse.Location=new System.Drawing.Point(531, 63);
			this.btnUpdateTargetfolderBrowse.Name="btnUpdateTargetfolderBrowse";
			this.btnUpdateTargetfolderBrowse.Size=new System.Drawing.Size(103, 21);
			this.btnUpdateTargetfolderBrowse.TabIndex=28;
			this.btnUpdateTargetfolderBrowse.Text="Durchsuchen...";
			this.btnUpdateTargetfolderBrowse.UseVisualStyleBackColor=true;
			this.btnUpdateTargetfolderBrowse.Click+=new System.EventHandler(this.btnUpdateTargetfolderBrowse_Click);
			// 
			// tbUpdateTargetfolder
			// 
			this.tbUpdateTargetfolder.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.tbUpdateTargetfolder.Location=new System.Drawing.Point(12, 64);
			this.tbUpdateTargetfolder.Name="tbUpdateTargetfolder";
			this.tbUpdateTargetfolder.ReadOnly=true;
			this.tbUpdateTargetfolder.Size=new System.Drawing.Size(513, 20);
			this.tbUpdateTargetfolder.TabIndex=27;
			// 
			// btnStartUpdate
			// 
			this.btnStartUpdate.Anchor=((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Right)));
			this.btnStartUpdate.Location=new System.Drawing.Point(531, 90);
			this.btnStartUpdate.Name="btnStartUpdate";
			this.btnStartUpdate.Size=new System.Drawing.Size(103, 23);
			this.btnStartUpdate.TabIndex=26;
			this.btnStartUpdate.Text="Start";
			this.btnStartUpdate.UseVisualStyleBackColor=true;
			this.btnStartUpdate.Click+=new System.EventHandler(this.btnStartUpdate_Click);
			// 
			// btnUpdateDataLastClientBrowse
			// 
			this.btnUpdateDataLastClientBrowse.Anchor=((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Right)));
			this.btnUpdateDataLastClientBrowse.Location=new System.Drawing.Point(531, 20);
			this.btnUpdateDataLastClientBrowse.Name="btnUpdateDataLastClientBrowse";
			this.btnUpdateDataLastClientBrowse.Size=new System.Drawing.Size(103, 21);
			this.btnUpdateDataLastClientBrowse.TabIndex=25;
			this.btnUpdateDataLastClientBrowse.Text="Durchsuchen...";
			this.btnUpdateDataLastClientBrowse.UseVisualStyleBackColor=true;
			this.btnUpdateDataLastClientBrowse.Click+=new System.EventHandler(this.btnUpdateDataLastClientBrowse_Click);
			// 
			// tbUpdateDataLastClient
			// 
			this.tbUpdateDataLastClient.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.tbUpdateDataLastClient.Location=new System.Drawing.Point(12, 21);
			this.tbUpdateDataLastClient.Name="tbUpdateDataLastClient";
			this.tbUpdateDataLastClient.ReadOnly=true;
			this.tbUpdateDataLastClient.Size=new System.Drawing.Size(513, 20);
			this.tbUpdateDataLastClient.TabIndex=23;
			// 
			// label14
			// 
			this.label14.AutoSize=true;
			this.label14.Location=new System.Drawing.Point(9, 5);
			this.label14.Name="label14";
			this.label14.Size=new System.Drawing.Size(201, 13);
			this.label14.TabIndex=21;
			this.label14.Text="Data Verzeichnis -> Letzte Client Version:";
			// 
			// label15
			// 
			this.label15.AutoSize=true;
			this.label15.Location=new System.Drawing.Point(9, 48);
			this.label15.Name="label15";
			this.label15.Size=new System.Drawing.Size(80, 13);
			this.label15.TabIndex=29;
			this.label15.Text="Zielverzeichnis:";
			// 
			// FormCreateClientUpdate
			// 
			this.AutoScaleDimensions=new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize=new System.Drawing.Size(646, 120);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.btnUpdateTargetfolderBrowse);
			this.Controls.Add(this.tbUpdateTargetfolder);
			this.Controls.Add(this.btnStartUpdate);
			this.Controls.Add(this.btnUpdateDataLastClientBrowse);
			this.Controls.Add(this.tbUpdateDataLastClient);
			this.Controls.Add(this.label14);
			this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon=((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name="FormCreateClientUpdate";
			this.StartPosition=System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text="Client Update erstellen";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnUpdateTargetfolderBrowse;
		private System.Windows.Forms.TextBox tbUpdateTargetfolder;
		private System.Windows.Forms.Button btnStartUpdate;
		private System.Windows.Forms.Button btnUpdateDataLastClientBrowse;
		private System.Windows.Forms.TextBox tbUpdateDataLastClient;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
	}
}