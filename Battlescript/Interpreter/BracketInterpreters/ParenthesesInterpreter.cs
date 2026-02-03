namespace Battlescript;

public class ParenthesesInterpreter : IBracketInterpreter
{
    public Variable? Interpret(CallStack callStack, Closure closure, ArrayInstruction instruction, Variable? context)
    {
        if (context is FunctionVariable functionVariable)
        {
            return InterpretFunctionCall(callStack, closure, instruction, functionVariable);
        }
        else if (context is ClassVariable classVariable)
        {
            return InterpretClassInstantiation(callStack, closure, instruction, classVariable);
        }
        else
        {
            return InterpretTuple(callStack, closure, instruction);
        }
    }

    private static Variable? InterpretFunctionCall(CallStack callStack, Closure closure, ArrayInstruction instruction, FunctionVariable functionVariable)
    {
        var result = functionVariable.RunFunction(callStack, closure, new ArgumentSet(callStack, closure, instruction.Values!), instruction);
        return instruction.InterpretNext(callStack, closure, result);
    }

    private static Variable? InterpretClassInstantiation(CallStack callStack, Closure closure, ArrayInstruction instruction, ClassVariable classVariable)
    {
        var objectVariable = classVariable.CreateObject();
        objectVariable.RunConstructor(callStack, closure, new ArgumentSet(callStack, closure, instruction.Values!), instruction);
        return instruction.InterpretNext(callStack, closure, objectVariable);
    }

    public static Variable? InterpretTuple(CallStack callStack, Closure closure, ArrayInstruction instruction)
    {
        Variable result;

        if (instruction.Values.Count > 1 || instruction.IsExplicitTuple)
        {
            var values = instruction.Values.Select(x => x!.Interpret(callStack, closure));
            result = BtlTypes.Create(BtlTypes.Types.Tuple, new SequenceVariable(values.ToList()));
        }
        else if (instruction.Values.Count == 1)
        {
            result = instruction.Values[0]!.Interpret(callStack, closure)!;
        }
        else
        {
            result = BtlTypes.Create(BtlTypes.Types.Tuple, new SequenceVariable());
        }

        return instruction.InterpretNext(callStack, closure, result);
    }
}
