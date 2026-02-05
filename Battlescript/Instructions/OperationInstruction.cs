using System.Collections.Frozen;

namespace Battlescript;

public class OperationInstruction : Instruction, IEquatable<OperationInstruction>
{
    public string? Operation { get; set; }
    public Instruction? Left { get; set; }
    public Instruction? Right { get; set; }

    public OperationInstruction(List<Token> tokens) : base(tokens)
    {
        var operatorIndex = InstructionUtilities.GetLowestPriorityOperatorIndex(tokens);
        var operatorToken = tokens[operatorIndex];
        var result = InstructionUtilities.ParseLeftAndRightAroundIndex(tokens, operatorIndex);

        Operation = operatorToken.Value;
        Left = GetValueWithOrWithoutParens(result.Left);
        Right = GetValueWithOrWithoutParens(result.Right);
    }

    private Instruction? GetValueWithOrWithoutParens(Instruction? inst)
    {
        if (inst is ArrayInstruction { Bracket: ArrayInstruction.BracketTypes.Parentheses } array)
        {
            return array.Values[0];
        }
        else
        {
            return inst;
        }
    }

    public OperationInstruction(
        string operation,
        Instruction? left = null,
        Instruction? right = null,
        int? line = null,
        string? expression = null) : base(line, expression)
    {
        Operation = operation;
        Left = left;
        Right = right;
    }

    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        if (BooleanOperators.Contains(Operation))
        {
            return Operator.BooleanOperate(callStack, closure, Operation, Left, Right);
        }
        var left = Left?.Interpret(callStack, closure, instructionContext);
        var right = Right?.Interpret(callStack, closure, instructionContext);
        return Operator.Operate(callStack, closure, Operation, left, right);
    }
    
    private static readonly FrozenSet<string> BooleanOperators =
        FrozenSet.ToFrozenSet(["and", "or", "not", "is", "is not", "in", "not in"]);

    #region Equality

    public override bool Equals(object? obj) => obj is OperationInstruction inst && Equals(inst);

    public bool Equals(OperationInstruction? other) =>
        other is not null && Operation == other.Operation && Equals(Left, other.Left) && Equals(Right, other.Right);

    public override int GetHashCode() => HashCode.Combine(Operation, Left, Right);

    public static bool operator ==(OperationInstruction? left, OperationInstruction? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(OperationInstruction? left, OperationInstruction? right) => !(left == right);

    #endregion
}