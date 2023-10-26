using System;
using System.Collections.Generic;
public class SymbolTable
{
    public Dictionary<string, TokenType> m_symbols;

    public SymbolTable()
    {
        m_symbols = new Dictionary<string, TokenType>();
        InititalizeSymbolTable();
    }
    public void InititalizeSymbolTable()
    {
        // Symbols
        m_symbols[";"] = TokenType.TT_SEMICOLON;
        m_symbols["="] = TokenType.TT_ASSIGN;

        // Logic operators
        m_symbols["=="] = TokenType.TT_EQUAL;
        m_symbols["!="] = TokenType.TT_NOT_EQUAL;
        m_symbols["<"] = TokenType.TT_LOWER;
        m_symbols["<="] = TokenType.TT_LOWER_EQUAL;
        m_symbols[">"] = TokenType.TT_GREATER;
        m_symbols[">="] = TokenType.TT_GREATER_EQUAL;

        // Arithmetic operators
        m_symbols["+"] = TokenType.TT_ADD;
        m_symbols["-"] = TokenType.TT_SUB;
        m_symbols["*"] = TokenType.TT_MUL;
        m_symbols["/"] = TokenType.TT_DIV;
        m_symbols["%"] = TokenType.TT_MOD;
        m_symbols["^"] = TokenType.TT_POT;

        // Keywords
        m_symbols["program"] = TokenType.TT_PROGRAM;
        m_symbols["while"] = TokenType.TT_WHILE;
        m_symbols["do"] = TokenType.TT_DO;
        m_symbols["done"] = TokenType.TT_DONE;
        m_symbols["if"] = TokenType.TT_IF;
        m_symbols["then"] = TokenType.TT_THEN;
        m_symbols["else"] = TokenType.TT_ELSE;
        m_symbols["output"] = TokenType.TT_OUTPUT;
        m_symbols["true"] = TokenType.TT_TRUE;
        m_symbols["false"] = TokenType.TT_FALSE;
        m_symbols["read"] = TokenType.TT_READ;
        m_symbols["not"] = TokenType.TT_NOT;
    }
    public bool contains(string token)
    {
        return m_symbols.ContainsKey(token);
    }
    public TokenType find(string token)
    {
        if (m_symbols.TryGetValue(token, out TokenType type))
        {
            return type;
        }
        else
        {
            return TokenType.TT_VAR; // Retorna TT_VAR se o token não for encontrado
        }
    }

}
