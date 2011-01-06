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
			private set;
		}

		public QIfVarType VariableType
		{
			get;
			private set;
		}

		public QIfCharacterType CharacterType
		{
			get;
			private set;
		}

		public string VariableName
		{
			get;
			private set;
		}

		public QIfOperator Operator
		{
			get;
			private set;
		}

		public int Value
		{
			get;
			private set;
		}

		public DateTime TimeOne
		{
			get;
			private set;
		}

		public DateTime TimeTwo
		{
			get;
			private set;
		}

		public bool Else
		{
			get;
			private set;
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

		public QDIf(QIfCharacterType chartype, string varname, QIfOperator varoperator, int value, bool elsed)
		{
			Type=QIfType.Variable;
			CharacterType=chartype;
			VariableName=varname;
			Operator=varoperator;
			Value=value;
			Else=elsed;
		}

		public QDIf(DateTime time1, DateTime time2, bool elsed)
		{
			Type=QIfType.Variable;
			TimeOne=time1;
			TimeTwo=time2;
			Else=elsed;
		}
	}
}
