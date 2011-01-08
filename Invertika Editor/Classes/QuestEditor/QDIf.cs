using System;
using System.Collections.Generic;
using System.Text;

namespace Invertika_Editor.Classes.QuestEditor
{
	public enum QIfType
	{
		Variable,
		Character,
		Time
	}

	public enum QIfVarType
	{
		Character,
		Account,
		Map,
		Global
	}

	public enum QIfCharacterType
	{
		HP,
		Gold
	}

	public enum QIfOperator
	{
		Bigger,
		BiggerEqual,
		Equal,
		SmallerEqual,
		Smaller
	}

	public class QDIf : IQuestDataClass
	{
		public QIfType Type
		{
			get;
			set;
		}

		public QIfVarType VariableType
		{
			get;
			set;
		}

		public QIfCharacterType CharacterType
		{
			get;
			set;
		}

		public string VariableName
		{
			get;
			set;
		}

		public QIfOperator Operator
		{
			get;
			set;
		}

		public int Value
		{
			get;
			set;
		}

		public DateTime TimeOne
		{
			get;
			set;
		}

		public DateTime TimeTwo
		{
			get;
			set;
		}

		public bool Else
		{
			get;
			set;
		}

		public IQuestDataClass ElseData
		{
			get;
			private set;
		}

		public QDIf(QIfVarType vartype, string varname, QIfOperator varoperator, int value, bool elsed)
		{
			Type=QIfType.Variable;
			VariableType=vartype;
			VariableName=varname;
			Operator=varoperator;
			Value=value;
			Else=elsed;
		}

		public QDIf(QIfCharacterType chartype, QIfOperator varoperator, int value, bool elsed)
		{
			Type=QIfType.Character;
			CharacterType=chartype;
			Operator=varoperator;
			Value=value;
			Else=elsed;
		}

		public QDIf(DateTime time1, DateTime time2, bool elsed)
		{
			Type=QIfType.Time;
			TimeOne=time1;
			TimeTwo=time2;
			Else=elsed;
		}
	}
}
