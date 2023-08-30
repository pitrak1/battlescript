using System.Diagnostics;
using BattleScript.Tokens;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.ParserNS;

public partial class InstructionParser
{
    private Instruction HandleOperation(List<Token> tokens, int mathematicalOperatorIndex)
    {
        Instruction left = Run(GetTokensBeforeIndex(tokens, mathematicalOperatorIndex));
        Instruction right = Run(GetTokensAfterIndex(tokens, mathematicalOperatorIndex));

        return new OperationInstruction(
            tokens[mathematicalOperatorIndex].Value,
            left,
            right,
            tokens[0].Line,
            tokens[0].Column
        );
    }
}