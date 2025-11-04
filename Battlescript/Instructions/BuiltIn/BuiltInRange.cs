namespace Battlescript.BuiltIn;

public static class BuiltInRange
{
    public static Variable Run(CallStack callStack, Closure closure, List<Instruction> arguments)
    {
        int startingValue = 0;
        int count = 0;
        int step = 1;

        if (arguments.Count == 1)
        {
            var countExpression = arguments[0].Interpret(callStack, closure);
            count = BsTypes.GetIntValue(countExpression);
        } else if (arguments.Count == 2)
        {
            var startingValueExpression = arguments[0].Interpret(callStack, closure);
            var countExpression = arguments[1].Interpret(callStack, closure);
            startingValue = BsTypes.GetIntValue(startingValueExpression);
            count = BsTypes.GetIntValue(countExpression);
        } else if (arguments.Count == 3)
        {
            var startingValueExpression = arguments[0].Interpret(callStack, closure);
            var countExpression = arguments[1].Interpret(callStack, closure);
            var stepExpression = arguments[2].Interpret(callStack, closure);
            startingValue = BsTypes.GetIntValue(startingValueExpression);
            count = BsTypes.GetIntValue(countExpression);
            step = BsTypes.GetIntValue(stepExpression);
        }
        else
        {
            throw new Exception("Bad arguments, clean this up later");
        }
        
        List<Variable> values = [];

        if (startingValue < count)
        {
            if (step > 0)
            {
                for (var i = startingValue; i < count; i += step)
                {
                    values.Add(BsTypes.Create(BsTypes.Types.Int, i));
                }
            }
            
            return BsTypes.Create(BsTypes.Types.List, values);
        }
        else
        {
            if (step < 0)
            {
                for (var i = startingValue; i > count; i += step)
                {
                    values.Add(BsTypes.Create(BsTypes.Types.Int, i));
                }
            }
            return BsTypes.Create(BsTypes.Types.List, values);
        }
    }
}