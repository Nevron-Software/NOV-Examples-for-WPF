namespace Nevron.Nov.Examples
{
	internal class NVbSyntaxHighlighter : NSyntaxHighlighter
	{
		#region Property Overrides

		public override string[] Keywords
		{
			get
			{
				return VbKeywords;
			}
		}

		public override char CommentChar
		{
			get
			{
				return SingleQuote;
			}
		}

		#endregion

		#region Protected Overrides

		protected override bool IsStringLiteralStartChar(char c)
		{
			return c == DoubleQuote || c == SingleQuote;
		}
		protected override void ReadAfterCommentChar()
		{
			// Visual Basic supports only one line comments, so read to the end of the line
			string comment;
			if (ReadTo(CommentEndChars, out comment))
			{
				OnComment(CommentChar + comment);
			}

			m_TokenStartPos = m_Position;
		}

		#endregion

		#region Constants

		private static readonly string[] VbKeywords = new string[]
		{
			"AddHandler", "Alias", "And", "AndAlso", "As",
			"Boolean", "ByRef", "Byte", "ByVal",
			"Call", "Case", "Catch", "CBool", "CByte", "CChar", "CDate", "CDbl", "CDec", "Char", "CInt", "Class", "CLng", "CObj", "Const", "Continue", "CSByte", "CShort", "CSng", "CStr", "CType", "CUInt", "CULng", "CUShort",
			"Date", "Decimal", "Declare", "Default", "Delegate", "Dim", "DirectCast", "Do", "Double",
			"Each", "Else", "ElseIf", "End", "EndIf", "Enum", "Erase", "Error", "Event", "Exit", "False",
			"Finally", "For", "Friend", "Function",
			"Get", "GetType", "GetXMLNamespace", "Global", "GoSub", "GoTo",
			"Handles",
			"If", "Implements", "Imports", "In", "Inherits", "Integer", "Interface", "Is", "IsNot",
			"Let", "Lib", "Like", "Long", "Loop",
			"Me", "Mod", "Module", "MustInherit", "MustOverride", "MyBase", "MyClass",
			"NameOf", "Namespace", "Narrowing", "New", "Next", "Not", "Nothing", "NotInheritable", "NotOverridable",
			"Object", "Of", "On", "Operator", "Option", "Optional", "Or", "OrElse", "Out", "Overloads", "Overridable", "Overrides",
			"ParamArray", "Partial", "Private", "Property", "Protected", "Public",
			"RaiseEvent", "ReadOnly", "ReDim", "Region", "REM", "RemoveHandler", "Resume", "Return",
			"SByte", "Select", "Set", "Shadows", "Shared", "Short", "Single", "Static", "Step", "Stop", "String", "Structure", "Sub", "SyncLock",
			"Then", "Throw", "To", "True", "Try", "TryCast", "TypeOf",
			"UInteger", "ULong", "UShort", "Using",
			"Variant",
			"Wend", "When", "While", "Widening", "With", "WithEvents", "WriteOnly",
			"Xor",
			"#Const", "#Else", "#ElseIf", "#End", "#If", "#Region"
		};

		#endregion
	}
}