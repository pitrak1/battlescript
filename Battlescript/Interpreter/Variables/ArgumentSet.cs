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
            if (argument is AssignmentInstruction assignmentInstruction)
            {
                var keywordName = ((VariableInstruction)assignmentInstruction.Left).Name;
                Keywords[keywordName] = assignmentInstruction.Right.Interpret(callStack, closure);
            }
            else
            {
                if (Keywords.Count > 0)
                {
                    throw new InterpreterKeywordArgBeforePositionalArgException();
                }

                var value = argument.Interpret(callStack, closure);
                Positionals.Add(value);
            }
        }
    }

    public ArgumentSet(List<Variable> arguments)
    {
        // If we're given a list of variables instead of instructions, just treat them as positional arguments
        Positionals = arguments;
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
