namespace Battlescript;

public class OperationInstruction : Instruction, IEquatable<OperationInstruction>
{
    public string? Operation { get; set; } 
    public Instruction? Left { get; set; } 
    public Instruction? Right { get; set; }

    public OperationInstruction(List<Token> tokens)
    {
        var operatorIndex = ParserUtilities.GetOperatorIndex(tokens);
        var operatorToken = tokens[operatorIndex];
        var result = RunLeftAndRightAroundIndex(tokens, operatorIndex);

        Operation = operatorToken.Value;
        Left = result.Left;
        Right = result.Right;
        Line = operatorToken.Line;
        Column = operatorToken.Column;
    }

    public OperationInstruction(string operation, Instruction? left = null, Instruction? right = null)
    {
        Operation = operation;
        Left = left;
        Right = right;
    }

    public override Variable Interpret(        
        Memory memory, 
        Variable? context = null, 
        Variable? objectContext = null)
    {
        var left = Left?.Interpret(memory);
        var right = Right?.Interpret(memory);
        return InterpreterUtilities.ConductOperation(memory, Operation, left, right);
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as OperationInstruction);
    public bool Equals(OperationInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;

        if (Operation != instruction.Operation || Left != instruction.Left || Right != instruction.Right) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Operation, Left, Right, Instructions);
    public static bool operator ==(OperationInstruction left, OperationInstruction right) => left is null ? right is null : left.Equals(right);
    public static bool operator !=(OperationInstruction left, OperationInstruction right) => !(left == right);
}