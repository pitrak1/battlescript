namespace Battlescript;

public class NonlocalInstruction : Instruction, IEquatable<NonlocalInstruction>
{
    public string Name { get; set; }

    public NonlocalInstruction(List<Token> tokens) : base(tokens)
    {
        Name = tokens[1].Value;
    }

    public NonlocalInstruction(string name, int? line = null, string? expression = null) : base(line, expression)
    {
        Name = name;
    }

    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        closure.CreateNonlocalReference(Name);
        return null;
    }

    #region Equality

    public override bool Equals(object? obj) => obj is NonlocalInstruction inst && Equals(inst);

    public bool Equals(NonlocalInstruction? other) =>
        other is not null && Name == other.Name;

    public override int GetHashCode() => Name.GetHashCode();

    public static bool operator ==(NonlocalInstruction? left, NonlocalInstruction? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(NonlocalInstruction? left, NonlocalInstruction? right) => !(left == right);

    #endregion
}