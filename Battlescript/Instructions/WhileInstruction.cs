namespace Battlescript;

public class WhileInstruction : Instruction, IEquatable<WhileInstruction>
{
    public Instruction Condition { get; set; }

    public WhileInstruction(List<Token> tokens) : base(tokens)
    {
        CheckTokenValidity(tokens);
        var conditionTokens = tokens.GetRange(1, tokens.Count - 2);
        Condition = InstructionFactory.Create(conditionTokens)!;
    }

    private void CheckTokenValidity(List<Token> tokens)
    {
        if (tokens[^1].Value != ":")
        {
            throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "invalid syntax");
        }
    }

    public WhileInstruction(
        Instruction condition, 
        int? line = null, 
        string? expression = null) : base(line, expression)
    {
        Condition = condition;
    }

    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        var condition = Condition.Interpret(callStack, closure);
        while (Truthiness.IsTruthy(callStack, closure, condition, this))
        {
            try
            {
                foreach (var inst in Instructions)
                {
                    inst.Interpret(callStack, closure);
                }
            }
            catch (InternalContinueException)
            {
            }
            catch (InternalBreakException)
            {
                break;
            }

            condition = Condition.Interpret(callStack, closure);
        }

        return null;
    }
    
    #region Equality

    public override bool Equals(object? obj) => obj is WhileInstruction inst && Equals(inst);

    public bool Equals(WhileInstruction? other) =>
        other is not null &&
        Instructions.SequenceEqual(other.Instructions) &&
        Equals(Condition, other.Condition);

    public override int GetHashCode() => HashCode.Combine(Instructions, Condition);

    public static bool operator ==(WhileInstruction? left, WhileInstruction? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(WhileInstruction? left, WhileInstruction? right) => !(left == right);

    #endregion
}