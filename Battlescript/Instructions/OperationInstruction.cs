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

    public OperationInstruction(string operation, Instruction? left = null, Instruction? right = null) : base([])
    {
        Operation = operation;
        Left = left;
        Right = right;
    }

    public override Variable? Interpret(        
        CallStack callStack, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var left = Left?.Interpret(callStack);
        var right = Right?.Interpret(callStack);
        return Operator.Operate(callStack, Operation, left, right);
    }
}