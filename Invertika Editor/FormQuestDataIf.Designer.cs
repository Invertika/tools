namespace Invertika_Editor
{
	partial class FormQuestDataIf
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
			this.rbVariable=new System.Windows.Forms.RadioButton();
			this.rbTime=new System.Windows.Forms.RadioButton();
			this.cbVarType=new System.Windows.Forms.ComboBox();
			this.label1=new System.Windows.Forms.Label();
			this.tbVarName=new System.Windows.Forms.TextBox();
			this.label2=new System.Windows.Forms.Label();
			this.label3=new System.Windows.Forms.Label();
			this.cbVarOperator=new System.Windows.Forms.ComboBox();
			this.nudVarValue=new System.Windows.Forms.NumericUpDown();
			this.label4=new System.Windows.Forms.Label();
			this.label5=new System.Windows.Forms.Label();
			this.nudTimeOneHour=new System.Windows.Forms.NumericUpDown();
			this.nudTimeOneMinute=new System.Windows.Forms.NumericUpDown();
			this.label6=new System.Windows.Forms.Label();
			this.nudTimeTwoMinute=new System.Windows.Forms.NumericUpDown();
			this.nudTimeTwoHour=new System.Windows.Forms.NumericUpDown();
			this.label7=new System.Windows.Forms.Label();
			this.rbCharacter=new System.Windows.Forms.RadioButton();
			this.cbCharType=new System.Windows.Forms.ComboBox();
			this.nudCharValue=new System.Windows.Forms.NumericUpDown();
			this.cbCharOperator=new System.Windows.Forms.ComboBox();
			this.cbElse=new System.Windows.Forms.CheckBox();
			this.btnSave=new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.nudVarValue)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudTimeOneHour)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudTimeOneMinute)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudTimeTwoMinute)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudTimeTwoHour)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudCharValue)).BeginInit();
			this.SuspendLayout();
			// 
			// rbVariable
			// 
			this.rbVariable.AutoSize=true;
			this.rbVariable.Checked=true;
			this.rbVariable.Location=new System.Drawing.Point(12, 33);
			this.rbVariable.Name="rbVariable";
			this.rbVariable.Size=new System.Drawing.Size(63, 17);
			this.rbVariable.TabIndex=0;
			this.rbVariable.TabStop=true;
			this.rbVariable.Text="Variable";
			this.rbVariable.UseVisualStyleBackColor=true;
			// 
			// rbTime
			// 
			this.rbTime.AutoSize=true;
			this.rbTime.Location=new System.Drawing.Point(12, 79);
			this.rbTime.Name="rbTime";
			this.rbTime.Size=new System.Drawing.Size(58, 17);
			this.rbTime.TabIndex=1;
			this.rbTime.Text="Uhrzeit";
			this.rbTime.UseVisualStyleBackColor=true;
			// 
			// cbVarType
			// 
			this.cbVarType.DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbVarType.FormattingEnabled=true;
			this.cbVarType.Items.AddRange(new object[] {
            "Charakter",
            "Account",
            "Karte",
            "Global"});
			this.cbVarType.Location=new System.Drawing.Point(81, 29);
			this.cbVarType.Name="cbVarType";
			this.cbVarType.Size=new System.Drawing.Size(114, 21);
			this.cbVarType.TabIndex=2;
			// 
			// label1
			// 
			this.label1.AutoSize=true;
			this.label1.Location=new System.Drawing.Point(78, 9);
			this.label1.Name="label1";
			this.label1.Size=new System.Drawing.Size(82, 13);
			this.label1.TabIndex=3;
			this.label1.Text="Art der Variable:";
			// 
			// tbVarName
			// 
			this.tbVarName.Location=new System.Drawing.Point(201, 29);
			this.tbVarName.Name="tbVarName";
			this.tbVarName.Size=new System.Drawing.Size(192, 20);
			this.tbVarName.TabIndex=4;
			// 
			// label2
			// 
			this.label2.AutoSize=true;
			this.label2.Location=new System.Drawing.Point(198, 9);
			this.label2.Name="label2";
			this.label2.Size=new System.Drawing.Size(80, 13);
			this.label2.TabIndex=5;
			this.label2.Text="Variablenname:";
			// 
			// label3
			// 
			this.label3.AutoSize=true;
			this.label3.Location=new System.Drawing.Point(396, 9);
			this.label3.Name="label3";
			this.label3.Size=new System.Drawing.Size(51, 13);
			this.label3.TabIndex=6;
			this.label3.Text="Operator:";
			// 
			// cbVarOperator
			// 
			this.cbVarOperator.DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbVarOperator.FormattingEnabled=true;
			this.cbVarOperator.Items.AddRange(new object[] {
            "Größer",
            "Größer/Gleich",
            "Gleich",
            "Kleiner/Gleich",
            "Kleiner"});
			this.cbVarOperator.Location=new System.Drawing.Point(399, 28);
			this.cbVarOperator.Name="cbVarOperator";
			this.cbVarOperator.Size=new System.Drawing.Size(96, 21);
			this.cbVarOperator.TabIndex=7;
			// 
			// nudVarValue
			// 
			this.nudVarValue.Location=new System.Drawing.Point(504, 28);
			this.nudVarValue.Name="nudVarValue";
			this.nudVarValue.Size=new System.Drawing.Size(98, 20);
			this.nudVarValue.TabIndex=8;
			// 
			// label4
			// 
			this.label4.AutoSize=true;
			this.label4.Location=new System.Drawing.Point(501, 9);
			this.label4.Name="label4";
			this.label4.Size=new System.Drawing.Size(33, 13);
			this.label4.TabIndex=9;
			this.label4.Text="Wert:";
			// 
			// label5
			// 
			this.label5.AutoSize=true;
			this.label5.Location=new System.Drawing.Point(78, 81);
			this.label5.Name="label5";
			this.label5.Size=new System.Drawing.Size(64, 13);
			this.label5.TabIndex=10;
			this.label5.Text="ist zwischen";
			// 
			// nudTimeOneHour
			// 
			this.nudTimeOneHour.Location=new System.Drawing.Point(148, 74);
			this.nudTimeOneHour.Name="nudTimeOneHour";
			this.nudTimeOneHour.Size=new System.Drawing.Size(47, 20);
			this.nudTimeOneHour.TabIndex=11;
			// 
			// nudTimeOneMinute
			// 
			this.nudTimeOneMinute.Location=new System.Drawing.Point(201, 74);
			this.nudTimeOneMinute.Name="nudTimeOneMinute";
			this.nudTimeOneMinute.Size=new System.Drawing.Size(47, 20);
			this.nudTimeOneMinute.TabIndex=12;
			// 
			// label6
			// 
			this.label6.AutoSize=true;
			this.label6.Location=new System.Drawing.Point(254, 79);
			this.label6.Name="label6";
			this.label6.Size=new System.Drawing.Size(45, 13);
			this.label6.TabIndex=13;
			this.label6.Text="Uhr und";
			// 
			// nudTimeTwoMinute
			// 
			this.nudTimeTwoMinute.Location=new System.Drawing.Point(358, 74);
			this.nudTimeTwoMinute.Name="nudTimeTwoMinute";
			this.nudTimeTwoMinute.Size=new System.Drawing.Size(47, 20);
			this.nudTimeTwoMinute.TabIndex=15;
			// 
			// nudTimeTwoHour
			// 
			this.nudTimeTwoHour.Location=new System.Drawing.Point(305, 74);
			this.nudTimeTwoHour.Name="nudTimeTwoHour";
			this.nudTimeTwoHour.Size=new System.Drawing.Size(47, 20);
			this.nudTimeTwoHour.TabIndex=14;
			// 
			// label7
			// 
			this.label7.AutoSize=true;
			this.label7.Location=new System.Drawing.Point(415, 79);
			this.label7.Name="label7";
			this.label7.Size=new System.Drawing.Size(24, 13);
			this.label7.TabIndex=16;
			this.label7.Text="Uhr";
			// 
			// rbCharacter
			// 
			this.rbCharacter.AutoSize=true;
			this.rbCharacter.Location=new System.Drawing.Point(12, 56);
			this.rbCharacter.Name="rbCharacter";
			this.rbCharacter.Size=new System.Drawing.Size(71, 17);
			this.rbCharacter.TabIndex=17;
			this.rbCharacter.Text="Charakter";
			this.rbCharacter.UseVisualStyleBackColor=true;
			// 
			// cbCharType
			// 
			this.cbCharType.DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbCharType.FormattingEnabled=true;
			this.cbCharType.Items.AddRange(new object[] {
            "HP",
            "Gold"});
			this.cbCharType.Location=new System.Drawing.Point(81, 52);
			this.cbCharType.Name="cbCharType";
			this.cbCharType.Size=new System.Drawing.Size(114, 21);
			this.cbCharType.TabIndex=18;
			// 
			// nudCharValue
			// 
			this.nudCharValue.Location=new System.Drawing.Point(504, 52);
			this.nudCharValue.Name="nudCharValue";
			this.nudCharValue.Size=new System.Drawing.Size(98, 20);
			this.nudCharValue.TabIndex=23;
			// 
			// cbCharOperator
			// 
			this.cbCharOperator.DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbCharOperator.FormattingEnabled=true;
			this.cbCharOperator.Items.AddRange(new object[] {
            "Größer",
            "Größer/Gleich",
            "Gleich",
            "Kleiner/Gleich",
            "Kleiner"});
			this.cbCharOperator.Location=new System.Drawing.Point(399, 53);
			this.cbCharOperator.Name="cbCharOperator";
			this.cbCharOperator.Size=new System.Drawing.Size(96, 21);
			this.cbCharOperator.TabIndex=22;
			// 
			// cbElse
			// 
			this.cbElse.AutoSize=true;
			this.cbElse.Location=new System.Drawing.Point(12, 102);
			this.cbElse.Name="cbElse";
			this.cbElse.Size=new System.Drawing.Size(84, 17);
			this.cbElse.TabIndex=25;
			this.cbElse.Text="Else Zweig?";
			this.cbElse.UseVisualStyleBackColor=true;
			// 
			// btnSave
			// 
			this.btnSave.Location=new System.Drawing.Point(504, 115);
			this.btnSave.Name="btnSave";
			this.btnSave.Size=new System.Drawing.Size(98, 23);
			this.btnSave.TabIndex=26;
			this.btnSave.Text="Speichern";
			this.btnSave.UseVisualStyleBackColor=true;
			this.btnSave.Click+=new System.EventHandler(this.btnSave_Click);
			// 
			// FormQuestDataIf
			// 
			this.AutoScaleDimensions=new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize=new System.Drawing.Size(607, 144);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.cbElse);
			this.Controls.Add(this.nudCharValue);
			this.Controls.Add(this.cbCharOperator);
			this.Controls.Add(this.cbCharType);
			this.Controls.Add(this.rbCharacter);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.nudTimeTwoMinute);
			this.Controls.Add(this.nudTimeTwoHour);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.nudTimeOneMinute);
			this.Controls.Add(this.nudTimeOneHour);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.nudVarValue);
			this.Controls.Add(this.cbVarOperator);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.tbVarName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cbVarType);
			this.Controls.Add(this.rbTime);
			this.Controls.Add(this.rbVariable);
			this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name="FormQuestDataIf";
			this.StartPosition=System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text="If Editor";
			this.Load+=new System.EventHandler(this.FormQuestDataIf_Load);
			((System.ComponentModel.ISupportInitialize)(this.nudVarValue)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudTimeOneHour)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudTimeOneMinute)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudTimeTwoMinute)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudTimeTwoHour)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudCharValue)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RadioButton rbVariable;
		private System.Windows.Forms.RadioButton rbTime;
		private System.Windows.Forms.ComboBox cbVarType;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbVarName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cbVarOperator;
		private System.Windows.Forms.NumericUpDown nudVarValue;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown nudTimeOneHour;
		private System.Windows.Forms.NumericUpDown nudTimeOneMinute;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.NumericUpDown nudTimeTwoMinute;
		private System.Windows.Forms.NumericUpDown nudTimeTwoHour;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.RadioButton rbCharacter;
		private System.Windows.Forms.ComboBox cbCharType;
		private System.Windows.Forms.NumericUpDown nudCharValue;
		private System.Windows.Forms.ComboBox cbCharOperator;
		private System.Windows.Forms.CheckBox cbElse;
		private System.Windows.Forms.Button btnSave;
	}
}