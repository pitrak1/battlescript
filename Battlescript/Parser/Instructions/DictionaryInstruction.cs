using System.Diagnostics;

namespace Battlescript;

public class DictionaryInstruction : Instruction
{
    public List<Instruction> Values { get; set; }

    public DictionaryInstruction(List<Token> tokens)
    {
        var results = ParseAndRunEntriesWithinSeparator(tokens, [","]);
        
        // There should be no characters following a dictionary definition
        CheckForNoFollowingTokens(tokens, results.Count);

        Values = results.Values;
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public DictionaryInstruction(List<Instruction> values)
    {
        Values = values;
    }

    public override Variable Interpret(Memory memory, Variable? context = null)
    {
        var variableValue = new Dictionary<string, Variable>();
        foreach (var kvp in Values)
        {
            Debug.Assert(kvp is KeyValuePairInstruction);
            var kvpInstruction = (KeyValuePairInstruction)kvp;
            var key = kvpInstruction.Left.Interpret(memory);
            var value = kvpInstruction.Right.Interpret(memory);
            variableValue[key.Value] = value;
        }
        return new Variable(Consts.VariableTypes.Dictionary, variableValue);
    }
}