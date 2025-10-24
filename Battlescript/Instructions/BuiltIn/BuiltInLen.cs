namespace Battlescript.BuiltIn;

public static class BuiltInLen
{
    public static Variable Run(Memory memory, List<Instruction> arguments)
    {
        if (arguments.Count != 1)
        {
            throw new Exception("Bad arguments, clean this up later");
        }
        
        var firstExpression = arguments[0].Interpret(memory);
        if (firstExpression is StringVariable stringVariable)
        {
            return BsTypes.Create(BsTypes.Types.Int, stringVariable.Value.Length);
        }
        else if (firstExpression is SequenceVariable sequenceVariable)
        {
            return BsTypes.Create(BsTypes.Types.Int, sequenceVariable.Values.Count);
        }
        else if (firstExpression is MappingVariable mappingVariable)
        {
            return BsTypes.Create(BsTypes.Types.Int,
                mappingVariable.IntValues.Count + mappingVariable.StringValues.Count > 0);
        }
        else if (firstExpression is ObjectVariable objectVariable)
        {
            var lenFunc = objectVariable.GetMember(memory, new MemberInstruction("__len__"));
            if (lenFunc is FunctionVariable funcVariable)
            {
                return funcVariable.RunFunction(memory, new ArgumentSet([objectVariable]));
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
}