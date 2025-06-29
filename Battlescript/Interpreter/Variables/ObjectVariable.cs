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

    public override bool SetItem(Memory memory, Variable valueVariable, SquareBracketsInstruction index, ObjectVariable? objectContext = null)
    {
        var indexVariable = index.Values[0].Interpret(memory);

        var setItemOverride = GetItem(memory, "__setitem__");
        if (setItemOverride is FunctionVariable functionVariable)
        {
            functionVariable.RunFunction(memory, [this, indexVariable, valueVariable], this);
            return true;
        } else if (indexVariable is StringVariable stringVariable)
        {
            if (Values.ContainsKey(stringVariable.Value))
            {
                if (index.Next is SquareBracketsInstruction nextInstruction)
                {
                    return Values[stringVariable.Value].SetItem(memory, valueVariable, nextInstruction, objectContext);
                }
                else
                {
                    Values[stringVariable.Value] = valueVariable;
                    return true;
                }
            }
            else
            {
                Values.Add(stringVariable.Value, valueVariable);
                return true;
            }
        }
        else
        {
            throw new Exception("Can't index a class with anything but a string");
        }
    }
    
    public override Variable? GetItem(Memory memory, SquareBracketsInstruction index, ObjectVariable? objectContext = null)
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
                foundItem = Class.GetItem(memory, new SquareBracketsInstruction(new StringInstruction(stringVariable.Value)), this);
            }
        }
        else
        {
            throw new Exception("Need to index an object with a string or override []");
        }
        
        if (index.Next is SquareBracketsInstruction nextInstruction)
        {
            return foundItem.GetItem(memory, nextInstruction);
        }
        else
        {
            return foundItem;
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
