using System.Diagnostics;

namespace Battlescript;

public class DictionaryInstruction : Instruction, IEquatable<DictionaryInstruction>
{
    public List<KeyValuePairInstruction> Values { get; set; }

    public DictionaryInstruction(List<Token> tokens)
    {
        var results = ParserUtilities.ParseEntriesWithinSeparator(tokens, [","]);
        
        // There should be no characters following a dictionary definition
        if (tokens.Count > results.Count)
        {
            throw new ParserUnexpectedTokenException(tokens[results.Count]);
        }
        
        Debug.Assert(results.Values.All(result => result is KeyValuePairInstruction));
        
        var kvpResults = results.Values.Cast<KeyValuePairInstruction>().ToList();

        Values = kvpResults;
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public DictionaryInstruction(List<KeyValuePairInstruction> values)
    {
        Values = values;
    }

    public override Variable Interpret(
        Memory memory, 
        Variable? context = null, 
        Variable? objectContext = null)
    {
        var variableValue = new List<KeyValuePairVariable>();
        foreach (var kvp in Values)
        {
            var key = kvp.Left.Interpret(memory);
            var value = kvp.Right.Interpret(memory);
            
            variableValue.Add(new KeyValuePairVariable(key, value));
        }
        return new DictionaryVariable(variableValue);
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as DictionaryInstruction);
    public bool Equals(DictionaryInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;
        
        if (!Values.SequenceEqual(instruction.Values)) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Values, Instructions);
}