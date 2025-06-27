namespace Battlescript;

public class WhileInstruction : Instruction, IEquatable<WhileInstruction>
{
    public Instruction Condition { get; set; }

    public WhileInstruction(List<Token> tokens)
    {
        if (tokens[^1].Value != ":")
        {
            throw new ParserMissingExpectedTokenException(tokens[^1], ":");
        }
    
        Condition = Parse(tokens.GetRange(1, tokens.Count - 2));
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
        while (Truthiness.IsTruthy(condition))
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
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as WhileInstruction);
    public bool Equals(WhileInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;

        if (!Condition.Equals(instruction.Condition)) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Condition, Instructions);
}