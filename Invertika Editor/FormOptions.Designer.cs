namespace Invertika_Editor
{
	partial class FormOptions
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
			System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager(typeof(FormOptions));
			this.tabControl1=new System.Windows.Forms.TabControl();
			this.tabPage1=new System.Windows.Forms.TabPage();
			this.btnRepositoryTrunkBrowse=new System.Windows.Forms.Button();
			this.tbRepositoryTrunk=new System.Windows.Forms.TextBox();
			this.label1=new System.Windows.Forms.Label();
			this.tabPage2=new System.Windows.Forms.TabPage();
			this.tbFTPWorldmapPasswort=new System.Windows.Forms.TextBox();
			this.tbFTPWorldmapFolder=new System.Windows.Forms.TextBox();
			this.tbFTPWorldmapUser=new System.Windows.Forms.TextBox();
			this.tbFTPWorldmapServer=new System.Windows.Forms.TextBox();
			this.label12=new System.Windows.Forms.Label();
			this.label11=new System.Windows.Forms.Label();
			this.label10=new System.Windows.Forms.Label();
			this.label9=new System.Windows.Forms.Label();
			this.btnOK=new System.Windows.Forms.Button();
			this.btnCancel=new System.Windows.Forms.Button();
			this.folderBrowserDialog=new System.Windows.Forms.FolderBrowserDialog();
			this.tabPage3=new System.Windows.Forms.TabPage();
			this.tbMediawikiPassword=new System.Windows.Forms.TextBox();
			this.tbMediawikiURL=new System.Windows.Forms.TextBox();
			this.tbMediawikiUsername=new System.Windows.Forms.TextBox();
			this.label2=new System.Windows.Forms.Label();
			this.label3=new System.Windows.Forms.Label();
			this.label4=new System.Windows.Forms.Label();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Dock=System.Windows.Forms.DockStyle.Top;
			this.tabControl1.Location=new System.Drawing.Point(0, 0);
			this.tabControl1.Name="tabControl1";
			this.tabControl1.SelectedIndex=0;
			this.tabControl1.Size=new System.Drawing.Size(579, 187);
			this.tabControl1.TabIndex=0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.btnRepositoryTrunkBrowse);
			this.tabPage1.Controls.Add(this.tbRepositoryTrunk);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Location=new System.Drawing.Point(4, 22);
			this.tabPage1.Name="tabPage1";
			this.tabPage1.Padding=new System.Windows.Forms.Padding(3);
			this.tabPage1.Size=new System.Drawing.Size(571, 161);
			this.tabPage1.TabIndex=0;
			this.tabPage1.Text="Pfade";
			this.tabPage1.UseVisualStyleBackColor=true;
			// 
			// btnRepositoryTrunkBrowse
			// 
			this.btnRepositoryTrunkBrowse.Location=new System.Drawing.Point(463, 19);
			this.btnRepositoryTrunkBrowse.Name="btnRepositoryTrunkBrowse";
			this.btnRepositoryTrunkBrowse.Size=new System.Drawing.Size(100, 21);
			this.btnRepositoryTrunkBrowse.TabIndex=2;
			this.btnRepositoryTrunkBrowse.Text="Durchsuchen...";
			this.btnRepositoryTrunkBrowse.UseVisualStyleBackColor=true;
			this.btnRepositoryTrunkBrowse.Click+=new System.EventHandler(this.btnRepositoryTrunkBrowse_Click);
			// 
			// tbRepositoryTrunk
			// 
			this.tbRepositoryTrunk.Location=new System.Drawing.Point(11, 19);
			this.tbRepositoryTrunk.Name="tbRepositoryTrunk";
			this.tbRepositoryTrunk.ReadOnly=true;
			this.tbRepositoryTrunk.Size=new System.Drawing.Size(446, 20);
			this.tbRepositoryTrunk.TabIndex=1;
			// 
			// label1
			// 
			this.label1.AutoSize=true;
			this.label1.Location=new System.Drawing.Point(8, 3);
			this.label1.Name="label1";
			this.label1.Size=new System.Drawing.Size(175, 13);
			this.label1.TabIndex=0;
			this.label1.Text="Trunk Pfad des Repositories (lokal):";
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.tbFTPWorldmapPasswort);
			this.tabPage2.Controls.Add(this.tbFTPWorldmapFolder);
			this.tabPage2.Controls.Add(this.tbFTPWorldmapUser);
			this.tabPage2.Controls.Add(this.tbFTPWorldmapServer);
			this.tabPage2.Controls.Add(this.label12);
			this.tabPage2.Controls.Add(this.label11);
			this.tabPage2.Controls.Add(this.label10);
			this.tabPage2.Controls.Add(this.label9);
			this.tabPage2.Location=new System.Drawing.Point(4, 22);
			this.tabPage2.Name="tabPage2";
			this.tabPage2.Padding=new System.Windows.Forms.Padding(3);
			this.tabPage2.Size=new System.Drawing.Size(571, 161);
			this.tabPage2.TabIndex=1;
			this.tabPage2.Text="FTP (Weltkarte)";
			this.tabPage2.UseVisualStyleBackColor=true;
			// 
			// tbFTPWorldmapPasswort
			// 
			this.tbFTPWorldmapPasswort.Location=new System.Drawing.Point(284, 66);
			this.tbFTPWorldmapPasswort.Name="tbFTPWorldmapPasswort";
			this.tbFTPWorldmapPasswort.PasswordChar='*';
			this.tbFTPWorldmapPasswort.Size=new System.Drawing.Size(279, 20);
			this.tbFTPWorldmapPasswort.TabIndex=37;
			// 
			// tbFTPWorldmapFolder
			// 
			this.tbFTPWorldmapFolder.Location=new System.Drawing.Point(284, 24);
			this.tbFTPWorldmapFolder.Name="tbFTPWorldmapFolder";
			this.tbFTPWorldmapFolder.Size=new System.Drawing.Size(279, 20);
			this.tbFTPWorldmapFolder.TabIndex=36;
			// 
			// tbFTPWorldmapUser
			// 
			this.tbFTPWorldmapUser.Location=new System.Drawing.Point(8, 66);
			this.tbFTPWorldmapUser.Name="tbFTPWorldmapUser";
			this.tbFTPWorldmapUser.Size=new System.Drawing.Size(270, 20);
			this.tbFTPWorldmapUser.TabIndex=35;
			// 
			// tbFTPWorldmapServer
			// 
			this.tbFTPWorldmapServer.Location=new System.Drawing.Point(8, 24);
			this.tbFTPWorldmapServer.Name="tbFTPWorldmapServer";
			this.tbFTPWorldmapServer.Size=new System.Drawing.Size(270, 20);
			this.tbFTPWorldmapServer.TabIndex=34;
			// 
			// label12
			// 
			this.label12.AutoSize=true;
			this.label12.Location=new System.Drawing.Point(281, 50);
			this.label12.Name="label12";
			this.label12.Size=new System.Drawing.Size(76, 13);
			this.label12.TabIndex=33;
			this.label12.Text="FTP Passwort:";
			// 
			// label11
			// 
			this.label11.AutoSize=true;
			this.label11.Location=new System.Drawing.Point(5, 50);
			this.label11.Name="label11";
			this.label11.Size=new System.Drawing.Size(55, 13);
			this.label11.TabIndex=32;
			this.label11.Text="FTP User:";
			// 
			// label10
			// 
			this.label10.AutoSize=true;
			this.label10.Location=new System.Drawing.Point(281, 8);
			this.label10.Name="label10";
			this.label10.Size=new System.Drawing.Size(87, 13);
			this.label10.TabIndex=31;
			this.label10.Text="FTP Verzeichnis:";
			// 
			// label9
			// 
			this.label9.AutoSize=true;
			this.label9.Location=new System.Drawing.Point(5, 8);
			this.label9.Name="label9";
			this.label9.Size=new System.Drawing.Size(64, 13);
			this.label9.TabIndex=30;
			this.label9.Text="FTP Server:";
			// 
			// btnOK
			// 
			this.btnOK.Location=new System.Drawing.Point(419, 193);
			this.btnOK.Name="btnOK";
			this.btnOK.Size=new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex=1;
			this.btnOK.Text="OK";
			this.btnOK.UseVisualStyleBackColor=true;
			this.btnOK.Click+=new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult=System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location=new System.Drawing.Point(500, 193);
			this.btnCancel.Name="btnCancel";
			this.btnCancel.Size=new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex=2;
			this.btnCancel.Text="Abbrechen";
			this.btnCancel.UseVisualStyleBackColor=true;
			this.btnCancel.Click+=new System.EventHandler(this.btnCancel_Click);
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.tbMediawikiPassword);
			this.tabPage3.Controls.Add(this.tbMediawikiURL);
			this.tabPage3.Controls.Add(this.tbMediawikiUsername);
			this.tabPage3.Controls.Add(this.label2);
			this.tabPage3.Controls.Add(this.label3);
			this.tabPage3.Controls.Add(this.label4);
			this.tabPage3.Location=new System.Drawing.Point(4, 22);
			this.tabPage3.Name="tabPage3";
			this.tabPage3.Padding=new System.Windows.Forms.Padding(3);
			this.tabPage3.Size=new System.Drawing.Size(571, 161);
			this.tabPage3.TabIndex=2;
			this.tabPage3.Text="Wiki";
			this.tabPage3.UseVisualStyleBackColor=true;
			// 
			// tbMediawikiPassword
			// 
			this.tbMediawikiPassword.Location=new System.Drawing.Point(286, 67);
			this.tbMediawikiPassword.Name="tbMediawikiPassword";
			this.tbMediawikiPassword.PasswordChar='*';
			this.tbMediawikiPassword.Size=new System.Drawing.Size(279, 20);
			this.tbMediawikiPassword.TabIndex=42;
			// 
			// tbMediawikiURL
			// 
			this.tbMediawikiURL.Location=new System.Drawing.Point(8, 24);
			this.tbMediawikiURL.Name="tbMediawikiURL";
			this.tbMediawikiURL.Size=new System.Drawing.Size(270, 20);
			this.tbMediawikiURL.TabIndex=41;
			// 
			// tbMediawikiUsername
			// 
			this.tbMediawikiUsername.Location=new System.Drawing.Point(10, 67);
			this.tbMediawikiUsername.Name="tbMediawikiUsername";
			this.tbMediawikiUsername.Size=new System.Drawing.Size(270, 20);
			this.tbMediawikiUsername.TabIndex=40;
			// 
			// label2
			// 
			this.label2.AutoSize=true;
			this.label2.Location=new System.Drawing.Point(5, 8);
			this.label2.Name="label2";
			this.label2.Size=new System.Drawing.Size(82, 13);
			this.label2.TabIndex=39;
			this.label2.Text="Mediawiki URL:";
			// 
			// label3
			// 
			this.label3.AutoSize=true;
			this.label3.Location=new System.Drawing.Point(283, 51);
			this.label3.Name="label3";
			this.label3.Size=new System.Drawing.Size(53, 13);
			this.label3.TabIndex=38;
			this.label3.Text="Passwort:";
			// 
			// label4
			// 
			this.label4.AutoSize=true;
			this.label4.Location=new System.Drawing.Point(7, 51);
			this.label4.Name="label4";
			this.label4.Size=new System.Drawing.Size(67, 13);
			this.label4.TabIndex=37;
			this.label4.Text="Nutzername:";
			// 
			// FormOptions
			// 
			this.AcceptButton=this.btnOK;
			this.AutoScaleDimensions=new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton=this.btnCancel;
			this.ClientSize=new System.Drawing.Size(579, 223);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.tabControl1);
			this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon=((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox=false;
			this.MinimizeBox=false;
			this.Name="FormOptions";
			this.StartPosition=System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text="Optionen";
			this.Load+=new System.EventHandler(this.FormOptions_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.tabPage3.ResumeLayout(false);
			this.tabPage3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnRepositoryTrunkBrowse;
		private System.Windows.Forms.TextBox tbRepositoryTrunk;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TextBox tbFTPWorldmapPasswort;
		private System.Windows.Forms.TextBox tbFTPWorldmapFolder;
		private System.Windows.Forms.TextBox tbFTPWorldmapUser;
		private System.Windows.Forms.TextBox tbFTPWorldmapServer;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.TextBox tbMediawikiPassword;
		private System.Windows.Forms.TextBox tbMediawikiURL;
		private System.Windows.Forms.TextBox tbMediawikiUsername;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
	}
}