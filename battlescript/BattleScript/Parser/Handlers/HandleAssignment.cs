using System.Diagnostics;
using BattleScript.Tokens;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.ParserNS;

public partial class InstructionParser
{
    private Instruction HandleAssignment(List<Token> tokens, int assignmentOperatorIndex)
    {
        Instruction left = Run(tokens.GetRange(0, assignmentOperatorIndex));
        Instruction right = Run(tokens.GetRange(assignmentOperatorIndex + 1, tokens.Count - assignmentOperatorIndex - 1));

        return new AssignmentInstruction(left, right).SetDebugInfo(tokens[0].Line, tokens[0].Column);
    }
}