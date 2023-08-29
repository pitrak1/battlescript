using System.Diagnostics;
using BattleScript.Tokens;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.ParserNS;

public partial class InstructionParser
{
    public Instruction Run(List<Token> currentTokenSet)
    {
        int assignmentOperatorIndex = ParserUtilities.GetAssignmentOperatorIndex(currentTokenSet);
        int mathematicalOperatorIndex = ParserUtilities.GetMathematicalOperatorIndex(currentTokenSet);

        if (assignmentOperatorIndex != -1)
        {
            return HandleAssignment(currentTokenSet, assignmentOperatorIndex);
        }
        else if (currentTokenSet[0].Type == Consts.TokenTypes.Separator)
        {
            return HandleSeparator(currentTokenSet);
        }
        else if (currentTokenSet[0].Type == Consts.TokenTypes.Keyword)
        {
            return HandleKeyword(currentTokenSet);
        }
        else if (mathematicalOperatorIndex != -1)
        {
            return HandleOperation(currentTokenSet, mathematicalOperatorIndex);
        }
        else if (currentTokenSet[0].Type == Consts.TokenTypes.Identifier)
        {
            return HandleIdentifier(currentTokenSet);
        }
        else
        {
            return HandleLiteral(currentTokenSet);
        }
    }
}