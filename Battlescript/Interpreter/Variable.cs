namespace Battlescript;

public class Variable(Consts.VariableTypes type, dynamic? value)
{
    public Consts.VariableTypes Type { get; set; } = type;
    public dynamic? Value { get; set; } = value;

    // This method is useful for assignments because it will allow us to copy a value we've interpreted
    // from the instructions to a variable while also keeping that variable's place in memory.  If we
    // were just to set the variable to the other, we would lose the reference
    public Variable Copy(Variable copy)
    {
        Type = copy.Type;
        Value = copy.Value;
        return this;
    }

    public Variable GetIndex(dynamic key)
    {
        if (Type != Consts.VariableTypes.List &&
            Type != Consts.VariableTypes.Dictionary &&
            Type != Consts.VariableTypes.Set &&
            Type != Consts.VariableTypes.Tuple)
        {
            throw new Exception("wrong variable type bro");
        }
        
        // For lists, sets, and tuples
        if (Value is List<Variable>)
        {
            var index = (int)key % Value.Count;
            return Value[index];
        }
        // For dicitonaries
        else
        {
            // This is because in python (I think), getting a dictionary entry that doens't exist just returns null
            if (!Value.ContainsKey(key))
            {
                var variable = new Variable(Consts.VariableTypes.Null, null);
                Value[key] = variable;
                return variable;
            }
            
            return Value[key];
        }
    }
    
    public Variable GetRangeIndex(int? left, int? right)
    {
        if (Type != Consts.VariableTypes.List && Type != Consts.VariableTypes.Tuple)
        {
            throw new Exception("wrong variable type bro");
        }
        
        var index = left ?? 0;
        var count = right == null ? Value.Count - 1 : right - index;

        Console.WriteLine(index + ", " + count);
        
        return new Variable(Consts.VariableTypes.List, Value.GetRange(index, count));
    }
}