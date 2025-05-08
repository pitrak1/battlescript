using System.Diagnostics;

namespace Battlescript;

public class DictionaryInstruction : Instruction
{
    public List<KeyValuePairInstruction> Values { get; set; }

    public DictionaryInstruction(List<Token> tokens)
    {
        var results = ParseAndRunEntriesWithinSeparator(tokens, [","]);
        
        // There should be no characters following a dictionary definition
        CheckForNoFollowingTokens(tokens, results.Count);
        
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

    public override Variable Interpret(Memory memory, Variable? context = null)
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
}