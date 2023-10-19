using System;
namespace TinyInterpreter{
    class Program{
        static void Main(string[] args)
        {

            if (args.Length != 1)
            {
                Console.WriteLine("Usage: " + args[0] + " [Tiny program]");
                Environment.Exit(1);
            }

            try
            {
                LexicalAnalysis lex = new LexicalAnalysis(args[0]);
                SyntaticAnalysis s = new SyntaticAnalysis(lex);
                Console.WriteLine("Arquivo que está sendo usado: " + args[0]);
                
                Command cmd = s.start();
                cmd.Execute();
            }
            catch (Exception error)
            {
                Console.Error.WriteLine("error: " + error.Message);
            }
        }
    }
}
