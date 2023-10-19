using System;
using System.Text;
public class Lexeme{
    public string token{get;set;}
    public TokenType type{get;set;}

    public Lexeme(string t1, TokenType t2){
        token = t1;
        type = t2;
    }
    public Lexeme(){
        
    }
    public override string ToString(){
        StringBuilder sb = new StringBuilder();
        sb.Append("(\"");
        sb.Append(token);
        sb.Append("\", ");
        sb.Append(TokenTypeExtensions.tt2str(type));
        sb.Append(")");
        return sb.ToString();
    }
}
