namespace Battlescript;

public class Closure
{
    public enum ClosureTypes { Function, Class }
    public List<ClosureScope> Scopes { get; set; }

    public Closure()
    {
        Scopes = [new ClosureScope()];
    }

    public Closure(Closure closure, ClosureTypes type = ClosureTypes.Function)
    {
        Scopes = [];
        foreach (var scope in closure.Scopes)
        {
            Scopes.Add(scope);
        }
        Scopes.Add(new ClosureScope(type));
    }
    
    public Variable? GetVariable(CallStack callStack, string name)
    {
        return GetVariable(callStack, new VariableInstruction(name));
    }

    public Variable? GetVariable(CallStack callStack, VariableInstruction variableInstruction)
    {
        for (var i = Scopes.Count - 1; i >= 0; i--)
        {
            if (Scopes[i].Type == ClosureTypes.Function && Scopes[i].Values.ContainsKey(variableInstruction.Name))
            {
                var foundVariable = Scopes[i].Values[variableInstruction.Name];
                if (variableInstruction.Next is SquareBracketsInstruction squareBracketsInstruction)
                {
                    return foundVariable.GetItem(callStack, this, squareBracketsInstruction);
                }
                else if (variableInstruction.Next is MemberInstruction memberInstruction)
                {
                    return foundVariable.GetMember(callStack, this, memberInstruction);
                }
                else
                {
                    return foundVariable;
                }
            }
        }
        
        throw new InternalRaiseException(BsTypes.Types.NameError, $"name '{variableInstruction.Name}' is not defined");
    }
    
    public void SetVariable(CallStack callStack, VariableInstruction variableInstruction, Variable valueVariable)
    {
        // We need to pass in the full instruction here to handle assigning to indexes
        if (Scopes[^1].Values.ContainsKey(variableInstruction.Name))
        {
            SetVariableInScope(callStack, this, variableInstruction, valueVariable, Scopes[^1]);
        }
        else if (Scopes[^1].Nonlocals.Contains(variableInstruction.Name))
        {
            for (var i = Scopes.Count - 2; i >= 0; i--)
            {
                if (Scopes[i].Values.ContainsKey(variableInstruction.Name))
                {
                    SetVariableInScope(callStack, this, variableInstruction, valueVariable, Scopes[i]);;
                    break;
                }
            }
        }
        else if (Scopes[^1].Globals.Contains(variableInstruction.Name))
        {
            SetVariableInScope(callStack, this, variableInstruction, valueVariable, Scopes[0]);
        }
        else
        {
            Scopes[^1].Values.Add(variableInstruction.Name, valueVariable);
        }
    }

    private void SetVariableInScope(
        CallStack callStack, 
        Closure closure, 
        VariableInstruction variableInstruction,
        Variable valueVariable,
        ClosureScope scope)
    {
        if (variableInstruction.Next is SquareBracketsInstruction squareBracketsInstruction)
        {
            scope.Values[variableInstruction.Name].SetItem(
                callStack,
                closure, 
                valueVariable, 
                squareBracketsInstruction);
        }
        else if (variableInstruction.Next is MemberInstruction memberInstruction)
        {
            scope.Values[variableInstruction.Name].SetMember(
                callStack,
                closure, 
                valueVariable, 
                memberInstruction);
        }
        else if (variableInstruction.Next is ParenthesesInstruction)
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "cannot assign to function call");
        }
        else
        {
            scope.Values[variableInstruction.Name] = valueVariable;
        }
    }

    public Dictionary<string, Variable> GetLastScope()
    {
        return Scopes[^1].Values;
    }

    public void CreateGlobalReference(string name)
    {
        if (Scopes[0].Values.ContainsKey(name))
        {
            Scopes[^1].Globals.Add(name);
        }
    }

    public void CreateNonlocalReference(string name)
    {
        for (var i = Scopes.Count - 2; i >= 0; i--)
        {
            if (Scopes[i].Values.ContainsKey(name))
            {
                Scopes[^1].Nonlocals.Add(name);
                break;
            }
        }
    }
}