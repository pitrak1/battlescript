using System.Diagnostics;
using BattleScript.Tokens;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.ParserNS;

public partial class InstructionParser
{
    private Instruction HandleClass(List<Token> tokens)
    {
        Instruction? value = null;
        if (tokens.Count > 1)
        {
            Debug.Assert(tokens.Count == 3);
            Debug.Assert(tokens[1].Value == "extends");
            value = new VariableInstruction(tokens[2].Value);
        }

        return new ClassInstruction(value, null, tokens[0].Line, tokens[0].Column);
    }
}