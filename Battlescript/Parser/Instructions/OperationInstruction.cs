namespace Battlescript;

public class OperationInstruction : Instruction
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

    public override Variable Interpret(Memory memory, Variable? context = null)
    {
        var left = Left?.Interpret(memory);
        var right = Right?.Interpret(memory);
        return InterpreterUtilities.ConductOperation(Operation, left, right);
    }
}