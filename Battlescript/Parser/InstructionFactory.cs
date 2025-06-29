namespace Battlescript;

public static class InstructionFactory
{
    public static Instruction? Create(List<Token> tokens)
    {
        if (tokens.Count == 0)
        {
            return null;
        }
        
        var assignmentIndex = InstructionUtilities.GetTokenIndex(
            tokens, 
            types: [Consts.TokenTypes.Assignment]
        );
        var colonIndex = InstructionUtilities.GetTokenIndex(tokens, [":"]);
        var commaIndex = InstructionUtilities.GetTokenIndex(tokens, [","]);
        var operatorIndex = InstructionUtilities.GetOperatorIndex(tokens);
        
        if (assignmentIndex != -1)
        {
            return new AssignmentInstruction(tokens);
        }
        else if (tokens[0].Type == Consts.TokenTypes.Keyword)
        {
            switch (tokens[0].Value)
            {
                case "if":
                    return new IfInstruction(tokens);
                case "while":
                    return new WhileInstruction(tokens);
                case "def":
                    return new FunctionInstruction(tokens);
                case "return":
                    return new ReturnInstruction(tokens);
                case "class":
                    return new ClassInstruction(tokens);
                case "None":
                    return new NoneInstruction();
                case "for":
                    return new ForInstruction(tokens);
                case "else":
                case "elif":
                    return new ElseInstruction(tokens);
                case "break":
                    return new BreakInstruction();
                case "pass":
                    return new NoneInstruction();
                case "continue":
                    return new ContinueInstruction();
                case "lambda":
                    return new LambdaInstruction(tokens);
                default:
                    throw new ParserUnexpectedTokenException(tokens[0]);
            }
        }
        else if (tokens[0].Type == Consts.TokenTypes.BuiltIn)
        {
            return new BuiltInInstruction(tokens);
        }
        else if (commaIndex != -1)
        {
            return new CommaSeparatedArrayInstruction(tokens);
        } else if (colonIndex != -1)
        {
            return new ColonSeparatedArrayInstruction(tokens);
        }
        else if (operatorIndex != -1)
        {
            return new OperationInstruction(tokens);
        }
        else if (tokens[0].Type == Consts.TokenTypes.Separator)
        {
            switch (tokens[0].Value)
            {
                case "[":
                    return new SquareBracketsInstruction(tokens);
                case "(":
                    return new ParenthesesInstruction(tokens);
                case ".":
                    return new SquareBracketsInstruction(tokens);
                case "{":
                    return new CurlyBracesInstruction(tokens);
                default:
                    throw new ParserUnexpectedTokenException(tokens[0]);
            }
        }
        else if (tokens[0].Type == Consts.TokenTypes.Identifier)
        {
            return new VariableInstruction(tokens);
        }
        else if (tokens[0].Type == Consts.TokenTypes.Float)
        {
            return new FloatInstruction(tokens);
        }
        else if (tokens[0].Type == Consts.TokenTypes.Integer)
        {
            return new IntegerInstruction(tokens);
        }
        else if (tokens[0].Type == Consts.TokenTypes.String)
        {
            return new StringInstruction(tokens);
        }
        else if (tokens[0].Type == Consts.TokenTypes.Constant)
        {
            return new ConstantInstruction(tokens);
        }
        else
        {
            throw new ParserUnexpectedTokenException(tokens[0]);
        }
    }
}