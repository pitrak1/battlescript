using System.Diagnostics;
using System.Security.Cryptography;
using BattleScript.Exceptions;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.InterpreterNS;

public partial class Interpreter
{
    private ScopeVariable HandleParens(Instruction instruction)
    {
        Debug.Assert(!OngoingContexts.Empty());

        ScopeVariable called = OngoingContexts.GetCurrentContext();

        switch (called.Type)
        {
            case Consts.VariableTypes.Function:
                if (!SelfContexts.Empty())
                {
                    LexicalContexts.Add(SelfContexts.GetCurrentContext());
                }

                ScopeVariable functionScope = RunFunction(called.Value, called.Instructions, instruction.Value, called.ClassObject);

                ScopeVariable? returnValue = new ScopeVariable(Consts.VariableTypes.Literal);
                if (functionScope.HasVariable("return"))
                {
                    returnValue = functionScope.GetVariable("return");
                }

                if (!SelfContexts.Empty())
                {
                    LexicalContexts.Pop();
                }
                return returnValue;
            case Consts.VariableTypes.Class:
                ScopeVariable createdObject = null;
                ScopeVariable constructor = null;
                if (SelfContexts.Empty())
                {
                    createdObject = new ScopeVariable().CreateObject(called);
                    constructor = createdObject.ClassObject.GetConstructorForClass();
                }
                else
                {
                    createdObject = SelfContexts.GetCurrentContext();
                    constructor = OngoingContexts.GetCurrentContext().GetConstructorForClass();
                }
                if (constructor is not null)
                {
                    SelfContexts.Add(createdObject);
                    LexicalContexts.Add(createdObject);
                    RunFunction(constructor.Value, constructor.Instructions, instruction.Value, constructor.ClassObject);
                    SelfContexts.Pop();
                    LexicalContexts.Pop();
                }
                return createdObject;
            default:
                throw new SystemException("Invalid instruction type to call");
        }
    }
}