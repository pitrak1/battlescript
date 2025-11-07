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
        if (variableInstruction.Name == "a")
        {
            Console.WriteLine("a");
        }
        
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
            if (variableInstruction.Next is SquareBracketsInstruction squareBracketsInstruction)
            {
                Scopes[^1].Values[variableInstruction.Name].SetItem(
                    callStack,
                    this, 
                    valueVariable, 
                    squareBracketsInstruction);
            }
            else if (variableInstruction.Next is MemberInstruction memberInstruction)
            {
                Scopes[^1].Values[variableInstruction.Name].SetMember(
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
                Scopes[^1].Values[variableInstruction.Name] = valueVariable;
            }
        }
        else
        {
            Scopes[^1].Values.Add(variableInstruction.Name, valueVariable);
        }
    }

    public Dictionary<string, Variable> GetLastScope()
    {
        return Scopes[^1].Values;
    }
}