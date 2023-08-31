using System.Diagnostics;
using System.Security.Cryptography;
using BattleScript.Exceptions;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.InterpreterNS;

public partial class Interpreter
{
    private ScopeVariable HandleSuper(Instruction instruction)
    {
        ScopeVariable classObject = ClassContexts.GetCurrentContext();
        ScopeVariable superObject = classObject.GetVariable("super");

        if (instruction.Next is not null)
        {
            OngoingContexts.Add(superObject);
            superObject = InterpretInstruction(instruction.Next);
            OngoingContexts.Pop();
        }
        return superObject;
    }
}