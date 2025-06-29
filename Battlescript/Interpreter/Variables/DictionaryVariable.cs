using System.Diagnostics;

namespace Battlescript;

public class DictionaryVariable : Variable, IEquatable<DictionaryVariable>
{ 
    public Dictionary<Variable, Variable> Values { get; set; }

    public DictionaryVariable(Dictionary<Variable, Variable>? values)
    {
        Values = values ?? [];
        Type = Consts.VariableTypes.Reference;
    }
    
    public override bool SetItem(Memory memory, Variable valueVariable, SquareBracketsInstruction index, ObjectVariable? objectContext = null)
    {
        var indexVariable = index.Values[0].Interpret(memory);
        
        if (index.Next is null)
        {
            Values[indexVariable] = valueVariable;
            return true;
        }
        else
        {
            return Values[indexVariable].SetItem(memory, valueVariable, (SquareBracketsInstruction)index.Next);
        }
    }

    public override Variable? GetItem(Memory memory, SquareBracketsInstruction index, ObjectVariable? objectContext = null)
    {
        var indexVariable = index.Values[0].Interpret(memory);
        
        if (index.Next is null)
        {
            return Values[indexVariable];
        }
        else
        {
            return Values[indexVariable].GetItem(memory, (SquareBracketsInstruction)index.Next);
        }
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