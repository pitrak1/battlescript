namespace Battlescript;

public class AssignmentInstruction : Instruction, IEquatable<AssignmentInstruction>
{
    public string Operation { get; set; }
    public Instruction Left { get; set; }
    public Instruction Right { get; set; }

    public AssignmentInstruction(List<Token> tokens) : base(tokens)
    {
        var assignmentIndex = InstructionUtilities.GetTokenIndex(
            tokens, 
            types: [Consts.TokenTypes.Assignment]
        );
        var assignmentToken = tokens[assignmentIndex];
        var operands = InstructionUtilities.ParseLeftAndRightAroundIndex(tokens, assignmentIndex);
        
        Operation = assignmentToken.Value;
        Left = operands.Left!;
        Right = operands.Right!;
    }

    public AssignmentInstruction(
        string operation, 
        Instruction left, 
        Instruction right, 
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
        if (Left is VariableInstruction left)
        {
            Operator.Assign(callStack, closure, Operation, left, Right, this);
        }
        else if (Left is ArrayInstruction { Delimiter: ArrayInstruction.DelimiterTypes.Comma } leftTuple && Operation == "=")
        {
            var rightVariable = Right.Interpret(callStack, closure);
            TupleUnpacker.UnpackTuple(callStack, closure, leftTuple, rightVariable!);
        }
        else if (Left is ConstantInstruction or NumericInstruction or StringInstruction)
        {
            throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "cannot assign to literal");
        }
        else if (Left is FunctionInstruction or LambdaInstruction or BuiltInInstruction)
        {
            throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "cannot assign to function call");
        }
        return null;
    }

    #region Equality

    public override bool Equals(object? obj) => obj is AssignmentInstruction inst && Equals(inst);

    public bool Equals(AssignmentInstruction? other) =>
        other is not null && Operation == other.Operation && Equals(Left, other.Left) && Equals(Right, other.Right);

    public override int GetHashCode() => HashCode.Combine(Operation, Left, Right);

    public static bool operator ==(AssignmentInstruction? left, AssignmentInstruction? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(AssignmentInstruction? left, AssignmentInstruction? right) => !(left == right);

    #endregion
}