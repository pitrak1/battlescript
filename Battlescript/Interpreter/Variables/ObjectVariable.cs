using System.Diagnostics;

namespace Battlescript;

public class ObjectVariable : Variable, IEquatable<ObjectVariable>
{
    public Dictionary<string, Variable> Values { get; set; }
    public ClassVariable Class { get; set; }

    public ObjectVariable(Dictionary<string, Variable>? values, ClassVariable classVariable)
    {
        Values = values ?? [];
        Class = classVariable;
    }

    public override Variable? SetItemDirectly(Memory memory, Variable valueVariable, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        var indexVariable = index.Values[0].Interpret(memory);

        var setItemOverride = Class.GetMember(memory, new MemberInstruction("__setitem__"));
        if (setItemOverride is FunctionVariable functionVariable)
        {
            return functionVariable.RunFunction(memory, [indexVariable, valueVariable], this);
        }
        else
        {
            throw new Exception("Must define __setitem__ to index an object");
        }
    }

    public override Variable? SetMemberDirectly(
        Memory memory, 
        Variable valueVariable, 
        MemberInstruction member, 
        ObjectVariable? objectContext = null)
    {
        var memberName = member.Value;
        
        if (member.Next is null)
        {
            Values[memberName] = valueVariable;
            return valueVariable;
        }
        else
        {
            return Values[memberName];
        }
    }
    
    public override Variable? GetItemDirectly(Memory memory, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        var indexVariable = index.Values[0].Interpret(memory);

        var getItemOverride = Class.GetMember(memory, new MemberInstruction("__getitem__"));
        if (getItemOverride is FunctionVariable functionVariable)
        {
            return functionVariable.RunFunction(memory, [indexVariable], this);
        }
        else
        {
            throw new Exception("Must define __getitem__ to index an object");
        }
    }

    public override Variable? GetMemberDirectly(
        Memory memory, 
        MemberInstruction member,
        ObjectVariable? objectContext = null)
    {
        var memberName = member.Value;
        if (Values.ContainsKey(memberName))
        { 
            return Values[memberName];
        }
        else
        {
            return Class.GetMember(memory, new MemberInstruction(memberName), this);
        }
    }

    public bool IsInstance(ClassVariable classVariable)
    {
        if (Class.Equals(classVariable))
        {
            return true;
        }
        else
        {
            return Class.IsSubclass(classVariable);
        }
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as ObjectVariable);
    public bool Equals(ObjectVariable? variable)
    {
        if (variable is null) return false;
        if (ReferenceEquals(this, variable)) return true;
        if (GetType() != variable.GetType()) return false;
        
        var valuesEqual = Values.OrderBy(kvp => kvp.Key).SequenceEqual(variable.Values.OrderBy(kvp => kvp.Key));
        return valuesEqual && Class.Equals(variable.Class);
    }
    
    public override int GetHashCode()
    {
        int hash = 17;
        foreach (var kvp in Values.OrderBy(kvp => kvp.Key))
        {
            hash = hash * 23 + kvp.Key.GetHashCode();
            hash = hash * 23 + kvp.Value.GetHashCode();
        }
        
        hash = hash * 23 + Class.GetHashCode();
        return hash;
    }
}
