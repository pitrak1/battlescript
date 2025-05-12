using System.Diagnostics;

namespace Battlescript;

public class ObjectVariable (Dictionary<string, Variable>? values, ClassVariable classVariable) : Variable
{
    public Dictionary<string, Variable> Values { get; set; } = values ?? [];
    public ClassVariable ClassVariable { get; set; } = classVariable;

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
            throw new Exception("Can't index an object with anything but a string");
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
                if (index.Next is SquareBracketsInstruction nextInstruction)
                {
                    return Values[indexStringVariable.Value].GetIndex(memory, nextInstruction!);
                }
                else
                {
                    return Values[indexStringVariable.Value];
                }
            }
            else
            {
                return ClassVariable.GetIndex(memory, index);
            }
            
        }
        else
        {
            throw new Exception("Can't index an object with anything but a string");
        }
    }
}
