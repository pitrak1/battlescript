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
        Dictionary<dynamic, ScopeVariable> entries = InterpretListOfKeyValuePairInstructions(instruction.Value);
        return new ScopeVariable(Consts.VariableTypes.Dictionary, entries);
    }
}