using System.Diagnostics;
using System.Security.Cryptography;
using BattleScript.Exceptions;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.InterpreterNS;

public partial class Interpreter
{
    private ScopeVariable HandleSelf(Instruction instruction)
    {
        ScopeVariable var = SelfContexts.GetCurrentContext();
        if (instruction.Next is not null)
        {
            OngoingContexts.Add(var);
            var = InterpretInstruction(instruction.Next);
            OngoingContexts.Pop();
        }
        return var;
    }
}