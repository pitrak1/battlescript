using System.Diagnostics;
using BattleScript.Tokens;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.ParserNS;

public partial class InstructionParser
{
    private Instruction HandleOperation(List<Token> tokens, int mathematicalOperatorIndex)
    {
        Instruction left = Run(tokens.GetRange(0, mathematicalOperatorIndex));
        Instruction right = Run(tokens.GetRange(mathematicalOperatorIndex + 1, tokens.Count - mathematicalOperatorIndex - 1));

        return new OperationInstruction(
            tokens[mathematicalOperatorIndex].Value,
            left,
            right
        ).SetDebugInfo(tokens[0].Line, tokens[0].Column);
    }
}