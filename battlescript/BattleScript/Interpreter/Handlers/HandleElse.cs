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
        ScopeVariable condition = new ScopeVariable(Consts.VariableTypes.Literal);
        if (instruction.Value is not null)
        {
            condition = InterpretInstruction(instruction.Value);
        }

        if (instruction.Value is null || isTruthy(condition))
        {
            LexicalContexts.AddNewScope();
            foreach (Instruction elseInstruction in instruction.Instructions)
            {
                InterpretInstruction(elseInstruction);
            }
            LexicalContexts.Pop();
        }
        else if (instruction.Next is not null)
        {
            InterpretInstruction(instruction.Next);
        }
        return new ScopeVariable(Consts.VariableTypes.Literal);
    }
}