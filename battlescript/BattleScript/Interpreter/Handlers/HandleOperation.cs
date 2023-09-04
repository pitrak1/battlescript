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
        ScopeVariable left = getOperand(instruction.Left!);
        ScopeVariable right = getOperand(instruction.Right!);

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

    private ScopeVariable getOperand(Instruction instruction)
    {
        ScopeVariable operand = InterpretInstruction(instruction);
        Debug.Assert(operand.Value is int, "Operands must be integers");
        return operand;
    }
}