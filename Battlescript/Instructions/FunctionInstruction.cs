namespace Battlescript;

public class FunctionInstruction : Instruction
{
    public string Name { get; set; } 
    public ParameterSet Parameters { get; set; }

    public FunctionInstruction(List<Token> tokens) : base(tokens)
    {
        if (tokens[^1].Value is not ":")
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "invalid syntax");
        }

        if (tokens[^2].Value is not ")" || tokens[2].Value is not "(")
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "invalid syntax");
        }
        
        var tokensInParens = tokens.GetRange(3, tokens.Count - 5);
        var parameters = InstructionUtilities.ParseEntriesBetweenDelimiters(tokensInParens, [","]);

        if (tokens[1].Type != Consts.TokenTypes.Identifier)
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "invalid syntax");
        }
        
        Name = tokens[1].Value;
        Parameters = new ParameterSet(parameters!);
    }

    public FunctionInstruction(string name, ParameterSet? parameters = null, List<Instruction>? instructions = null) : base([])
    {
        Name = name;
        Parameters = parameters ?? new ParameterSet();
        Instructions = instructions ?? [];
    }

    public override Variable? Interpret(
        CallStack callStack,
        Closure closure,
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var functionValue = new FunctionVariable(Name, Parameters, Instructions);
        callStack.SetVariable(closure, new VariableInstruction(Name), functionValue);
        return functionValue;
    }
}
