using System.Diagnostics;
using BattleScript.Tokens;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.ParserNS;

public partial class InstructionParser
{
    private Instruction HandleSelf(List<Token> tokens)
    {
        Instruction? next = null;
        if (tokens.Count > 1)
        {
            next = Run(GetAllTokensButFirst(tokens));
        }

        return new SelfInstruction(next, tokens[0].Line, tokens[0].Column);
    }
}