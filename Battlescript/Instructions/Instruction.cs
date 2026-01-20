namespace Battlescript;

public abstract class Instruction
{
    public int Line { get; set; }
    public string Expression { get; set; }
    public List<Instruction> Instructions { get; set; }
    
    public Instruction? Next { get; set; }

    public Instruction(List<Token> tokens)
    {
        Initialize(tokens);
    }

    protected void Initialize(List<Token> tokens)
    {
        if (tokens.Count > 0)
        {
            Line = tokens[0].Line;
            Expression = tokens[0].Expression;
        }
        Instructions = [];
    }

    public Instruction(int? line = null, string? expression = null)
    {
        Line = line ?? 0;
        Expression = expression ?? "";
        Instructions = [];
    }
    
    public virtual Variable? Interpret(
        CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        throw new Exception($"Instructions of type {GetType().Name} are not meant to be interpreted directly");
    }

    protected void ParseNext(List<Token> tokens, int expectedTokenCount)
    {
        if (tokens.Count > expectedTokenCount)
        {
            Next = InstructionFactory.Create(tokens.GetRange(expectedTokenCount, tokens.Count - expectedTokenCount));
            
            // Next can only be a function call (), an index [], or a member .
            if (Next is not ArrayInstruction && Next is not MemberInstruction)
            {
                throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "invalid syntax");
            }
        }
    }
}