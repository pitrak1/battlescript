using System.Diagnostics;

namespace Battlescript;

public class NonlocalInstruction : Instruction
{
    public string Name { get; set; } 

    public NonlocalInstruction(List<Token> tokens) : base(tokens)
    {
        Name = tokens[1].Value;
    }

    public NonlocalInstruction(string name, int? line = null, string? expression = null) : base(line, expression)
    {
        Name = name;
    }

    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        closure.CreateNonlocalReference(Name);
        return null;
    }
}