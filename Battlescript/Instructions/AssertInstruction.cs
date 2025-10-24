namespace Battlescript;

public class AssertInstruction : Instruction
{
    public Instruction Condition { get; set; }
    public AssertInstruction(List<Token> tokens) : base(tokens)
    {
        // Want to ignore assert keyword at start
        Condition = InstructionFactory.Create(tokens.GetRange(1, tokens.Count - 1));
    }

    public AssertInstruction(Instruction condition) : base([])
    {
        Condition = condition;
    }
    
    public override Variable? Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var condition = Condition.Interpret(memory);
        
        if (!Truthiness.IsTruthy(memory, condition, this))
        {
            throw new InternalRaiseException(BsTypes.Types.AssertionError, "");
        }

        return null;
    }
}