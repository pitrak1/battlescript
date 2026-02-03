namespace Battlescript;

public interface IBracketInterpreter
{
    Variable? Interpret(CallStack callStack, Closure closure, ArrayInstruction instruction, Variable? context);
}
