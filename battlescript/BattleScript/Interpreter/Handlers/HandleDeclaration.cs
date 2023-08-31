using System.Diagnostics;
using System.Security.Cryptography;
using BattleScript.Exceptions;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.InterpreterNS;

public partial class Interpreter
{
    private ScopeVariable HandleDeclaration(Instruction instruction)
    {
        Debug.Assert(instruction.Value is string);
        List<string> path = new List<string>() { instruction.Value };
        return LexicalContexts.AddVariable(path);
    }
}