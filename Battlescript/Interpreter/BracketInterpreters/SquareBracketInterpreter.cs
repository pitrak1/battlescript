namespace Battlescript;

public class SquareBracketInterpreter : IBracketInterpreter
{
    public Variable? Interpret(CallStack callStack, Closure closure, ArrayInstruction instruction, Variable? context)
    {
        // If square brackets follow something, it's an index. Otherwise, it's list creation
        if (context is not null)
        {
            return InterpretIndex(callStack, closure, instruction, context);
        }
        else
        {
            return InterpretListCreation(callStack, closure, instruction);
        }
    }

    private static Variable? InterpretIndex(CallStack callStack, Closure closure, ArrayInstruction instruction, Variable context)
    {
        return context.GetItem(callStack, closure, instruction, context as ObjectVariable);
    }

    private static Variable InterpretListCreation(CallStack callStack, Closure closure, ArrayInstruction instruction)
    {
        var interpretedValues = instruction.Values.Select(value =>
        {
            if (value is null)
            {
                throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "List elements cannot be null");
            }
            return value.Interpret(callStack, closure);
        });

        return BtlTypes.Create(BtlTypes.Types.List, interpretedValues.ToList());
    }
}
