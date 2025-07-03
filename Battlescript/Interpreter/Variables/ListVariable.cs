using System.Diagnostics;

namespace Battlescript;

public class ListVariable : Variable, IEquatable<ListVariable>
{
    public List<Variable?> Values { get; set; }

    public ListVariable(List<Variable?>? values = null)
    {
        Values = values ?? [];
        Type = Consts.VariableTypes.Reference;
    }
    
    public override Variable? SetItemDirectly(Memory memory, Variable valueVariable, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        // This needs to be rewritten to support ranged assignments, look at list methods in python to see test cases
        var indexVariable = index.Interpret(memory) as ListVariable;

        if (indexVariable.Values[0] is IntegerVariable indexInteger)
        {
            if (index.Next is null)
            {
                Values[indexInteger.Value] = valueVariable;
                return valueVariable;
            }
            else
            {
                return Values[indexInteger.Value];
            }
        }
        else
        {
            throw new Exception("Can't index a list with anything but a number");
        }
    }
    
    public override Variable? GetItemDirectly(Memory memory, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        var indexVariable = index.Interpret(memory) as ListVariable;

        if (indexVariable.Values.Count > 1)
        {
            return GetRangeIndex(indexVariable);
        }
        else
        {
            if (indexVariable.Values[0] is IntegerVariable indexInteger)
            {
                return Values[indexInteger.Value];
            }
            else
            {
                throw new Exception("Can't index a list with anything but a number");
            }
            
        }
    }
    
    public ListVariable GetRangeIndex(ListVariable argVariable)
    {
        int start = 0;
        int stop = Values.Count;
        int step = 1;
        
        List<IntegerVariable?> values = [];
        foreach (var value in argVariable.Values)
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

        return result;
    }

    public Variable RunMethod(Memory memory, string method, Instruction? arguments)
    {
        if (arguments is not ArrayInstruction { Separator: "(" } parens)
        {
            throw new Exception("must use parens to call list method, fix this later");
        }

        List<Variable> argumentVariables = [];
        foreach (var arg in parens.Values)
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