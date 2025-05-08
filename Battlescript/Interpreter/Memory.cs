using System.Diagnostics;

namespace Battlescript;

public class Memory
{
    private List<Dictionary<string, Variable>> scopes = [];

    public Memory()
    {
        scopes.Add(new Dictionary<string, Variable>());
    }

    public Variable? GetVariable(VariableInstruction variableInstruction)
    {
        // We need to pass in the full instruction here so we can handle indexing
        for (var i = scopes.Count - 1; i >= 0; i--)
        {
            if (scopes[i].ContainsKey(variableInstruction.Name))
            {
                if (variableInstruction.Next is null)
                {
                    return scopes[i][variableInstruction.Name];
                }
                else
                {
                    Debug.Assert(scopes[i][variableInstruction.Name] is ListVariable || 
                                 scopes[i][variableInstruction.Name] is DictionaryVariable);
                    Debug.Assert(variableInstruction.Next is SquareBracketsInstruction);
                    var nextInstruction = variableInstruction.Next as SquareBracketsInstruction;
                    return scopes[i][variableInstruction.Name].GetIndex(this, nextInstruction!);
                }
            }
        }
    
        return null;
    }

    public void AssignToVariable(VariableInstruction variableInstruction, Variable valueVariable)
    {
        if (VariableExists(variableInstruction.Name))
        {
            for (var i = scopes.Count - 1; i >= 0; i--)
            {
                if (scopes[i].ContainsKey(variableInstruction.Name))
                {
                    if (variableInstruction.Next is null)
                    {
                        scopes[i][variableInstruction.Name] = valueVariable;
                    }
                    else
                    {
                        Debug.Assert(variableInstruction.Next is SquareBracketsInstruction);
                        var nextInstruction = variableInstruction.Next as SquareBracketsInstruction;
                        scopes[i][variableInstruction.Name].AssignToIndexOrKey(this, valueVariable, nextInstruction!);
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

    public void AddScope()
    {
        scopes.Add(new Dictionary<string, Variable>());
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