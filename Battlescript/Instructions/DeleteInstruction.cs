namespace Battlescript;

public class DeleteInstruction : Instruction, IEquatable<DeleteInstruction>
{
    public Instruction Target { get; set; }

    public DeleteInstruction(List<Token> tokens) : base(tokens)
    {
        var targetTokens = tokens.GetRange(1, tokens.Count - 1);
        Target = InstructionFactory.Create(targetTokens)!;
    }

    public DeleteInstruction(
        Instruction target,
        int? line = null,
        string? expression = null) : base(line, expression)
    {
        Target = target;
    }

    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        // Handle: del obj.attr (where obj is a variable with member access)
        if (Target is VariableInstruction variableInstruction)
        {
            if (variableInstruction.Next is MemberInstruction memberInstruction)
            {
                // del obj.attr or del obj.a.b
                var baseVar = closure.GetVariable(callStack, variableInstruction.Name);
                baseVar.DeleteMember(callStack, closure, memberInstruction, baseVar as ObjectVariable);
                return null;
            }
            else if (variableInstruction.Next is ArrayInstruction arrayInstruction)
            {
                // del obj[i] or del obj[i][j]
                var baseVar = closure.GetVariable(callStack, variableInstruction.Name);
                baseVar.DeleteItem(callStack, closure, arrayInstruction, baseVar as ObjectVariable);
                return null;
            }
            else
            {
                // del variable - delete the variable from scope
                closure.DeleteVariable(callStack, variableInstruction.Name);
                return null;
            }
        }

        throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "cannot delete this expression");
    }

    #region Equality

    public override bool Equals(object? obj) => obj is DeleteInstruction inst && Equals(inst);

    public bool Equals(DeleteInstruction? other) =>
        other is not null && Equals(Target, other.Target);

    public override int GetHashCode() => Target.GetHashCode();

    public static bool operator ==(DeleteInstruction? left, DeleteInstruction? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(DeleteInstruction? left, DeleteInstruction? right) => !(left == right);

    #endregion
}
