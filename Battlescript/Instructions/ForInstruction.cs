namespace Battlescript;

public class ForInstruction : Instruction, IBlockInstruction, IEquatable<ForInstruction>
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
            throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "invalid syntax");
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

        if (!BtlTypes.Is(BtlTypes.Types.List, range!))
        {
            throw new Exception("Invalid iterator for loop, fix this later");
        }
        
        var values = BtlTypes.GetListValue(range!).Values;
        foreach (var t in values)
        {
            closure.SetVariable(callStack, BlockVariable, t!);

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

    #region Equality

    public override bool Equals(object? obj) => obj is ForInstruction inst && Equals(inst);

    public bool Equals(ForInstruction? other) =>
        other is not null &&
        Instructions.SequenceEqual(other.Instructions) &&
        Equals(BlockVariable, other.BlockVariable) &&
        Equals(Range, other.Range) &&
        Equals(Next, other.Next);

    public override int GetHashCode() => HashCode.Combine(Instructions, BlockVariable, Range, Next);

    public static bool operator ==(ForInstruction? left, ForInstruction? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(ForInstruction? left, ForInstruction? right) => !(left == right);

    #endregion
}