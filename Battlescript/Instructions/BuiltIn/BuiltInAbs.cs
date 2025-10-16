using Microsoft.VisualBasic;

namespace Battlescript;

public static class BuiltInAbs
{
    public static Variable Run(Memory memory, List<Instruction> arguments)
    {
        if (arguments.Count != 1)
        {
            throw new InternalRaiseException(Memory.BsTypes.TypeError,
                $"abs() takes exactly one argument ({arguments.Count} given)");
        }

        var value = arguments[0].Interpret(memory);
        if (value is ObjectVariable objectVariable)
        {
            var absFunc = objectVariable.GetMember(memory, new MemberInstruction("__abs__"));
            if (absFunc is FunctionVariable funcVariable)
            {
                var asdsf = funcVariable.RunFunction(memory, new ArgumentSet([value]));
                return asdsf;
            }
            else
            {
                throw new InternalRaiseException(Memory.BsTypes.TypeError, $"bad operand type for abs(): '{objectVariable.Class.Name}'");
            }
        }
        else
        {
            var objectValue = value as ObjectVariable;
            throw new InternalRaiseException(Memory.BsTypes.TypeError, $"bad operand type for abs(): '{objectValue.Class.Name}'");
        }
    }
}