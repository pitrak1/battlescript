namespace Battlescript;

public class ForInstruction : Instruction
{
    public VariableInstruction BlockVariable { get; set; }
    public Instruction Range { get; set; }

    public ForInstruction(List<Token> tokens) : base(tokens)
    {
        CheckTokenValidity(tokens);
        BlockVariable = new VariableInstruction(tokens[1].Value);
        Range = InstructionFactory.Create(tokens.GetRange(3, tokens.Count - 4));
    }

    private void CheckTokenValidity(List<Token> tokens)
    {
        if (tokens[^1].Value != ":")
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "invalid syntax");
        }

        if (tokens[2].Value != "in")
        {
            throw new ParserMissingExpectedTokenException(tokens[2], "in");
        }
    }

    public ForInstruction(
        VariableInstruction blockVariable, 
        Instruction range, 
        List<Instruction>? instructions = null, 
        int? line = null, 
        string? expression = null) : base(line, expression)
    {
        BlockVariable = blockVariable;
        Range = range;
        Instructions = instructions ?? [];
    }

    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        var range = Range.Interpret(callStack, closure) as ObjectVariable;

        if (BsTypes.Is(BsTypes.Types.List, range))
        {
            var values = (range.Values["__btl_value"] as SequenceVariable).Values;
            for (var i = 0; i < values.Count; i++)
            {
                closure.SetVariable(callStack, BlockVariable, values[i]);

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

    // All the code below is to override equality
    public override bool Equals(object? obj) => Equals(obj as ForInstruction);
    public bool Equals(ForInstruction? inst)
    {
        if (inst is null) return false;
        if (ReferenceEquals(this, inst)) return true;
        if (GetType() != inst.GetType()) return false;
        
        var instructionsEqual = Instructions.SequenceEqual(inst.Instructions);
        return instructionsEqual && 
               BlockVariable.Equals(inst.BlockVariable) && 
               Range.Equals(inst.Range) && 
               Equals(Next, inst.Next);
    }
    
    public override int GetHashCode()
    {
        int hash = 40;

        for (int i = 0; i < Instructions.Count; i++)
        {
            hash += Instructions[i].GetHashCode() * 22 * (i + 1);
        }

        var nextHash = Next?.GetHashCode() * 4 ?? 49;
        hash += BlockVariable.GetHashCode() * 39 + Range.GetHashCode() * 28 + nextHash;
        return hash;
    }
}