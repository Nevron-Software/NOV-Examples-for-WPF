namespace Nevron.Nov.Examples
{
	internal class NCsSyntaxHighlighter : NSyntaxHighlighter
	{
		#region Property Overrides

		public override string[] Keywords
		{
			get
			{
				return CSharpKeywords;
			}
		}
		public override char CommentChar
		{
			get
			{
				return CSharpCommentChar;
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
			char c;
			PeekChar(out c);

			if (c == Slash)
			{
				// This is a one line comment, so read to the end of the line
				string comment;
				if (ReadTo(CommentEndChars, out comment))
				{
					OnComment(SlashString + comment);
				}

				m_TokenStartPos = m_Position;
			}
			else if (c == Asterisk)
			{
				// Consume the asterisk
				ReadChar(out c);

				// This is a multi line comment, so read to its end - "*/"
				string comment;
				if (ReadTo(MultilineCommentEnd, out comment))
				{
					OnComment("/*" + comment + "*/");
				}

				// Consume the "*/" char sequence
				ReadChar(out c);
				ReadChar(out c);
				m_TokenStartPos = m_Position;
			}
		}

		#endregion

		#region Constants

		private const char CSharpCommentChar = '/';
		private static readonly string[] CSharpKeywords = new string[] {
			"abstract", "alias", "as", "async", "await",
			"base", "bool", "break", "byte",
			"case", "catch", "char", "checked", "class", "const", "continue",
			"decimal", "default", "delegate", "do", "double",
			"else", "enum", "event", "explicit", "extern",
			"false", "finally", "fixed", "float", "for", "foreach",
			"get", "global", "goto",
			"if", "implicit", "in", "int", "interface", "internal", "is",
			"lock", "long",
			"namespace", "new", "null",
			"object", "operator", "out", "override",
			"params", "partial", "private", "protected", "public",
			"readonly", "ref", "return",
			"sbyte", "sealed", "set", "short", "sizeof", "stackalloc", "static", "string", "struct", "switch",
			"this", "throw", "true", "try", "typeof",
			"uint", "ulong", "unchecked", "unsafe", "ushort", "using",
			"var", "virtual", "void", "volatile",
			"while",
			"yield",
			"#if", "#elif", "#endregion", "#endif", "#region"
		};

		private const string MultilineCommentEnd = "*/";
		private const string SlashString = "/";

		#endregion
	}
}