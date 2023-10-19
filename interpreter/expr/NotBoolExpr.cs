using System;

public class NotBoolExpr : BoolExpr
{
    private BoolExpr m_expr;

    public NotBoolExpr(int line, BoolExpr expr) : base(line)
    {
        m_expr = expr;
    }

    public override bool Expr()
    {
        return !m_expr.Expr();
    }
}