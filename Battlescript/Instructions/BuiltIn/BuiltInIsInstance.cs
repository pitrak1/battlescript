namespace Battlescript.BuiltIn;

public static class BuiltInIsInstance
{
    public static Variable Run(CallStack callStack, Closure closure, List<Instruction> arguments)
    {
        CheckArguments(arguments);
        
        var objectExpression = arguments[0].Interpret(callStack, closure);
        if (arguments[1] is ConversionTypeInstruction conversionTypeInstruction)
        {
            switch (conversionTypeInstruction.Value)
            {
                case "__btl_numeric__":
                    return BsTypes.Create(BsTypes.Types.Bool,
                        objectExpression is NumericVariable);
                case "__btl_sequence__":
                    return BsTypes.Create(BsTypes.Types.Bool, objectExpression is SequenceVariable);
                default:
                    return BsTypes.Create(BsTypes.Types.Bool, false);
            }
        }
        else
        {
            var classExpression = arguments[1].Interpret(callStack, closure);
        
            if (objectExpression is ObjectVariable objectVariable && classExpression is ClassVariable classVariable)
            {
                return BsTypes.Create(BsTypes.Types.Bool, objectVariable.IsInstance(classVariable));
            }
            else
            {
                return BsTypes.Create(BsTypes.Types.Bool, false);
            }
        }
    }

    private static void CheckArguments(List<Instruction> arguments)
    {
        if (arguments.Count != 2)
        {
            throw new Exception("Bad arguments, clean this up later");
        }
    }
}