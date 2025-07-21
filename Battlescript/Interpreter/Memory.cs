using System.Diagnostics;

namespace Battlescript;

public class Memory(List<Dictionary<string, Variable>>? scopes = null)
{
    public List<Dictionary<string, Variable>> Scopes { get; } = scopes ?? [new Dictionary<string, Variable>()];

    public Dictionary<BsTypes.Types, ClassVariable> BuiltInReferences = [];

    public void PopulateBuiltInReferences()
    {
        foreach (var builtin in BsTypes.TypeStrings)
        {
            var type = BsTypes.GetTypeFromString(builtin);
            BuiltInReferences[type] = GetVariable(builtin) as ClassVariable;
        }
    }

    public ClassVariable GetBuiltIn(BsTypes.Types type)
    {
        return BuiltInReferences[type];
    }
    
    public Variable? GetVariable(string name)
    {
        return GetVariable(new VariableInstruction(name));
    }

    public Variable? GetVariable(VariableInstruction variableInstruction)
    {
        for (var i = Scopes.Count - 1; i >= 0; i--)
        {
            if (Scopes[i].ContainsKey(variableInstruction.Name))
            {
                var foundVariable = Scopes[i][variableInstruction.Name];
                if (variableInstruction.Next is ArrayInstruction { Separator: "[" } squareBracketsInstruction)
                {
                    return foundVariable.GetItem(this, squareBracketsInstruction);
                }
                else if (variableInstruction.Next is MemberInstruction memberInstruction)
                {
                    return foundVariable.GetMember(this, memberInstruction);
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
            for (var i = Scopes.Count - 1; i >= 0; i--)
            {
                if (Scopes[i].ContainsKey(variableInstruction.Name))
                {
                    if (variableInstruction.Next is ArrayInstruction { Separator: "[" } squareBracketsInstruction)
                    {
                        Scopes[i][variableInstruction.Name].SetItem(
                            this, 
                            valueVariable, 
                            squareBracketsInstruction);
                    }
                    else if (variableInstruction.Next is MemberInstruction memberInstruction)
                    {
                        Scopes[i][variableInstruction.Name].SetMember(
                            this, 
                            valueVariable, 
                            memberInstruction);
                    }
                    else
                    {
                        Scopes[i][variableInstruction.Name] = valueVariable;
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
        Scopes[^1].Add(variableInstruction.Name, valueVariable);
    }
    
    public void AddScope(Dictionary<string, Variable>? scope = null)
    {
        Scopes.Add(scope ?? new Dictionary<string, Variable>());
    }

    public Dictionary<string, Variable> RemoveScope()
    {
        var removedScope = Scopes[^1];
        Scopes.RemoveAt(Scopes.Count - 1);
        return removedScope;
    }

    public void RemoveScopes(int count)
    {
        for (var i = 0; i < count; i++)
        {
            RemoveScope();
        }
    }
}