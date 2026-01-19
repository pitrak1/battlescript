namespace Battlescript;

public class FunctionInstruction : Instruction
{
    public string Name { get; set; } 
    public ParameterSet Parameters { get; set; }

    public FunctionInstruction(List<Token> tokens) : base(tokens)
    {
        CheckTokenValidity(tokens);
        var tokensInParens = tokens.GetRange(3, tokens.Count - 5);
        var parameters = InstructionUtilities.ParseEntriesBetweenDelimiters(tokensInParens, [","]);
        Name = tokens[1].Value;
        Parameters = new ParameterSet(parameters!);
    }

    private void CheckTokenValidity(List<Token> tokens)
    {
        if (tokens[^1].Value is not ":")
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "invalid syntax");
        }

        if (tokens[^2].Value is not ")" || tokens[2].Value is not "(")
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "invalid syntax");
        }
        
        if (tokens[1].Type != Consts.TokenTypes.Identifier)
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "invalid syntax");
        }
    }

    public FunctionInstruction(
        string name, 
        ParameterSet? parameters = null, 
        List<Instruction>? instructions = null, 
        int? line = null, 
        string? expression = null) : base(line, expression)
    {
        Name = name;
        Parameters = parameters ?? new ParameterSet();
        Instructions = instructions ?? [];
    }

    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        var functionValue = new FunctionVariable(Name, closure, Parameters, Instructions);
        closure.SetVariable(callStack, new VariableInstruction(Name), functionValue);
        return functionValue;
    }
    
    // All the code below is to override equality
    public override bool Equals(object? obj) => Equals(obj as FunctionInstruction);
    public bool Equals(FunctionInstruction? inst)
    {
        if (inst is null) return false;
        if (ReferenceEquals(this, inst)) return true;
        if (GetType() != inst.GetType()) return false;
        
        var instructionsEqual = Instructions.SequenceEqual(inst.Instructions);
        return instructionsEqual && Name == inst.Name && Parameters.Equals(inst.Parameters);
    }
    
    public override int GetHashCode() => HashCode.Combine(Instructions, Name, Parameters);
}
