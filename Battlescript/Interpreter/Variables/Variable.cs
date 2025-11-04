namespace Battlescript;

public abstract class Variable
{
    public void SetItem(
        CallStack callStack, 
        Variable value, 
        ArrayInstruction index,
        ObjectVariable? objectContext = null)
    {
        var result = SetItemDirectly(callStack, value, index, objectContext);
        if (index.Next is ParenthesesInstruction)
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "cannot assign to function call");
        } else if (index.Next is ArrayInstruction arrayInstruction)
        {
            result.SetItemDirectly(callStack, value, arrayInstruction, objectContext);
        }
    }

    public virtual Variable? SetItemDirectly(CallStack callStack, Variable value, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        throw new InterpreterInvalidIndexException(this);
    }
    
    public void SetMember(
        CallStack callStack, 
        Variable value, 
        MemberInstruction member,
        ObjectVariable? objectContext = null)
    {
        var result = SetMemberDirectly(callStack, value, member, objectContext);
        if (member.Next is ParenthesesInstruction)
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "cannot assign to function call");
        }
        else if (member.Next is ArrayInstruction arrayInstruction)
        {
            result.SetItemDirectly(callStack, value, arrayInstruction, objectContext);
        }
        else if (member.Next is MemberInstruction memberInstruction)
        {
            result.SetMemberDirectly(callStack, value, memberInstruction, objectContext);
        }
    }

    public virtual Variable? SetMemberDirectly(CallStack callStack, Variable value, MemberInstruction member, ObjectVariable? objectContext = null)
    {
        throw new InterpreterInvalidIndexException(this);
    }

    public Variable? GetItem(
        CallStack callStack,
        ArrayInstruction index,
        ObjectVariable? objectContext = null)
    {
        var result = GetItemDirectly(callStack, index, objectContext);
        if (index.Next is not null)
        {
            return index.Next.Interpret(callStack, result, objectContext);
        }
        else
        {
            return result;
        }
    }
    
    public virtual Variable? GetItemDirectly(CallStack callStack, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        throw new InterpreterInvalidIndexException(this);
    }
    
    public Variable? GetMember(
        CallStack callStack,
        MemberInstruction member,
        ObjectVariable? objectContext = null)
    {
        var result = GetMemberDirectly(callStack, member, objectContext);
        if (member.Next is not null)
        {
            return member.Next.Interpret(callStack, result, objectContext);
        }
        else
        {
            return result;
        }
    }

    public virtual Variable? GetMemberDirectly(
        CallStack callStack,
        MemberInstruction member,
        ObjectVariable? objectContext = null)
    {
        throw new InterpreterInvalidIndexException(this);
    }
}