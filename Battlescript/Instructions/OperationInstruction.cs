namespace Battlescript;

public class OperationInstruction : Instruction
{
    public string? Operation { get; set; } 
    public Instruction? Left { get; set; } 
    public Instruction? Right { get; set; }

    public OperationInstruction(List<Token> tokens) : base(tokens)
    {
        var operatorIndex = InstructionUtilities.GetOperatorIndex(tokens);
        var operatorToken = tokens[operatorIndex];
        var result = InstructionUtilities.ParseLeftAndRightAroundIndex(tokens, operatorIndex);

        Operation = operatorToken.Value;
        Left = result.Left;
        Right = result.Right;
                
        if (result.Left is ParenthesesInstruction left)
        {
            Left = left.Values[0];
        }

        if (result.Right is ParenthesesInstruction right)
        {
            Right = right.Values[0];
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
        var left = Left?.Interpret(callStack, closure);
        var right = Right?.Interpret(callStack, closure);
        return Operator.Operate(callStack, closure, Operation, left, right);
    }
}