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
			this.tbNachricht=new System.Windows.Forms.TextBox();
			this.rbRandomMessage=new System.Windows.Forms.RadioButton();
			this.rbOnlyOneMessage=new System.Windows.Forms.RadioButton();
			this.btnSave=new System.Windows.Forms.Button();
			this.rtbMessages=new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// tbNachricht
			// 
			this.tbNachricht.Location=new System.Drawing.Point(27, 35);
			this.tbNachricht.Name="tbNachricht";
			this.tbNachricht.Size=new System.Drawing.Size(561, 20);
			this.tbNachricht.TabIndex=1;
			// 
			// rbRandomMessage
			// 
			this.rbRandomMessage.AutoSize=true;
			this.rbRandomMessage.Checked=true;
			this.rbRandomMessage.Location=new System.Drawing.Point(12, 61);
			this.rbRandomMessage.Name="rbRandomMessage";
			this.rbRandomMessage.Size=new System.Drawing.Size(234, 17);
			this.rbRandomMessage.TabIndex=4;
			this.rbRandomMessage.TabStop=true;
			this.rbRandomMessage.Text="zufällige Nachricht (pro Nachricht eine Zeile)";
			this.rbRandomMessage.UseVisualStyleBackColor=true;
			// 
			// rbOnlyOneMessage
			// 
			this.rbOnlyOneMessage.AutoSize=true;
			this.rbOnlyOneMessage.Location=new System.Drawing.Point(12, 12);
			this.rbOnlyOneMessage.Name="rbOnlyOneMessage";
			this.rbOnlyOneMessage.Size=new System.Drawing.Size(94, 17);
			this.rbOnlyOneMessage.TabIndex=5;
			this.rbOnlyOneMessage.Text="eine Nachricht";
			this.rbOnlyOneMessage.UseVisualStyleBackColor=true;
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
			this.rtbMessages.Location=new System.Drawing.Point(27, 84);
			this.rtbMessages.Name="rtbMessages";
			this.rtbMessages.Size=new System.Drawing.Size(561, 134);
			this.rtbMessages.TabIndex=7;
			this.rtbMessages.Text="";
			// 
			// FormQuestDataMessage
			// 
			this.AutoScaleDimensions=new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize=new System.Drawing.Size(594, 255);
			this.Controls.Add(this.rtbMessages);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.rbOnlyOneMessage);
			this.Controls.Add(this.rbRandomMessage);
			this.Controls.Add(this.tbNachricht);
			this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name="FormQuestDataMessage";
			this.StartPosition=System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text="Nachrichteneditor";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbNachricht;
		private System.Windows.Forms.RadioButton rbRandomMessage;
		private System.Windows.Forms.RadioButton rbOnlyOneMessage;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.RichTextBox rtbMessages;
	}
}