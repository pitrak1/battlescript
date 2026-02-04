namespace Battlescript;

public static class TupleUnpacker
{
    /// <summary>
    /// Unpacks an iterable into multiple variables, supporting nested unpacking.
    /// Example: a, b, c = [1, 2, 3] or (x, (y, z)) = [1, [2, 3]]
    /// </summary>
    public static void UnpackTuple(
        CallStack callStack,
        Closure closure,
        ArrayInstruction leftTuple,
        Variable rightVariable)
    {
        // Get iterator from the right-hand side
        FunctionVariable nextMethod;
        try
        {
            nextMethod = BtlTypes.GetIteratorNext(callStack, closure, rightVariable, new VariableInstruction("temp"));
        }
        catch (InternalRaiseException ex) when (ex.Type == "TypeError")
        {
            var typeName = rightVariable switch
            {
                ObjectVariable obj => obj.Class.Name,
                _ => rightVariable.GetType().Name
            };
            throw new InternalRaiseException(BtlTypes.Types.TypeError, $"cannot unpack non-iterable {typeName} object");
        }

        // Collect values from iterator
        var values = new List<Variable>();
        while (true)
        {
            try
            {
                var value = nextMethod.RunFunction(callStack, closure, new ArgumentSet([]), new VariableInstruction("temp"));
                values.Add(value);
            }
            catch (InternalRaiseException ex) when (ex.Type == "StopIteration")
            {
                break;
            }
        }

        // Validate we got the right number of values
        if (values.Count > leftTuple.Values.Count)
        {
            throw new InternalRaiseException(BtlTypes.Types.ValueError,
                $"too many values to unpack (expected {leftTuple.Values.Count})");
        }
        if (values.Count < leftTuple.Values.Count)
        {
            throw new InternalRaiseException(BtlTypes.Types.ValueError,
                $"not enough values to unpack (expected {leftTuple.Values.Count}, got {values.Count})");
        }

        // Assign values to variables
        for (var i = 0; i < leftTuple.Values.Count; i++)
        {
            if (leftTuple.Values[i] is VariableInstruction varInst)
            {
                closure.SetVariable(callStack, varInst, values[i]);
            }
            else if (leftTuple.Values[i] is ArrayInstruction arrInst)
            {
                // Recursively unpack nested tuples
                UnpackTuple(callStack, closure, arrInst, values[i]);
            }
            else
            {
                throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "cannot assign to literal");
            }
        }
    }
}
