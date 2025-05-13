using System.Diagnostics;

namespace Battlescript;

public class ClassVariable (Dictionary<string, Variable>? values, List<ClassVariable>? superclasses = null) : Variable
{
    public Dictionary<string, Variable> Values { get; set; } = values ?? [];
    public List<ClassVariable> SuperClasses { get; set; } = superclasses ?? [];
    
    public override void AssignToIndexOrKey(Memory memory, Variable valueVariable, SquareBracketsInstruction index)
    {
        Debug.Assert(index.Values.Count == 1);

        var indexVariable = index.Values.First().Interpret(memory);

        if (indexVariable is StringVariable indexStringVariable)
        {
            if (index.Next is null)
            {
                Values[indexStringVariable.Value] = valueVariable;
            }
            else
            {
                Debug.Assert(index.Next is SquareBracketsInstruction);
                var nextInstruction = index.Next as SquareBracketsInstruction;
                Values[indexStringVariable.Value].AssignToIndexOrKey(memory, valueVariable, nextInstruction!);
            }
        }
        else
        {
            throw new Exception("Can't index a class with anything but a string");
        }
    }
    
    public override Variable? GetIndex(Memory memory, SquareBracketsInstruction index)
    {
        Debug.Assert(index.Values.Count == 1);

        var indexVariable = index.Values.First().Interpret(memory);

        if (indexVariable is StringVariable indexStringVariable)
        {
            if (Values.ContainsKey(indexStringVariable.Value))
            {
                if (index.Next is SquareBracketsInstruction squareBracketsInstruction)
                {
                    return Values[indexStringVariable.Value].GetIndex(memory, squareBracketsInstruction!);
                }
                else
                {
                    return Values[indexStringVariable.Value];
                }
            }
            else
            {
                foreach (var superclass in SuperClasses)
                {
                    try
                    {
                        return superclass.GetIndex(memory, index);
                    }
                    catch (KeyNotFoundException)
                    {
                        
                    }
                }
            }
            
        }
        else
        {
            throw new Exception("Can't index a class with anything but a string");
        }

        return null;
    }

    public ObjectVariable CreateObject()
    {
        var objectValues = new Dictionary<string, Variable>();
        AddToObjectValues(objectValues);
        return new ObjectVariable(objectValues, this);
    }

    public void AddToObjectValues(Dictionary<string, Variable> objectValues)
    {
        foreach (var kvp in Values)
        {
            if (kvp.Value is not FunctionVariable)
            {
                objectValues[kvp.Key] = kvp.Value;
            }
        }

        foreach (var superclass in SuperClasses)
        {
            superclass.AddToObjectValues(objectValues);
        }
    }

    public int AddClassToMemoryScopes(Memory memory)
    {
        var addedScopesCount = 0;
        
        // Reverse depth first search to add all required scopes to memory
        for (var i = SuperClasses.Count - 1; i >= 0; i--)
        {
            var scopesCount = SuperClasses[i].AddClassToMemoryScopes(memory);
            addedScopesCount += scopesCount;
        }
        
        memory.AddScope(Values);

        return addedScopesCount + 1;
    }
}
