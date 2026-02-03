namespace Battlescript;

public class CurlyBraceInterpreter : IBracketInterpreter
{
    public Variable? Interpret(CallStack callStack, Closure closure, ArrayInstruction instruction, Variable? context)
    {
        var values = new MappingVariable();

        // A comma delimiter here means we have multiple kvps
        if (instruction.Delimiter == ArrayInstruction.DelimiterTypes.Comma)
        {
            foreach (var value in instruction.Values)
            {
                InterpretAndAddKvp(callStack, closure, values, value!);
            }
        }
        else
        {
            InterpretAndAddKvp(callStack, closure, values, instruction);
        }

        return BtlTypes.Create(BtlTypes.Types.Dictionary, values);
    }

    private static void InterpretAndAddKvp(CallStack callStack, Closure closure, MappingVariable values, Instruction? instruction)
    {
        if (instruction is not ArrayInstruction { Values.Count: > 0 }) return;

        ValidateKvp(instruction);
        var kvp = (ArrayInstruction)instruction;

        var (key, value) = InterpretKvp(callStack, closure, kvp);

        if (BtlTypes.Is(BtlTypes.Types.Int, key))
        {
            values.IntValues.Add(BtlTypes.GetIntValue(key), value);
        }
        else
        {
            values.StringValues.Add(BtlTypes.GetStringValue(key), value);
        }
    }

    private static void ValidateKvp(Instruction? instruction)
    {
        if (instruction is not ArrayInstruction { Delimiter: ArrayInstruction.DelimiterTypes.Colon, Values: [not null, not null] })
        {
            throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "Invalid dictionary key-value pair");
        }
    }

    private static (Variable key, Variable value) InterpretKvp(CallStack callStack, Closure closure, ArrayInstruction kvp)
    {
        var value = kvp.Values[1]!.Interpret(callStack, closure)!;
        var key = kvp.Values[0]!.Interpret(callStack, closure)!;
        return (key, value);
    }
}
