using System;
public abstract class Command
{
    public int Line { get; }
    //public int Line;

    public Command(int line)
    {
      Line = line;
    }

    // public int Line
    // {

    //     get { return Line; }

    // }

    public abstract void Execute();
}
