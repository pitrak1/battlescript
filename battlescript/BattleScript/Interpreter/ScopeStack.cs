using System.Diagnostics;
using BattleScript.Exceptions;

namespace BattleScript.InterpreterNS;

public class ScopeStack : ContextStack
{
    public ScopeStack()
    {
        contexts.Add(new ScopeVariable(null, new Dictionary<string, ScopeVariable>()));
    }

    public override void Add(ScopeVariable? context = null)
    {
        if (context is not null)
        {
            Debug.Assert(context.Value is Dictionary<string, ScopeVariable>);
            contexts.Add(new ScopeVariable(null, context.Value));
        }
        else
        {
            contexts.Add(new ScopeVariable(null, new Dictionary<string, ScopeVariable>()));
        }
    }

    public ScopeVariable AddVariable(List<string> path, ScopeVariable? var = null)
    {
        return GetCurrentContext().AddVariable(path, var);
    }

    public ScopeVariable GetVariable(string key)
    {
        for (int i = (contexts.Count - 1); i >= 0; i--)
        {
            if (contexts[i].HasVariable(key))
            {
                return contexts[i].GetVariable(key);
            }
        }
        throw new VariableNotFoundException(key);
    }
}