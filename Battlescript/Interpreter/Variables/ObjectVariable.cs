using System.Diagnostics;

namespace Battlescript;

public class ObjectVariable (Dictionary<string, Variable>? values) : Variable
{
    public Dictionary<string, Variable> Values { get; set; } = values ?? [];
    
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
    
    public override Variable GetIndex(Memory memory, SquareBracketsInstruction index)
    {
        Debug.Assert(index.Values.Count == 1);

        var indexVariable = index.Values.First().Interpret(memory);

        if (indexVariable is StringVariable indexStringVariable)
        {
            if (index.Next is null)
            {
                return Values[indexStringVariable.Value];
            }
            else
            {
                Debug.Assert(index.Next is SquareBracketsInstruction);
                var nextInstruction = index.Next as SquareBracketsInstruction;
                return Values[indexStringVariable.Value].GetIndex(memory, nextInstruction!);
            }
        }
        else
        {
            throw new Exception("Can't index an object with anything but a string");
        }
    }
}
