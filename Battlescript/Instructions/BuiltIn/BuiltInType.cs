namespace Battlescript.BuiltIn;

public static class BuiltInType
{
    public static Variable Run(CallStack callStack, Closure closure, List<Instruction> arguments)
    {
        CheckArguments(arguments);
        
        var value = arguments[0].Interpret(callStack, closure);
        switch (value)
        {
            case StringVariable:
                return BtlTypes.Create(BtlTypes.Types.String, "<class '__btl_string__'>");
            case MappingVariable:
                return BtlTypes.Create(BtlTypes.Types.String, "<class '__btl_mapping__'>");
            case NumericVariable:
                return BtlTypes.Create(BtlTypes.Types.String, "<class '__btl_numeric__'>");
            case SequenceVariable:
                return BtlTypes.Create(BtlTypes.Types.String, "<class '__btl_sequence__'>");
            case FunctionVariable:
                return BtlTypes.Create(BtlTypes.Types.String, "<class 'function'>");
            case ClassVariable:
                return BtlTypes.Create(BtlTypes.Types.String, "<class 'type'>");
            case ObjectVariable objectVariable:
                return BtlTypes.Create(BtlTypes.Types.String, $"<class '{objectVariable.Class.Name}'>");
            default:
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