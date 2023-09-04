using System.Diagnostics;
using System.Security.Cryptography;
using BattleScript.Exceptions;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.InterpreterNS;

public partial class Interpreter
{
    private ScopeVariable HandleVariable(Instruction instruction)
    {
        ScopeVariable var = LexicalContexts.GetVariable(instruction.Value);
        if (instruction.Next is not null)
        {
            return handleNext(var, instruction.Next);
        }
        return var;
    }

    private ScopeVariable handleNext(ScopeVariable var, Instruction next)
    {
        OngoingContexts.Add(var);
        // if (var.Type == Consts.VariableTypes.Object) { SelfContexts.Add(var); }

        ScopeVariable result = InterpretInstruction(next);

        // if (var.Type == Consts.VariableTypes.Object) { SelfContexts.Pop(); }
        OngoingContexts.Pop();

        return result;
    }
}