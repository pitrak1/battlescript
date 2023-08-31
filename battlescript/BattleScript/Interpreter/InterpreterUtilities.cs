using System.Diagnostics;
using System.Security.Cryptography;
using BattleScript.Exceptions;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.InterpreterNS;

public partial class Interpreter
{
    public static bool isTruthy(ScopeVariable var)
    {
        if (var.Value is null)
        {
            return false;
        }
        else if ((var.Value is int) && (var.Value == 0))
        {
            return false;
        }
        else if ((var.Value is string) && (var.Value == ""))
        {
            return false;
        }
        else if (var.Value is bool)
        {
            return var.Value;
        }
        else
        {
            return true;
        }
    }

    private ScopeVariable RunFunction(List<ScopeVariable> functionValue, List<Instruction> instructions, List<Instruction> args, ScopeVariable? classObject = null)
    {
        LexicalContexts.Add();

        if (args.Count != functionValue.Count)
        {
            throw new WrongNumberOfArgumentsException(args.Count, functionValue.Count);
        }

        if (classObject != null)
        {
            ClassContexts.Add(classObject);
        }

        for (int i = 0; i < args.Count; i++)
        {
            string argName = functionValue[i].Value;
            ScopeVariable argValue = InterpretInstruction(args[i]);
            LexicalContexts.AddVariable(new List<string>() { argName }, argValue);
        }

        foreach (Instruction funcInstruction in instructions)
        {
            InterpretInstruction(funcInstruction);
        }

        if (classObject != null)
        {
            ClassContexts.Pop();
        }

        return LexicalContexts.Pop();
    }
}