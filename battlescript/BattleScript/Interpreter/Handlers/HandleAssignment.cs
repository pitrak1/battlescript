using System.Diagnostics;
using System.Security.Cryptography;
using BattleScript.Exceptions;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.InterpreterNS;

public partial class Interpreter
{
    private ScopeVariable HandleAssignment(Instruction instruction)
    {
        ScopeVariable left = InterpretInstruction(instruction.Left);
        ScopeVariable right = InterpretInstruction(instruction.Right);
        return left.Copy(right);
    }
}