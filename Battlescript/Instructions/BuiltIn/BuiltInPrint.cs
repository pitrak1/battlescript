namespace Battlescript;

public static class BuiltInPrint
{
    public static void Run(CallStack callStack, List<Instruction> arguments)
    {
        if (arguments.Count != 1)
        {
            throw new Exception("Bad arguments, clean this up later");
        }
        
        var firstExpression = arguments[0].Interpret(callStack);
        Console.WriteLine(StringUtilities.GetVariableAsString(callStack, firstExpression));
    }
}