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
            LexicalContexts.Add();
            foreach (Instruction ifInstruction in instruction.Instructions)
            {
                InterpretInstruction(ifInstruction);
            }
            LexicalContexts.Pop();
            condition = InterpretInstruction(instruction.Value);
        }

        return new ScopeVariable(Consts.VariableTypes.Value);
    }
}