using System.Diagnostics;

namespace Battlescript;

public class NonlocalInstruction : Instruction
{
    public string Name { get; set; } 

    public NonlocalInstruction(List<Token> tokens) : base(tokens)
    {
        Name = tokens[1].Value;
    }

    public NonlocalInstruction(string name) : base([])
    {
        Name = name;
    }

    public override Variable? Interpret(
        CallStack callStack,
        Closure closure,
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        closure.CreateNonlocalReference(Name);
        return null;
    }
}