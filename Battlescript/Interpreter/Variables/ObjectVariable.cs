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

    public override Variable? SetItemDirectly(CallStack callStack, Closure closure, Variable valueVariable, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        var indexVariable = index.Values.Select(x => x?.Interpret(callStack, closure) ?? null).ToList();

        var setItemOverride = GetMember(callStack, closure, new MemberInstruction("__setitem__"));
        if (setItemOverride is FunctionVariable functionVariable)
        {
            var indexArgument = BtlTypes.Create(BtlTypes.Types.List, indexVariable);
            return functionVariable.RunFunction(callStack, closure, new ArgumentSet([indexArgument, valueVariable]), index);
        }
        else
        {
            throw new Exception("Must define __setitem__ to index an object");
        }
    }

    public override Variable? SetMemberDirectly(
        CallStack callStack,
        Closure closure,
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
            return GetMemberDirectly(callStack, closure, member, objectContext);
        }
    }
    
    public override Variable? GetItemDirectly(CallStack callStack, Closure closure, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        var indexVariables = index.Values.Select(x => x?.Interpret(callStack, closure) ?? null).ToList();
        var indexList = BtlTypes.Create(BtlTypes.Types.List, indexVariables);

        var getItemOverride = GetMember(callStack, closure, new MemberInstruction("__getitem__"));
        if (getItemOverride is FunctionVariable functionVariable)
        {
            return functionVariable.RunFunction(callStack, closure, new ArgumentSet([indexList]), index);
        }
        else
        {
            throw new Exception("Must define __getitem__ to index an object");
        }
    }

    public override Variable? GetMemberDirectly(
        CallStack callStack,
        Closure closure,
        MemberInstruction member,
        ObjectVariable? objectContext = null)
    {
        var memberName = member.Value;
        if (Values.ContainsKey(memberName))
        {
            if (Values[memberName] is FunctionVariable functionVariable)
            {
                return new ObjectMethodPair(this, functionVariable);
            }
            else
            {
                return Values[memberName];
            }
            
        }
        else
        {
            return Class.GetMember(callStack, closure, new MemberInstruction(memberName), this);
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

    public void RunConstructor(CallStack callStack, Closure closure, ArgumentSet arguments, Instruction inst)
    {
        var constructor = GetMember(callStack, closure, new MemberInstruction("__init__"));
        if (constructor is FunctionVariable functionVariable)
        {
            functionVariable.RunFunction(callStack, closure, arguments, inst);
        }
    }
    
    #region Equality

    public override bool Equals(object? obj) => obj is ObjectVariable variable && Equals(variable);

    public bool Equals(ObjectVariable? other) =>
        other is not null &&
        Values.OrderBy(kvp => kvp.Key).SequenceEqual(other.Values.OrderBy(kvp => kvp.Key)) &&
        Equals(Class, other.Class);

    public override bool Equals(Variable? other) => other is ObjectVariable variable && Equals(variable);

    public override int GetHashCode() => HashCode.Combine(Values, Class);

    public static bool operator ==(ObjectVariable? left, ObjectVariable? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(ObjectVariable? left, ObjectVariable? right) => !(left == right);

    #endregion
}
