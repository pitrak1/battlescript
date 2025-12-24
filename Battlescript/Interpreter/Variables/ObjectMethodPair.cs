namespace Battlescript;

public class ObjectMethodPair(ObjectVariable obj, FunctionVariable method) : FunctionVariable(method)
{
    public ObjectVariable Object { get; set; } = obj;
    public override Variable RunFunction(CallStack callStack, Closure closure, ArgumentSet arguments, Instruction? inst = null)
    {
        var lineValue = inst?.Line ?? 0;
        var expressionValue = inst?.Expression ?? "";
        callStack.AddFrame(lineValue, expressionValue, Name);
        var newClosure = new Closure(FunctionClosure, this);
        arguments.Positionals.Insert(0, Object);
        arguments.ApplyToMemory(callStack, newClosure, Parameters);
        var returnValue = RunInstructions(callStack, newClosure);
        callStack.RemoveFrame();
        return returnValue ?? new ConstantVariable();
    }
}