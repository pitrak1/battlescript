namespace Battlescript.BuiltIn;

public static class BuiltInRange
{
    public static Variable Run(CallStack callStack, Closure closure, List<Instruction> arguments)
    {
        var startingValue = 0;
        var endingValue = 0;
        var step = 1;

        switch (arguments.Count)
        {
            case 1:
                // Treat only argument as the ending value, use defaults for others
                endingValue = BtlTypes.GetIntValue(arguments[0].Interpret(callStack, closure));
                break;
            case 2:
                // Treat two arguments as start and end, use default for step
                startingValue = BtlTypes.GetIntValue(arguments[0].Interpret(callStack, closure));
                endingValue = BtlTypes.GetIntValue(arguments[1].Interpret(callStack, closure));
                break;
            case 3:
                startingValue = BtlTypes.GetIntValue(arguments[0].Interpret(callStack, closure));
                endingValue = BtlTypes.GetIntValue(arguments[1].Interpret(callStack, closure));
                step = BtlTypes.GetIntValue(arguments[2].Interpret(callStack, closure));
                break;
            default:
                throw new Exception("Bad arguments, clean this up later");
        }
        
        List<Variable> values = [];

        if (startingValue < endingValue)
        {
            if (step > 0)
            {
                for (var i = startingValue; i < endingValue; i += step)
                {
                    values.Add(BtlTypes.Create(BtlTypes.Types.Int, i));
                }
            }
            
            return BtlTypes.Create(BtlTypes.Types.List, values);
        }
        else
        {
            if (step < 0)
            {
                for (var i = startingValue; i > endingValue; i += step)
                {
                    values.Add(BtlTypes.Create(BtlTypes.Types.Int, i));
                }
            }
            return BtlTypes.Create(BtlTypes.Types.List, values);
        }
    }
}