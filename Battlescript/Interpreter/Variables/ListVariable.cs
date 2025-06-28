using System.Diagnostics;

namespace Battlescript;

public class ListVariable(List<Variable>? values = null) : ReferenceVariable, IEquatable<ListVariable>
{
    public List<Variable> Values { get; set; } = values ?? [];
    
    public override bool SetItem(Memory memory, Variable valueVariable, SquareBracketsInstruction index, ObjectVariable? objectContext = null)
    {
        Debug.Assert(index.Values.Count == 1);

        var indexVariable = index.Values.First().Interpret(memory);
        Debug.Assert(indexVariable is IntegerVariable);

        if (indexVariable is IntegerVariable indexNumberVariable)
        {
            if (index.Next is null)
            {
                Values[indexNumberVariable.Value] = valueVariable;
                return true;
            }
            else
            {
                Debug.Assert(index.Next is SquareBracketsInstruction);
                var nextInstruction = index.Next as SquareBracketsInstruction;
                return Values[indexNumberVariable.Value].SetItem(memory, valueVariable, nextInstruction!);
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
        
        if (index.Values.First() is ArrayInstruction)
        {
            var indexArrayVariable = indexVariable as ArrayVariable;
            
            if (index.Next is null)
            {
                return GetRangeIndex(indexArrayVariable);
            }
            else
            {
                Debug.Assert(index.Next is SquareBracketsInstruction);
                var nextInstruction = index.Next as SquareBracketsInstruction;
                return GetRangeIndex(indexArrayVariable).GetItem(memory, nextInstruction!);
            }
        }
        else
        {
            var indexNumberVariable = indexVariable as IntegerVariable;
            
            if (index.Next is null)
            {
                return Values[indexNumberVariable.Value];
            }
            else
            {
                Debug.Assert(index.Next is SquareBracketsInstruction);
                var nextInstruction = index.Next as SquareBracketsInstruction;
                return Values[indexNumberVariable.Value].GetItem(memory, nextInstruction!);
            }
        }
    }
    
    public ListVariable GetRangeIndex(ArrayVariable arrayVariable)
    {
        int start = 0;
        int stop = Values.Count;
        int step = 1;
        
        List<IntegerVariable?> values = [];
        foreach (var value in arrayVariable.Values)
        {
            if (value is IntegerVariable integerVariable)
            {
                values.Add(integerVariable);
            } else if (value is null)
            {
                values.Add(null);
            }
            else
            {
                throw new Exception("Invalid range index, fix this later");
            }
        }
        
        if (values.Count == 3)
        {
            if (values[2] is IntegerVariable stepInt)
            {
                step = stepInt.Value;
            }
            
            if (values[0] is IntegerVariable startInt)
            {
                start = startInt.Value % Values.Count;
            } else if (values[0] is null && step < 0)
            {
                start = Values.Count - 1;
            }

            if (values[1] is IntegerVariable stopInt)
            {
                stop = stopInt.Value % Values.Count;
            }
            else if (values[1] is null && step < 0)
            {
                stop = -1;
            }
        } else if (values.Count == 2)
        {
            if (values[0] is IntegerVariable startInt)
            {
                start = startInt.Value % Values.Count;
            }
            
            if (values[1] is IntegerVariable stopInt)
            {
                stop = stopInt.Value % Values.Count;
            }
        }
        else
        {
            throw new Exception("Invalid range index, fix this later");
        }

        int index = start;
        ListVariable result = new ListVariable();
        if (step < 0)
        {
            while (index > stop)
            {
                result.Values.Add(Values[index]);
                index += step;
            }
        }
        else
        {
            while (index < stop)
            {
                result.Values.Add(Values[index]);
                index += step;
            }
        }
        
        
        
        // Need huge changes here, especially to support appending and step
        return result;
        // if (kvpVariable.Left is not null && kvpVariable.Left is not IntegerVariable)
        // {
        //     throw new InterpreterInvalidIndexException(kvpVariable.Left);
        // }
        //
        // if (kvpVariable.Right is not null && kvpVariable.Right is not IntegerVariable)
        // {
        //     throw new InterpreterInvalidIndexException(kvpVariable.Right);
        // }
        //
        // int left = kvpVariable.Left is IntegerVariable leftNumber ? leftNumber.Value : 0;
        // int right = kvpVariable.Right is IntegerVariable rightNumber ? rightNumber.Value : Values.Count - 1;
        //
        // var index = left;
        // int count = right - left + 1;
        //
        // return new ListVariable(Values.GetRange(index, count));
    }

    public Variable RunMethod(Memory memory, string method, Instruction? arguments)
    {
        if (arguments is not ParensInstruction parens)
        {
            throw new Exception("must use parens to call list method, fix this later");
        }

        List<Variable> argumentVariables = [];
        foreach (var arg in parens.Instructions)
        {
            argumentVariables.Add(arg.Interpret(memory));
        }

        switch (method)
        {
            case "append":
                return Append(argumentVariables);
            case "extend":
                return Extend(argumentVariables);
            case "insert":
                return Insert(argumentVariables);
            case "remove":
                return Remove(argumentVariables);
            case "pop":
                return Pop(argumentVariables);
            case "clear":
                return Clear(argumentVariables);
            case "count":
                return Count(argumentVariables);
            case "reverse":
                return Reverse(argumentVariables);
            case "copy":
                return Copy(argumentVariables);
            default:
                throw new Exception("Invalid list method: " + method);
        }
    }

    public Variable Append(List<Variable> arguments)
    {
        if (arguments.Count != 1)
        {
            throw new Exception("Expected 1 argument for list append");
        }

        Values.Add(arguments[0]);

        return new ConstantVariable();
    }
    
    public Variable Extend(List<Variable> arguments)
    {
        if (arguments.Count != 1 || arguments[0] is not ListVariable)
        {
            throw new Exception("Expected 1 list argument for list append");
        }
        var newList = arguments[0] as ListVariable;
        Values.AddRange(newList.Values);

        return new ConstantVariable();
    }

    public Variable Insert(List<Variable> arguments)
    {
        if (arguments.Count != 2 || arguments[0] is not IntegerVariable)
        {
            throw new Exception("Expected 2 arguments for list append, argument 1 must be integer in range + 1");
        }
        
        var i = arguments[0] as IntegerVariable;
        if (i.Value == Values.Count)
        {
            Values.Add(arguments[1]);
        } else if (i.Value < Values.Count && i.Value >= 0)
        {
            Values.Insert(i.Value, arguments[1]);
        }
        else
        {
            throw new Exception("Expected 2 arguments for list append, argument 1 must be integer in range + 1");
        }
        
        return new ConstantVariable();
    }

    public Variable Remove(List<Variable> arguments)
    {
        if (arguments.Count != 1)
        {
            throw new Exception("Expected 1 argument for list append");
        }

        if (!Values.Contains(arguments[0]))
        {
            throw new Exception("Python ValueError");
        }

        Values.Remove(arguments[0]);
        
        return new ConstantVariable();
    }

    public Variable Pop(List<Variable> arguments)
    {
        if (arguments.Count == 1 && arguments[0] is IntegerVariable i)
        {
            if (i.Value < Values.Count && i.Value >= 0)
            {
                var value = Values[i.Value];
                Values.RemoveAt(i.Value);
                return value;
            }
            else
            {
                throw new Exception("Argument 1 must be integer in range if given");
            }
        } else if (arguments.Count == 0)
        {
            var value = Values.Last();
            Values.RemoveAt(Values.Count - 1);
            return value;
        }
        else
        {
            throw new Exception("Expected 1 optional argument for list pop, argument 1 must be integer in range");
        }
    }

    public Variable Clear(List<Variable> arguments)
    {
        Values.Clear();
        return new ConstantVariable();
    }

    public Variable Count(List<Variable> arguments)
    {
        if (arguments.Count != 1)
        {
            throw new Exception("Expected 1 argument for list count");
        }

        var count = 0;
        foreach (var value in Values)
        {
            if (value.Equals(arguments[0]))
            {
                count++;
            }
        }
        
        return new IntegerVariable(count);
    }
    
    public Variable Reverse(List<Variable> arguments)
    {
        
        Values.Reverse();
        
        return new ConstantVariable();
    }
    
    public Variable Copy(List<Variable> arguments)
    {

        var copyValues = new List<Variable>(Values);
        
        return new ListVariable(copyValues);
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