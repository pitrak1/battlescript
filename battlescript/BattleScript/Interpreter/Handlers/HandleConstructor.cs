using System.Diagnostics;
using System.Security.Cryptography;
using BattleScript.Exceptions;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.InterpreterNS;

public partial class Interpreter
{
    private ScopeVariable HandleConstructor(Instruction instruction)
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

        List<string> path = new List<string>() { "constructor" };
        ScopeVariable left = LexicalContexts.AddVariable(path);
        return left.Copy(new ScopeVariable(Consts.VariableTypes.Function, args, instruction.Instructions, classObject));
    }
}