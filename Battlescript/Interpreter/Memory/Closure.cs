namespace Battlescript;

public class Closure
{
    public List<Dictionary<string, Variable>> Scopes { get; set; }

    public Closure()
    {
        Scopes = [new Dictionary<string, Variable>()];
    }

    public Closure(Closure closure)
    {
        Scopes = [];
        foreach (var scope in closure.Scopes)
        {
            Scopes.Add(new Dictionary<string, Variable>());
            foreach (var (key, value) in scope)
            {
                Scopes[^1].Add(key, value);
            }
        }
        Scopes.Add(new Dictionary<string, Variable>());
    }
    
    public Variable? GetVariable(CallStack callStack, string name)
    {
        return GetVariable(callStack, new VariableInstruction(name));
    }

    public Variable? GetVariable(CallStack callStack, VariableInstruction variableInstruction)
    {
        for (var i = Scopes.Count - 1; i >= 0; i--)
        {
            if (Scopes[i].ContainsKey(variableInstruction.Name))
            {
                var foundVariable = Scopes[i][variableInstruction.Name];
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
    
        return null;
    }
    
    public void SetVariable(CallStack callStack, VariableInstruction variableInstruction, Variable valueVariable)
    {
        // We need to pass in the full instruction here to handle assigning to indexes
        if (Scopes[^1].ContainsKey(variableInstruction.Name))
        {
            if (variableInstruction.Next is SquareBracketsInstruction squareBracketsInstruction)
            {
                Scopes[^1][variableInstruction.Name].SetItem(
                    callStack,
                    this, 
                    valueVariable, 
                    squareBracketsInstruction);
            }
            else if (variableInstruction.Next is MemberInstruction memberInstruction)
            {
                Scopes[^1][variableInstruction.Name].SetMember(
                    callStack,
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
                Scopes[^1][variableInstruction.Name] = valueVariable;
            }
        }
        else
        {
            Scopes[^1].Add(variableInstruction.Name, valueVariable);
        }
    }

    public Dictionary<string, Variable> GetLastScope()
    {
        return Scopes[^1];
    }
}