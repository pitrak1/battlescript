namespace Battlescript;

public class ArrayInstruction : Instruction, IEquatable<ArrayInstruction>
{
    public string Separator { get; set; }

    public ArrayInstruction(List<Token> tokens)
    {
        var commaIndex = ParserUtilities.GetTokenIndex(tokens, [","]);
        var colonIndex = ParserUtilities.GetTokenIndex(tokens, [":"]);
        
        // Commas will (usually) take higher priority than colons (i.e. {4: 5, 6: 7})
        if (commaIndex != -1)
        {
            var results = ParserUtilities.ParseEntriesBetweenSeparatingCharacters(tokens, [","]);
            Instructions = results.Values;
            Separator = ",";
            Line = tokens[commaIndex].Line;
            Column = tokens[commaIndex].Column;
        } else if (colonIndex != -1)
        {
            var results = ParserUtilities.ParseEntriesBetweenSeparatingCharacters(tokens, [":"]);
            Instructions = results.Values;
            Separator = ":";
            Line = tokens[colonIndex].Line;
            Column = tokens[colonIndex].Column;
        }
        else
        {
            throw new Exception("No separator tokens given to ArrayInstruction");
        }
    }

    public ArrayInstruction(string separator, List<Instruction> instructions)
    {
        Separator = separator;
        Instructions = instructions;
    }

    public override Variable Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var values = new List<Variable>();
        foreach (var instruction in Instructions)
        {
            values.Add(instruction.Interpret(memory, instructionContext, objectContext, lexicalContext));
        }
        return new ArrayVariable(values);
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as ArrayInstruction);
    public bool Equals(ArrayInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;
        
        if (!Separator.Equals(instruction.Separator)) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Separator, Instructions);
}