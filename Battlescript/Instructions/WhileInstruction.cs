namespace Battlescript;

public class WhileInstruction : Instruction
{
    public Instruction Condition { get; set; }

    public WhileInstruction(List<Token> tokens) : base(tokens)
    {
        CheckTokenValidity(tokens);
        var conditionTokens = tokens.GetRange(1, tokens.Count - 2);
        Condition = InstructionFactory.Create(conditionTokens)!;
    }

    private void CheckTokenValidity(List<Token> tokens)
    {
        if (tokens[^1].Value != ":")
        {
            throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "invalid syntax");
        }
    }

    public WhileInstruction(
        Instruction condition, 
        int? line = null, 
        string? expression = null) : base(line, expression)
    {
        Condition = condition;
    }

    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        var condition = Condition.Interpret(callStack, closure);
        while (Truthiness.IsTruthy(callStack, closure, condition, this))
        {
            try
            {
                foreach (var inst in Instructions)
                {
                    inst.Interpret(callStack, closure);
                }
            }
            catch (InternalContinueException)
            {
            }
            catch (InternalBreakException)
            {
                break;
            }

            condition = Condition.Interpret(callStack, closure);
        }

        return null;
    }
    
    // All the code below is to override equality
    public override bool Equals(object? obj) => Equals(obj as WhileInstruction);
    public bool Equals(WhileInstruction? inst)
    {
        if (inst is null) return false;
        if (ReferenceEquals(this, inst)) return true;
        if (GetType() != inst.GetType()) return false;
        
        var instructionsEqual = Instructions.SequenceEqual(inst.Instructions);
        return instructionsEqual && Condition.Equals(inst.Condition);
    }
    
    public override int GetHashCode() => HashCode.Combine(Instructions, Condition);
}