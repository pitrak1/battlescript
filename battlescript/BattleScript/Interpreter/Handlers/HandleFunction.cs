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
        List<ScopeVariable> args = getArgumentValues(instruction.Value);

        // ScopeVariable? classObject = null;
        // if (!ClassContexts.IsEmpty())
        // {
        //     classObject = ClassContexts.GetCurrentContext();
        // }
        // return new ScopeVariable(Consts.VariableTypes.Function, args, instruction.Instructions, classObject);
        return new ScopeVariable(Consts.VariableTypes.Function, args, instruction.Instructions);

    }

    private List<ScopeVariable> getArgumentValues(List<Instruction> instructions)
    {
        List<ScopeVariable> args = new List<ScopeVariable>();

        foreach (Instruction inst in instructions)
        {
            args.Add(new ScopeVariable(Consts.VariableTypes.Literal, inst.Value));
        }

        return args;
    }
}