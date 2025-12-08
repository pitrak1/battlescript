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

    public IfInstruction(
        Instruction condition, 
        Instruction? next = null, 
        List<Instruction>? instructions = null, 
        int? line = null, 
        string? expression = null) : base(line, expression)
    {
        Condition = condition;
        Next = next;
        Instructions = instructions ?? [];
    }

    public override Variable? Interpret(
        CallStack callStack,
        Closure closure,
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var condition = Condition.Interpret(callStack, closure);
        if (Truthiness.IsTruthy(callStack, closure, condition, this))
        {
            foreach (var inst in Instructions)
            {
                inst.Interpret(callStack, closure);
            }
        }
        else if (Next is not null)
        {
            Next.Interpret(callStack, closure, instructionContext, objectContext, lexicalContext);
        }

        return null;
    }
}