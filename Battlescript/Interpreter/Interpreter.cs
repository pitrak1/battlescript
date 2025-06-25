namespace Battlescript;

public class Interpreter(List<Instruction> instructions)
{
    private List<Instruction> _instructions = instructions;

    public Memory Run(Memory memory)
    {
        foreach (var instruction in _instructions)
        {
            instruction.Interpret(memory);
        }
        
        return memory;
    }
}