namespace Battlescript;

public class VariableInstruction : Instruction, IEquatable<VariableInstruction>
{
    public string Name { get; set; }

    public VariableInstruction(List<Token> tokens) : base(tokens)
    {
        Name = tokens[0].Value;
        ParseNext(tokens, 1);
    }

    public VariableInstruction(
        string name,
        Instruction? next = null,
        int? line = null,
        string? expression = null) : base(line, expression)
    {
        Name = name;
        Next = next;
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
        other is not null && Name == other.Name;

    public override int GetHashCode() => Name.GetHashCode();

    public static bool operator ==(VariableInstruction? left, VariableInstruction? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(VariableInstruction? left, VariableInstruction? right) => !(left == right);

    #endregion
}