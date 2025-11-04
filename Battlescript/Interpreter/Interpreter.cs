namespace Battlescript;

public class Interpreter(List<Instruction> instructions)
{
    private List<Instruction> _instructions = instructions;

    public CallStack Run(CallStack callStack, Closure closure)
    {
        foreach (var instruction in _instructions)
        {
            instruction.Interpret(callStack, closure);
        }
        
        return callStack;
    }
}