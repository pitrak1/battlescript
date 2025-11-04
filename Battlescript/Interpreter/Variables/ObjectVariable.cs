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

    public override Variable? SetItemDirectly(CallStack callStack, Variable valueVariable, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        var indexVariable = index.Values.Select(x => x?.Interpret(callStack) ?? null).ToList();

        var setItemOverride = Class.GetMember(callStack, new MemberInstruction("__setitem__"));
        if (setItemOverride is FunctionVariable functionVariable)
        {
            var indexArgument = BsTypes.Create(BsTypes.Types.List, indexVariable);
            return functionVariable.RunFunction(callStack, new ArgumentSet([this, indexArgument, valueVariable]), index);
        }
        else
        {
            throw new Exception("Must define __setitem__ to index an object");
        }
    }

    public override Variable? SetMemberDirectly(
        CallStack callStack, 
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
            return GetMemberDirectly(callStack, member, objectContext);
        }
    }
    
    public override Variable? GetItemDirectly(CallStack callStack, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        var indexVariables = index.Values.Select(x => x?.Interpret(callStack) ?? null).ToList();
        var indexList = BsTypes.Create(BsTypes.Types.List, indexVariables);

        var getItemOverride = Class.GetMember(callStack, new MemberInstruction("__getitem__"));
        if (getItemOverride is FunctionVariable functionVariable)
        {
            return functionVariable.RunFunction(callStack, new ArgumentSet([this, indexList]), index);
        }
        else
        {
            throw new Exception("Must define __getitem__ to index an object");
        }
    }

    public override Variable? GetMemberDirectly(
        CallStack callStack, 
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
            return Class.GetMember(callStack, new MemberInstruction(memberName), this);
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
