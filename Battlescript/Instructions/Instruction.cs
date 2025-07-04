namespace Battlescript;

public abstract class Instruction(int line = 0, int column = 0) : IEquatable<Instruction>
{
    public int Line { get; set; } = line;
    public int Column { get; set; } = column;
    public List<Instruction> Instructions { get; set; } = [];
    
    public Instruction? Next { get; set; }

    // These three context are used for three distinct things:
    // - instructionContext is used for ongoing interpretations of a single instruction, i.e. a parens instruction
    // needs to know whether it's calling a function or class to be interpreted
    // - objectContext is used for class methods because the first argument to a method will always be `self`
    // - lexicalContext is used for keywords like `super` because we need to know in what class a method was actually
    // defined to find its superclass, the object is not enough
    public abstract Variable Interpret(
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

    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as Instruction);
    public bool Equals(Instruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;
        
        return Instructions.SequenceEqual(instruction.Instructions);
    }
    
    public override int GetHashCode() => HashCode.Combine(Instructions);
}