using System.Diagnostics;

namespace Battlescript;

public class ListVariable(List<Variable>? values = null) : Variable
{
    public List<Variable> Values { get; set; } = values ?? [];
    
    public override void AssignToIndexOrKey(Memory memory, Variable valueVariable, SquareBracketsInstruction index)
    {
        Debug.Assert(index.Values.Count == 1);

        var indexVariable = index.Values.First().Interpret(memory);
        Debug.Assert(indexVariable is NumberVariable);

        if (indexVariable is NumberVariable indexNumberVariable)
        {
            if (index.Next is null)
            {
                Values[(int)indexNumberVariable.Value] = valueVariable;
            }
            else
            {
                Debug.Assert(index.Next is SquareBracketsInstruction);
                var nextInstruction = index.Next as SquareBracketsInstruction;
                Values[(int)indexNumberVariable.Value].AssignToIndexOrKey(memory, valueVariable, nextInstruction!);
            }
        }
        else
        {
            throw new Exception("Can't index a list with anything but a number");
        }
    }
    
    public override Variable GetIndex(Memory memory, SquareBracketsInstruction index)
    {
        Debug.Assert(index.Values.Count == 1);

        var indexVariable = index.Values.First().Interpret(memory);

        if (indexVariable is NumberVariable indexNumberVariable)
        {
            if (index.Next is null)
            {
                return Values[(int)indexNumberVariable.Value];
            }
            else
            {
                Debug.Assert(index.Next is SquareBracketsInstruction);
                var nextInstruction = index.Next as SquareBracketsInstruction;
                return Values[(int)indexNumberVariable.Value].GetIndex(memory, nextInstruction!);
            }
        }
        else if (indexVariable is KeyValuePairVariable indexKvpVariable)
        {
            if (index.Next is null)
            {
                return GetRangeIndex(indexKvpVariable);
            }
            else
            {
                Debug.Assert(index.Next is SquareBracketsInstruction);
                var nextInstruction = index.Next as SquareBracketsInstruction;
                return GetRangeIndex(indexKvpVariable).GetIndex(memory, nextInstruction!);
            }
        }
        else
        {
            throw new Exception("Can't index a list with anything but a number or a range");
        }
    }
    
    private ListVariable GetRangeIndex(KeyValuePairVariable kvpVariable)
    {
        int? left = null;
        if (kvpVariable.Left is NumberVariable leftNumber)
        {
            left = (int)leftNumber.Value;
        } else if (kvpVariable.Left is not NullVariable)
        {
            throw new Exception("Left index must be a number or null");
        }
        
        int? right = null;
        if (kvpVariable.Right is NumberVariable rightNumber)
        {
            right = (int)rightNumber.Value;
        } else if (kvpVariable.Right is not NullVariable)
        {
            throw new Exception("Right index must be a number or null");
        }

        var index = left ?? 0;
        var count = right - left ?? Values.Count - 1;
        
        return new ListVariable(Values.GetRange(index, count));
    }
}