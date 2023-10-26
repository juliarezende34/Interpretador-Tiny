using System;
public class SyntaticAnalysis
{
    private LexicalAnalysis m_lex { get; set; }
    private Lexeme m_current { get; set; }

    public SyntaticAnalysis(LexicalAnalysis Lex)
    {
        m_current = Lex.NextToken();
        m_lex = Lex;
    }

    public Command start()
    {
        Command cmd = procProgram();
        eat(TokenType.TT_END_OF_FILE);
        return cmd;
    }

    public void advance()
    {
        m_current = m_lex.NextToken();
    }

    public void eat(TokenType type)
    {
        //Console.WriteLine($"Expected (..., {TokenTypeExtensions.tt2str(type)}), found (\"{m_current.token}\", {TokenTypeExtensions.tt2str(m_current.type)})");
        if (type == m_current.type)
        {
            advance();
        }
        else
        {
            showError();
        }
    }

    public void showError()
    {
        Console.Write($"{m_lex.Line():D2}: ");

        switch (m_current.type)
        {
            case TokenType.TT_INVALID_TOKEN:
                Console.WriteLine($"Lexema inválido [{m_current.token}]");
                break;
            case TokenType.TT_UNEXPECTED_EOF:
            case TokenType.TT_END_OF_FILE:
                Console.WriteLine("Fim de arquivo inesperado");
                break;
            default:
                Console.WriteLine($"Lexema não esperado [{m_current.token}]");
                break;
        }

        Environment.Exit(1);

    }

    // <program>   ::= program <cmdlist>

    public Command procProgram()
    {
        eat(TokenType.TT_PROGRAM);
        Command cmd = procCmdList();
        return cmd;
    }

    // <cmdlist>   ::= <cmd> { <cmd> }

    public BlocksCommand procCmdList()
    {
        int line = m_lex.Line();
        BlocksCommand cmds = new BlocksCommand(line);

        Command cmd = procCmd();
        cmds.AddCommand(cmd);

        while (m_current.type == TokenType.TT_VAR ||
            m_current.type == TokenType.TT_OUTPUT ||
            m_current.type == TokenType.TT_IF ||
            m_current.type == TokenType.TT_WHILE)
        {
            cmd = procCmd();
            cmds.AddCommand(cmd);
        }

        return cmds;
    }

    // <cmd>       ::= (<assign> | <output> | <if> | <while>) ;

    public Command procCmd()
    {
        Command cmd = null;

        if (m_current.type == TokenType.TT_VAR)
        {
            cmd = procAssign();


        }
        else if (m_current.type == TokenType.TT_OUTPUT)
        {
            cmd = procOutput();

        }
        else if (m_current.type == TokenType.TT_IF)
        {
            cmd = procIf();
        }
        else if (m_current.type == TokenType.TT_WHILE)
        {
            cmd = procWhile();

        }
        else
        {
            showError();
        }

        eat(TokenType.TT_SEMICOLON);
        return cmd;
    }

    // <assign>    ::= <var> = <intexpr>
    public AssignCommand procAssign()
    {
        int line = m_lex.Line();
        Variable var = procVar();
        eat(TokenType.TT_ASSIGN);
        IntExpr expr = procIntExpr();
        AssignCommand cmd = new AssignCommand(line, var, expr);
        return cmd;

    }

    // <output>    ::= output <intexpr>
    public OutputCommand procOutput()
    {
        eat(TokenType.TT_OUTPUT);
        int line = m_lex.Line();

        IntExpr expr = procIntExpr();
        OutputCommand cmd = new OutputCommand(line, expr);
        return cmd;
    }

    // <if>        ::= if <boolexpr> then <cmdlist> [ else <cmdlist> ] done
    public IfCommand procIf()
    {
        eat(TokenType.TT_IF);
        int line = m_lex.Line();

        BoolExpr cond = procBoolExpr();
        eat(TokenType.TT_THEN);
        Command thenCmds = procCmdList();
        Command elseCmds = null;

        if (m_current.type == TokenType.TT_ELSE)
        {
            advance();
            elseCmds = procCmdList();
        }

        eat(TokenType.TT_DONE);

        IfCommand cmd = new IfCommand(line, cond, thenCmds, elseCmds);
        return cmd;
    }

    // <while>     ::= while <boolexpr> do <cmdlist> done
    public WhileCommand procWhile()
    {
        eat(TokenType.TT_WHILE);
        int line = m_lex.Line();

        BoolExpr expr = procBoolExpr();

        eat(TokenType.TT_DO);

        Command cmds = procCmdList();
        eat(TokenType.TT_DONE);

        WhileCommand cmd = new WhileCommand(line, expr, cmds);
        return cmd;
    }

