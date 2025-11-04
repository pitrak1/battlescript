namespace Battlescript;

public class BreakInstruction() : Instruction([])
{
    public override Variable? Interpret(
        CallStack callStack, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        throw new InternalBreakException();
    }
}