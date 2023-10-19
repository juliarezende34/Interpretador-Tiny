using System;
public class ReadIntExpr : IntExpr
{
    public ReadIntExpr(int line) : base(line)
    {
        // Implemente a lógica do construtor da classe ReadIntExpr (se necessário).
    }

    public override int Expr()
    {
        int value;
        if (int.TryParse(Console.ReadLine(), out value))
        {
            return value;
        }
        else
        {
            // Trate o erro de conversão aqui, se necessário.
            throw new Exception("Erro na conversão de entrada para int.");
        }
    }
}
