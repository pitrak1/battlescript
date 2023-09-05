using System.Diagnostics;
using System.Security.Cryptography;
using BattleScript.Exceptions;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.InterpreterNS;

public partial class Interpreter
{
    private ScopeVariable HandleWhile(Instruction instruction)
    {
        ScopeVariable condition = InterpretInstruction(instruction.Value);
        while (isTruthy(condition))
        {
            RunCodeBlock(instruction.Instructions);
            condition = InterpretInstruction(instruction.Value);
        }

        return new ScopeVariable(Consts.VariableTypes.Literal);
    }
}