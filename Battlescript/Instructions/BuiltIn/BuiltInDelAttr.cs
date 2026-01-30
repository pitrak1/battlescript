namespace Battlescript;

public static class BuiltInDelAttr
{
    public static void Run(CallStack callStack, Closure closure, List<Instruction> arguments)
    {
        if (arguments.Count != 2)
        {
            throw new InternalRaiseException(BtlTypes.Types.TypeError,
                $"delattr expected 2 arguments, got {arguments.Count}");
        }

        var obj = arguments[0].Interpret(callStack, closure) as ObjectVariable;
        var attrNameVariable = arguments[1].Interpret(callStack, closure)!;
        var name = BtlTypes.GetStringValue(attrNameVariable);

        obj!.DeleteMember(callStack, closure, new MemberInstruction(name), obj);
    }
}
