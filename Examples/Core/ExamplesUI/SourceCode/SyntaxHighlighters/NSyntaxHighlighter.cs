using System;
using System.IO;
using System.Text;

using Nevron.Nov.IO;
using Nevron.Nov.Text;

namespace Nevron.Nov.Examples
{
	internal abstract class NSyntaxHighlighter : NStringParser
	{
		#region Constructors

		protected NSyntaxHighlighter()
		{
			m_bInlineStyles = false;
		}

		#endregion

		#region Properties - Must Override

		public abstract string[] Keywords
		{
			get;
		}
		public abstract char CommentChar
		{
			get;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Determines whether the CSS styles should be inlined in "style" attributes
		/// or not. By default set to false.
		/// </summary>
		public bool InlineStyles
		{
			get
			{
				return m_bInlineStyles;
			}
			set
			{
				m_bInlineStyles = value;
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Analyzes the given source code stream, highlights its syntax and returns
		/// the resulting HTML and CSS code as a stream.
		/// </summary>
		/// <param name="sourceCodeStream"></param>
		/// <returns></returns>
		public Stream Highlight(Stream sourceCodeStream)
		{
			int byteIndex = 0;
			byte[] data = NStreamHelpers.ReadToEnd(sourceCodeStream);
			if (data.Length > 3 && data[0] == 0xEF && data[1] == 0xBB && data[2] == 0xBF)
			{
				// The data starts with a BOM
				byteIndex = 3;
			}

			m_Chars = NEncoding.UTF8.GetChars(data, byteIndex, data.Length - byteIndex);
			m_StartPosition = 0;
			m_Position = 0;
			m_EndPosition = m_Chars.Length - 1;
			m_TokenStartPos = 0;
			m_Output = new StringBuilder();

			char c;
			while (ReadChar(out c))
			{
				if (IsLetter(c) || c == '_' || c == '#')
					continue;

				if (c == CommentChar)
				{
					// This is a comment
					ReadAfterCommentChar();
					continue;
				}
				else if (c == SingleQuote || c == DoubleQuote)
				{
					// This is a string literal
					string str;
					if (ReadTo(c, Backslash, out str))
					{
						OnString(c + str + c);
						ReadChar(out c);
						m_TokenStartPos = m_Position;
					}
					continue;
				}

				if (m_TokenStartPos < m_Position - 1)
				{
					// Add a token
					string token = new String(m_Chars, m_TokenStartPos, m_Position - 1 - m_TokenStartPos);
					OnToken(token);
				}

				OnDelimiter(c);
				m_TokenStartPos = m_Position;
			}

			// Add the last token
			if (m_TokenStartPos < m_Position)
			{
				string token = new String(m_Chars, m_TokenStartPos, m_Position - m_TokenStartPos);
				OnToken(token);
			}

			// Create and return the result
			string stylesheet;
			string codeStyle;
			if (m_bInlineStyles)
			{
				stylesheet = String.Empty;
				codeStyle = " style=\"" + CodeStyle + "\"";
			}
			else
			{
				stylesheet = StyleSheet;
				codeStyle = String.Empty;
			}

			string output = HtmlTemplate.Replace("{STYLESHEET}", stylesheet);
			output = output.Replace("{CODE_STYLE}", codeStyle);
			output = output.Replace("{GENERATED_HTML}", m_Output.ToString());

			byte[] bytes = NEncoding.UTF8.GetBytes(output);
			return new MemoryStream(bytes);
		}

		#endregion

		#region Protected Must Override

		protected abstract void ReadAfterCommentChar();
		protected abstract bool IsStringLiteralStartChar(char c);

		#endregion

		#region Implementation - States

		private void OnToken(string token)
		{
			if (Array.IndexOf(Keywords, token) != -1)
			{
				string style = m_bInlineStyles ? "style=\"" + KeyStyle + "\">" : "class=\"key\">";
				m_Output.Append("<span ");
				m_Output.Append(style);
				m_Output.Append(token);
				m_Output.Append("</span>");
			}
			else
			{
				m_Output.Append(token);
			}
		}
		protected void OnComment(string comment)
		{
			comment = NormalizeString(comment);
			string style = m_bInlineStyles ? "style=\"" + ComStyle + "\">" : "class=\"com\">";
			m_Output.Append("<span ");
			m_Output.Append(style);
			m_Output.Append(comment);
			m_Output.Append("</span>");
		}
		private void OnString(string str)
		{
			str = NormalizeString(str);
			string style = m_bInlineStyles ? "style=\"" + StrStyle + "\">" : "class=\"str\">";
			m_Output.Append("<span ");
			m_Output.Append(style);
			m_Output.Append(str);
			m_Output.Append("</span>");
		}
		private void OnDelimiter(char c)
		{
			string token;
			switch (c)
			{
				case '\t':
					token = "    ";
					break;
				case '<':
					token = "&lt;";
					break;
				case '>':
					token = "&gt;";
					break;
				default:
					token = new String(c, 1);
					break;
			}

			m_Output.Append(token);
		}

		#endregion

		#region Fields

		private bool m_bInlineStyles;
		protected int m_TokenStartPos;
		private StringBuilder m_Output;

		#endregion

		#region Static Methods

		public static NSyntaxHighlighter Create(ENProgrammingLanguage language)
		{
			if (language == ENProgrammingLanguage.CSharp)
				return new NCsSyntaxHighlighter();
			else
				return new NVbSyntaxHighlighter();
		}

		private static bool IsLetter(char c)
		{
			return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
		}
		private string NormalizeString(string str)
		{
			return str.Replace("<", "&lt;").Replace(">", "&gt;");
		}

		#endregion

		#region Constants

		private const char Backslash = '\\'; // Used for escaping
		
		protected static readonly char[] CommentEndChars = new char[] { '\n', '\r' };

		private static readonly string HtmlTemplate = @"
<!doctype html>
<html>
<head>
    {STYLESHEET}
</head>
<body>
    <pre class=""cscode""><code{CODE_STYLE}>{GENERATED_HTML}</code></pre>
</body>
</html>
";
		private static readonly string StyleSheet = @"
    <style type=""text/css"">
		pre.cscode {" + PreStyle + @"}
		.cscode code {" + CodeStyle.Replace("&quot;", "\"") + @"}
		.cscode .key {" + KeyStyle + @"}
		.cscode .com {" + ComStyle + @"}
		.cscode .str {" + StrStyle + @"}
    </style>
";

		private const string PreStyle = @"margin:0em; overflow:auto; background-color:#ffffff;";
		private const string CodeStyle = @"font-family:Consolas,&quot;Courier New&quot;,Courier,Monospace; font-size:10pt; color:#000000;";
		private const string KeyStyle = @"color:#0000ff;";
		private const string ComStyle = @"color:#008000;";
		private const string StrStyle = @"color:#a31515;";

		#endregion
	}
}