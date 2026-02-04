namespace Battlescript;

public class ForInstruction : Instruction, IBlockInstruction, IEquatable<ForInstruction>
{
    public Instruction BlockVariable { get; set; }
    public Instruction Range { get; set; }

    public ForInstruction(List<Token> tokens) : base(tokens)
    {
        if (tokens[^1].Value != ":")
        {
            throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "invalid syntax");
        }
        
        var inIndex = InstructionUtilities.GetTokenIndex(tokens, ["in"]);

        if (inIndex == -1)
        {
            throw new ParserMissingExpectedTokenException(tokens[2], "in");
        }

        BlockVariable = InstructionFactory.Create(tokens[1..inIndex]);
        var tokensInIterableExpression = tokens.GetRange(inIndex + 1, tokens.Count - inIndex - 2);
        Range = InstructionFactory.Create(tokensInIterableExpression);
    }
    
    public ForInstruction(
        Instruction blockVariable, 
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
        var iterable = Range.Interpret(callStack, closure)!;

        // Get __next__ method from iterator
        var nextMethod = BtlTypes.GetIteratorNext(callStack, closure, iterable, Range);

        while (true)
        {
            Variable? value;
            try
            {
                value = nextMethod.RunFunction(callStack, closure, new ArgumentSet([]), Range);
            }
            catch (InternalRaiseException ex) when (ex.Type == "StopIteration")
            {
                break;
            }

            closure.SetVariable(callStack, BlockVariable, value!);

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