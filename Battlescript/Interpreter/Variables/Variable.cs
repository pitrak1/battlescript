namespace Battlescript;

public abstract class Variable
{
    public abstract bool SetItem(Memory memory, Variable valueVariable, SquareBracketsInstruction index, ObjectVariable? objectContext = null);
    
    public void SetItem(Memory memory, Variable valueVariable, string index, ObjectVariable? objectContext = null)
    {
        SetItem(memory, valueVariable, new SquareBracketsInstruction([new StringInstruction(index)]), objectContext);
    }
    
    public void SetItem(Memory memory, Variable valueVariable, int index, ObjectVariable? objectContext = null)
    {
        SetItem(memory, valueVariable, new SquareBracketsInstruction([new IntegerInstruction(index)]), objectContext);
    }
    
    public abstract Variable? GetItem(Memory memory, SquareBracketsInstruction index, ObjectVariable? objectContext = null);

    public Variable? GetItem(Memory memory, string index, ObjectVariable? objectContext = null)
    {
        return GetItem(memory, new SquareBracketsInstruction([new StringInstruction(index)]), objectContext);
    }
    
    public Variable? GetItem(Memory memory, int index, ObjectVariable? objectContext = null)
    {
        return GetItem(memory, new SquareBracketsInstruction([new IntegerInstruction(index)]), objectContext);
    }
}