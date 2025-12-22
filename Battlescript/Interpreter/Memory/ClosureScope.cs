namespace Battlescript;

public class ClosureScope(Variable? owner = null)
{
    public Dictionary<string, Variable> Values { get; set; } = new();

    public List<string> Nonlocals { get; set; } = [];
    public List<string> Globals { get; set; } = [];
    public Variable? Owner { get; set; } = owner;

    public bool IsFunctionScope()
    {
        return Owner is FunctionVariable || Owner is null;
    }
}