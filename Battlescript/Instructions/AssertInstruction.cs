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
        CallStack callStack,
        Closure closure,
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var condition = Condition.Interpret(callStack, closure);
        
        if (!Truthiness.IsTruthy(callStack, closure, condition, this))
        {
            throw new InternalRaiseException(BsTypes.Types.AssertionError, "");
        }

        return null;
    }
}