namespace Battlescript;

public abstract class Variable
{
    public abstract void SetItem(Memory memory, Variable valueVariable, SquareBracketsInstruction index);
    
    public abstract Variable? GetItem(Memory memory, SquareBracketsInstruction index, ObjectVariable? objectContext = null);

    public Variable? GetItem(Memory memory, string index)
    {
        return GetItem(memory, new SquareBracketsInstruction([new StringInstruction(index)]));
    }
    
    public Variable? GetItem(Memory memory, int index)
    {
        return GetItem(memory, new SquareBracketsInstruction([new NumberInstruction(index)]));
    }
}