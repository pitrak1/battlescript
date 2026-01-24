namespace Battlescript;

public class FunctionInstruction : Instruction, IBlockInstruction, IEquatable<FunctionInstruction>
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
            throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "invalid syntax");
        }

        if (tokens[^2].Value is not ")" || tokens[2].Value is not "(")
        {
            throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "invalid syntax");
        }
        
        if (tokens[1].Type != Consts.TokenTypes.Identifier)
        {
            throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "invalid syntax");
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
    
    #region Equality

    public override bool Equals(object? obj) => obj is FunctionInstruction inst && Equals(inst);

    public bool Equals(FunctionInstruction? other) =>
        other is not null &&
        Instructions.SequenceEqual(other.Instructions) &&
        Name == other.Name &&
        Equals(Parameters, other.Parameters);

    public override int GetHashCode() => HashCode.Combine(Instructions, Name, Parameters);

    public static bool operator ==(FunctionInstruction? left, FunctionInstruction? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(FunctionInstruction? left, FunctionInstruction? right) => !(left == right);

    #endregion
}
