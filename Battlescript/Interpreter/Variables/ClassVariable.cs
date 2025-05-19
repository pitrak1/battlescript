using System.Diagnostics;

namespace Battlescript;

public class ClassVariable (Dictionary<string, Variable>? values, List<ClassVariable>? superclasses = null) : Variable, IEquatable<ClassVariable>
{
    public Dictionary<string, Variable> Values { get; set; } = values ?? [];
    public List<ClassVariable> SuperClasses { get; set; } = superclasses ?? [];
    
    public override void SetItem(Memory memory, Variable valueVariable, SquareBracketsInstruction index)
    {
        Debug.Assert(index.Values.Count == 1);

        var indexVariable = index.Values.First().Interpret(memory);

        if (indexVariable is StringVariable indexStringVariable)
        {
            if (index.Next is null)
            {
                Values[indexStringVariable.Value] = valueVariable;
            }
            else
            {
                Debug.Assert(index.Next is SquareBracketsInstruction);
                var nextInstruction = index.Next as SquareBracketsInstruction;
                Values[indexStringVariable.Value].SetItem(memory, valueVariable, nextInstruction!);
            }
        }
        else
        {
            throw new Exception("Can't index a class with anything but a string");
        }
    }
    
    public override Variable? GetItem(Memory memory, SquareBracketsInstruction index, ObjectVariable? objectContext = null)
    {
        Debug.Assert(index.Values.Count == 1);
        var indexVariable = index.Values.First().Interpret(memory);

        Variable? foundItem;
        if (indexVariable is StringVariable indexStringVariable && indexStringVariable.Value == "__getitem__")
        {
            foundItem = FindItemDirectly(memory, indexStringVariable.Value);
        } else if (GetItem(memory, "__getitem__") is FunctionVariable functionVariable)
        {
            foundItem = functionVariable.RunFunction(memory, [objectContext, indexVariable], objectContext);
        } else if (indexVariable is StringVariable stringVariable)
        {
            foundItem = FindItemDirectly(memory, stringVariable.Value);
        }
        else
        {
            throw new Exception("Can't index a class with anything but a string");
        }

        if (index.Next is SquareBracketsInstruction nextInstruction)
        {
            return foundItem.GetItem(memory, nextInstruction, objectContext);
        }
        else
        {
            return foundItem;
        }
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
                Variable? result;
                superclass.Values.TryGetValue(item, out result);
                if (result is not null)
                {
                    return result;
                }
            }
        }

        return null;
    }
    
    private Variable? RunOverride(
        Memory memory,
        Variable foundVariable, 
        FunctionVariable overrideFunction,
        Variable indexVariable)
    {
        if (foundVariable is ObjectVariable objectVariable)
        {
            return overrideFunction.RunFunction(memory, [foundVariable, indexVariable], objectVariable);
        }
        else
        {
            return overrideFunction.RunFunction(memory, [foundVariable, indexVariable]);
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

    public int AddClassToMemoryScopes(Memory memory)
    {
        var addedScopesCount = 0;
        
        // Reverse depth first search to add all required scopes to memory
        for (var i = SuperClasses.Count - 1; i >= 0; i--)
        {
            var scopesCount = SuperClasses[i].AddClassToMemoryScopes(memory);
            addedScopesCount += scopesCount;
        }
        
        memory.AddScope(Values);

        return addedScopesCount + 1;
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
    
    public override int GetHashCode() => HashCode.Combine(Values, SuperClasses);
}
