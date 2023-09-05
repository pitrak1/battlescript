using System.Diagnostics;
using System.Security.Cryptography;
using BattleScript.Exceptions;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.InterpreterNS;

public partial class Interpreter
{
    private ScopeVariable HandleElse(Instruction instruction)
    {
        ScopeVariable condition = interpretCondition(instruction);

        if (ShouldRun(instruction, condition))
        {
            RunCodeBlock(instruction.Instructions);
        }
        else if (instruction.Next is not null)
        {
            InterpretInstruction(instruction.Next);
        }
        return new ScopeVariable(Consts.VariableTypes.Literal);
    }

    private ScopeVariable interpretCondition(Instruction instruction)
    {
        if (instruction.Value is not null)
        {
            return InterpretInstruction(instruction.Value);
        }
        else
        {
            return new ScopeVariable(Consts.VariableTypes.Literal);
        }
    }

    private bool ShouldRun(Instruction instruction, ScopeVariable condition)
    {
        return instruction.Value is null || isTruthy(condition);
    }
}