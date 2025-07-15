namespace Battlescript;

public class ContinueInstruction : Instruction
{
    public override Variable Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        throw new InternalContinueException();
    }
}