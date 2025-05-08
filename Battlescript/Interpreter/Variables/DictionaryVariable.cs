using System.Diagnostics;

namespace Battlescript;

public class DictionaryVariable(List<KeyValuePairVariable>? values): Variable
{ 
    public List<KeyValuePairVariable> Values { get; set; } = values ?? [];
    
    public override void AssignToIndexOrKey(Memory memory, Variable valueVariable, SquareBracketsInstruction index)
    {
        Debug.Assert(index.Values.Count == 1);

        var indexVariable = index.Values.First().Interpret(memory);
        var value = GetKvpForVariableKey(indexVariable);
        
        if (index.Next is null)
        {
            value.Right = valueVariable;
        }
        else
        {
            value.Right.AssignToIndexOrKey(memory, valueVariable, (SquareBracketsInstruction)index.Next);
        }
    }

    public override Variable GetIndex(Memory memory, SquareBracketsInstruction index)
    {
        Debug.Assert(index.Values.Count == 1);

        var indexVariable = index.Values.First().Interpret(memory);
        var value = GetKvpForVariableKey(indexVariable);
        
        if (index.Next is null)
        {
            return value.Right;
        }
        else
        {
            return value.Right.GetIndex(memory, (SquareBracketsInstruction)index.Next);
        }
    }

    private KeyValuePairVariable? GetKvpForVariableKey(Variable key)
    {
        foreach (var pair in Values)
        {
            if (pair.Left is NumberVariable leftNumberVariable && key is NumberVariable numberVariable)
            {
                if ((int)leftNumberVariable.Value == (int)numberVariable.Value)
                {
                    return pair;
                }
            } else if (pair.Left is StringVariable leftStringVariable && key is StringVariable stringVariable)
            {
                if (leftStringVariable.Value == stringVariable.Value)
                {
                    return pair;
                }
            }
        }

        return null;
    }
    
}