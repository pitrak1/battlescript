namespace Battlescript;

public class InstructionParser
{
    public Instruction Run(List<Token> tokens)
    {
        var assignmentIndex = InstructionParserUtilities.GetAssignmentIndex(tokens);
        var operatorIndex = InstructionParserUtilities.GetOperatorIndex(tokens);
        
        if (assignmentIndex != -1)
        {
            return HandleAssignment(tokens, assignmentIndex);
        }
        else if (tokens[0].Type == Consts.TokenTypes.Separator)
        {
            switch (tokens[0].Value)
            {
                case "[":
                    return HandleSquareBraces(tokens);
                // case "{":
                //     return HandleCurlyBraces(tokens);
                // case "(":
                //     return HandleParenthesis(tokens);
                // case ".":
                //     return HandleMember(tokens);
                default:
                    throw new Exception("Unexpected token");
            }
        }
        else if (operatorIndex != -1)
        {
            return HandleOperation(tokens, operatorIndex);
        }
        else if (tokens[0].Type == Consts.TokenTypes.Identifier)
        {
            return HandleIdentifier(tokens);
        }
        else if (tokens[0].Type is Consts.TokenTypes.Number or Consts.TokenTypes.String or Consts.TokenTypes.Boolean)
        {
            return HandleLiteral(tokens, tokens[0].Type);
        }
        else
        {
            throw new Exception("Syntax error");
        }
    }

    private Instruction HandleAssignment(List<Token> tokens, int assignmentIndex)
    {
        var assignmentToken = tokens[assignmentIndex];
        var result = RunLeftAndRightAroundIndex(tokens, assignmentIndex);
        return new Instruction(
            assignmentToken.Line, 
            assignmentToken.Column, 
            Consts.InstructionTypes.Assignment,
            assignmentToken.Value,
            result.Left,
            result.Right
        );
    }

    private Instruction HandleSquareBraces(List<Token> tokens)
    {
        var results = ParseAndRunEntriesWithinSeparator(tokens, [","]);
        var next = CheckAndRunFollowingTokens(tokens, results.Count);
        
        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            Consts.InstructionTypes.SquareBraces,
            results.Values,
            next
        );
    }

    private Instruction HandleOperation(List<Token> tokens, int operatorIndex)
    {
        var operatorToken = tokens[operatorIndex];
        var result = RunLeftAndRightAroundIndex(tokens, operatorIndex);
        return new Instruction(
            operatorToken.Line, 
            operatorToken.Column, 
            Consts.InstructionTypes.Operation,
            operatorToken.Value,
            result.Left,
            result.Right
        );
    }

    private Instruction HandleIdentifier(List<Token> tokens)
    {
        var next = CheckAndRunFollowingTokens(tokens, 1);
        
        return new Instruction(
            tokens[0].Line, 
            tokens[0].Column, 
            Consts.InstructionTypes.Variable, 
            tokens[0].Value, 
            next
        );
    }

    private Instruction HandleLiteral(List<Token> tokens, Consts.TokenTypes type)
    {
        if (tokens.Count != 1)
        {
            throw new Exception("Unexpected token");
        }

        dynamic? value;
        Consts.InstructionTypes instructionType;
        switch (type)
        {
            case Consts.TokenTypes.Number:
                value = Convert.ToDouble(tokens[0].Value);
                instructionType = Consts.InstructionTypes.Number;
                break;
            case Consts.TokenTypes.String:
                value = tokens[0].Value;
                instructionType = Consts.InstructionTypes.String;
                break;
            default:
                value = tokens[0].Value == "True";
                instructionType = Consts.InstructionTypes.Boolean;
                break;
        }
        
        return new Instruction(tokens[0].Line, tokens[0].Column, instructionType, value);
    }

    private (int Count, List<Instruction> Values) ParseAndRunEntriesWithinSeparator(
        List<Token> tokens, 
        List<string> separators
    )
    {
        var results = InstructionParserUtilities.ParseTokensUntilMatchingSeparator(tokens, separators);
        
        List<Instruction> values = [];
        foreach (var entry in results.Entries)
        {
            values.Add(Run(entry));
        }

        return (results.Count, values);
    }

    private (Instruction? Left, Instruction? Right) RunLeftAndRightAroundIndex(List<Token> tokens, int index)
    {
        var left = Run(tokens.GetRange(0, index));
        var right = Run(tokens.GetRange(index + 1, tokens.Count - index - 1));

        return (left, right);
    }

    private Instruction? CheckAndRunFollowingTokens(List<Token> tokens, int expectedCount)
    {
        return tokens.Count > expectedCount ? 
            Run(tokens.GetRange(expectedCount, tokens.Count - expectedCount)) : 
            null;
    }
}