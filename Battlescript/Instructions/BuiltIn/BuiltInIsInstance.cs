namespace Battlescript;

public static class BuiltInIsInstance
{
    public static Variable Run(CallStack callStack, Closure closure, List<Instruction> arguments)
    {
        if (arguments.Count != 2)
        {
            throw new Exception("wrong number of arguments");
        }
        
        var objectExpression = arguments[0].Interpret(callStack, closure);
        var classExpression = arguments[1].Interpret(callStack, closure);
        
        if (classExpression is BindingVariable bindingClass)
        {
            var boolValue = bindingClass.Value switch
            {
                "__btl_numeric__" => objectExpression is NumericVariable,
                "__btl_sequence__" => objectExpression is SequenceVariable,
                "__btl_mapping__" => objectExpression is MappingVariable,
                "__btl_string__" => objectExpression is StringVariable,
                _ => false
            };
            return BtlTypes.Create(BtlTypes.Types.Bool, boolValue);
        }
        else
        {
            if (objectExpression is ObjectVariable objectVariable && classExpression is ClassVariable classVariable)
            {
                return BtlTypes.Create(BtlTypes.Types.Bool, objectVariable.IsInstance(classVariable));
            } 
            return BtlTypes.Create(BtlTypes.Types.Bool, false);
        }
    }
}