using System;
using System.IO;
using System.Collections.Generic;

public class LexicalAnalysis : IDisposable
{
	private int m_line;
	private SymbolTable m_st;
	private StreamReader m_input;

	public LexicalAnalysis(string filename)
	{
		m_line = 1;
		m_st = new SymbolTable();
		try
		{
			m_input = new StreamReader(filename);
		}
		catch (FileNotFoundException)
		{
			throw new Exception("unable to open file");
		}
	}

	public int Line()
	{
		return m_line;
	}

	public void Dispose()
	{
		m_input.Dispose();
	}

	public Lexeme NextToken()
	{
		int state;
		Lexeme lex = new Lexeme();

		state = 1;
		while (state != 7 && state != 8)
		{
			int c;
			if(m_input.Peek() == -1){
				state = 2;
				c = -1;
			}

			if ((state == 5))
			{
				if ((m_input.Peek() == '_') || (char.IsLetterOrDigit((char)m_input.Peek())))
				{
					c = m_input.Read();
				}
				else
				{
					c = m_input.Peek();
				}
			}
			else if ((state == 6))
			{
				if (char.IsDigit((char)m_input.Peek()))
				{
					c = m_input.Read();
				}
				else
				{
					c = m_input.Peek();
				}
			}
			else
			{
				c = m_input.Read();
			}
			switch (state)
			{
				case 1:
					if (c == ' ' || c == '\t' || c == '\r')
					{
						state = 1;
					}
					else if (c == '\n')
					{
						m_line++;
						state = 1;
					}
					else if (c == '#')
					{
						state = 2;
					}
					else if (c == '=' || c == '<' || c == '>')
					{
						lex.token += (char)c;
						state = 3;
					}
					else if (c == '!')
					{
						lex.token += (char)c;
						state = 4;
					}
					else if (c == ';' || c == '+' || c == '-' ||
							c == '*' || c == '/' || c == '%')
					{
						lex.token += (char)c;
						state = 7;
					}
					else if (c == '_' || char.IsLetter((char)c))
					{
						lex.token += (char)c;
						state = 5;
					}
					else if (char.IsDigit((char)c))
					{
						lex.token += (char)c;
						state = 6;
					}
					else
					{
						if (c == -1)
						{
							lex.type = TokenType.TT_END_OF_FILE;
							state = 8;
						}
						else
						{
							lex.token += (char)c;
							lex.type = TokenType.TT_INVALID_TOKEN;
							state = 8;
						}
					}
					break;
				case 2:
					if (c == '\n')
					{
						m_line++;
						state = 1;
					}
					else if (c == -1)
					{
						lex.type = TokenType.TT_END_OF_FILE;
						state = 8;
					}
					else
					{
						state = 2;
					}
					break;
				case 3:
					if (c == '=')
					{
						lex.token += (char)c;
						state = 7;
					}
					else
					{
						if (c != -1)
							m_input.BaseStream.Seek(-1, SeekOrigin.Current);

						state = 7;
					}
					break;
				case 4:
					if (c == '=')
					{
						lex.token += (char)c;
						state = 7;
					}
					else
					{
						if (c == -1)
						{
							lex.type = TokenType.TT_UNEXPECTED_EOF;
							state = 8;
						}
						else
						{
							lex.type = TokenType.TT_INVALID_TOKEN;
							state = 8;
						}
					}
					break;
				case 5:
					if (c == '_' || char.IsLetterOrDigit((char)c))
					{
						lex.token += (char)c;
						state = 5;
					}
					else
					{

						//.WriteLine("Caracter " + (char)c);
						//if (c != -1)
						//	m_input.BaseStream.Seek(-1, SeekOrigin.Current);

						state = 7;
					}
					break;
				case 6:
					if (char.IsDigit((char)c))
					{
						lex.token += (char)c;
						state = 6;
					}
					else
					{
						//if (c != -1)
						//m_input.BaseStream.Seek(-1, SeekOrigin.Current);

						lex.type = TokenType.TT_NUMBER;
						state = 8;
					}
					break;
				default:
					throw new Exception("invalid state");
			}
		}

		if (state == 7)
			lex.type = m_st.find(lex.token);

		return lex;

	}
}