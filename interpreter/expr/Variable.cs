using System;
using System.Collections.Generic;

public class Variable : IntExpr
{
    private string m_name;

    public Variable(int line, string name) : base(line)
    {
        m_name = name;
    }

    public string Name => m_name;

    public int value()
    {
        return Memory.Read(m_name);
    }

    public void setValue(int value)
    {
        Memory.Write(m_name, value);
    }

    public override int Expr()
    {
        return value();
    }
}
