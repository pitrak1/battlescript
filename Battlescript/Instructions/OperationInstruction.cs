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
        var left = Left?.Interpret(callStack, closure);
        var right = Right?.Interpret(callStack, closure);
        return Operator.Operate(callStack, closure, Operation, left, right);
    }
    
    // All the code below is to override equality
    public override bool Equals(object? obj) => Equals(obj as OperationInstruction);
    public bool Equals(OperationInstruction? inst)
    {
        if (inst is null) return false;
        if (ReferenceEquals(this, inst)) return true;
        if (GetType() != inst.GetType()) return false;
        
        return Operation == inst.Operation && Equals(Left, inst.Left) && Equals(Right, inst.Right);
    }
    
    public override int GetHashCode()
    {
        var operationHash = Operation?.GetHashCode() * 3 ?? 98;
        var leftHash = Left?.GetHashCode() * 7 ?? 41; 
        var rightHash = Right?.GetHashCode() * 11 ?? 17;
       return operationHash + leftHash + rightHash;
    }
}