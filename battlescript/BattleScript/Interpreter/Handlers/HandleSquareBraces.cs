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
        if (OngoingContexts.Empty())
        {
            // Handle array
            List<ScopeVariable> entries = new List<ScopeVariable>();
            foreach (Instruction entryInstruction in instruction.Value)
            {
                ScopeVariable entryResult = InterpretInstruction(entryInstruction);
                entries.Add(entryResult);
            }
            return new ScopeVariable(Consts.VariableTypes.Array, entries);
        }
        else
        {
            // Handle index
            ScopeVariable index = InterpretInstruction(instruction.Value[0]);
            ScopeVariable indexed = OngoingContexts.GetCurrentContext();
            ScopeVariable var = indexed.GetIndex(index.Value);

            if (index.Value is string && index.Value == "super")
            {
                ClassContexts.Add(OngoingContexts.GetCurrentContext());
                ScopeVariable result = HandleSuper(instruction);
                ClassContexts.Pop();
                return result;
            }

            if (instruction.Next is not null)
            {
                OngoingContexts.SetCurrentContext(var);
                var = InterpretInstruction(instruction.Next);
            }

            return var;
        }
    }
}