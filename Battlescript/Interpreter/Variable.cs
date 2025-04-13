namespace Battlescript;

// I would love to split this up into different classes for each type, but there's the following problem:]
// In our variable assignments, we ideally would like to have the left hand side return the memory address
// of where the right side should be stored.  I think this should be possible through polymorphism, but
// C# has all other sorts of concerns because of the garbage collector and heap management.  It uses the
// class GCHandle, which I believe makes you create the reference when allocating, specifically setting
// the memory space as fixed/pinned.  It's possible I'll look into this again later *shrug*
public class Variable(
    Consts.VariableTypes type, 
    dynamic? value, 
    List<Instruction>? instructions = null, 
    List<Variable>? classVariable = null)
{
    public Consts.VariableTypes Type { get; set; } = type;
    public dynamic? Value { get; set; } = value;
    public List<Instruction> Instructions { get; set; } = instructions ?? [];
    public List<Variable>? ClassVariable { get; set; } = classVariable;

    // This method is useful for assignments because it will allow us to copy a value we've interpreted
    // from the instructions to a variable while also keeping that variable's place in memory.  If we
    // were just to set the variable to the other, we would lose the reference
    public Variable Copy(Variable copy)
    {
        Type = copy.Type;
        Value = copy.Value;
        Instructions = copy.Instructions;
        ClassVariable = copy.ClassVariable;
        return this;
    }

    public Variable GetIndex(dynamic key)
    {
        if (Type != Consts.VariableTypes.List &&
            Type != Consts.VariableTypes.Dictionary &&
            Type != Consts.VariableTypes.Set &&
            Type != Consts.VariableTypes.Tuple &&
            Type != Consts.VariableTypes.Class &&
            Type != Consts.VariableTypes.Object)
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
        
        return new Variable(Consts.VariableTypes.List, Value.GetRange(index, count));
    }
}