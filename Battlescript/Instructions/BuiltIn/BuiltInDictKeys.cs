namespace Battlescript;

public static class BuiltInDictKeys
{
    public static Variable Run(CallStack callStack, Closure closure, List<Instruction> arguments)
    {
        if (arguments.Count != 1)
        {
            throw new InternalRaiseException(BtlTypes.Types.TypeError,
                $"__btl_dict_keys__ expected 1 argument, got {arguments.Count}");
        }

        var dictObj = arguments[0].Interpret(callStack, closure) as ObjectVariable;
        var mappingVar = dictObj!.Values["__btl_value"] as MappingVariable;

        var keys = new List<Variable?>();

        foreach (var key in mappingVar!.IntValues.Keys)
        {
            keys.Add(BtlTypes.Create(BtlTypes.Types.Int, key));
        }

        foreach (var key in mappingVar.StringValues.Keys)
        {
            keys.Add(BtlTypes.Create(BtlTypes.Types.String, key));
        }

        return BtlTypes.Create(BtlTypes.Types.List, keys);
    }
}
