namespace Battlescript.BuiltIn;

public static class BuiltInIsInstance
{
    public static Variable Run(CallStack callStack, List<Instruction> arguments)
    {
        if (arguments.Count != 2)
        {
            throw new Exception("Bad arguments, clean this up later");
        }
        
        var objectExpression = arguments[0].Interpret(callStack);
        if (arguments[1] is PrincipleTypeInstruction principleTypeInstruction)
        {
            switch (principleTypeInstruction.Value)
            {
                case "__numeric__":
                    return BsTypes.Create(BsTypes.Types.Bool,
                        objectExpression is NumericVariable);
                case "__sequence__":
                    return BsTypes.Create(BsTypes.Types.Bool, objectExpression is SequenceVariable);
                default:
                    return BsTypes.Create(BsTypes.Types.Bool, false);
            }
        }
        else
        {
            var classExpression = arguments[1].Interpret(callStack);
        
            if (objectExpression is ObjectVariable objectVariable && classExpression is ClassVariable classVariable)
            {
                return BsTypes.Create(BsTypes.Types.Bool, objectVariable.IsInstance(classVariable));
            }
            else
            {
                return BsTypes.Create(BsTypes.Types.Bool, false);
            }
        }
    }
}