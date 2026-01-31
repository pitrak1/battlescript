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
        var operatorIndex = InstructionUtilities.GetLowestPriorityOperatorIndex(tokens);
        
        if (assignmentIndex != -1)
            return new AssignmentInstruction(tokens);

        if (IsListComprehension(tokens))
            return new ListComprehensionInstruction(tokens);

        if (tokens[0].Type == Consts.TokenTypes.Keyword)
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
                    return new ConstantInstruction("None");
                case "for":
                    return new ForInstruction(tokens);
                case "else":
                case "elif":
                    return new ElseInstruction(tokens);
                case "break":
                    return new BreakInstruction();
                case "pass":
                    return new ConstantInstruction("None");
                case "continue":
                    return new ContinueInstruction();
                case "del":
                    return new DeleteInstruction(tokens);
                case "lambda":
                    return new LambdaInstruction(tokens);
                case "from":
                    return new ImportInstruction(tokens);
                case "raise":
                    return new RaiseInstruction(tokens);
                case "try":
                    return new TryInstruction(tokens);
                case "except":
                    return new ExceptInstruction(tokens);
                case "finally":
                    return new FinallyInstruction(tokens);
                case "assert":
                    return new AssertInstruction(tokens);
                case "global":
                    return new GlobalInstruction(tokens);
                case "nonlocal":
                    return new NonlocalInstruction(tokens);
                default:
                    throw new ParserUnexpectedTokenException(tokens[0]);
            }
        }

        if (tokens[0].Type == Consts.TokenTypes.Binding)
            return new BindingInstruction(tokens);

        if (commaIndex != -1 || colonIndex != -1)
            return new ArrayInstruction(tokens);

        if (operatorIndex != -1)
            return new OperationInstruction(tokens);

        if (tokens[0].Value == ".")
            return new MemberInstruction(tokens);

        if (tokens[0].Value == "*")
            return new ConstantInstruction("*");

        if (tokens[0].Type == Consts.TokenTypes.Bracket)
            return new ArrayInstruction(tokens);

        if (tokens[0].Type == Consts.TokenTypes.SpecialVariable)
            return new SpecialVariableInstruction(tokens);
        
        if (tokens[0].Type == Consts.TokenTypes.Identifier)
            return new VariableInstruction(tokens);

        if (tokens[0].Type is Consts.TokenTypes.String or Consts.TokenTypes.FormattedString)
            return new StringInstruction(tokens);

        if (tokens[0].Type == Consts.TokenTypes.Constant)
            return new ConstantInstruction(tokens);

        if (tokens[0].Type == Consts.TokenTypes.Numeric)
            return new NumericInstruction(tokens);

        throw new ParserUnexpectedTokenException(tokens[0]);
    }
    
    // Checks if the list of tokens is wrapped in square brackets and a "for" exists inside
    private static bool IsListComprehension(List<Token> tokens)
    {
        if (tokens.Count <= 2)
            return false;

        var closingBracketIndex = InstructionUtilities.GetTokenIndex(tokens, ["]"]);
        if (closingBracketIndex == -1)
            return false;

        var forIndex = InstructionUtilities.GetTokenIndex(tokens.GetRange(1, closingBracketIndex - 1), ["for"]);
        return forIndex != -1;
    }
}