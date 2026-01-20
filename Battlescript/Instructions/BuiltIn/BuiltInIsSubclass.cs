namespace Battlescript.BuiltIn;

public static class BuiltInIsSubclass
{
    public static Variable Run(CallStack callStack, Closure closure, List<Instruction> arguments)
    {
        CheckArguments(arguments);
        
        var firstExpression = arguments[0].Interpret(callStack, closure);
        var secondExpression = arguments[1].Interpret(callStack, closure);

        if (firstExpression is ClassVariable firstVariable && secondExpression is ClassVariable secondVariable)
        {
            return BtlTypes.Create(BtlTypes.Types.Bool, firstVariable.IsSubclass(secondVariable));
        }
        else
        {
            throw new Exception("Bad arguments, clean this up later");
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