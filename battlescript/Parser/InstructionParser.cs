using System.Diagnostics;
using BattleScript.Tokens;
using BattleScript.Instructions;

namespace BattleScript.Core;

public class InstructionParser
{
    public Instruction Run(List<Token> tokens)
    {
        int assignmentOperatorIndex = Utilities.GetAssignmentOperatorIndex(tokens);
        int mathematicalOperatorIndex = Utilities.GetMathematicalOperatorIndex(tokens);

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
                    return handleDeclaration(tokens);
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
                case "breakpoint":
                    return handleBreakpoint(tokens);
                default:
                    throw new SystemException("Invalid keyword found");
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
        Instruction left = Run(tokens.GetRange(0, assignmentOperatorIndex));
        Instruction right = Run(
            tokens.GetRange(
                assignmentOperatorIndex + 1,
                tokens.Count - assignmentOperatorIndex - 1
            )
        );

        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            Consts.InstructionTypes.Assignment,
            null,
            null,
            null,
            left,
            right
        );
    }

    private Instruction handleBoolean(List<Token> tokens)
    {
        Debug.Assert(tokens.Count == 1);
        Debug.Assert(tokens[0].Type == Consts.TokenTypes.Boolean);

        bool value = tokens[0].Value == "true";

        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            Consts.InstructionTypes.Boolean,
            value
        );
    }

    private Instruction handleBtl(List<Token> tokens)
    {
        Instruction? next = null;
        if (tokens.Count > 1)
        {
            List<Token> allTokensButFirst = tokens.GetRange(1, tokens.Count - 1);
            next = Run(allTokensButFirst);
        }

        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            Consts.InstructionTypes.Btl,
            null,
            next
        );
    }

    private Instruction handleBreakpoint(List<Token> tokens)
    {
        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            Consts.InstructionTypes.Breakpoint
        );
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
            condition = Run(tokens.GetRange(3, tokens.Count - 4));
            instruction.Value = condition;
        }

        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            Consts.InstructionTypes.Else,
            condition
        );
    }

    private Instruction handleFunction(List<Token> tokens)
    {
        List<Token> argTokens = tokens.GetRange(1, tokens.Count - 1);
        (int Count, List<List<Token>> Result) tokenizedArgs =
            Utilities.ParseUntilMatchingSeparator(argTokens, new List<string>() { "," });

        List<Instruction> instructionArgs = new List<Instruction>();
        foreach (List<Token> arg in tokenizedArgs.Result)
        {
            if (arg.Count > 0)
            {
                Instruction instructionArg = Run(arg);
                Debug.Assert(instructionArg.Type == Consts.InstructionTypes.Variable);
                instructionArgs.Add(instructionArg);
            }
        }

        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            Consts.InstructionTypes.Function,
            instructionArgs
        );
    }

    private Instruction handleIdentifier(List<Token> tokens)
    {
        Instruction next = null;
        if (tokens.Count > 1)
        {
            next = Run(tokens.GetRange(1, tokens.Count - 1));
        }

        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            Consts.InstructionTypes.Variable,
            tokens[0].Value,
            next
        );
    }

    private Instruction handleIf(List<Token> tokens)
    {
        // exclude the if itself and the start and ending parens
        Instruction condition = Run(tokens.GetRange(2, tokens.Count - 3));

        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            Consts.InstructionTypes.If,
            condition
        );
    }

    private Instruction handleNumber(List<Token> tokens)
    {
        Debug.Assert(tokens.Count == 1);
        Debug.Assert(tokens[0].Type == Consts.TokenTypes.Number);

        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            Consts.InstructionTypes.Number,
            Convert.ToDouble(tokens[0].Value)
        );
    }

    private Instruction handleOperation(List<Token> tokens, int mathematicalOperatorIndex)
    {
        Instruction left = Run(tokens.GetRange(0, mathematicalOperatorIndex));
        Instruction right = Run(tokens.GetRange(
            mathematicalOperatorIndex + 1,
            tokens.Count - mathematicalOperatorIndex - 1
        ));

        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            Consts.InstructionTypes.Operation,
            tokens[mathematicalOperatorIndex].Value,
            null,
            null,
            left,
            right
        );
    }

    private Instruction handleReturn(List<Token> tokens)
    {
        Instruction? returnValue = null;
        if (tokens.Count > 1)
        {
            returnValue = Run(tokens.GetRange(1, tokens.Count - 1));
        }

        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            Consts.InstructionTypes.Return,
            returnValue
        );
    }

    private Instruction handleSquareBraces(List<Token> tokens)
    {
        (int Count, List<List<Token>> Result) entries =
            Utilities.ParseUntilMatchingSeparator(tokens, new List<string>() { "," });
        List<Instruction> values = new List<Instruction>();
        foreach (List<Token> entry in entries.Result)
        {
            values.Add(Run(entry));
        }

        Instruction next = null;
        if (tokens.Count > entries.Count)
        {
            next = Run(tokens.GetRange(entries.Count, tokens.Count - entries.Count));
        }

        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            Consts.InstructionTypes.SquareBraces,
            values,
            next
        );
    }

    private Instruction handleCurlyBraces(List<Token> tokens)
    {
        (int Count, List<List<Token>> Result) entries =
            Utilities.ParseUntilMatchingSeparator(tokens, new List<string>() { ",", ":" });
        Debug.Assert(entries.Result.Count % 2 == 0);

        List<Instruction> values = new List<Instruction>();
        foreach (List<Token> entry in entries.Result)
        {
            values.Add(Run(entry));
        }

        Instruction next = null;
        if (tokens.Count > entries.Count)
        {
            next = Run(tokens.GetRange(entries.Count, tokens.Count - entries.Count));
        }

        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            Consts.InstructionTypes.Dictionary,
            values,
            next
        );
    }

    private Instruction handleParens(List<Token> tokens)
    {
        (int Count, List<List<Token>> Result) entries =
            Utilities.ParseUntilMatchingSeparator(tokens, new List<string>() { "," });
        List<Instruction> values = new List<Instruction>();
        foreach (List<Token> entry in entries.Result)
        {
            if (entry.Count > 0)
            {
                values.Add(Run(entry));
            }
        }

        Instruction next = null;
        if (tokens.Count > entries.Count)
        {
            next = Run(tokens.GetRange(entries.Count, tokens.Count - entries.Count));
        }

        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            Consts.InstructionTypes.Parens,
            values,
            next
        );
    }

    private Instruction handleMember(List<Token> tokens)
    {
        Instruction property = new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            Consts.InstructionTypes.String,
            tokens[1].Value
        );

        Instruction? next = null;
        if (tokens.Count > 2)
        {
            next = Run(tokens.GetRange(2, tokens.Count - 2));
        }

        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            Consts.InstructionTypes.SquareBraces,
            new List<Instruction>() { property },
            next
        );
    }

    private Instruction handleString(List<Token> tokens)
    {
        Debug.Assert(tokens.Count == 1);
        Debug.Assert(tokens[0].Type == Consts.TokenTypes.String);

        string trimmedValue = tokens[0].Value.Substring(1, tokens[0].Value.Length - 2);

        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            Consts.InstructionTypes.String,
            trimmedValue
        );
    }

    private Instruction handleDeclaration(List<Token> tokens)
    {
        Debug.Assert(tokens.Count == 2);
        Debug.Assert(tokens[0].Type == Consts.TokenTypes.Keyword);
        Debug.Assert(tokens[0].Value == "var");
        Debug.Assert(tokens[1].Type == Consts.TokenTypes.Identifier);

        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            Consts.InstructionTypes.Declaration,
            tokens[1].Value
        );
    }

    private Instruction handleWhile(List<Token> tokens)
    {
        // exclude the while itself and the start and ending parens
        Instruction condition = Run(tokens.GetRange(2, tokens.Count - 3));

        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            Consts.InstructionTypes.While,
            condition
        );
    }
}