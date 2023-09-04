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

        dynamic? result = getOperationResult(instruction.Value, left, right);
        return new ScopeVariable(Consts.VariableTypes.Literal, result);
    }

    private ScopeVariable getOperand(Instruction instruction)
    {
        ScopeVariable operand = InterpretInstruction(instruction);
        Debug.Assert(operand.Value is int, "Operands must be integers");
        return operand;
    }

    private dynamic? getOperationResult(string operatorString, ScopeVariable left, ScopeVariable right)
    {
        switch (operatorString)
        {
            case "==":
                return left.Value == right.Value;
            case "<":
                return left.Value < right.Value;
            case ">":
                return left.Value > right.Value;
            case "+":
                return left.Value + right.Value;
            case "*":
                return left.Value * right.Value;
            default:
                throw new SystemException("Invalid operator");
        }
    }
}