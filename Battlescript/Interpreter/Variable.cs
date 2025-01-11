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
}