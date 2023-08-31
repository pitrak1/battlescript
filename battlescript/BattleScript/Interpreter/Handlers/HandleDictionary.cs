using System.Diagnostics;
using System.Security.Cryptography;
using BattleScript.Exceptions;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.InterpreterNS;

public partial class Interpreter
{
    private ScopeVariable HandleDictionary(Instruction instruction)
    {
        Dictionary<dynamic, ScopeVariable> entries = new Dictionary<dynamic, ScopeVariable>();

        for (int i = 0; i < instruction.Value.Count; i = i + 2)
        {
            ScopeVariable key = InterpretInstruction(instruction.Value[i]);
            ScopeVariable value = InterpretInstruction(instruction.Value[i + 1]);
            entries.Add(key.Value, value);
        }

        return new ScopeVariable(Consts.VariableTypes.Dictionary, entries);
    }
}