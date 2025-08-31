namespace Battlescript;

public static class BuiltInPrint
{
    public static void Run(Memory memory, List<Instruction> arguments)
    {
        if (arguments.Count != 1)
        {
            throw new Exception("Bad arguments, clean this up later");
        }
        
        var firstExpression = arguments[0].Interpret(memory);
        Console.WriteLine(StringUtilities.GetVariableAsString(memory, firstExpression));
    }
}