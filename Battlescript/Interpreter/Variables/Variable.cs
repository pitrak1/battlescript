namespace Battlescript;

public abstract class Variable
{
    public void SetItem(
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

    public virtual Variable? SetItemDirectly(Memory memory, Variable value, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        throw new InterpreterInvalidIndexException(this);
    }
    
    public void SetMember(
        Memory memory, 
        Variable value, 
        MemberInstruction member,
        ObjectVariable? objectContext = null)
    {
        var result = SetMemberDirectly(memory, value, member, objectContext);
        if (member.Next is ArrayInstruction arrayInstruction)
        {
            result.SetItemDirectly(memory, value, arrayInstruction, objectContext);
        }
        else if (member.Next is MemberInstruction memberInstruction)
        {
            result.SetMemberDirectly(memory, value, memberInstruction, objectContext);
        }
    }

    public virtual Variable? SetMemberDirectly(Memory memory, Variable value, MemberInstruction member, ObjectVariable? objectContext = null)
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
    
    public virtual Variable? GetItemDirectly(Memory memory, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        throw new InterpreterInvalidIndexException(this);
    }
    
    public Variable? GetMember(
        Memory memory,
        MemberInstruction member,
        ObjectVariable? objectContext = null)
    {
        var result = GetMemberDirectly(memory, member, objectContext);
        if (member.Next is not null)
        {
            return member.Next.Interpret(memory, result, objectContext);
        }
        else
        {
            return result;
        }
    }

    public virtual Variable? GetMemberDirectly(
        Memory memory,
        MemberInstruction member,
        ObjectVariable? objectContext = null)
    {
        throw new InterpreterInvalidIndexException(this);
    }
    
    public virtual Variable Operate(Memory memory, string operation, Variable? other, bool isInverted = false)
    {
        throw new InterpreterInvalidIndexException(this);
    }
}