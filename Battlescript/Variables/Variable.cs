namespace Battlescript;

public abstract class Variable
{
    public abstract void AssignToIndexOrKey(Memory memory, Variable valueVariable, SquareBracketsInstruction index);
    
    public abstract Variable? GetIndex(Memory memory, SquareBracketsInstruction index);
}