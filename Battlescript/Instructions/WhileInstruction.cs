namespace Battlescript;

public class WhileInstruction : Instruction
{
    public Instruction Condition { get; set; }

    public WhileInstruction(List<Token> tokens) : base(tokens)
    {
        if (tokens[^1].Value != ":")
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "invalid syntax");
        }
    
        Condition = InstructionFactory.Create(tokens.GetRange(1, tokens.Count - 2));
    }

    public WhileInstruction(Instruction condition) : base([])
    {
        Condition = condition;
    }

    public override Variable? Interpret(
        CallStack callStack, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var condition = Condition.Interpret(callStack);
        while (Truthiness.IsTruthy(callStack, condition, this))
        {
            try
            {
                foreach (var inst in Instructions)
                {
                    inst.Interpret(callStack);
                }
            }
            catch (InternalContinueException)
            {
            }
            catch (InternalBreakException)
            {
                break;
            }

            condition = Condition.Interpret(callStack);
        }

        return null;
    }
}