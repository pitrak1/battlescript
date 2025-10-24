namespace Battlescript.BuiltIn;

public static class BuiltInType
{
    public static Variable Run(Memory memory, List<Instruction> arguments)
    {
        if (arguments.Count != 1)
        {
            throw new Exception("Bad arguments, clean this up later");
        }
        
        var firstExpression = arguments[0].Interpret(memory);
        switch (firstExpression)
        {
            case StringVariable:
                return BsTypes.Create(BsTypes.Types.String, "<class '__string__'>");
            case MappingVariable:
                return BsTypes.Create(BsTypes.Types.String, "<class '__mapping__'>");
            case NumericVariable:
                return BsTypes.Create(BsTypes.Types.String, "<class '__numeric__'>");
            case SequenceVariable:
                return BsTypes.Create(BsTypes.Types.String, "<class '__sequence__'>");
            case FunctionVariable:
                return BsTypes.Create(BsTypes.Types.String, "<class 'function'>");
            case ClassVariable:
                return BsTypes.Create(BsTypes.Types.String, "<class 'type'>");
            case ObjectVariable objectVariable:
                var className = objectVariable.Class.Name;
                return BsTypes.Create(BsTypes.Types.String, $"<class '{className}'>");
            default:
                throw new Exception("Bad arguments, clean this up later");
        }
    }
}