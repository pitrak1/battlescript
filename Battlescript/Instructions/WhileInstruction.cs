namespace Battlescript;

public class WhileInstruction : Instruction
{
    public Instruction Condition { get; set; }

    public WhileInstruction(List<Token> tokens)
    {
        if (tokens[^1].Value != ":")
        {
            throw new ParserMissingExpectedTokenException(tokens[^1], ":");
        }
    
        Condition = InstructionFactory.Create(tokens.GetRange(1, tokens.Count - 2));
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public WhileInstruction(Instruction condition)
    {
        Condition = condition;
    }

    public override Variable Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var condition = Condition.Interpret(memory);
        while (Truthiness.IsTruthy(memory, condition))
        {
            memory.AddScope();

            try
            {
                foreach (var inst in Instructions)
                {
                    inst.Interpret(memory);
                }
            }
            catch (InternalContinueException)
            {
            }
            catch (InternalBreakException)
            {
                memory.RemoveScope();
                break;
            }

            memory.RemoveScope();
            condition = Condition.Interpret(memory);
        }

        return new ConstantVariable();
    }
}