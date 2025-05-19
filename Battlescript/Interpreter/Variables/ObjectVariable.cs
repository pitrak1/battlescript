using System.Diagnostics;

namespace Battlescript;

public class ObjectVariable (Dictionary<string, Variable>? values, ClassVariable classVariable) : Variable, IEquatable<ObjectVariable>
{
    public Dictionary<string, Variable> Values { get; set; } = values ?? [];
    public ClassVariable ClassVariable { get; set; } = classVariable;

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
            throw new Exception("Can't index an object with anything but a string");
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
            foundItem = functionVariable.RunFunction(memory, [this, indexVariable], this);
        } else if (indexVariable is StringVariable stringVariable)
        {
            foundItem = FindItemDirectly(memory, stringVariable.Value);
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

    private Variable? FindItemDirectly(Memory memory, string item)
    {
        if (Values.ContainsKey(item))
        { 
            return Values[item];
        }
        else
        {
            return ClassVariable.GetItem(memory, new SquareBracketsInstruction([new StringInstruction(item)]), this);
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
