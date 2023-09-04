using System.Diagnostics;
using System.Security.Cryptography;
using BattleScript.Exceptions;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.InterpreterNS;

public partial class Interpreter
{
    private ScopeVariable HandleOperation(Instruction instruction)
    {
        ScopeVariable left = InterpretInstruction(instruction.Left);
        ScopeVariable right = InterpretInstruction(instruction.Right);

        Debug.Assert(left.Value is int);
        Debug.Assert(right.Value is int);

        dynamic? result;
        switch (instruction.Value)
        {
            case "==":
                result = left.Value == right.Value;
                break;
            case "<":
                result = left.Value < right.Value;
                break;
            case ">":
                result = left.Value > right.Value;
                break;
            case "+":
                result = left.Value + right.Value;
                break;
            case "*":
                result = left.Value * right.Value;
                break;
            default:
                throw new SystemException("Invalid operator");
        }

        return new ScopeVariable(Consts.VariableTypes.Literal, result);
    }
}