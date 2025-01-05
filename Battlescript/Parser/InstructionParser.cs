namespace Battlescript;

public class InstructionParser
{
    public Instruction Run(List<Token> tokens)
    {
        var assignmentIndex = InstructionParserUtilities.GetAssignmentIndex(tokens);
        var operatorIndex = InstructionParserUtilities.GetOperatorIndex(tokens);
        
        Console.WriteLine(tokens.Count);
        Console.WriteLine(assignmentIndex);

        if (assignmentIndex != -1)
        {
            return HandleAssignment(tokens, assignmentIndex);
        }
        else if (operatorIndex != -1)
        {
            return HandleOperation(tokens, operatorIndex);
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
        );{}
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