using System;

public abstract class IntExpr
{

    public int Line { get; }
    //public int Line { get; private set; }

    protected IntExpr(int line)
    {
        Line = line;
    }

    public abstract int Expr();
}
