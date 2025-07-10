using System.Diagnostics;

namespace Battlescript;

public class ClassVariable : Variable, IEquatable<ClassVariable>
{
    public Dictionary<string, Variable> Values { get; set; }
    public List<ClassVariable> SuperClasses { get; set; }

    public ClassVariable(Dictionary<string, Variable>? values, List<ClassVariable>? superclasses = null)
    {
        Values = values ?? [];
        SuperClasses = superclasses ?? [];
        Type = Consts.VariableTypes.Reference;
    }
    
    public override Variable? GetItemDirectly(Memory memory, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        var indexVariable = index.Values[0].Interpret(memory);

        Variable? foundItem;
        if (indexVariable is StringVariable indexStringVariable && indexStringVariable.Value == "__getitem__")
        {
            foundItem = FindItemDirectly(memory, "__getitem__");
        } else if (GetItem(memory, "__getitem__") is FunctionVariable functionVariable && objectContext is not null)
        {
            foundItem = functionVariable.RunFunction(memory, [indexVariable], objectContext);
        } else if (indexVariable is StringVariable stringVariable)
        {
            foundItem = FindItemDirectly(memory, stringVariable.Value);
        }
        else
        {
            throw new Exception("Can't index a class with anything but a string");
        }
        
        return foundItem;
    }

    private Variable? FindItemDirectly(Memory memory, string item)
    {
        if (Values.ContainsKey(item))
        {
            return Values[item];
        }
        else
        {
            foreach (var superclass in SuperClasses)
            {
                var result = superclass.GetItem(memory, item);
                if (result is not null)
                {
                    return result;
                }
            }
        }

        return null;
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
