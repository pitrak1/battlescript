namespace Battlescript;

public class Memory
{
    private List<Dictionary<string, Variable>> scopes = [];

    public Memory()
    {
        scopes.Add(new Dictionary<string, Variable>());
    }

    public Variable? Get(string name)
    {
        for (var i = scopes.Count - 1; i >= 0; i--)
        {
            if (scopes[i].ContainsKey(name))
            {
                return scopes[i][name];
            }
        }

        return null;
    }

    public void Set(string name, Variable value)
    {
        scopes[^1].Add(name, value);
    }

    public Variable GetAndCreateIfNotExists(string name)
    {
        var result = Get(name);
        if (result is not null)
        {
            return result;
        }

        var newVariable = new Variable(Consts.VariableTypes.Null, null);
        Set(name, newVariable);
        return newVariable;
    }

    public void AddScope()
    {
        scopes.Add(new Dictionary<string, Variable>());
    }

    public List<Dictionary<string, Variable>> GetScopes()
    {
        return scopes.ToList();
    }
}