    // <boolexpr>  ::= false | true |
    //                 not <boolexpr> |
    //                 <intterm> (== | != | < | > | <= | >=) <intterm>
    public BoolExpr procBoolExpr()
    {
        if (m_current.type == TokenType.TT_FALSE)
        {
            advance();
            return new ConstBoolExpr(m_lex.Line(), false);
        }
        else if (m_current.type == TokenType.TT_TRUE)
        {
            advance();
            return new ConstBoolExpr(m_lex.Line(), false);
        }
        else if (m_current.type == TokenType.TT_NOT)
        {
            advance();
            int line = m_lex.Line();
            BoolExpr expr = procBoolExpr();
            return new NotBoolExpr(line, expr);
        }
        else
        {
            int line = m_lex.Line();
            IntExpr left = procIntTerm();

            SingleBoolExpr.Op op = SingleBoolExpr.Op.EQUAL;


            switch (m_current.type)
            {
                case TokenType.TT_EQUAL:
                    op = SingleBoolExpr.Op.EQUAL;
                    advance();
                    break;

                case TokenType.TT_NOT_EQUAL:
                    op = SingleBoolExpr.Op.NOT_EQUAL;
                    advance();
                    break;
                case TokenType.TT_LOWER:
                    op = SingleBoolExpr.Op.LOWER;
                    advance();
                    break;
                case TokenType.TT_GREATER:
                    op = SingleBoolExpr.Op.GREATER;
                    advance();
                    break;

                case TokenType.TT_LOWER_EQUAL:
                    op = SingleBoolExpr.Op.LOWER_EQUAL;
                    advance();
                    break;

                case TokenType.TT_GREATER_EQUAL:
                    op = SingleBoolExpr.Op.GREATER_EQUAL;
                    advance();
                    break;
                default:
                    showError();
                    break;
            }
            IntExpr right = procIntTerm();
            BoolExpr expr = new SingleBoolExpr(line, left, op, right);
            return expr;
        }
    }
    // <intexpr>   ::= [ + | - ] <intterm> [ (+ | - | * | / | % | ^) <intterm> ]
    public IntExpr procIntExpr()
    {
        bool isNegative = false;
        if (m_current.type == TokenType.TT_ADD)
        {
            advance();
        }
        else if (m_current.type == TokenType.TT_SUB)
        {
            advance();
            isNegative = true;
        }

        IntExpr left;
        if (isNegative)
        {
            int line = m_lex.Line();
            IntExpr tmp = procIntTerm();
            left = new NegIntExpr(line, tmp);
        }
        else
        {
            left = procIntTerm();
        }

        if (m_current.type == TokenType.TT_ADD ||
            m_current.type == TokenType.TT_SUB ||
            m_current.type == TokenType.TT_MUL ||
            m_current.type == TokenType.TT_DIV ||
            m_current.type == TokenType.TT_MOD ||
            m_current.type == TokenType.TT_POT)
        {
            int line = m_lex.Line();

            BinaryIntExpr.Op op;
            switch (m_current.type)
            {
                case TokenType.TT_ADD:
                    op = BinaryIntExpr.Op.ADD;
                    advance();
                    break;
                case TokenType.TT_SUB:
                    op = BinaryIntExpr.Op.SUB;
                    advance();
                    break;
                case TokenType.TT_MUL:
                    op = BinaryIntExpr.Op.MUL;
                    advance();
                    break;
                case TokenType.TT_DIV:
                    op = BinaryIntExpr.Op.DIV;
                    advance();
                    break;
                case TokenType.TT_POT:
                    op = BinaryIntExpr.Op.POT;
                    advance();
                    break;
                case TokenType.TT_MOD:
                default:
                    op = BinaryIntExpr.Op.MOD;
                    advance();
                    break;
            }

            IntExpr right = procIntTerm();

            left = new BinaryIntExpr(line, left, op, right);
        }

        return left;
    }

    // <intterm>   ::= <var> | <const> | read

    public IntExpr procIntTerm()
    {
        //tamo adiantadas rs
        if (m_current.type == TokenType.TT_VAR)
        {
            return procVar();
        }
        else if (m_current.type == TokenType.TT_NUMBER)
        {
            return procConst();
        }
        else
        {
            eat(TokenType.TT_READ);
            int line = m_lex.Line();
            ReadIntExpr expr = new ReadIntExpr(line);
            return expr;
        }
    }

    // <var>       ::= id


    public Variable procVar()
    {
        string tmp = m_current.token;

        eat(TokenType.TT_VAR);
        int line = m_lex.Line();

        Variable var = new Variable(line, tmp);
        return var;
    }


    // <const>     ::= number

    public ConstIntExpr procConst()
    {
        string tmp = m_current.token;

        eat(TokenType.TT_NUMBER);
        int line = m_lex.Line();

        int value = int.Parse(tmp);
        ConstIntExpr expr = new ConstIntExpr(line, value);
        return expr;
    }
}