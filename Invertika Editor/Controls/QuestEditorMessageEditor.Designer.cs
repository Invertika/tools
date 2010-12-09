namespace Invertika_Editor.Controls
{
	partial class QuestEditorMessageEditor
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
			this.rbOneValue=new System.Windows.Forms.RadioButton();
			this.rbRandomValues=new System.Windows.Forms.RadioButton();
			this.tbOneValue=new System.Windows.Forms.TextBox();
			this.btnSave=new System.Windows.Forms.Button();
			this.rtbRandomValues=new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// rbOneValue
			// 
			this.rbOneValue.AutoSize=true;
			this.rbOneValue.Checked=true;
			this.rbOneValue.Location=new System.Drawing.Point(12, 12);
			this.rbOneValue.Name="rbOneValue";
			this.rbOneValue.Size=new System.Drawing.Size(100, 17);
			this.rbOneValue.TabIndex=1;
			this.rbOneValue.TabStop=true;
			this.rbOneValue.Text="Feste Nachricht";
			this.rbOneValue.UseVisualStyleBackColor=true;
			// 
			// rbRandomValues
			// 
			this.rbRandomValues.AutoSize=true;
			this.rbRandomValues.Location=new System.Drawing.Point(12, 61);
			this.rbRandomValues.Name="rbRandomValues";
			this.rbRandomValues.Size=new System.Drawing.Size(192, 17);
			this.rbRandomValues.TabIndex=2;
			this.rbRandomValues.Text="Zufällige Sätze (eine Zeile pro Satz)";
			this.rbRandomValues.UseVisualStyleBackColor=true;
			// 
			// tbOneValue
			// 
			this.tbOneValue.Location=new System.Drawing.Point(31, 35);
			this.tbOneValue.Name="tbOneValue";
			this.tbOneValue.Size=new System.Drawing.Size(549, 20);
			this.tbOneValue.TabIndex=3;
			// 
			// btnSave
			// 
			this.btnSave.Location=new System.Drawing.Point(505, 261);
			this.btnSave.Name="btnSave";
			this.btnSave.Size=new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex=4;
			this.btnSave.Text="Speichern";
			this.btnSave.UseVisualStyleBackColor=true;
			this.btnSave.Click+=new System.EventHandler(this.button1_Click);
			// 
			// rtbRandomValues
			// 
			this.rtbRandomValues.Location=new System.Drawing.Point(31, 84);
			this.rtbRandomValues.Name="rtbRandomValues";
			this.rtbRandomValues.Size=new System.Drawing.Size(549, 171);
			this.rtbRandomValues.TabIndex=5;
			this.rtbRandomValues.Text="";
			// 
			// QuestEditorMessageEditor
			// 
			this.AutoScaleDimensions=new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize=new System.Drawing.Size(592, 296);
			this.Controls.Add(this.rtbRandomValues);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.tbOneValue);
			this.Controls.Add(this.rbRandomValues);
			this.Controls.Add(this.rbOneValue);
			this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name="QuestEditorMessageEditor";
			this.StartPosition=System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text="QuestEditorMessageEditor";
			this.Load+=new System.EventHandler(this.QuestEditorMessageEditor_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RadioButton rbOneValue;
		private System.Windows.Forms.RadioButton rbRandomValues;
		private System.Windows.Forms.TextBox tbOneValue;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.RichTextBox rtbRandomValues;

	}
}