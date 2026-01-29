namespace Battlescript;

public static class BuiltInGetAttr
{
    public static Variable Run(CallStack callStack, Closure closure, List<Instruction> arguments)
    {
        if (arguments.Count != 2)
        {
            throw new Exception("wrong number of arguments");
        }
        
        var objectToSearch = arguments[0].Interpret(callStack, closure) as ObjectVariable;
        var attrNameObjectVariable = arguments[1].Interpret(callStack, closure);
        var name = BtlTypes.GetStringValue(attrNameObjectVariable);
        var result = objectToSearch.GetMember(callStack, closure, new MemberInstruction(name), objectToSearch);

        if (result is null)
        {
            throw new InternalRaiseException(BtlTypes.Types.AttributeError, $"cannot find attribute {name}");
        }

        return result;
    }
}