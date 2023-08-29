using System.Diagnostics;
using BattleScript.Tokens;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.ParserNS;

public partial class InstructionParser
{
    private Instruction HandleLiteral(List<Token> tokens)
    {
        switch (tokens[0].Type)
        {
            case Consts.TokenTypes.Number:
                return HandleNumber(tokens);
            case Consts.TokenTypes.String:
                return HandleString(tokens);
            case Consts.TokenTypes.Boolean:
                return HandleBoolean(tokens);
            default:
                return new Instruction();
        }
    }

    private Instruction HandleNumber(List<Token> tokens)
    {
        Debug.Assert(tokens.Count == 1);
        Debug.Assert(tokens[0].Type == Consts.TokenTypes.Number);

        return new NumberInstruction(int.Parse(tokens[0].Value)).SetDebugInfo(tokens[0].Line, tokens[0].Column);
    }

    private Instruction HandleString(List<Token> tokens)
    {
        Debug.Assert(tokens.Count == 1);
        Debug.Assert(tokens[0].Type == Consts.TokenTypes.String);

        string trimmedValue = tokens[0].Value.Substring(1, tokens[0].Value.Length - 2);

        return new StringInstruction(trimmedValue).SetDebugInfo(tokens[0].Line, tokens[0].Column);
    }

    private Instruction HandleBoolean(List<Token> tokens)
    {
        Debug.Assert(tokens.Count == 1);
        Debug.Assert(tokens[0].Type == Consts.TokenTypes.Boolean);

        bool value = tokens[0].Value == "true";

        return new BooleanInstruction(value).SetDebugInfo(tokens[0].Line, tokens[0].Column);
    }
}