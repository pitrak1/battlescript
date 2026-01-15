namespace Battlescript;

public class LambdaInstruction : Instruction
{
    public ParameterSet Parameters { get; set; }

    public LambdaInstruction(List<Token> tokens) : base(tokens)
    {
        CheckTokenValidity(tokens);
        
        var colonIndex = InstructionUtilities.GetTokenIndex(tokens, [":"]);
        var parametersTokens = tokens.GetRange(1, colonIndex - 1);
        var parameters = InstructionUtilities.ParseEntriesBetweenDelimiters(parametersTokens, [","]);

        var expressionTokens = tokens.GetRange(colonIndex + 1, tokens.Count - colonIndex - 1);
        var instruction = new ReturnInstruction(InstructionFactory.Create(expressionTokens));
        
        Parameters = new ParameterSet(parameters!);
        Instructions = [instruction];
    }

    private void CheckTokenValidity(List<Token> tokens)
    {
        var colonIndex = InstructionUtilities.GetTokenIndex(tokens, [":"]);

        // Missing colon or missing expression
        if (colonIndex == -1 || colonIndex == tokens.Count - 1)
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "invalid syntax");
        }
    }

    public LambdaInstruction(
        ParameterSet? parameters = null, 
        List<Instruction>? instructions = null, 
        int? line = null, 
        string? expression = null) : base(line, expression)
    {
        Parameters = parameters ?? new ParameterSet();
        Instructions = instructions ?? [];
    }

    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        return new FunctionVariable(null, closure, Parameters, Instructions);
    }
    
    // All the code below is to override equality
    public override bool Equals(object? obj) => Equals(obj as LambdaInstruction);
    public bool Equals(LambdaInstruction? inst)
    {
        if (inst is null) return false;
        if (ReferenceEquals(this, inst)) return true;
        if (GetType() != inst.GetType()) return false;
        
        var instructionsEqual = Instructions.SequenceEqual(inst.Instructions);
        return instructionsEqual && Parameters.Equals(inst.Parameters);
    }
    
    public override int GetHashCode()
    {
        int hash = 25;

        for (int i = 0; i < Instructions.Count; i++)
        {
            hash += Instructions[i].GetHashCode() * 20 * (i + 1);
        }

        hash += Parameters.GetHashCode() * 99;
        return hash;
    }
}
