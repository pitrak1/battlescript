namespace Battlescript;

public class ObjectMethodPair(ObjectVariable obj, FunctionVariable method) : FunctionVariable(method)
{
    public ObjectVariable Object { get; set; } = obj;
    public override Variable RunFunction(CallStack callStack, Closure closure, ArgumentSet arguments, Instruction? inst = null)
    {
        arguments.Positionals.Insert(0, Object);
        return base.RunFunction(callStack, closure, arguments, inst);
    }
}