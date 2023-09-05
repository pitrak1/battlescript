using System.Diagnostics;
using System.Security.Cryptography;
using BattleScript.Exceptions;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.InterpreterNS;

public partial class Interpreter
{
    private ScopeVariable HandleIf(Instruction instruction)
    {
        ScopeVariable condition = InterpretInstruction(instruction.Value);
        if (isTruthy(condition))
        {
            RunCodeBlock(instruction.Instructions);
        }
        // else if (instruction.Next is not null)
        // {
        //     InterpretInstruction(instruction.Next);
        // }

        return new ScopeVariable(Consts.VariableTypes.Literal);
    }
}