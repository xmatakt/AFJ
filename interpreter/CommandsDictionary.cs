using System.Collections.Generic;

namespace interpreter
{
    public static class CommandsDictionary
    {
        public static Dictionary<string, Commands> CmdDictionary = new Dictionary<string, Commands>()
        {
            {"0000", Commands.MoveRight},
            {"0001", Commands.MoveLeft},
            {"0010", Commands.Increment},
            {"0011", Commands.Decrement},
            {"0100", Commands.Write},
            {"0101", Commands.Read},
            {"0110", Commands.JumpForward},
            {"0111", Commands.JumpBackward},
            {"1000", Commands.AddDecadic},
            {"1001", Commands.RemoveDecadic},
            {"1010", Commands.Nop},
            {"1011", Commands.Reset},
            {"1100", Commands.JumpToBegin}
        };
    }
}
