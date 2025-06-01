namespace Battlescript;

public abstract class Instruction(int line = 0, int column = 0) : IEquatable<Instruction>
{
    public int Line { get; set; } = line;
    public int Column { get; set; } = column;
    public List<Instruction> Instructions { get; set; } = [];

    public static Instruction Parse(List<Token> tokens)
    {
        var assignmentIndex = ParserUtilities.GetTokenIndex(
            tokens, 
            types: [Consts.TokenTypes.Assignment]
        );
        var colonIndex = ParserUtilities.GetTokenIndex(tokens, [":"]);
        var operatorIndex = ParserUtilities.GetOperatorIndex(tokens);
        
        if (assignmentIndex != -1)
        {
            return new AssignmentInstruction(tokens);
        }
        else if (tokens[0].Type == Consts.TokenTypes.Separator)
        {
            switch (tokens[0].Value)
            {
                case "[":
                    return new SquareBracketsInstruction(tokens);
                case "(":
                    return new ParensInstruction(tokens);
                case ".":
                    return new SquareBracketsInstruction(tokens, isMember: true);
                case "{":
                    return new DictionaryInstruction(tokens);
                case ":":
                    return new KeyValuePairInstruction(tokens);
                default:
                    throw new ParserUnexpectedTokenException(tokens[0]);
            }
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
                default:
                    throw new ParserUnexpectedTokenException(tokens[0]);
            }
        }
        else if (colonIndex != -1)
        {
            return new KeyValuePairInstruction(tokens);
        }
        else if (operatorIndex != -1)
        {
            return new OperationInstruction(tokens);
        }
        else if (tokens[0].Type == Consts.TokenTypes.Identifier)
        {
            return new VariableInstruction(tokens);
        }
        else if (tokens[0].Type == Consts.TokenTypes.Number)
        {
            return new NumberInstruction(tokens);
        }
        else if (tokens[0].Type == Consts.TokenTypes.String)
        {
            return new StringInstruction(tokens);
        }
        else if (tokens[0].Type == Consts.TokenTypes.Boolean)
        {
            return new BooleanInstruction(tokens);
        }
        else
        {
            throw new ParserUnexpectedTokenException(tokens[0]);
        }
    }

    public abstract Variable Interpret(
        Memory memory,
        Variable? context = null,
        Variable? objectContext = null);

    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as Instruction);
    public bool Equals(Instruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;
        
        return Instructions.SequenceEqual(instruction.Instructions);
    }
    
    public override int GetHashCode() => HashCode.Combine(Instructions);
}