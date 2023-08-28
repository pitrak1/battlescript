using System.Diagnostics;
using BattleScript.Tokens;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.ParserNS;

public class InstructionParser
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
        else if (currentTokenSet[0].Type == Consts.TokenTypes.Number)
        {
            return HandleNumber(currentTokenSet);
        }
        else if (currentTokenSet[0].Type == Consts.TokenTypes.String)
        {
            return HandleString(currentTokenSet);
        }
        else if (currentTokenSet[0].Type == Consts.TokenTypes.Boolean)
        {
            return HandleBoolean(currentTokenSet);
        }
        else
        {
            return new Instruction();
        }
    }

    private Instruction HandleAssignment(List<Token> tokens, int assignmentOperatorIndex)
    {
        Instruction left = Run(tokens.GetRange(0, assignmentOperatorIndex));
        Instruction right = Run(tokens.GetRange(assignmentOperatorIndex + 1, tokens.Count - assignmentOperatorIndex - 1));

        return new AssignmentInstruction(left, right).SetDebugInfo(tokens[0].Line, tokens[0].Column);
    }

    private Instruction HandleSeparator(List<Token> tokens)
    {
        switch (tokens[0].Value)
        {
            case "[":
                return HandleSquareBraces(tokens);
            case "{":
                return HandleCurlyBraces(tokens);
            case "(":
                return HandleParens(tokens);
            case ".":
                return HandleMember(tokens);
            default:
                throw new SystemException("Invalid separator found");
        }
    }

    private Instruction HandleSquareBraces(List<Token> tokens)
    {
        List<List<Token>> entries = ParserUtilities.ParseUntilMatchingSeparator(tokens, new List<string>() { "," });
        List<Instruction> values = new List<Instruction>();
        foreach (List<Token> entry in entries)
        {
            values.Add(Run(entry));
        }
        return new SquareBracesInstruction(values).SetDebugInfo(tokens[0].Line, tokens[0].Column);
    }

    private Instruction HandleCurlyBraces(List<Token> tokens)
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

        return new DictionaryInstruction(values, next).SetDebugInfo(tokens[0].Line, tokens[0].Column);
    }

    private Instruction HandleParens(List<Token> tokens)
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

        return new ParensInstruction(values, next).SetDebugInfo(tokens[0].Line, tokens[0].Column);
    }

    private Instruction HandleMember(List<Token> tokens)
    {
        Instruction property = new StringInstruction(tokens[1].Value).SetDebugInfo(tokens[1].Line, tokens[1].Column);

        Instruction next = null;
        if (tokens.Count > 2)
        {
            next = Run(tokens.GetRange(2, tokens.Count - 2));
        }

        return new SquareBracesInstruction(new List<Instruction>() { property }, next).SetDebugInfo(tokens[0].Line, tokens[0].Column);
    }

    private Instruction HandleKeyword(List<Token> tokens)
    {
        switch (tokens[0].Value)
        {
            case "var":
                return HandleVar(tokens);
            case "const":
                return HandleConst(tokens);
            case "if":
                return HandleIf(tokens);
            case "else":
                return HandleElse(tokens);
            case "while":
                return HandleWhile(tokens);
            case "function":
                return HandleFunction(tokens);
            case "return":
                return HandleReturn(tokens);
            case "class":
                return HandleClass(tokens);
            case "self":
                return HandleSelf(tokens);
            case "super":
                return HandleSuper(tokens);
            case "constructor":
                return HandleConstructor(tokens);
            case "Btl":
                return HandleBtl(tokens);
            default:
                return new Instruction();
        }
    }

    private Instruction HandleVar(List<Token> tokens)
    {
        Debug.Assert(tokens.Count == 2);
        Debug.Assert(tokens[0].Type == Consts.TokenTypes.Keyword);
        Debug.Assert(tokens[0].Value == "var");
        Debug.Assert(tokens[1].Type == Consts.TokenTypes.Identifier);

        return new DeclarationInstruction(tokens[1].Value).SetDebugInfo(tokens[0].Line, tokens[0].Column);
    }

    private Instruction HandleConst(List<Token> tokens)
    {
        Debug.Assert(tokens.Count == 2);
        Debug.Assert(tokens[0].Type == Consts.TokenTypes.Keyword);
        Debug.Assert(tokens[0].Value == "const");
        Debug.Assert(tokens[1].Type == Consts.TokenTypes.Identifier);

        return new Instruction(
            Consts.InstructionTypes.ConstDeclaration,
            tokens[1].Value
        ).SetDebugInfo(tokens[0].Line, tokens[0].Column);
    }

    private Instruction HandleIf(List<Token> tokens)
    {
        // exclude the if itself and the start and ending parens
        Instruction condition = Run(tokens.GetRange(2, tokens.Count - 3));

        return new IfInstruction(condition).SetDebugInfo(tokens[0].Line, tokens[0].Column);
    }

    private Instruction HandleElse(List<Token> tokens)
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

        return new ElseInstruction(condition).SetDebugInfo(tokens[0].Line, tokens[0].Column);
    }

    private Instruction HandleWhile(List<Token> tokens)
    {
        // exclude the while itself and the start and ending parens
        Instruction condition = Run(tokens.GetRange(2, tokens.Count - 3));

        return new WhileInstruction(condition).SetDebugInfo(tokens[0].Line, tokens[0].Column);
    }

    private Instruction HandleFunction(List<Token> tokens)
    {
        // exclude the function itself and the start and ending parens
        List<Token> argTokens = tokens.GetRange(1, tokens.Count - 1);
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

        return new FunctionInstruction(instructionArgs).SetDebugInfo(tokens[0].Line, tokens[0].Column);
    }

    private Instruction HandleReturn(List<Token> tokens)
    {
        Instruction returnValue = Run(tokens.GetRange(1, tokens.Count - 1));

        return new ReturnInstruction(returnValue).SetDebugInfo(tokens[0].Line, tokens[0].Column);
    }

    private Instruction HandleClass(List<Token> tokens)
    {
        Instruction? value = null;
        if (tokens.Count > 1)
        {
            Debug.Assert(tokens.Count == 3);
            Debug.Assert(tokens[1].Value == "extends");
            value = new VariableInstruction(tokens[2].Value);
        }

        return new ClassInstruction(value).SetDebugInfo(tokens[0].Line, tokens[0].Column);
    }

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

    private Instruction HandleIdentifier(List<Token> tokens)
    {
        Instruction next = null;
        if (tokens.Count > 1)
        {
            next = Run(tokens.GetRange(1, tokens.Count - 1));
        }

        return new VariableInstruction(tokens[0].Value, next).SetDebugInfo(tokens[0].Line, tokens[0].Column);
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

    private Instruction HandleSelf(List<Token> tokens)
    {
        Instruction? next = null;
        if (tokens.Count > 1)
        {
            next = Run(tokens.GetRange(1, tokens.Count - 1));
        }

        return new SelfInstruction(next);
    }

    private Instruction HandleSuper(List<Token> tokens)
    {
        Instruction? next = null;
        if (tokens.Count > 1)
        {
            next = Run(tokens.GetRange(1, tokens.Count - 1));
        }

        return new SuperInstruction(next);
    }

    private Instruction HandleConstructor(List<Token> tokens)
    {
        // exclude the function itself and the start and ending parens
        List<Token> argTokens = tokens.GetRange(1, tokens.Count - 1);
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

        return new ConstructorInstruction(instructionArgs).SetDebugInfo(tokens[0].Line, tokens[0].Column);
    }

    private Instruction HandleBtl(List<Token> tokens)
    {
        Instruction? next = null;
        if (tokens.Count > 1)
        {
            next = Run(tokens.GetRange(1, tokens.Count - 1));
        }

        return new BtlInstruction(next);
    }
}