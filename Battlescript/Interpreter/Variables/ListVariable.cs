using System.Diagnostics;

namespace Battlescript;

public class ListVariable(List<Variable>? values = null) : Variable, IEquatable<ListVariable>
{
    public List<Variable> Values { get; set; } = values ?? [];
    
    public override bool SetItem(Memory memory, Variable valueVariable, SquareBracketsInstruction index, ObjectVariable? objectContext = null)
    {
        Debug.Assert(index.Values.Count == 1);

        var indexVariable = index.Values.First().Interpret(memory);
        Debug.Assert(indexVariable is NumberVariable);

        if (indexVariable is NumberVariable indexNumberVariable)
        {
            if (index.Next is null)
            {
                Values[(int)indexNumberVariable.Value] = valueVariable;
                return true;
            }
            else
            {
                Debug.Assert(index.Next is SquareBracketsInstruction);
                var nextInstruction = index.Next as SquareBracketsInstruction;
                return Values[(int)indexNumberVariable.Value].SetItem(memory, valueVariable, nextInstruction!);
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
        if (kvpVariable.Left is not null && kvpVariable.Left is not NumberVariable)
        {
            throw new InterpreterInvalidIndexException(kvpVariable.Left);
        }
        
        if (kvpVariable.Right is not null && kvpVariable.Right is not NumberVariable)
        {
            throw new InterpreterInvalidIndexException(kvpVariable.Right);
        }
        
        int left = kvpVariable.Left is NumberVariable leftNumber ? (int)leftNumber.Value : 0;
        int right = kvpVariable.Right is NumberVariable rightNumber ? (int)rightNumber.Value : Values.Count - 1;

        var index = left;
        int count = right - left + 1;
        
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