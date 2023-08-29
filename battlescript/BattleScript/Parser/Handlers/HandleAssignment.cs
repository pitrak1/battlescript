using System.Diagnostics;
using BattleScript.Tokens;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.ParserNS;

public partial class InstructionParser
{
    private Instruction HandleAssignment(List<Token> tokens, int assignmentOperatorIndex)
    {
        Instruction left = Run(GetTokensBeforeIndex(tokens, assignmentOperatorIndex));
        Instruction right = Run(GetTokensAfterIndex(tokens, assignmentOperatorIndex));

        return new AssignmentInstruction(left, right, tokens[0].Line, tokens[0].Column);
    }

    private List<Token> GetTokensBeforeIndex(List<Token> tokens, int index)
    {
        return tokens.GetRange(0, index);
    }

    private List<Token> GetTokensAfterIndex(List<Token> tokens, int index)
    {
        return tokens.GetRange(index + 1, tokens.Count - index - 1);
    }
}