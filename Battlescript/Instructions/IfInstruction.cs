namespace Battlescript;

public class IfInstruction : Instruction
{
    public Instruction Condition { get; set; }

    public IfInstruction(List<Token> tokens) : base(tokens)
    {
        if (tokens[^1].Value != ":")
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "invalid syntax");
        }

        Condition = InstructionFactory.Create(tokens.GetRange(1, tokens.Count - 2));
    }

    public IfInstruction(Instruction condition, Instruction? next = null, List<Instruction>? instructions = null) : base([])
    {
        Condition = condition;
        Next = next;
        Instructions = instructions ?? [];
    }

    public override Variable? Interpret(
        CallStack callStack, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var condition = Condition.Interpret(callStack);
        if (Truthiness.IsTruthy(callStack, condition, this))
        {
            foreach (var inst in Instructions)
            {
                inst.Interpret(callStack);
            }
        }
        else if (Next is not null)
        {
            Next.Interpret(callStack, instructionContext, objectContext, lexicalContext);
        }

        return null;
    }
}