namespace Battlescript;

public static class BuiltInCallable
{
    public static Variable Run(CallStack callStack, Closure closure, List<Instruction> arguments)
    {
        if (arguments.Count != 1)
            throw new ArgumentException("callable() takes exactly one argument");
    
        var variable = arguments[0].Interpret(callStack, closure);

        if (variable is ObjectVariable)
        {
            var result = variable.GetMember(callStack, closure, new MemberInstruction("__call__"));
            return BtlTypes.Create(BtlTypes.Types.Bool, result is not null);
        }
        else
        {
            return BtlTypes.Create(BtlTypes.Types.Bool, variable is FunctionVariable || variable is ClassVariable || variable is ObjectMethodPair);
        }
    }
}