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
        return GetVariable(new VariableInstruction(name));
    }

    public Variable? GetVariable(VariableInstruction variableInstruction)
    {
        for (var i = _scopes.Count - 1; i >= 0; i--)
        {
            if (_scopes[i].ContainsKey(variableInstruction.Name))
            {
                var foundVariable = _scopes[i][variableInstruction.Name];
                if (variableInstruction.Next is SquareBracketsInstruction squareBracketsInstruction)
                {
                    return foundVariable.GetItem(this, squareBracketsInstruction);
                }
                else
                {
                    return foundVariable;
                }
            }
        }
    
        return null;
    }
    
    public void SetVariable(VariableInstruction variableInstruction, Variable valueVariable)
    {
        // We need to pass in the full instruction here to handle assigning to indexes
        if (GetVariable(variableInstruction.Name) is not null)
        {
            for (var i = _scopes.Count - 1; i >= 0; i--)
            {
                if (_scopes[i].ContainsKey(variableInstruction.Name))
                {
                    if (variableInstruction.Next is SquareBracketsInstruction squareBracketsInstruction)
                    {
                        _scopes[i][variableInstruction.Name].SetItem(
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
            AddVariableToLastScope(variableInstruction, valueVariable);
        }
    }

    public void AddVariableToLastScope(VariableInstruction variableInstruction, Variable valueVariable)
    {
        _scopes[^1].Add(variableInstruction.Name, valueVariable);
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