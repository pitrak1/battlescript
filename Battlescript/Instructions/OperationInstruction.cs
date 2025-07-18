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
        var result = ParserUtilities.ParseLeftAndRightAroundIndex(tokens, operatorIndex);

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
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
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

        if (Operation != instruction.Operation || !Right.Equals(instruction.Right)) return false;
        
        if (Left is not null && !Left.Equals(instruction.Left)) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Operation, Left, Right, Instructions);
}