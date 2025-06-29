namespace Battlescript;

public class ForInstruction : Instruction, IEquatable<ForInstruction>
{
    public VariableInstruction BlockVariable { get; set; }
    public Instruction Range { get; set; }

    public ForInstruction(List<Token> tokens)
    {
        if (tokens[^1].Value != ":")
        {
            throw new ParserMissingExpectedTokenException(tokens[^1], ":");
        }

        if (tokens[2].Value != "in")
        {
            throw new ParserMissingExpectedTokenException(tokens[2], "in");
        }
        
        BlockVariable = new VariableInstruction(tokens[1].Value);
        Range = InstructionFactory.Create(tokens.GetRange(3, tokens.Count - 4));
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public ForInstruction(VariableInstruction blockVariable, Instruction range)
    {
        BlockVariable = blockVariable;
        Range = range;
    }

    public override Variable Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var range = Range.Interpret(memory);

        if (range is ListVariable rangeList)
        {
            for (var i = 0; i < rangeList.Values.Count; i++)
            {
                memory.AddScope();
                memory.AddVariableToLastScope(BlockVariable, rangeList.Values[i]);

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
            }
            return new ConstantVariable();
        }
        else
        {
            throw new Exception("Invalid iterator for loop, fix this later");
        }
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as ForInstruction);
    public bool Equals(ForInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;

        if (!Range.Equals(instruction.Range)) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Range, Instructions);
}