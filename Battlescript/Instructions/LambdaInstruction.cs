namespace Battlescript;

public class LambdaInstruction : Instruction
{
    public List<Instruction> Parameters { get; set; }

    public LambdaInstruction(List<Token> tokens) : base(tokens)
    {
        var colonIndex = InstructionUtilities.GetTokenIndex(tokens, [":"]);

        var parametersTokens = tokens.GetRange(1, colonIndex - 1);
        var parameters = InstructionUtilities.ParseEntriesBetweenDelimiters(parametersTokens, [","]);

        var expressionTokens = tokens.GetRange(colonIndex + 1, tokens.Count - colonIndex - 1);
        var instruction = new ReturnInstruction(InstructionFactory.Create(expressionTokens));
        
        Parameters = parameters!;
        Instructions = [instruction];
    }

    public LambdaInstruction(List<Instruction>? parameters = null, List<Instruction>? instructions = null) : base([])
    {
        Parameters = parameters ?? [];
        Instructions = instructions ?? [];
    }

    public override Variable? Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        return new FunctionVariable(null, Parameters, Instructions);
    }
}
