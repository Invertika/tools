using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Invertika_Editor.Classes.QuestEditor;

namespace Invertika_Editor
{
	public partial class FormQuestDataIf : Form
	{
		public QDIf Data=null;

		public FormQuestDataIf()
		{
			InitializeComponent();
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			if(rbVariable.Checked)
			{
				QIfVarType vartype=QIfVarType.Character;

				switch(cbVarType.Text)
				{
					case "Charakter":
						{
							vartype=QIfVarType.Character;
							break;
						}
					case "Account":
						{
							vartype=QIfVarType.Character;
							break;
						}
					case "Karte":
						{
							vartype=QIfVarType.Character;
							break;
						}
					case "Global":
						{
							vartype=QIfVarType.Character;
							break;
						}
				}

				string varname=tbVarName.Text;

				QIfOperator varoperator=QIfOperator.Equal;

				switch(cbVarOperator.Text)
				{
					case "Größer":
						{
							varoperator=QIfOperator.Bigger;
							break;
						}
					case "Größer/Gleich":
						{
							varoperator=QIfOperator.BiggerEqual;
							break;
						}
					case "Gleich":
						{
							varoperator=QIfOperator.Equal;
							break;
						}
					case "Kleiner/Gleich":
						{
							varoperator=QIfOperator.SmallerEqual;
							break;
						}
					case "Kleiner":
						{
							varoperator=QIfOperator.Smaller;
							break;
						}
				}

				int varValue=(int)nudVarValue.Value;
				bool elsed=cbElse.Checked;

				if(Data==null)
				{
					Data=new QDIf(vartype, varname, varoperator, varValue, elsed);
				}
				else
				{
					Data.Type=QIfType.Character;
					Data.VariableType=vartype;
					Data.Operator=varoperator;
					Data.Value=varValue;
					Data.Else=elsed;
				}
			}
			else if(rbCharacter.Checked)
			{
				QIfCharacterType chartype=QIfCharacterType.HP;

				switch(cbCharType.Text)
				{
					case "HP":
						{
							chartype=QIfCharacterType.HP;
							break;
						}
					case "Gold":
						{
							chartype=QIfCharacterType.Gold;
							break;
						}			
				}

				QIfOperator charOperator=QIfOperator.Equal;

				switch(cbCharOperator.Text)
				{
					case "Größer":
						{
							charOperator=QIfOperator.Bigger;
							break;
						}
					case "Größer/Gleich":
						{
							charOperator=QIfOperator.BiggerEqual;
							break;
						}
					case "Gleich":
						{
							charOperator=QIfOperator.Equal;
							break;
						}
					case "Kleiner/Gleich":
						{
							charOperator=QIfOperator.SmallerEqual;
							break;
						}
					case "Kleiner":
						{
							charOperator=QIfOperator.Smaller;
							break;
						}
				}

				int charValue=(int)nudCharValue.Value;
				bool elsed=cbElse.Checked;

				if(Data==null)
				{
					Data=new QDIf(chartype, charOperator, charValue, elsed);
				}
				else
				{
					Data.Type=QIfType.Character;
					Data.CharacterType=chartype;
					Data.Operator=charOperator;
					Data.Value=charValue;
					Data.Else=elsed;
				}
			}
			else //Time
			{
				DateTime One=new DateTime(1, 1, 1, (int)nudTimeOneHour.Value, (int)nudTimeOneMinute.Value, 0);
				DateTime Two=new DateTime(1, 1, 1, (int)nudTimeTwoHour.Value, (int)nudTimeTwoMinute.Value, 0);
				bool elsed=cbElse.Checked;

				if(Data==null)
				{
					Data=new QDIf(One, Two, elsed);
				}
				else
				{
					Data.Type=QIfType.Time;
					Data.TimeOne=One;
					Data.TimeTwo=Two;
					Data.Else=elsed;
				}
			}

			DialogResult=System.Windows.Forms.DialogResult.OK;
		}

		private void FormQuestDataIf_Load(object sender, EventArgs e)
		{
			//Standardwerte setzen
			cbVarType.SelectedIndex=0;
			cbVarOperator.SelectedIndex=0;
			cbCharType.SelectedIndex=0;
			cbCharOperator.SelectedIndex=0;

			if(Data!=null) //Bestehende Daten laden
			{
				switch(Data.Type)
				{
					case QIfType.Variable:
						{
							rbVariable.Checked=true;

							switch(Data.VariableType)
							{
								case QIfVarType.Character:
									{
										cbVarType.Text="Charakter";
										break;
									}
									case QIfVarType.Account:
									{
										cbVarType.Text="Account";
										break;
									}
									case QIfVarType.Map:
									{
										cbVarType.Text="Karte";
										break;
									}
									case QIfVarType.Global:
									{
										cbVarType.Text="Global";
										break;
									}
							}

							tbVarName.Text=Data.VariableName;

							switch(Data.Operator)
							{
								case QIfOperator.Bigger:
									{
										cbVarOperator.Text="Größer";
										break;
									}
									case QIfOperator.BiggerEqual:
									{
										cbVarOperator.Text="Größer/Gleich";
										break;
									}
									case QIfOperator.Equal:
									{
										cbVarOperator.Text="Gleich";
										break;
									}
									case QIfOperator.SmallerEqual:
									{
										cbVarOperator.Text="Kleiner/Gleich";
										break;
									}
									case QIfOperator.Smaller:
									{
										cbVarOperator.Text="Kleiner";
										break;
									}
							}

							nudVarValue.Value=Data.Value;

							cbElse.Checked=Data.Else;

							break;
						}
					case QIfType.Character:
						{
							rbCharacter.Checked=true;

							switch(Data.CharacterType)
							{
								case QIfCharacterType.Gold:
									{
										cbCharType.Text="Gold";
										break;
									}
								case QIfCharacterType.HP:
									{
										cbCharType.Text="HP";
										break;
									}
							}

							switch(Data.Operator)
							{
								case QIfOperator.Bigger:
									{
										cbCharOperator.Text="Größer";
										break;
									}
								case QIfOperator.BiggerEqual:
									{
										cbCharOperator.Text="Größer/Gleich";
										break;
									}
								case QIfOperator.Equal:
									{
										cbCharOperator.Text="Gleich";
										break;
									}
								case QIfOperator.SmallerEqual:
									{
										cbCharOperator.Text="Kleiner/Gleich";
										break;
									}
								case QIfOperator.Smaller:
									{
										cbCharOperator.Text="Kleiner";
										break;
									}
							}

							nudCharValue.Value=Data.Value;

							cbElse.Checked=Data.Else;

							break;
						}
					case QIfType.Time:
						{
							rbTime.Checked=true;

							nudTimeOneHour.Value=Data.TimeOne.Hour;
							nudTimeOneMinute.Value=Data.TimeOne.Minute;

							nudTimeTwoHour.Value=Data.TimeTwo.Hour;
							nudTimeTwoMinute.Value=Data.TimeTwo.Minute;

							cbElse.Checked=Data.Else;

							break;
						}
				}
			}
		}
	}
}
