namespace Battlescript;

public class ClosureScope(Closure.ClosureTypes type = Closure.ClosureTypes.Function)
{
    public Dictionary<string, Variable> Values { get; set; } = new();

    public List<string> Nonlocals { get; set; } = [];
    public List<string> Globals { get; set; } = [];
    public Closure.ClosureTypes Type { get; set; } = type;
}