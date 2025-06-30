namespace Battlescript;

public abstract class Variable
{
    public Consts.VariableTypes Type { get; protected set; } = Consts.VariableTypes.Value;
    
    public virtual bool SetItem(
        Memory memory, 
        Variable valueVariable, 
        ArrayInstruction index,
        ObjectVariable? objectContext = null)
    {
        throw new InterpreterInvalidIndexException(this);
    }
    
    public virtual void SetItem(
        Memory memory, 
        Variable valueVariable, 
        string index, 
        ObjectVariable? objectContext = null)
    {
        SetItem(memory, valueVariable, new ArrayInstruction([new StringInstruction(index)]), objectContext);
    }
    
    public virtual void SetItem(Memory memory, Variable valueVariable, int index, ObjectVariable? objectContext = null)
    {
        SetItem(memory, valueVariable, new ArrayInstruction([new IntegerInstruction(index)]), objectContext);
    }

    public virtual Variable? GetItem(
        Memory memory,
        ArrayInstruction index,
        ObjectVariable? objectContext = null)
    {
        throw new InterpreterInvalidIndexException(this);
    }

    public Variable? GetItem(Memory memory, string index, ObjectVariable? objectContext = null)
    {
        return GetItem(memory, new ArrayInstruction([new StringInstruction(index)]), objectContext);
    }
    
    public Variable? GetItem(Memory memory, int index, ObjectVariable? objectContext = null)
    {
        return GetItem(memory, new ArrayInstruction([new IntegerInstruction(index)]), objectContext);
    }
}