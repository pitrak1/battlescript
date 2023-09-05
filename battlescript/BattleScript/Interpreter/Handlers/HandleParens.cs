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
        Debug.Assert(!OngoingContexts.IsEmpty());

        ScopeVariable called = OngoingContexts.GetCurrentContext();

        switch (called.Type)
        {
            case Consts.VariableTypes.Function:
                // if (!SelfContexts.IsEmpty())
                // {
                //     LexicalContexts.Add(SelfContexts.GetCurrentContext());
                // }

                // ScopeVariable functionScope = RunFunction(called.Value, called.Instructions, instruction.Value, called.ClassObject);
                ScopeVariable functionScope = RunFunction(called.Value, called.Instructions, instruction.Value);


                // if (!SelfContexts.IsEmpty())
                // {
                //     LexicalContexts.Pop();
                // }
                return getReturnValue(functionScope);
            // case Consts.VariableTypes.Class:
            //     ScopeVariable createdObject = null;
            //     ScopeVariable constructor = null;
            //     if (SelfContexts.IsEmpty())
            //     {
            //         createdObject = new ScopeVariable().CreateObject(called);
            //         constructor = createdObject.ClassObject.GetConstructorForClass();
            //     }
            //     else
            //     {
            //         createdObject = SelfContexts.GetCurrentContext();
            //         constructor = OngoingContexts.GetCurrentContext().GetConstructorForClass();
            //     }
            //     if (constructor is not null)
            //     {
            //         SelfContexts.Add(createdObject);
            //         LexicalContexts.Add(createdObject);
            //         RunFunction(constructor.Value, constructor.Instructions, instruction.Value, constructor.ClassObject);
            //         SelfContexts.Pop();
            //         LexicalContexts.Pop();
            //     }
            //     return createdObject;
            default:
                throw new SystemException("Invalid instruction type to call");
        }
    }

    private ScopeVariable getReturnValue(ScopeVariable functionScope)
    {
        ScopeVariable returnValue = new ScopeVariable(Consts.VariableTypes.Literal);

        if (functionScope.HasVariable("return"))
        {
            returnValue = functionScope.GetVariable("return");
        }

        return returnValue;
    }
}