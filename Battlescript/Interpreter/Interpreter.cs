namespace Battlescript;

public class Interpreter(List<Instruction> instructions)
{
    private List<Instruction> _instructions = instructions;
    private Memory _memory = new ();

    public List<Dictionary<string, Variable>> Run()
    {
        foreach (var instruction in _instructions)
        {
            instruction.Interpret(_memory);
        }

        // This should always be the base scope and have only one entry, but at some point we're going
        // to add breakpoints, and then that will change
        return _memory.GetScopes();
    }
}