using System.Diagnostics;

namespace Battlescript;

public class ObjectVariable (Dictionary<string, Variable>? values, ClassVariable classVariable) : 
    Variable, IEquatable<ObjectVariable>
{
    public Dictionary<string, Variable> Values { get; set; } = values ?? [];
    public ClassVariable ClassVariable { get; set; } = classVariable;

    public override bool SetItem(Memory memory, Variable valueVariable, SquareBracketsInstruction index, ObjectVariable? objectContext = null)
    {
        Debug.Assert(index.Values.Count == 1);
        var indexVariable = index.Values.First().Interpret(memory);

        Variable? foundItem;
        if (GetItem(memory, "__setitem__") is FunctionVariable functionVariable)
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
        Debug.Assert(index.Values.Count == 1);
        var indexVariable = index.Values.First().Interpret(memory);

        var getItemOverride = ClassVariable.GetItem(memory, new SquareBracketsInstruction([new StringInstruction("__getitem__")]), this);
        Variable? foundItem;
        if (getItemOverride is FunctionVariable functionVariable)
        {
            foundItem = functionVariable.RunFunction(memory, [this, indexVariable], this);
        }
        else if (indexVariable is StringVariable stringVariable)
        {
            if (Values.ContainsKey(stringVariable.Value))
            { 
                return Values[stringVariable.Value];
            }
            else
            {
                return ClassVariable.GetItem(memory, new SquareBracketsInstruction([new StringInstruction(stringVariable.Value)]), this);
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
        return valuesEqual && ClassVariable.Equals(variable.ClassVariable);
    }
    
    public override int GetHashCode() => HashCode.Combine(Values, ClassVariable);
}
