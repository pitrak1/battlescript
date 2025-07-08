namespace Battlescript;

public abstract class Variable
{
    public Consts.VariableTypes Type { get; protected set; } = Consts.VariableTypes.Value;
    
    public virtual void SetItem(
        Memory memory, 
        Variable value, 
        ArrayInstruction index,
        ObjectVariable? objectContext = null)
    {
        var result = SetItemDirectly(memory, value, index, objectContext);
        if (index.Next is not null)
        {
            result.SetItemDirectly(memory, value, index.Next as ArrayInstruction, objectContext);
        }
    }
    
    public void SetItem(Memory memory, Variable value, string index, ObjectVariable? objectContext = null)
    {
        SetItem(memory, value, new ArrayInstruction([new StringInstruction(index)]), objectContext);
    }
    
    public void SetItem(Memory memory, Variable value, int index, ObjectVariable? objectContext = null)
    {
        SetItem(memory, value, new ArrayInstruction([new NumericInstruction(index)]), objectContext);
    }
    
    public virtual Variable? SetItemDirectly(Memory memory, Variable value, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        throw new InterpreterInvalidIndexException(this);
    }

    public Variable? GetItem(
        Memory memory,
        ArrayInstruction index,
        ObjectVariable? objectContext = null)
    {
        var result = GetItemDirectly(memory, index, objectContext);
        if (index.Next is not null)
        {
            return index.Next.Interpret(memory, result, objectContext);
        }
        else
        {
            return result;
        }
    }

    public Variable? GetItem(Memory memory, string index, ObjectVariable? objectContext = null)
    {
        return GetItem(memory, new ArrayInstruction([new StringInstruction(index)]), objectContext);
    }
    
    public Variable? GetItem(Memory memory, int index, ObjectVariable? objectContext = null)
    {
        return GetItem(memory, new ArrayInstruction([new NumericInstruction(index)]), objectContext);
    }

    public virtual Variable? GetItemDirectly(Memory memory, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        throw new InterpreterInvalidIndexException(this);
    }
}