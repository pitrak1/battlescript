using System.Diagnostics;

namespace Battlescript;

public class ListVariable(List<Variable>? values = null) : Variable, IEquatable<ListVariable>
{
    public List<Variable> Values { get; set; } = values ?? [];
    
    public override void SetItem(Memory memory, Variable valueVariable, SquareBracketsInstruction index)
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
                Values[(int)indexNumberVariable.Value].SetItem(memory, valueVariable, nextInstruction!);
            }
        }
        else
        {
            throw new Exception("Can't index a list with anything but a number");
        }
    }
    
    public override Variable? GetItem(Memory memory, SquareBracketsInstruction index, ObjectVariable? objectContext = null)
    {
        Debug.Assert(index.Values.Count == 1);

        var indexVariable = index.Values.First().Interpret(memory);
        
        if (index.Values.First() is KeyValuePairInstruction kvpInst)
        {
            var indexKvpVariable = indexVariable as KeyValuePairVariable;
            
            if (index.Next is null)
            {
                return GetRangeIndex(indexKvpVariable);
            }
            else
            {
                Debug.Assert(index.Next is SquareBracketsInstruction);
                var nextInstruction = index.Next as SquareBracketsInstruction;
                return GetRangeIndex(indexKvpVariable).GetItem(memory, nextInstruction!);
            }
        }
        else
        {
            var indexNumberVariable = indexVariable as NumberVariable;
            
            if (index.Next is null)
            {
                return Values[(int)indexNumberVariable.Value];
            }
            else
            {
                Debug.Assert(index.Next is SquareBracketsInstruction);
                var nextInstruction = index.Next as SquareBracketsInstruction;
                return Values[(int)indexNumberVariable.Value].GetItem(memory, nextInstruction!);
            }
        }
    }
    
    private ListVariable GetRangeIndex(KeyValuePairVariable kvpVariable)
    {
        int? left = null;
        if (kvpVariable.Left is NumberVariable leftNumber)
        {
            left = (int)leftNumber.Value;
        } else if (kvpVariable.Left is not null)
        {
            throw new Exception("Left index must be a number or null");
        }
        
        int? right = null;
        if (kvpVariable.Right is NumberVariable rightNumber)
        {
            right = (int)rightNumber.Value;
        } else if (kvpVariable.Right is not null)
        {
            throw new Exception("Right index must be a number or null");
        }

        var index = left ?? 0;
        var count = right - left ?? Values.Count - 1;
        
        return new ListVariable(Values.GetRange(index, count));
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as ListVariable);
    public bool Equals(ListVariable? variable)
    {
        if (variable is null) return false;
        if (ReferenceEquals(this, variable)) return true;
        if (GetType() != variable.GetType()) return false;
        
        return Values.SequenceEqual(variable.Values);
    }
    
    public override int GetHashCode() => HashCode.Combine(Values);
}