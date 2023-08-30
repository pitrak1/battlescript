using System.Diagnostics;
using BattleScript.Tokens;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.ParserNS;

public partial class InstructionParser
{
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
        return new SquareBracesInstruction(values, null, tokens[0].Line, tokens[0].Column);
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

        return new DictionaryInstruction(values, next, tokens[0].Line, tokens[0].Column);
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

        return new ParensInstruction(values, next, tokens[0].Line, tokens[0].Column);
    }

    private Instruction HandleMember(List<Token> tokens)
    {
        Instruction property = new StringInstruction(tokens[1].Value, tokens[0].Line, tokens[0].Column);

        Instruction next = null;
        if (tokens.Count > 2)
        {
            next = Run(GetAllTokensButFirstTwo(tokens));
        }

        return new SquareBracesInstruction(new List<Instruction>() { property }, next, tokens[0].Line, tokens[0].Column);
    }
}