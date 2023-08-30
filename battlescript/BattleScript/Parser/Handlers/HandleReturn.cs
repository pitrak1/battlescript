using System.Diagnostics;
using BattleScript.Tokens;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.ParserNS;

public partial class InstructionParser
{
    private Instruction HandleReturn(List<Token> tokens)
    {
        Instruction returnValue = Run(GetAllTokensButFirst(tokens));

        return new ReturnInstruction(returnValue, tokens[0].Line, tokens[0].Column);
    }
}