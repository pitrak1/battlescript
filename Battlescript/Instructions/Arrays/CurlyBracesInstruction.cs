namespace Battlescript;

public class CurlyBracesInstruction : GenericArrayInstruction<ColonSeparatedArrayInstruction>
{
    public CurlyBracesInstruction(List<Token> tokens)
    {
        var closeCurlyBracesIndex = InstructionUtilities.GetTokenIndex(tokens, ["}"]);

        var range = tokens.GetRange(1, closeCurlyBracesIndex - 1);
        PopulateValues(tokens.GetRange(1, closeCurlyBracesIndex - 1), ",");
        
        if (tokens.Count > closeCurlyBracesIndex + 1)
        {
            throw new Exception("No tokens hsoudl come after a dicitonary definition");
        }
    }

    public CurlyBracesInstruction(List<ColonSeparatedArrayInstruction> values)
    {
        PopulateValues(values);
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
            if (kvp.Values.Count != 2) throw new Exception("Badly formed ditionary");
            
            var key = kvp.Values[0].Interpret(memory);
            var value = kvp.Values[1].Interpret(memory);
            
            variableValue.Add(key, value);
        }
        return new DictionaryVariable(variableValue);
    }
}