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
            return memory.Create(Memory.BsTypes.Int, stringVariable.Value.Length);
        }
        else if (firstExpression is SequenceVariable sequenceVariable)
        {
            return memory.Create(Memory.BsTypes.Int, sequenceVariable.Values.Count);
        }
        else if (firstExpression is ObjectVariable objectVariable)
        {
            if (memory.Is(Memory.BsTypes.List, objectVariable))
            {
                var value = objectVariable.Values["__value"] as SequenceVariable;
                return memory.Create(Memory.BsTypes.Int, value.Values.Count);
            }
            throw new Exception("Bad arguments, clean this up later");
        }
        else
        {
            throw new Exception("Bad arguments, clean this up later");
        }
    }
}