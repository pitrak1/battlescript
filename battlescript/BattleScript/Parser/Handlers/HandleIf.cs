using System.Diagnostics;
using BattleScript.Tokens;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.ParserNS;

public partial class InstructionParser
{
    private Instruction HandleIf(List<Token> tokens)
    {
        // exclude the if itself and the start and ending parens
        Instruction condition = Run(GetTokensAfterKeywordWithoutParens(tokens));

        return new IfInstruction(condition, null, null, tokens[0].Line, tokens[0].Column);
    }
}