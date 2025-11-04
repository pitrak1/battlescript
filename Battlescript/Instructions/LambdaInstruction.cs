namespace Battlescript;

public class LambdaInstruction : Instruction
{
    public ParameterSet Parameters { get; set; }

    public LambdaInstruction(List<Token> tokens) : base(tokens)
    {
        var colonIndex = InstructionUtilities.GetTokenIndex(tokens, [":"]);

        // Missing colon or missing expression or missing arguments
        if (colonIndex == -1 || colonIndex == tokens.Count - 1 || colonIndex == 1)
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "invalid syntax");
        }

        var parametersTokens = tokens.GetRange(1, colonIndex - 1);
        var parameters = InstructionUtilities.ParseEntriesBetweenDelimiters(parametersTokens, [","]);

        var expressionTokens = tokens.GetRange(colonIndex + 1, tokens.Count - colonIndex - 1);
        var instruction = new ReturnInstruction(InstructionFactory.Create(expressionTokens));
        
        Parameters = new ParameterSet(parameters!);
        Instructions = [instruction];
    }

    public LambdaInstruction(ParameterSet? parameters = null, List<Instruction>? instructions = null) : base([])
    {
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
        return new FunctionVariable(null, Parameters, Instructions);
    }
}
