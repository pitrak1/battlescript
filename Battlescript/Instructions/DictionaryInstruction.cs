using System.Diagnostics;

namespace Battlescript;

public class DictionaryInstruction : Instruction, IEquatable<DictionaryInstruction>
{
    public Dictionary<Instruction, Instruction> Values { get; set; }

    public DictionaryInstruction(List<Token> tokens)
    {
        var results = ParserUtilities.ParseEntriesWithinSeparator(tokens, [","]);
        
        // There should be no characters following a dictionary definition
        if (tokens.Count > results.Count)
        {
            throw new ParserUnexpectedTokenException(tokens[results.Count]);
        }
        
        Debug.Assert(results.Values.All(result => result is ArrayInstruction arr && arr.Separator == ":"));

        Values = new Dictionary<Instruction, Instruction>();
        foreach (var kvp in results.Values)
        {
            if (
                kvp is ArrayInstruction arrayInstruction && 
                arrayInstruction.Values.Count == 2 &&
                arrayInstruction.Separator == ":")
            {
                Values.Add(arrayInstruction.Values[0], arrayInstruction.Values[1]);
            }
            else
            {
                throw new Exception("Badly formed dictionary, fix this later");
            }
        }
        
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public DictionaryInstruction(Dictionary<Instruction, Instruction> values)
    {
        Values = values;
    }

    public override Variable Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var variableValue = new Dictionary<Variable, Variable>();
        foreach (var kvp in Values)
        {
            var key = kvp.Key.Interpret(memory);
            var value = kvp.Value.Interpret(memory);
            
            variableValue.Add(key, value);
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