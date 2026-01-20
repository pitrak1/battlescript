namespace Battlescript;

public abstract class Variable : IEquatable<Variable>
{
    public void SetItem(
        CallStack callStack,
        Closure closure,
        Variable value,
        ArrayInstruction index,
        ObjectVariable? objectContext = null)
    {
        var result = SetItemDirectly(callStack, closure, value, index, objectContext);
        if (index.Next is ArrayInstruction { Bracket: ArrayInstruction.BracketTypes.Parentheses })
        {
            throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "cannot assign to function call");
        } else if (index.Next is ArrayInstruction arrayInstruction)
        {
            result.SetItemDirectly(callStack, closure, value, arrayInstruction, objectContext);
        }
    }

    public virtual Variable? SetItemDirectly(CallStack callStack, Closure closure, Variable value, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        throw new InterpreterInvalidIndexException(this);
    }

    public void SetMember(
        CallStack callStack,
        Closure closure,
        Variable value,
        MemberInstruction member,
        ObjectVariable? objectContext = null)
    {
        var result = SetMemberDirectly(callStack, closure, value, member, objectContext);
        if (member.Next is ArrayInstruction { Bracket: ArrayInstruction.BracketTypes.Parentheses })
        {
            throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "cannot assign to function call");
        }
        else if (member.Next is ArrayInstruction arrayInstruction)
        {
            result.SetItemDirectly(callStack, closure, value, arrayInstruction, objectContext);
        }
        else if (member.Next is MemberInstruction memberInstruction)
        {
            result.SetMemberDirectly(callStack, closure, value, memberInstruction, objectContext);
        }
    }

    public virtual Variable? SetMemberDirectly(CallStack callStack, Closure closure, Variable value, MemberInstruction member, ObjectVariable? objectContext = null)
    {
        throw new InterpreterInvalidIndexException(this);
    }

    public Variable? GetItem(
        CallStack callStack,
        Closure closure,
        ArrayInstruction index,
        ObjectVariable? objectContext = null)
    {
        var result = GetItemDirectly(callStack, closure, index, objectContext);
        if (index.Next is not null)
        {
            return index.Next.Interpret(callStack, closure, result);
        }
        else
        {
            return result;
        }
    }

    public virtual Variable? GetItemDirectly(CallStack callStack, Closure closure, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        throw new InterpreterInvalidIndexException(this);
    }

    public Variable? GetMember(
        CallStack callStack,
        Closure closure,
        MemberInstruction member,
        ObjectVariable? objectContext = null)
    {
        var result = GetMemberDirectly(callStack, closure, member, objectContext);
        if (member.Next is not null)
        {
            return member.Next.Interpret(callStack, closure, result);
        }
        else
        {
            return result;
        }
    }

    public virtual Variable? GetMemberDirectly(
        CallStack callStack,
        Closure closure,
        MemberInstruction member,
        ObjectVariable? objectContext = null)
    {
        throw new InterpreterInvalidIndexException(this);
    }

    #region Equality

    public abstract override bool Equals(object? obj);

    public abstract bool Equals(Variable? other);

    public abstract override int GetHashCode();

    #endregion
}
