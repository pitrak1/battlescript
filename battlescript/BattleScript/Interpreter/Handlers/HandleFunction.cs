using System.Diagnostics;
using System.Security.Cryptography;
using BattleScript.Exceptions;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.InterpreterNS;

public partial class Interpreter
{
    private ScopeVariable HandleFunction(Instruction instruction)
    {
        List<ScopeVariable> args = new List<ScopeVariable>();
        foreach (Instruction inst in instruction.Value)
        {
            args.Add(new ScopeVariable(Consts.VariableTypes.Literal, inst.Value));
        }

        ScopeVariable? classObject = null;
        if (!ClassContexts.Empty())
        {
            classObject = ClassContexts.GetCurrentContext();
        }
        return new ScopeVariable(Consts.VariableTypes.Function, args, instruction.Instructions, classObject);
    }
}