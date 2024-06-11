using System.Diagnostics;
using BattleScript.Core;
using BattleScript.Exceptions;

namespace BattleScript.Core;

public class ScopeStack : Stack<Dictionary<string, Variable>>
{
    public ScopeStack()
    {
        Push(new Dictionary<string, Variable>());
    }

    public Variable GetVariable(string key)
    {
        var scopes = ToArray();
        for (int i = 0; i < scopes.Length; i++)
        {
            if (scopes[i].ContainsKey(key))
            {
                return scopes[i][key];
            }
        }
        throw new VariableNotFoundException(key);
    }
}