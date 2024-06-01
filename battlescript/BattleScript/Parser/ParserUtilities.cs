using System.Diagnostics;
using BattleScript.Tokens;
using BattleScript.Core;

namespace BattleScript.ParserNS;

public class ParserUtilities
{
    public static bool BlockContainsSemicolon(List<Token> tokens, int tokenIndex)
    {
        while (tokenIndex < tokens.Count)
        {
            switch (tokens[tokenIndex].Value)
            {
                case "}":
                    return false;
                case ";":
                    return true;
                default:
                    tokenIndex++;
                    break;
            }
        }
        return false;
    }

    public static bool BlockContainsSemicolonReverse(List<Token> tokens, int tokenIndex)
    {
        while (tokenIndex >= 0)
        {
            switch (tokens[tokenIndex].Value)
            {
                case "{":
                    return false;
                case ";":
                    return true;
                default:
                    tokenIndex--;
                    break;
            }
        }
        return false;
    }
}