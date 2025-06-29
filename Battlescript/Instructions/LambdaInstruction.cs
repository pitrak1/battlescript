namespace Battlescript;

public class LambdaInstruction : Instruction, IEquatable<LambdaInstruction>
{
    public List<Instruction> Parameters { get; set; }

    public LambdaInstruction(List<Token> tokens)
    {
        var colonIndex = InstructionUtilities.GetTokenIndex(tokens, [":"]);

        var parametersTokens = tokens.GetRange(1, colonIndex - 1);
        var parameters = InstructionUtilities.ParseEntriesBetweenSeparatingCharacters(parametersTokens, [","]);

        var expressionTokens = tokens.GetRange(colonIndex + 1, tokens.Count - colonIndex - 1);
        var instruction = new ReturnInstruction(InstructionFactory.Create(expressionTokens));
        
        Parameters = parameters!;
        Instructions = [instruction];
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public LambdaInstruction(List<Instruction>? parameters = null, List<Instruction>? instructions = null)
    {
        Parameters = parameters ?? [];
        Instructions = instructions ?? [];
    }

    public override Variable Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        return new FunctionVariable(Parameters, Instructions);
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as LambdaInstruction);
    public bool Equals(LambdaInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;
        
        if (!Parameters.SequenceEqual(instruction.Parameters)) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Parameters, Instructions);
}
