using System.Diagnostics;

namespace Battlescript;

public class Memory
{
    private List<Dictionary<string, Variable>> scopes = [];

    public Memory()
    {
        scopes.Add(new Dictionary<string, Variable>());
    }

    public Variable? GetVariable(string name)
    {
        // We need to pass in the full instruction here so we can handle indexing
        for (var i = scopes.Count - 1; i >= 0; i--)
        {
            if (scopes[i].ContainsKey(name))
            {
                return scopes[i][name];
            }
        }
    
        return null;
    }

    public void AssignToVariable(VariableInstruction variableInstruction, Variable valueVariable)
    {
        // We need to pass in the full instruction here to handle assigning to indexes
        if (VariableExists(variableInstruction.Name))
        {
            for (var i = scopes.Count - 1; i >= 0; i--)
            {
                if (scopes[i].ContainsKey(variableInstruction.Name))
                {
                    if (variableInstruction.Next is SquareBracketsInstruction squareBracketsInstruction)
                    {
                        scopes[i][variableInstruction.Name].AssignToIndexOrKey(this, valueVariable, squareBracketsInstruction);
                    }
                    else
                    {
                        scopes[i][variableInstruction.Name] = valueVariable;
                    }
                }
            }
        }
        else
        {
            scopes[^1].Add(variableInstruction.Name, valueVariable);
        }
    }

    private bool VariableExists(string name)
    {
        for (var i = scopes.Count - 1; i >= 0; i--)
        {
            if (scopes[i].ContainsKey(name))
            {
                return true;
            }
        }
        
        return false;        
    }

    public void AddScope(Dictionary<string, Variable>? scope = null)
    {
        scopes.Add(scope ?? new Dictionary<string, Variable>());
    }

    public Dictionary<string, Variable> RemoveScope()
    {
        var removedScope = scopes[^1];
        scopes.RemoveAt(scopes.Count - 1);
        return removedScope;
    }

    public List<Dictionary<string, Variable>> GetScopes()
    {
        return scopes.ToList();
    }
}