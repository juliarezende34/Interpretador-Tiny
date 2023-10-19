using System;

public abstract class BoolExpr
{
    public int Line { get; }

    protected BoolExpr(int line)
    {
        Line = line;
    }

    public abstract bool Expr();
}
