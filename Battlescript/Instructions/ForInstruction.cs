namespace Battlescript;

public class ForInstruction : Instruction
{
    public VariableInstruction BlockVariable { get; set; }
    public Instruction Range { get; set; }

    public ForInstruction(List<Token> tokens) : base(tokens)
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
    }

    public ForInstruction(VariableInstruction blockVariable, Instruction range) : base([])
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
        var range = Range.Interpret(memory) as ObjectVariable;

        if (memory.Is(Memory.BsTypes.List, range))
        {
            var values = (range.Values["__value"] as SequenceVariable).Values;
            for (var i = 0; i < values.Count; i++)
            {
                memory.AddScope();
                memory.AddVariableToLastScope(BlockVariable, values[i]);

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

}