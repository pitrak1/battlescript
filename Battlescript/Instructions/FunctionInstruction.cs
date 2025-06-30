namespace Battlescript;

public class FunctionInstruction : Instruction, IEquatable<FunctionInstruction>
{
    public string Name { get; set; } 
    public List<Instruction> Parameters { get; set; }

    public FunctionInstruction(List<Token> tokens)
    {
        // def keyword, function name, open parens, close parens, and colon
        // if (tokens.Count < 5)
        // {
        //     ThrowErrorForToken("Invalid function definition", tokens[0]);
        // }
        //
        // if (tokens[^1].Value != ":")
        // {
        //     ThrowErrorForToken("Function definition should end with colon", tokens[0]);
        // }
        //
        // if (tokens[1].Type != Consts.TokenTypes.Identifier)
        // {
        //     ThrowErrorForToken("Invalid function name", tokens[1]);
        // }
        //
        // if (tokens[2].Type != Consts.TokenTypes.Separator || tokens[2].Value != "(")
        // {
        //     ThrowErrorForToken("Expected ( for function definition", tokens[2]);
        // }
        //
        // if (tokens[^2].Type != Consts.TokenTypes.Separator || tokens[^2].Value != ")")
        // {
        //     ThrowErrorForToken("Expected ) for function definition", tokens[^2]);
        // }

        var tokensInParens = tokens.GetRange(3, tokens.Count - 5);
        var parameters = InstructionUtilities.ParseEntriesBetweenDelimiters(tokensInParens, [","]);

        var inDefaultArguments = false;
        foreach (var parameter in parameters)
        {
            if (parameter is AssignmentInstruction)
            {
                inDefaultArguments = true;
            } else if (parameter is VariableInstruction && inDefaultArguments)
            {
                throw new Exception("Required arguments have to be before default arguments, fix this later");
            }
        }
        
        Name = tokens[1].Value;
        Parameters = parameters!;
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public FunctionInstruction(string name, List<Instruction>? parameters = null, List<Instruction>? instructions = null)
    {
        Name = name;
        Parameters = parameters ?? [];
        Instructions = instructions ?? [];
    }

    public override Variable Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var functionValue = new FunctionVariable(Parameters, Instructions);
        memory.SetVariable(new VariableInstruction(Name), functionValue);
        return functionValue;
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as FunctionInstruction);
    public bool Equals(FunctionInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;
        
        if (!Parameters.SequenceEqual(instruction.Parameters) || Name != instruction.Name) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Parameters, Name, Instructions);
}
