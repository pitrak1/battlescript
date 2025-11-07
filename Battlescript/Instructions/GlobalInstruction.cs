using System.Diagnostics;

namespace Battlescript;

public class GlobalInstruction : Instruction
{
    public string Name { get; set; } 

    public GlobalInstruction(List<Token> tokens) : base(tokens)
    {
        Name = tokens[1].Value;
    }

    public GlobalInstruction(string name) : base([])
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
        closure.CreateGlobalReference(Name);
        return null;
    }
}