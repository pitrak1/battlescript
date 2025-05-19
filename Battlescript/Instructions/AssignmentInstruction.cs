namespace Battlescript;

public class AssignmentInstruction : Instruction, IEquatable<AssignmentInstruction>
{
    public string Operation { get; set; } 
    public Instruction Left { get; set; }
    public Instruction Right { get; set; }

    public AssignmentInstruction(List<Token> tokens)
    {
        var assignmentIndex = ParserUtilities.GetTokenIndex(
            tokens, 
            types: [Consts.TokenTypes.Assignment]
        );
        var assignmentToken = tokens[assignmentIndex];
        var operands = RunLeftAndRightAroundIndex(tokens, assignmentIndex);
        
        Operation = assignmentToken.Value;
        Left = operands.Left;
        Right = operands.Right;
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
        Variable? context = null, 
        Variable? objectContext = null)
    {
        var left = Left.Interpret(memory);
        var right = Right.Interpret(memory);
        
        var result = InterpreterUtilities.ConductAssignment(memory, Operation, left, right);

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
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as AssignmentInstruction);
    public bool Equals(AssignmentInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;

        if (Operation != instruction.Operation || Left != instruction.Left || Right != instruction.Right) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Operation, Left, Right, Instructions);
    public static bool operator ==(AssignmentInstruction left, AssignmentInstruction right) => left is null ? right is null : left.Equals(right);
    public static bool operator !=(AssignmentInstruction left, AssignmentInstruction right) => !(left == right);
}