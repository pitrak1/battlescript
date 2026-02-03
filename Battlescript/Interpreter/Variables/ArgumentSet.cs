namespace Battlescript;

public class ArgumentSet : IEquatable<ArgumentSet>
{
    public List<Variable> Positionals { get; set; } = [];
    public Dictionary<string, Variable> Keywords { get; set; } = [];

    public ArgumentSet(CallStack callStack, Closure closure, List<Instruction> arguments, ObjectVariable? selfObject = null)
    {
        if (selfObject is not null)
        {
            Positionals.Add(selfObject);
        }

        foreach (var argument in arguments)
        {
            ProcessArgument(callStack, closure, argument);
        }
    }

    public ArgumentSet(List<Variable> arguments)
    {
        // If we're given a list of variables instead of instructions, just treat them as positional arguments
        Positionals = arguments;
    }

    private void ProcessArgument(CallStack callStack, Closure closure, Instruction argument)
    {
        switch (argument)
        {
            case AssignmentInstruction assignment:
                ProcessKeywordArgument(callStack, closure, assignment);
                break;
            // Note: SpecialVariableInstruction must be checked before VariableInstruction
            // since SpecialVariableInstruction inherits from VariableInstruction
            case SpecialVariableInstruction special:
                ProcessSpecialVariable(callStack, closure, special);
                break;
            default:
                ProcessPositionalArgument(callStack, closure, argument);
                break;
        }
    }

    private void ProcessKeywordArgument(CallStack callStack, Closure closure, AssignmentInstruction assignment)
    {
        var keywordName = ((VariableInstruction)assignment.Left).Name;
        Keywords[keywordName] = assignment.Right.Interpret(callStack, closure);
    }

    private void ProcessSpecialVariable(CallStack callStack, Closure closure, SpecialVariableInstruction special)
    {
        switch (special.Asterisks)
        {
            case 1:
                UnpackArgsVariable(callStack, closure, special);
                break;
            case 2:
                UnpackKwargsVariable(callStack, closure, special);
                break;
            default:
                throw new Exception("Invalid number of asterisks");
        }
    }

    private void UnpackArgsVariable(CallStack callStack, Closure closure, SpecialVariableInstruction special)
    {
        var args = special.Interpret(callStack, closure);

        if (!BtlTypes.Is(BtlTypes.Types.Tuple, args) && !BtlTypes.Is(BtlTypes.Types.List, args))
        {
            var typeName = args is ObjectVariable obj ? obj.Class.Name : "unknown";
            throw new InternalRaiseException(BtlTypes.Types.TypeError,
                $"argument after * must be an iterable, not {typeName}");
        }

        var sequence = BtlTypes.Is(BtlTypes.Types.Tuple, args)
            ? BtlTypes.GetTupleValue(args)
            : BtlTypes.GetListValue(args);

        foreach (var value in sequence.Values)
        {
            Positionals.Add(value);
        }
    }

    private void UnpackKwargsVariable(CallStack callStack, Closure closure, SpecialVariableInstruction special)
    {
        var kwargs = special.Interpret(callStack, closure);

        if (!BtlTypes.Is(BtlTypes.Types.Dictionary, kwargs))
        {
            var typeName = kwargs is ObjectVariable obj ? obj.Class.Name : "unknown";
            throw new InternalRaiseException(BtlTypes.Types.TypeError,
                $"argument after ** must be a mapping, not {typeName}");
        }

        var mapping = BtlTypes.GetDictValue(kwargs);
        foreach (var (key, value) in mapping.StringValues)
        {
            Keywords.Add(key, value);
        }
    }

    private void ProcessPositionalArgument(CallStack callStack, Closure closure, Instruction argument)
    {
        if (Keywords.Count > 0)
        {
            throw new InterpreterKeywordArgBeforePositionalArgException();
        }
        Positionals.Add(argument.Interpret(callStack, closure));
    }

    #region Equality

    public override bool Equals(object? obj) => obj is ArgumentSet other && Equals(other);

    public bool Equals(ArgumentSet? other) =>
        other is not null &&
        Positionals.SequenceEqual(other.Positionals) &&
        Keywords.OrderBy(kvp => kvp.Key).SequenceEqual(other.Keywords.OrderBy(kvp => kvp.Key));

    public override int GetHashCode() => HashCode.Combine(Positionals, Keywords);

    public static bool operator ==(ArgumentSet? left, ArgumentSet? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(ArgumentSet? left, ArgumentSet? right) => !(left == right);

    #endregion
}
