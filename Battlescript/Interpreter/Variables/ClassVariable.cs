using System.Diagnostics;

namespace Battlescript;

public class ClassVariable : Variable, IEquatable<ClassVariable>
{
    public string Name { get; set; }
    public Dictionary<string, Variable> Values { get; set; }
    public List<ClassVariable> SuperClasses { get; set; }
    
    public Closure ClassClosure { get; set; }

    public ClassVariable(string name, Dictionary<string, Variable>? values, Closure closure, List<ClassVariable>? superclasses = null)
    {
        Name = name;
        Values = values ?? [];
        ClassClosure = closure;
        SuperClasses = superclasses ?? [];
    }

    public override Variable? GetMemberDirectly(CallStack callStack, Closure closure, MemberInstruction member, ObjectVariable? objectContext = null)
    {
        var memberName = member.Value;
        if (Values.ContainsKey(memberName))
        {
            return Values[memberName];
        }
        else
        {
            foreach (var superclass in SuperClasses)
            {
                var result = superclass.GetMemberDirectly(callStack, closure, member, objectContext);
                if (result is not null)
                {
                    return result;
                }
            }
            return null;
        }
    }
    
    public override Variable? GetItemDirectly(CallStack callStack, Closure closure, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        var indexVariable = index.Values[0].Interpret(callStack, closure);

        var getItemFunction = GetMemberDirectly(callStack, closure, new MemberInstruction("__getitem__"));
        if (getItemFunction is FunctionVariable functionVariable && objectContext is not null)
        {
            return functionVariable.RunFunction(callStack, closure, new ArgumentSet([objectContext, indexVariable]));
        }
        else
        {
            throw new Exception("Must define __getitem__ to index a class");
        }
    }
    
    public ObjectVariable CreateObject()
    {
        var objectValues = new Dictionary<string, Variable>();
        AddToObjectValues(objectValues);
        return new ObjectVariable(objectValues, this);
    }

    public void AddToObjectValues(Dictionary<string, Variable> objectValues)
    {
        foreach (var kvp in Values)
        {
            if (kvp.Value is not FunctionVariable)
            {
                objectValues[kvp.Key] = kvp.Value;
            }
        }

        foreach (var superclass in SuperClasses)
        {
            superclass.AddToObjectValues(objectValues);
        }
    }
    
    public bool IsSubclass(ClassVariable classVariable)
    {
        if (Equals(classVariable))
        {
            return true;
        }
        else
        {
            foreach (var superclass in SuperClasses)
            {
                if (superclass.IsSubclass(classVariable))
                {
                    return true;
                }
            }
        }

        return false;
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as ClassVariable);
    public bool Equals(ClassVariable? variable)
    {
        if (variable is null) return false;
        if (ReferenceEquals(this, variable)) return true;
        if (GetType() != variable.GetType()) return false;
        if (Name != variable.Name) return false;
        
        var valuesEqual = Values.OrderBy(kvp => kvp.Key).SequenceEqual(variable.Values.OrderBy(kvp => kvp.Key));
        return SuperClasses.SequenceEqual(variable.SuperClasses) && valuesEqual;
    }
    
    public override int GetHashCode()
    {
        int hash = 17;
        foreach (var kvp in Values.OrderBy(kvp => kvp.Key))
        {
            hash = hash * 23 + kvp.Key.GetHashCode();
            hash = hash * 23 + kvp.Value.GetHashCode();
        }

        foreach (var superclass in SuperClasses)
        {
            hash = hash * 23 + superclass.GetHashCode();
        }
        
        return hash;
    }
}
