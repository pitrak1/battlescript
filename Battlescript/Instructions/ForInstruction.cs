namespace Battlescript;

public class ForInstruction : Instruction
{
    public VariableInstruction BlockVariable { get; set; }
    public Instruction Range { get; set; }

    public ForInstruction(List<Token> tokens) : base(tokens)
    {
        if (tokens[^1].Value != ":")
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "invalid syntax");
        }

        if (tokens[2].Value != "in")
        {
            throw new ParserMissingExpectedTokenException(tokens[2], "in");
        }
        
        BlockVariable = new VariableInstruction(tokens[1].Value);
        Range = InstructionFactory.Create(tokens.GetRange(3, tokens.Count - 4));
    }

    public ForInstruction(VariableInstruction blockVariable, Instruction range, List<Instruction>? instructions = null) : base([])
    {
        BlockVariable = blockVariable;
        Range = range;
        Instructions = instructions ?? [];
    }

    public override Variable? Interpret(
        CallStack callStack,
        Closure closure,
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var range = Range.Interpret(callStack, closure) as ObjectVariable;

        if (BsTypes.Is(BsTypes.Types.List, range))
        {
            var values = (range.Values["__value"] as SequenceVariable).Values;
            for (var i = 0; i < values.Count; i++)
            {
                callStack.SetVariable(closure, BlockVariable, values[i]);

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
            }
            return null;
        }
        else
        {
            throw new Exception("Invalid iterator for loop, fix this later");
        }
    }

}