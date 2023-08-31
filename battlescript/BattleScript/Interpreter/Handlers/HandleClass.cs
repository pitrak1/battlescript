using System.Diagnostics;
using System.Security.Cryptography;
using BattleScript.Exceptions;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.InterpreterNS;

public partial class Interpreter
{
    private ScopeVariable HandleClass(Instruction instruction)
    {
        ScopeVariable var = new ScopeVariable(Consts.VariableTypes.Class);

        ClassContexts.Add(var);
        ScopeVariable resultingScope = RunFunction(
            new List<ScopeVariable>(),
            instruction.Instructions,
            new List<Instruction>()
        );
        ClassContexts.Pop();

        var.Value = resultingScope.Value;
        if (instruction.Value is not null)
        {
            var.Value.Add("super", InterpretInstruction(instruction.Value));
        }

        return var;
    }
}