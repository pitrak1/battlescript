namespace Battlescript;

public class VariableInstruction : Instruction, IEquatable<VariableInstruction>
{
    public string Name { get; set; }
    public int Asterisks { get; set; }

    public VariableInstruction(List<Token> tokens) : base(tokens)
    {
        if (tokens.Count == 0)
        {
            throw new Exception($"Invalid number of arguments for VariableInstruction: {tokens.Count}");
        }

        Asterisks = tokens[0].Value.Length - tokens[0].Value.TrimStart('*').Length;
        Name = Asterisks > 0 ? tokens[0].Value[Asterisks..] : tokens[0].Value;
        ParseNext(tokens, 1);
    }

    public VariableInstruction(
        string name,
        Instruction? next = null,
        int? line = null,
        string? expression = null,
        int asterisks = 0) : base(line, expression)
    {
        Name = name;
        Next = next;
        Asterisks = asterisks;
    }

    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        var variable = closure.GetVariable(callStack, Name);

        if (Next is null)
        {
            return variable;
        }
        return Next.Interpret(callStack, closure, variable);
    }

    #region Equality

    public override bool Equals(object? obj) => obj is VariableInstruction inst && Equals(inst);

    public bool Equals(VariableInstruction? other) =>
        other is not null && Name == other.Name && Asterisks == other.Asterisks;

    public override int GetHashCode() => HashCode.Combine(Name, Asterisks);

    public static bool operator ==(VariableInstruction? left, VariableInstruction? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(VariableInstruction? left, VariableInstruction? right) => !(left == right);

    #endregion
}