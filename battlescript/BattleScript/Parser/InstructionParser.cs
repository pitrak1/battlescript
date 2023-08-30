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
            switch (currentTokenSet[0].Value)
            {
                case "var":
                    return HandleVar(currentTokenSet);
                case "if":
                    return HandleIf(currentTokenSet);
                case "else":
                    return HandleElse(currentTokenSet);
                case "while":
                    return HandleWhile(currentTokenSet);
                case "function":
                    return HandleFunction(currentTokenSet);
                case "return":
                    return HandleReturn(currentTokenSet);
                case "class":
                    return HandleClass(currentTokenSet);
                case "self":
                    return HandleSelf(currentTokenSet);
                case "super":
                    return HandleSuper(currentTokenSet);
                case "constructor":
                    return HandleConstructor(currentTokenSet);
                case "Btl":
                    return HandleBtl(currentTokenSet);
                default:
                    return new Instruction();
            }
        }
        else if (mathematicalOperatorIndex != -1)
        {
            return HandleOperation(currentTokenSet, mathematicalOperatorIndex);
        }
        else if (currentTokenSet[0].Type == Consts.TokenTypes.Identifier)
        {
            return HandleIdentifier(currentTokenSet);
        }
        else if (currentTokenSet[0].Type == Consts.TokenTypes.Number)
        {
            return HandleNumber(currentTokenSet);
        }
        else if (currentTokenSet[0].Type == Consts.TokenTypes.String)
        {
            return HandleString(currentTokenSet);
        }
        else
        {
            return HandleBoolean(currentTokenSet);
        }
    }
}