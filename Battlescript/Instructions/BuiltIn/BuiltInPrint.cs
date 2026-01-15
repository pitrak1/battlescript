namespace Battlescript;

public static class BuiltInPrint
{
    public static void Run(CallStack callStack, Closure closure, List<Instruction> arguments)
    {
        CheckArguments(arguments);
        var firstExpression = arguments[0].Interpret(callStack, closure);
        Console.WriteLine(StringUtilities.GetVariableAsString(callStack, closure, firstExpression));
    }

    private static void CheckArguments(List<Instruction> arguments)
    {
        if (arguments.Count != 1)
        {
            throw new Exception("Bad arguments, clean this up later");
        }
    }
}