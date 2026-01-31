namespace Battlescript;

public class SpecialVariableInstruction : Instruction, IEquatable<SpecialVariableInstruction>
{
    public string Name { get; set; }
    public int Asterisks { get; set; }
    
    public SpecialVariableInstruction(List<Token> tokens) : base(tokens)
    {
        if (tokens.Count != 1)
        {
            throw new Exception($"Invalid number of arguments for SpecialVariableInstruction: {tokens.Count}");
        }

        var firstNonAsteriskIndex = tokens[0].Value.Length - tokens[0].Value.TrimStart('*').Length;

        Asterisks = firstNonAsteriskIndex;
        Name = tokens[0].Value[firstNonAsteriskIndex..];
    }

    #region Equality

    public override bool Equals(object? obj) => obj is SpecialVariableInstruction inst && Equals(inst);

    public bool Equals(SpecialVariableInstruction? other) =>
        other is not null && Name == other.Name && Asterisks == other.Asterisks;

    public override int GetHashCode() => HashCode.Combine(Name, Asterisks);

    public static bool operator ==(SpecialVariableInstruction? left, SpecialVariableInstruction? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(SpecialVariableInstruction? left, SpecialVariableInstruction? right) => !(left == right);

    #endregion
}