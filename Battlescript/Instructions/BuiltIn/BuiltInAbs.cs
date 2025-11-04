using Microsoft.VisualBasic;

namespace Battlescript;

public static class BuiltInAbs
{
    public static Variable Run(CallStack callStack, Closure closure, List<Instruction> arguments)
    {
        if (arguments.Count != 1)
        {
            throw new InternalRaiseException(BsTypes.Types.TypeError,
                $"abs() takes exactly one argument ({arguments.Count} given)");
        }

        var value = arguments[0].Interpret(callStack, closure);
        if (value is ObjectVariable objectVariable)
        {
            var absFunc = objectVariable.GetMember(callStack, closure, new MemberInstruction("__abs__"));
            if (absFunc is FunctionVariable funcVariable)
            {
                var asdsf = funcVariable.RunFunction(callStack, closure, new ArgumentSet([value]));
                return asdsf;
            }
            else
            {
                throw new InternalRaiseException(BsTypes.Types.TypeError, $"bad operand type for abs(): '{objectVariable.Class.Name}'");
            }
        }
        else
        {
            var objectValue = value as ObjectVariable;
            throw new InternalRaiseException(BsTypes.Types.TypeError, $"bad operand type for abs(): '{objectValue.Class.Name}'");
        }
    }
}