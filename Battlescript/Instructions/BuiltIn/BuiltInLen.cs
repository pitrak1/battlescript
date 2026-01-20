namespace Battlescript.BuiltIn;

public static class BuiltInLen
{
    public static Variable Run(CallStack callStack, Closure closure, List<Instruction> arguments)
    {
        CheckArguments(arguments);
        var firstExpression = arguments[0].Interpret(callStack, closure);
        if (firstExpression is StringVariable stringVariable)
        {
            return BtlTypes.Create(BtlTypes.Types.Int, stringVariable.Value.Length);
        }
        else if (firstExpression is SequenceVariable sequenceVariable)
        {
            return BtlTypes.Create(BtlTypes.Types.Int, sequenceVariable.Values.Count);
        }
        else if (firstExpression is MappingVariable mappingVariable)
        {
            return BtlTypes.Create(BtlTypes.Types.Int,
                mappingVariable.IntValues.Count + mappingVariable.StringValues.Count > 0);
        }
        else if (firstExpression is ObjectVariable objectVariable)
        {
            var lenFunc = objectVariable.GetMember(callStack, closure, new MemberInstruction("__len__"));
            if (lenFunc is FunctionVariable funcVariable)
            {
                return funcVariable.RunFunction(callStack, closure, new ArgumentSet([]));
            }
            else
            {
                throw new Exception("Bad arguments, clean this up later");
            }
        }
        else
        {
            throw new Exception("Bad arguments, clean this up later");
        }
    }

    private static void CheckArguments(List<Instruction> arguments)
    {
        if (arguments.Count != 1)
        {
            throw new Exception("Bad arguments, clean this up later");
        }
    }
}