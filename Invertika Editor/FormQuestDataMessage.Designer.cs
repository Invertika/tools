namespace Invertika_Editor
{
	partial class FormQuestDataMessage
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
			this.btnSave=new System.Windows.Forms.Button();
			this.rtbMessages=new System.Windows.Forms.RichTextBox();
			this.label1=new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnSave
			// 
			this.btnSave.Location=new System.Drawing.Point(513, 224);
			this.btnSave.Name="btnSave";
			this.btnSave.Size=new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex=6;
			this.btnSave.Text="Speichern";
			this.btnSave.UseVisualStyleBackColor=true;
			this.btnSave.Click+=new System.EventHandler(this.btnSave_Click);
			// 
			// rtbMessages
			// 
			this.rtbMessages.Location=new System.Drawing.Point(2, 1);
			this.rtbMessages.Name="rtbMessages";
			this.rtbMessages.Size=new System.Drawing.Size(586, 217);
			this.rtbMessages.TabIndex=7;
			this.rtbMessages.Text="";
			// 
			// label1
			// 
			this.label1.AutoSize=true;
			this.label1.Location=new System.Drawing.Point(12, 233);
			this.label1.Name="label1";
			this.label1.Size=new System.Drawing.Size(419, 13);
			this.label1.TabIndex=8;
			this.label1.Text="Hinweis: Eine Nachricht pro Zeile. Bei mehr als einer Nachricht wird zufällig aus"+
				"gewählt.";
			// 
			// FormQuestDataMessage
			// 
			this.AutoScaleDimensions=new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize=new System.Drawing.Size(594, 255);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.rtbMessages);
			this.Controls.Add(this.btnSave);
			this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name="FormQuestDataMessage";
			this.StartPosition=System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text="Nachrichteneditor";
			this.Load+=new System.EventHandler(this.FormQuestDataMessage_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.RichTextBox rtbMessages;
		private System.Windows.Forms.Label label1;
	}
}