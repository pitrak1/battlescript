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
        Type = Consts.VariableTypes.Reference;
    }

    public override Variable? SetItemDirectly(Memory memory, Variable valueVariable, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        var indexVariable = index.Values[0].Interpret(memory);

        var setItemOverride = GetItem(memory, "__setitem__");
        if (setItemOverride is FunctionVariable functionVariable)
        {
            return functionVariable.RunFunction(memory, [this, indexVariable, valueVariable], this);
        } else if (indexVariable is StringVariable stringVariable)
        {
            if (index.Next is null)
            {
                Values[stringVariable.Value] = valueVariable;
                return valueVariable;
            }
            else
            {
                return Values[stringVariable.Value];
            }
        }
        else
        {
            throw new Exception("Can't index a class with anything but a string");
        }
    }
    
    public override Variable? GetItemDirectly(Memory memory, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        var indexVariable = index.Values[0].Interpret(memory);

        var getItemOverride = Class.GetItem(memory, "__getitem__");
        Variable? foundItem;
        if (getItemOverride is FunctionVariable functionVariable)
        {
            foundItem = functionVariable.RunFunction(memory, [indexVariable], this);
        }
        else if (indexVariable is StringVariable stringVariable)
        {
            if (Values.ContainsKey(stringVariable.Value))
            { 
                foundItem = Values[stringVariable.Value];
            }
            else
            {
                foundItem = Class.GetItem(memory, new ArrayInstruction([new StringInstruction(stringVariable.Value)]), this);
            }
        }
        else
        {
            throw new Exception("Need to index an object with a string or override []");
        }
        
        return foundItem;
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
    
    public override int GetHashCode() => HashCode.Combine(Values, Class);
}
