namespace Battlescript;

public abstract class Variable
{
    public abstract bool SetItem(Memory memory, Variable valueVariable, SquareBracketsInstruction index, ObjectVariable? objectContext = null);
    
    public void SetItem(Memory memory, Variable valueVariable, string index)
    {
        SetItem(memory, valueVariable, new SquareBracketsInstruction([new StringInstruction(index)]));
    }
    
    public void SetItem(Memory memory, Variable valueVariable, int index)
    {
        SetItem(memory, valueVariable, new SquareBracketsInstruction([new NumberInstruction(index)]));
    }
    
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