namespace Battlescript;

public class AssignmentInstruction : Instruction
{
    public string Operation { get; set; } 
    public Instruction Left { get; set; }
    public Instruction Right { get; set; }

    public AssignmentInstruction(List<Token> tokens)
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
        Line = assignmentToken.Line;
        Column = assignmentToken.Column;
    }

    public AssignmentInstruction(string operation, Instruction left, Instruction right)
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
        var left = Left.Interpret(memory);
        var right = Right.Interpret(memory);
        
        var result = Operator.AssignmentOperation(memory, Operation, left, right);
        
        if (Left is VariableInstruction variableInst)
        {
            memory.SetVariable(variableInst, result);
            return result;
        }
        else
        {
            throw new Exception("Cannot assign to anything but variable");
        }
    }
}