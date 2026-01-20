namespace Battlescript;

public class GlobalInstruction : Instruction, IEquatable<GlobalInstruction>
{
    public string Name { get; set; }

    public GlobalInstruction(List<Token> tokens) : base(tokens)
    {
        Name = tokens[1].Value;
    }

    public GlobalInstruction(string name, int? line = null, string? expression = null) : base(line, expression)
    {
        Name = name;
    }

    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        closure.CreateGlobalReference(Name);
        return null;
    }

    #region Equality

    public override bool Equals(object? obj) => obj is GlobalInstruction inst && Equals(inst);

    public bool Equals(GlobalInstruction? other) =>
        other is not null && Name == other.Name;

    public override int GetHashCode() => Name.GetHashCode();

    public static bool operator ==(GlobalInstruction? left, GlobalInstruction? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(GlobalInstruction? left, GlobalInstruction? right) => !(left == right);

    #endregion
}