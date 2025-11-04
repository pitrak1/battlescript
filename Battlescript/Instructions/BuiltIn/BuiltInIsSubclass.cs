namespace Battlescript.BuiltIn;

public static class BuiltInIsSubclass
{
    public static Variable Run(CallStack callStack, List<Instruction> arguments)
    {
        if (arguments.Count != 2)
        {
            throw new Exception("Bad arguments, clean this up later");
        }
        
        var firstExpression = arguments[0].Interpret(callStack);
        var secondExpression = arguments[1].Interpret(callStack);

        if (firstExpression is ClassVariable firstVariable && secondExpression is ClassVariable secondVariable)
        {
            return BsTypes.Create(BsTypes.Types.Bool, firstVariable.IsSubclass(secondVariable));
        }
        else
        {
            throw new Exception("Bad arguments, clean this up later");
        }
    }
}