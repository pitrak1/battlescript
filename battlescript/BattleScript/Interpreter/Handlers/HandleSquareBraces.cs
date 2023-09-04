using System.Diagnostics;
using System.Security.Cryptography;
using BattleScript.Exceptions;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.InterpreterNS;

public partial class Interpreter
{
    private ScopeVariable HandleSquareBraces(Instruction instruction)
    {
        if (OngoingContexts.IsEmpty())
        {
            return handleArray(instruction);
        }
        else
        {
            return handleIndex(instruction);
        }
    }

    private ScopeVariable handleArray(Instruction instruction)
    {
        List<ScopeVariable> initializationEntries = InterpretListOfInstructions(instruction.Value);
        return new ScopeVariable(Consts.VariableTypes.Array, initializationEntries);
    }

    private ScopeVariable handleIndex(Instruction instruction)
    {
        Debug.Assert(instruction.Value is List<Instruction>, $"Expected the index value to be an array of instructions");
        ScopeVariable index = InterpretInstruction(instruction.Value![0]);

        Debug.Assert(!OngoingContexts.IsEmpty(), "Expected to have a non-null indexed value");
        ScopeVariable indexed = OngoingContexts.GetCurrentContext();

        ScopeVariable result = indexed.GetIndex(index.Value);

        // if (index.Value is string && index.Value == "super")
        // {
        //     ClassContexts.Add(OngoingContexts.GetCurrentContext());
        //     ScopeVariable result = HandleSuper(instruction);
        //     ClassContexts.Pop();
        //     return result;
        // }

        // if (instruction.Next is not null)
        // {
        //     OngoingContexts.SetCurrentContext(var);
        //     var = InterpretInstruction(instruction.Next);
        // }

        return result;
    }
}