using System.Diagnostics;
using BattleScript.Tokens;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.ParserNS;

public class InstructionParser
{
    public Instruction Run(List<Token> tokens)
    {
        int assignmentOperatorIndex = ParserUtilities.GetAssignmentOperatorIndex(tokens);
        int mathematicalOperatorIndex = ParserUtilities.GetMathematicalOperatorIndex(tokens);

        if (assignmentOperatorIndex != -1)
        {
            return handleAssignment(tokens, assignmentOperatorIndex);
        }
        else if (tokens[0].Type == Consts.TokenTypes.Separator)
        {
            switch (tokens[0].Value)
            {
                case "[":
                    return handleSquareBraces(tokens);
                case "{":
                    return handleCurlyBraces(tokens);
                case "(":
                    return handleParens(tokens);
                case ".":
                    return handleMember(tokens);
                default:
                    throw new SystemException("Invalid separator found");
            }
        }
        else if (tokens[0].Type == Consts.TokenTypes.Keyword)
        {
            switch (tokens[0].Value)
            {
                case "var":
                    return handleVar(tokens);
                case "if":
                    return handleIf(tokens);
                case "else":
                    return handleElse(tokens);
                case "while":
                    return handleWhile(tokens);
                case "function":
                    return handleFunction(tokens);
                case "return":
                    return handleReturn(tokens);
                case "Btl":
                    return handleBtl(tokens);
                default:
                    return new Instruction();
            }
        }
        else if (mathematicalOperatorIndex != -1)
        {
            return handleOperation(tokens, mathematicalOperatorIndex);
        }
        else if (tokens[0].Type == Consts.TokenTypes.Identifier)
        {
            return handleIdentifier(tokens);
        }
        else if (tokens[0].Type == Consts.TokenTypes.Number)
        {
            return handleNumber(tokens);
        }
        else if (tokens[0].Type == Consts.TokenTypes.String)
        {
            return handleString(tokens);
        }
        else
        {
            return handleBoolean(tokens);
        }
    }

    private Instruction handleAssignment(List<Token> tokens, int assignmentOperatorIndex)
    {
        Instruction left = Run(GetTokensBeforeIndex(tokens, assignmentOperatorIndex));
        Instruction right = Run(GetTokensAfterIndex(tokens, assignmentOperatorIndex));

        return new AssignmentInstruction(left, right, tokens[0].Line, tokens[0].Column);
    }

    private Instruction handleBoolean(List<Token> tokens)
    {
        Debug.Assert(tokens.Count == 1);
        Debug.Assert(tokens[0].Type == Consts.TokenTypes.Boolean);

        bool value = tokens[0].Value == "true";

        return new BooleanInstruction(value, tokens[0].Line, tokens[0].Column);
    }

    private Instruction handleBtl(List<Token> tokens)
    {
        Instruction? next = null;
        if (tokens.Count > 1)
        {
            next = Run(GetAllTokensButFirst(tokens));
        }

        return new BtlInstruction(next, tokens[0].Line, tokens[0].Column);
    }

    private Instruction handleElse(List<Token> tokens)
    {
        Instruction instruction = new Instruction();

        // this is an else if block
        Instruction? condition = null;
        if (tokens.Count > 1)
        {
            Debug.Assert(tokens[1].Value == "if");
            // exclude the else if and the start and ending parens
            condition = Run(GetTokensAfterTwoKeywordsWithoutParens(tokens));
            instruction.Value = condition;
        }

        return new ElseInstruction(condition, null, null, tokens[0].Line, tokens[0].Column);
    }

    private Instruction handleFunction(List<Token> tokens)
    {
        List<Token> argTokens = GetAllTokensButFirst(tokens);
        List<List<Token>> tokenizedArgs =
            ParserUtilities.ParseUntilMatchingSeparator(argTokens, new List<string>() { "," });

        List<Instruction> instructionArgs = new List<Instruction>();
        foreach (List<Token> arg in tokenizedArgs)
        {
            if (arg.Count > 0)
            {
                Instruction instructionArg = Run(arg);
                Debug.Assert(instructionArg.Type == Consts.InstructionTypes.Variable);
                instructionArgs.Add(instructionArg);
            }
        }

        return new FunctionInstruction(instructionArgs, null, tokens[0].Line, tokens[0].Column);
    }

    private Instruction handleIdentifier(List<Token> tokens)
    {
        Instruction next = null;
        if (tokens.Count > 1)
        {
            next = Run(GetAllTokensButFirst(tokens));
        }

        return new VariableInstruction(tokens[0].Value, next, tokens[0].Line, tokens[0].Column);
    }

    private Instruction handleIf(List<Token> tokens)
    {
        // exclude the if itself and the start and ending parens
        Instruction condition = Run(GetTokensAfterKeywordWithoutParens(tokens));

        return new IfInstruction(condition, null, null, tokens[0].Line, tokens[0].Column);
    }

