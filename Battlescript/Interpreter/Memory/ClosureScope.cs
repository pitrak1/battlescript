namespace Battlescript;

public class ClosureScope(Closure.ClosureTypes type = Closure.ClosureTypes.Function)
{
    public Dictionary<string, Variable> Values { get; set; } = new();
    public Closure.ClosureTypes Type { get; set; } = type;
}