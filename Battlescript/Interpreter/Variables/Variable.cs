namespace Battlescript;

public abstract class Variable
{
    public abstract void AssignToIndexOrKey(Memory memory, Variable valueVariable, SquareBracketsInstruction index);
    
    public abstract Variable? GetIndex(Memory memory, SquareBracketsInstruction index);

    public virtual Variable? GetIndex(Memory memory, string index)
    {
        return GetIndex(memory, new SquareBracketsInstruction([new StringInstruction(index)]));
    }
    
    public virtual Variable? GetIndex(Memory memory, int index)
    {
        return GetIndex(memory, new SquareBracketsInstruction([new NumberInstruction(index)]));
    }
}