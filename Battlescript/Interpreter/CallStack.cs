namespace Battlescript;

public class CallStack
{
    public List<StackFrame> Scopes { get; set; }

    public CallStack()
    {
        Scopes = [new StackFrame("main", "<module>")];
    }
    
    public Variable? GetVariable(string name)
    {
        return GetVariable(new VariableInstruction(name));
    }

    public Variable? GetVariable(VariableInstruction variableInstruction)
    {
        for (var i = Scopes.Count - 1; i >= 0; i--)
        {
            if (Scopes[i].Values.ContainsKey(variableInstruction.Name))
            {
                var foundVariable = Scopes[i].Values[variableInstruction.Name];
                if (variableInstruction.Next is SquareBracketsInstruction squareBracketsInstruction)
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
                if (Scopes[i].Values.ContainsKey(variableInstruction.Name))
                {
                    if (variableInstruction.Next is SquareBracketsInstruction squareBracketsInstruction)
                    {
                        Scopes[i].Values[variableInstruction.Name].SetItem(
                            this, 
                            valueVariable, 
                            squareBracketsInstruction);
                    }
                    else if (variableInstruction.Next is MemberInstruction memberInstruction)
                    {
                        Scopes[i].Values[variableInstruction.Name].SetMember(
                            this, 
                            valueVariable, 
                            memberInstruction);
                    }
                    else if (variableInstruction.Next is ParenthesesInstruction)
                    {
                        throw new InternalRaiseException(BsTypes.Types.SyntaxError, "cannot assign to function call");
                    }
                    else
                    {
                        Scopes[i].Values[variableInstruction.Name] = valueVariable;
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
        Scopes[^1].Values.Add(variableInstruction.Name, valueVariable);
    }
    
    public void AddScope(int entryLine, string entryExpression, string function, string? file = null)
    {
        Scopes[^1].UpdateLineAndExpression(entryLine, entryExpression);
        var fileValue = file ?? Scopes[^1].File;
        Scopes.Add(new StackFrame(fileValue, function));
    }

    public StackFrame RemoveScope()
    {
        var removedScope = Scopes[^1];
        Scopes.RemoveAt(Scopes.Count - 1);
        return removedScope;
    }
}