using System.Diagnostics;
using BattleScript.Tokens;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.ParserNS;

public partial class InstructionParser
{
    private Instruction HandleKeyword(List<Token> tokens)
    {
        switch (tokens[0].Value)
        {
            case "var":
                return HandleVar(tokens);
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

        return new DeclarationInstruction(tokens[1].Value, tokens[0].Line, tokens[0].Column);
    }

    private Instruction HandleIf(List<Token> tokens)
    {
        // exclude the if itself and the start and ending parens
        Instruction condition = Run(GetTokensAfterKeywordWithoutParens(tokens));

        return new IfInstruction(condition, null, null, tokens[0].Line, tokens[0].Column);
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
            condition = Run(GetTokensAfterTwoKeywordsWithoutParens(tokens));
            instruction.Value = condition;
        }

        return new ElseInstruction(condition, null, null, tokens[0].Line, tokens[0].Column);
    }

    private Instruction HandleWhile(List<Token> tokens)
    {
        // exclude the while itself and the start and ending parens
        Instruction condition = Run(GetTokensAfterKeywordWithoutParens(tokens));

        return new WhileInstruction(condition, null, tokens[0].Line, tokens[0].Column);
    }

    private Instruction HandleFunction(List<Token> tokens)
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

    private Instruction HandleReturn(List<Token> tokens)
    {
        Instruction returnValue = Run(GetAllTokensButFirst(tokens));

        return new ReturnInstruction(returnValue, tokens[0].Line, tokens[0].Column);
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

        return new ClassInstruction(value, null, tokens[0].Line, tokens[0].Column);
    }

    private Instruction HandleSelf(List<Token> tokens)
    {
        Instruction? next = null;
        if (tokens.Count > 1)
        {
            next = Run(GetAllTokensButFirst(tokens));
        }

        return new SelfInstruction(next, tokens[0].Line, tokens[0].Column);
    }

    private Instruction HandleSuper(List<Token> tokens)
    {
        Instruction? next = null;
        if (tokens.Count > 1)
        {
            next = Run(GetAllTokensButFirst(tokens));
        }

        return new SuperInstruction(next, tokens[0].Line, tokens[0].Column);
    }

    private Instruction HandleConstructor(List<Token> tokens)
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

        return new ConstructorInstruction(instructionArgs, null, tokens[0].Line, tokens[0].Column);
    }

    private Instruction HandleBtl(List<Token> tokens)
    {
        Instruction? next = null;
        if (tokens.Count > 1)
        {
            next = Run(GetAllTokensButFirst(tokens));
        }

        return new BtlInstruction(next, tokens[0].Line, tokens[0].Column);
    }
}