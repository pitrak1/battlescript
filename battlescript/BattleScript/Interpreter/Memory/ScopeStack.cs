using System.Diagnostics;
using BattleScript.Core;
using BattleScript.Exceptions;

namespace BattleScript.InterpreterNS;

public class ScopeStack : ContextStack
{
    public ScopeStack()
    {
        contexts.Add(new ScopeVariable(Consts.VariableTypes.Scope, new Dictionary<string, ScopeVariable>()));
    }

    public void AddNewScope()
    {
        contexts.Add(new ScopeVariable(null, new Dictionary<string, ScopeVariable>()));
    }

    public void AddExistingScope(ScopeVariable context)
    {
        Debug.Assert(context.Value is Dictionary<string, ScopeVariable>, "Adding a new scope requires it to be a Dictionary<string, ScopeVariable>");
        contexts.Add(new ScopeVariable(null, context.Value));
    }

    public ScopeVariable AddVariableToCurrentScope(List<string> path, ScopeVariable? var = null)
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