using System.Diagnostics;
using System.Security.Cryptography;
using BattleScript.Exceptions;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.InterpreterNS;

public partial class Interpreter
{
    private ScopeVariable HandleValueType(Instruction instruction)
    {
        return new ScopeVariable(Consts.VariableTypes.Value, instruction.Value);
    }
}