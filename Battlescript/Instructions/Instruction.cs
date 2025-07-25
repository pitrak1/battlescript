namespace Battlescript;

public abstract class Instruction
{
    public int? Line { get; set; }
    public string? FileName { get; set; }
    public string? Expression { get; set; }
    public List<Instruction> Instructions { get; set; }
    
    public Instruction? Next { get; set; }

    public Instruction(List<Token> tokens)
    {
        if (tokens.Count > 0)
        {
            Line = tokens[0].Line;
            FileName = tokens[0].FileName;
            Expression = tokens[0].Expression;
        }
        Instructions = [];
    }

    // These three context are used for three distinct things:
    // - instructionContext is used for ongoing interpretations of a single instruction, i.e. a parens instruction
    // needs to know whether it's calling a function or class to be interpreted
    // - objectContext is used for class methods because the first argument to a method will always be `self`
    // - lexicalContext is used for keywords like `super` because we need to know in what class a method was actually
    // defined to find its superclass, the object is not enough
    public abstract Variable? Interpret(
        Memory memory,
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null);

    protected void ParseNext(List<Token> tokens, int expectedTokenCount)
    {
        if (tokens.Count > expectedTokenCount)
        {
            Next = InstructionFactory.Create(tokens.GetRange(expectedTokenCount, tokens.Count - expectedTokenCount));
        }
    }
}