using System.Diagnostics;

namespace Battlescript;

public class DictionaryVariable(List<KeyValuePairVariable>? values): Variable, IEquatable<DictionaryVariable>
{ 
    public List<KeyValuePairVariable> Values { get; set; } = values ?? [];
    
    public override void SetItem(Memory memory, Variable valueVariable, SquareBracketsInstruction index)
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
            value.Right.SetItem(memory, valueVariable, (SquareBracketsInstruction)index.Next);
        }
    }

    public override Variable? GetItem(Memory memory, SquareBracketsInstruction index, ObjectVariable? objectContext = null)
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
            return value.Right.GetItem(memory, (SquareBracketsInstruction)index.Next);
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
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as DictionaryVariable);
    public bool Equals(DictionaryVariable? variable)
    {
        if (variable is null) return false;
        if (ReferenceEquals(this, variable)) return true;
        if (GetType() != variable.GetType()) return false;
        
        return Values.SequenceEqual(variable.Values);
    }
    
    public override int GetHashCode() => HashCode.Combine(Values);
}