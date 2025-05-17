using System.Diagnostics;

namespace Battlescript;

public class Memory
{
    private List<Dictionary<string, Variable>> _scopes;

    public Memory(List<Dictionary<string, Variable>>? scopes = null)
    {
        _scopes = scopes ?? [new Dictionary<string, Variable>()];
    }

    public Variable? GetVariable(string name)
    {
        for (var i = _scopes.Count - 1; i >= 0; i--)
        {
            if (_scopes[i].ContainsKey(name))
            {
                return _scopes[i][name];
            }
        }
    
        return null;
    }

    public void AssignToVariable(VariableInstruction variableInstruction, Variable valueVariable)
    {
        // We need to pass in the full instruction here to handle assigning to indexes
        if (VariableExists(variableInstruction.Name))
        {
            for (var i = _scopes.Count - 1; i >= 0; i--)
            {
                if (_scopes[i].ContainsKey(variableInstruction.Name))
                {
                    if (variableInstruction.Next is SquareBracketsInstruction squareBracketsInstruction)
                    {
                        _scopes[i][variableInstruction.Name].AssignToIndexOrKey(
                            this, 
                            valueVariable, 
                            squareBracketsInstruction);
                    }
                    else
                    {
                        _scopes[i][variableInstruction.Name] = valueVariable;
                    }

                    return;
                }
            }
        }
        else
        {
            _scopes[^1].Add(variableInstruction.Name, valueVariable);
        }
    }

    private bool VariableExists(string name)
    {
        for (var i = _scopes.Count - 1; i >= 0; i--)
        {
            if (_scopes[i].ContainsKey(name))
            {
                return true;
            }
        }
        
        return false;        
    }

    public void AddScope(Dictionary<string, Variable>? scope = null)
    {
        _scopes.Add(scope ?? new Dictionary<string, Variable>());
    }

    public Dictionary<string, Variable> RemoveScope()
    {
        var removedScope = _scopes[^1];
        _scopes.RemoveAt(_scopes.Count - 1);
        return removedScope;
    }

    public void RemoveScopes(int count)
    {
        for (var i = 0; i < count; i++)
        {
            RemoveScope();
        }
    }

    public List<Dictionary<string, Variable>> GetScopes()
    {
        return _scopes.ToList();
    }
}