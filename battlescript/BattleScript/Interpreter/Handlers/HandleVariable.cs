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
            OngoingContexts.Add(var);
            if (var.Type == Consts.VariableTypes.Object) { SelfContexts.Add(var); }
            ScopeVariable result = InterpretInstruction(instruction.Next);
            if (var.Type == Consts.VariableTypes.Object) { SelfContexts.Pop(); }
            OngoingContexts.Pop();
            var = result;
        }
        return var;
    }
}