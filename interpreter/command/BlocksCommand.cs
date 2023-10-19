using System;
using System.Collections.Generic;

public class BlocksCommand : Command
{
    private List<Command> m_cmds = new List<Command>();

    public BlocksCommand(int line) : base(line)
    {
    }

    public void AddCommand(Command cmd)
    {
        m_cmds.Add(cmd);
    }

    public override void Execute()
    {
        foreach (Command cmd in m_cmds)
        {
            cmd.Execute();
        }
    }
}