    private Instruction handleNumber(List<Token> tokens)
    {
        Debug.Assert(tokens.Count == 1);
        Debug.Assert(tokens[0].Type == Consts.TokenTypes.Number);

        return new NumberInstruction(int.Parse(tokens[0].Value), tokens[0].Line, tokens[0].Column);
    }

    private Instruction handleOperation(List<Token> tokens, int mathematicalOperatorIndex)
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

    private Instruction handleReturn(List<Token> tokens)
    {
        Instruction returnValue = Run(GetAllTokensButFirst(tokens));

        return new ReturnInstruction(returnValue, tokens[0].Line, tokens[0].Column);
    }

    private Instruction handleSquareBraces(List<Token> tokens)
    {
        List<List<Token>> entries = ParserUtilities.ParseUntilMatchingSeparator(tokens, new List<string>() { "," });
        List<Instruction> values = new List<Instruction>();
        foreach (List<Token> entry in entries)
        {
            values.Add(Run(entry));
        }
        return new SquareBracesInstruction(values, null, tokens[0].Line, tokens[0].Column);
    }

    private Instruction handleCurlyBraces(List<Token> tokens)
    {
        List<List<Token>> entries = ParserUtilities.ParseUntilMatchingSeparator(tokens, new List<string>() { ",", ":" });
        Debug.Assert(entries.Count % 2 == 0);

        List<Instruction> values = new List<Instruction>();
        foreach (List<Token> entry in entries)
        {
            values.Add(Run(entry));
        }

        Instruction next = null;
        int entriesLength = ParserUtilities.GetTokenLengthOfEntries(entries);
        if (tokens.Count > entriesLength)
        {
            next = Run(tokens.GetRange(entriesLength, tokens.Count - entriesLength));
        }

        return new DictionaryInstruction(values, next, tokens[0].Line, tokens[0].Column);
    }

    private Instruction handleParens(List<Token> tokens)
    {
        List<List<Token>> entries = ParserUtilities.ParseUntilMatchingSeparator(tokens, new List<string>() { "," });
        List<Instruction> values = new List<Instruction>();
        foreach (List<Token> entry in entries)
        {
            if (entry.Count > 0)
            {
                values.Add(Run(entry));
            }
        }

        Instruction next = null;
        int entriesLength = ParserUtilities.GetTokenLengthOfEntries(entries);
        if (tokens.Count > entriesLength)
        {
            next = Run(tokens.GetRange(entriesLength, tokens.Count - entriesLength));
        }

        return new ParensInstruction(values, next, tokens[0].Line, tokens[0].Column);
    }

    private Instruction handleMember(List<Token> tokens)
    {
        Instruction property = new StringInstruction(tokens[1].Value, tokens[0].Line, tokens[0].Column);

        Instruction next = null;
        if (tokens.Count > 2)
        {
            next = Run(GetAllTokensButFirstTwo(tokens));
        }

        return new SquareBracesInstruction(new List<Instruction>() { property }, next, tokens[0].Line, tokens[0].Column);
    }

    private Instruction handleString(List<Token> tokens)
    {
        Debug.Assert(tokens.Count == 1);
        Debug.Assert(tokens[0].Type == Consts.TokenTypes.String);

        string trimmedValue = tokens[0].Value.Substring(1, tokens[0].Value.Length - 2);

        return new StringInstruction(trimmedValue, tokens[0].Line, tokens[0].Column);
    }

    private Instruction handleVar(List<Token> tokens)
    {
        Debug.Assert(tokens.Count == 2);
        Debug.Assert(tokens[0].Type == Consts.TokenTypes.Keyword);
        Debug.Assert(tokens[0].Value == "var");
        Debug.Assert(tokens[1].Type == Consts.TokenTypes.Identifier);

        return new DeclarationInstruction(tokens[1].Value, tokens[0].Line, tokens[0].Column);
    }

    private Instruction handleWhile(List<Token> tokens)
    {
        // exclude the while itself and the start and ending parens
        Instruction condition = Run(GetTokensAfterKeywordWithoutParens(tokens));

        return new WhileInstruction(condition, null, tokens[0].Line, tokens[0].Column);
    }

    private List<Token> GetTokensBeforeIndex(List<Token> tokens, int index)
    {
        return tokens.GetRange(0, index);
    }

    private List<Token> GetTokensAfterIndex(List<Token> tokens, int index)
    {
        return tokens.GetRange(index + 1, tokens.Count - index - 1);
    }

    private List<Token> GetAllTokensButFirst(List<Token> tokens)
    {
        return tokens.GetRange(1, tokens.Count - 1);
    }

    private List<Token> GetAllTokensButFirstTwo(List<Token> tokens)
    {
        return tokens.GetRange(2, tokens.Count - 2);
    }

    private List<Token> GetTokensAfterKeywordWithoutParens(List<Token> tokens)
    {
        return tokens.GetRange(2, tokens.Count - 3);
    }

    private List<Token> GetTokensAfterTwoKeywordsWithoutParens(List<Token> tokens)
    {
        return tokens.GetRange(3, tokens.Count - 4);
    }
}