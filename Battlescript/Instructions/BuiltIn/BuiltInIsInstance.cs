namespace Battlescript.BuiltIn;

public static class BuiltInIsInstance
{
    public static Variable Run(Memory memory, List<Instruction> arguments)
    {
        if (arguments.Count != 2)
        {
            throw new Exception("Bad arguments, clean this up later");
        }
        
        var objectExpression = arguments[0].Interpret(memory);
        if (arguments[1] is PrincipleTypeInstruction principleTypeInstruction)
        {
            switch (principleTypeInstruction.Value)
            {
                case "__numeric__":
                    return memory.Create(Memory.BsTypes.Bool,
                        objectExpression is NumericVariable);
                case "__sequence__":
                    return memory.Create(Memory.BsTypes.Bool, objectExpression is SequenceVariable);
                default:
                    return memory.Create(Memory.BsTypes.Bool, false);
            }
        }
        else
        {
            var classExpression = arguments[1].Interpret(memory);
        
            if (objectExpression is ObjectVariable objectVariable && classExpression is ClassVariable classVariable)
            {
                return memory.Create(Memory.BsTypes.Bool, objectVariable.IsInstance(classVariable));
            }
            else
            {
                return memory.Create(Memory.BsTypes.Bool, false);
            }
        }
    }
}