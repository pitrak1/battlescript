namespace Battlescript;

public class BuiltInInstruction : Instruction
{
    public string Name { get; set; } 
    public List<Instruction>? Parameters { get; set; }
    
    public Instruction? Next { get; set; }
    
    public BuiltInInstruction(List<Token> tokens)
    {
        var tokensAfterBuiltinName = tokens.GetRange(1, tokens.Count - 1);
        var results = ParserUtilities.ParseEntriesWithinSeparator(tokensAfterBuiltinName, [","]);
        Next = tokensAfterBuiltinName.Count > results.Count  ? Parse(tokens.Slice(results.Count + 1, tokens.Count - results.Count - 1)) : null;
        Name = tokens[0].Value;
        Parameters = results.Values;
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }
    
    public BuiltInInstruction(string name, List<Instruction>? parameters = null, Instruction? next = null)
    {
        Name = name;
        Parameters = parameters ?? [];
        Next = next;
    }
    
    public override Variable Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        switch (Name)
        {
            case "super":
                break;
                
        }
        // TODO
        return new NullVariable();
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as BuiltInInstruction);
    public bool Equals(BuiltInInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;
        
        if (!Parameters.SequenceEqual(instruction.Parameters) || Name != instruction.Name || !Next.Equals(instruction.Next)) return false;
        
        return true;
    }
    
    public override int GetHashCode() => HashCode.Combine(Parameters, Name);
}