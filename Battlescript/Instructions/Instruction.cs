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
                default:
                    throw new ParserUnexpectedTokenException(tokens[0]);
            }
        }
        else if (tokens[0].Type == Consts.TokenTypes.BuiltIn)
        {
            return new BuiltInInstruction(tokens);
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
        else if (tokens[0].Type == Consts.TokenTypes.Boolean)
        {
            return new BooleanInstruction(tokens);
        }
        else
        {
            throw new ParserUnexpectedTokenException(tokens[0]);
        }
    }

    // These three context are used for three distinct things:
    // - instructionContext is used for ongoing interpretations of a single instruction, i.e. a parens instruction
    // needs to know whether it's calling a function or class to be interpreted
    // - objectContext is used for class methods because the first argument to a method will always be `self`
    // - lexicalContext is used for keywords like `super` because we need to know in what class a method was actually
    // defined to find its superclass, the object is not enough
    public abstract Variable Interpret(
        Memory memory,
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null);

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