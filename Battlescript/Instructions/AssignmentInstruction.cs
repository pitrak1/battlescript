namespace Battlescript;

public class AssignmentInstruction : Instruction
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

    public AssignmentInstruction(string operation, Instruction left, Instruction right) : base([])
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
        if (Left is VariableInstruction left)
        {
            Operator.Assign(callStack, Operation, left, Right, this);
        }
        else if (Left is ConstantInstruction or NumericInstruction or StringInstruction)
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "cannot assign to literal");
        }
        else if (Left is FunctionInstruction or LambdaInstruction or BuiltInInstruction)
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "cannot assign to function call");
        }
        return null;
    }
}