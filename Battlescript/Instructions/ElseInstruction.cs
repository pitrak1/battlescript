namespace Battlescript;

public class ElseInstruction : Instruction, IEquatable<ElseInstruction>
{
    public Instruction? Condition { get; set; }

    public ElseInstruction(List<Token> tokens)
    {
        if (tokens[^1].Value != ":")
        {
            throw new ParserMissingExpectedTokenException(tokens[^1], ":");
        }

        if (tokens[0].Value == "elif")
        {
            Condition = InstructionFactory.Create(tokens.GetRange(1, tokens.Count - 2));
        }

        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public ElseInstruction(Instruction? condition, Instruction? next, List<Instruction>? instructions = null)
    {
        Condition = condition;
        Next = next;
        Instructions = instructions ?? [];
    }

    public override Variable Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        if (Condition is not null)
        {
            var condition = Condition.Interpret(memory);
            if (Truthiness.IsTruthy(memory, condition))
            {
                try
                {
                    memory.AddScope();
                    foreach (var inst in Instructions)
                    {
                        inst.Interpret(memory);
                    }

                    memory.RemoveScope();
                }
                catch (InternalReturnException e)
                {
                    memory.RemoveScope();
                    throw;
                }
                
            } else if (Next is not null)
            {
                return Next.Interpret(memory, instructionContext, objectContext, lexicalContext);
            }
        }
        else
        {
            try {
                memory.AddScope();
                foreach (var inst in Instructions)
                {
                    inst.Interpret(memory);
                }
                memory.RemoveScope();
            }
            catch (InternalReturnException e)
            {
                memory.RemoveScope();
                throw;
            }
        }
        
        return new ConstantVariable();
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as ElseInstruction);
    public bool Equals(ElseInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;
        
        if (Condition is not null && !Condition.Equals(instruction.Condition)) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Condition, Next, Instructions);
}