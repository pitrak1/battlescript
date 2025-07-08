namespace Battlescript;

public class PrincipleTypeInstruction : Instruction, IEquatable<PrincipleTypeInstruction>
{
    public string Value { get; set; }
    public List<Instruction> Parameters { get; set; }

    public PrincipleTypeInstruction(List<Token> tokens)
    {
        var endOfArgumentsIndex = InstructionUtilities.GetTokenIndex(tokens, [")"]);
        var argumentTokens = tokens.GetRange(2, endOfArgumentsIndex - 2);
        Parameters = InstructionUtilities.ParseEntriesBetweenDelimiters(argumentTokens, [","])!;
        
        if (tokens.Count > endOfArgumentsIndex + 1)
        {
            throw new ParserUnexpectedTokenException(tokens[endOfArgumentsIndex + 1]);
        }

        Value = tokens[0].Value;
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public PrincipleTypeInstruction(string value)
    {
        Value = value;
    }
    
    public override Variable Interpret(        
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        switch (Value)
        {
            case "__numeric__":
                if (Parameters.Count == 1 && Parameters[0] is NumericInstruction numInstruction)
                {
                    return new NumericVariable(numInstruction.Value);
                }
                return new NumericVariable(0);
        }
        return new ConstantVariable();
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as PrincipleTypeInstruction);
    public bool Equals(PrincipleTypeInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;

        if (Value != instruction.Value) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Value, Instructions);
}