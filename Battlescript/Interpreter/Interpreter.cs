namespace Battlescript;

public class Interpreter(List<Instruction> instructions)
{
    private List<Instruction> _instructions = instructions;
    private Memory _memory = new ();

    public Memory Run()
    {
        foreach (var instruction in _instructions)
        {
            instruction.Interpret(_memory);
        }
        
        return _memory;
    }
}