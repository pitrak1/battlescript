using Microsoft.VisualBasic;

namespace Battlescript;

public static class BuiltInAbs
{
    public static Variable Run(CallStack callStack, Closure closure, List<Instruction> arguments)
    {
        CheckArguments(arguments);

        // This is kind of questionable.  Ultimately, if somebody is calling abs with something that isn't
        // an object, that means they're using the transition types directly, so I guess it's kinda on them.
        var value = arguments[0].Interpret(callStack, closure) as ObjectVariable;
        var absFunc = value.GetMember(callStack, closure, new MemberInstruction("__abs__"));
        
        if (absFunc is FunctionVariable funcVariable)
        {
            return funcVariable.RunFunction(callStack, closure, new ArgumentSet([]));
        }
        else
        {
            throw new InternalRaiseException(BtlTypes.Types.TypeError, $"bad operand type for abs(): '{value.Class.Name}'");
        }
    }

    private static void CheckArguments(List<Instruction> arguments)
    {
        if (arguments.Count != 1)
        {
            throw new InternalRaiseException(BtlTypes.Types.TypeError,
                $"abs() takes exactly one argument ({arguments.Count} given)");
        }
    }
}