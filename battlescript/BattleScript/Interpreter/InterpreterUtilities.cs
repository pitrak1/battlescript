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

    public void RunCodeBlock(List<Instruction> instructions)
    {
        LexicalContexts.AddNewScope();
        foreach (Instruction insideBlockInstruction in instructions)
        {
            InterpretInstruction(insideBlockInstruction);
        }
        LexicalContexts.Pop();
    }

    private List<ScopeVariable> InterpretListOfInstructions(List<Instruction> instructions)
    {
        List<ScopeVariable> resultVariables = new List<ScopeVariable>();

        foreach (Instruction instruction in instructions)
        {
            ScopeVariable result = InterpretInstruction(instruction);
            resultVariables.Add(result);
        }

        return resultVariables;
    }

    private Dictionary<dynamic, ScopeVariable> InterpretListOfKeyValuePairInstructions(List<Instruction> instructions)
    {
        Dictionary<dynamic, ScopeVariable> entries = new Dictionary<dynamic, ScopeVariable>();

        for (int i = 0; i < instructions.Count; i = i + 2)
        {
            ScopeVariable key = InterpretInstruction(instructions[i]);
            ScopeVariable value = InterpretInstruction(instructions[i + 1]);
            entries.Add(key.Value, value);
        }

        return entries;
    }

    private ScopeVariable RunFunction(List<ScopeVariable> functionValue, List<Instruction> instructions, List<Instruction> args)
    {
        if (args.Count != functionValue.Count)
        {
            throw new WrongNumberOfArgumentsException(args.Count, functionValue.Count);
        }

        LexicalContexts.AddNewScope();
        addArgumentsToContext(functionValue, args);
        foreach (Instruction insideBlockInstruction in instructions)
        {
            InterpretInstruction(insideBlockInstruction);
        }
        return LexicalContexts.Pop();
    }

    private void addArgumentsToContext(List<ScopeVariable> functionValue, List<Instruction> args)
    {
        for (int i = 0; i < args.Count; i++)
        {
            string argName = functionValue[i].Value!;
            ScopeVariable argValue = InterpretInstruction(args[i]);
            LexicalContexts.AddVariableToCurrentScope(new List<string>() { argName }, argValue);
        }
    }

    // private ScopeVariable RunFunction(List<ScopeVariable> functionValue, List<Instruction> instructions, List<Instruction> args, ScopeVariable? classObject = null)
    // {
    //     LexicalContexts.AddNewScope();

    //     if (args.Count != functionValue.Count)
    //     {
    //         throw new WrongNumberOfArgumentsException(args.Count, functionValue.Count);
    //     }

    //     if (classObject != null)
    //     {
    //         ClassContexts.Add(classObject);
    //     }

    //     for (int i = 0; i < args.Count; i++)
    //     {
    //         string argName = functionValue[i].Value!;
    //         ScopeVariable argValue = InterpretInstruction(args[i]);
    //         LexicalContexts.AddVariableToCurrentScope(new List<string>() { argName }, argValue);
    //     }

    //     foreach (Instruction funcInstruction in instructions)
    //     {
    //         InterpretInstruction(funcInstruction);
    //     }

    //     if (classObject != null)
    //     {
    //         ClassContexts.Pop();
    //     }

    //     return LexicalContexts.Pop();
    // }
}