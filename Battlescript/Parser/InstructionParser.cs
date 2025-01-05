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
        Instruction left = Run(tokens.GetRange(0, assignmentIndex));
        Instruction right = Run(tokens.GetRange(assignmentIndex + 1, tokens.Count - assignmentIndex - 1));
        
        return new Instruction(
            tokens[assignmentIndex].Line, 
            tokens[assignmentIndex].Column, 
            Consts.InstructionTypes.Assignment,
            tokens[assignmentIndex].Value,
            left,
            right
        );
    }

    private Instruction HandleSquareBraces(List<Token> tokens)
    {
        var results = InstructionParserUtilities.ParseTokensUntilMatchingSeparator(tokens, [","]);
        
        List<Instruction> values = [];
        foreach (var entry in results.Entries)
        {
              values.Add(Run(entry));
        }
        
        Instruction? next = null;
        if (tokens.Count > results.Count) 
        {
            next = Run(tokens.GetRange(results.Count, tokens.Count - results.Count));
        }
        
        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            Consts.InstructionTypes.SquareBraces,
            values,
            next
        );
    }

    private Instruction HandleOperation(List<Token> tokens, int operatorIndex)
    {
        Instruction left = Run(tokens.GetRange(0, operatorIndex));
        Instruction right = Run(tokens.GetRange(operatorIndex + 1, tokens.Count - operatorIndex - 1));
        
        return new Instruction(
            tokens[operatorIndex].Line, 
            tokens[operatorIndex].Column, 
            Consts.InstructionTypes.Operation,
            tokens[operatorIndex].Value,
            left,
            right
        );
    }

    private Instruction HandleIdentifier(List<Token> tokens)
    {
        Instruction? next = null;
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

    private Instruction HandleLiteral(List<Token> tokens, Consts.TokenTypes type)
    {
        if (tokens.Count != 1)
        {
            throw new Exception("Unexpected token");
        }

        dynamic? value;
        Consts.InstructionTypes instructionType;
        if (type == Consts.TokenTypes.Number)
        {
            value = Convert.ToDouble(tokens[0].Value);
            instructionType = Consts.InstructionTypes.Number;
        }
        else if (type == Consts.TokenTypes.String)
        {
            value = tokens[0].Value;
            instructionType = Consts.InstructionTypes.String;
        }
        else
        {
            value = tokens[0].Value == "True";
            instructionType = Consts.InstructionTypes.Boolean;
        }
        
        return new Instruction(tokens[0].Line, tokens[0].Column, instructionType, value);
    }
}