using System;
public class BinaryIntExpr : IntExpr
{
    public enum Op
    {
        ADD,
        SUB,
        MUL,
        DIV,
        MOD,
        POT
    }

    private IntExpr m_left;
    private Op m_op;
    private IntExpr m_right;

    public BinaryIntExpr(int line, IntExpr left, Op op, IntExpr right) : base(line)
    {
        m_left = left;
        m_op = op;
        m_right = right;
    }

    public override int Expr()
    {
        int v1 = m_left.Expr();
        int v2 = m_right.Expr();

        switch (m_op)
        {
            case Op.ADD:
                return v1 + v2;
            case Op.SUB:
                return v1 - v2;
            case Op.MUL:
                return v1 * v2;
            case Op.DIV:
                return v1 / v2;
            case Op.POT:
                return (int)Math.Pow((double)v1, (double)v2);
            case Op.MOD:
            default:
                return v1 % v2;
        }
    }
}
