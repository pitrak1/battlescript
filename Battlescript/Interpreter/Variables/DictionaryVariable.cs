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
    
    public override Variable? SetItemDirectly(Memory memory, Variable valueVariable, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        var indexVariable = index.Interpret(memory) as ListVariable;
        
        if (index.Next is null)
        {
            Values[indexVariable.Values[0]] = valueVariable;
            return valueVariable;
        }
        else
        {
            return Values[indexVariable.Values[0]];
        }
    }

    public override Variable? GetItemDirectly(Memory memory, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        var indexVariable = index.Values[0].Interpret(memory);
        return Values[indexVariable];
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