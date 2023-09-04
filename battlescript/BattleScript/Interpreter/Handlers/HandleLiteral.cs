using System.Diagnostics;
using System.Security.Cryptography;
using BattleScript.Exceptions;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.InterpreterNS;

public partial class Interpreter
{
    private ScopeVariable HandleLiteral(Instruction instruction)
    {
        return new ScopeVariable(Consts.VariableTypes.Literal, instruction.Value);
    }
}