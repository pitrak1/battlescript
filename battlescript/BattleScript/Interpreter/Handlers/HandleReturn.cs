using System.Diagnostics;
using System.Security.Cryptography;
using BattleScript.Exceptions;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.InterpreterNS;

public partial class Interpreter
{
    private ScopeVariable HandleReturn(Instruction instruction)
    {
        ScopeVariable result = InterpretInstruction(instruction.Value);
        LexicalContexts.AddVariableToCurrentScope(new List<string>() { "return" }, result);
        return result;
    }
}