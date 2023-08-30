using System.Diagnostics;
using BattleScript.Tokens;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.ParserNS;

public partial class InstructionParser
{
    private List<Token> GetTokensBeforeIndex(List<Token> tokens, int index)
    {
        return tokens.GetRange(0, index);
    }

    private List<Token> GetTokensAfterIndex(List<Token> tokens, int index)
    {
        return tokens.GetRange(index + 1, tokens.Count - index - 1);
    }

    private List<Token> GetAllTokensButFirst(List<Token> tokens)
    {
        return tokens.GetRange(1, tokens.Count - 1);
    }

    private List<Token> GetAllTokensButFirstTwo(List<Token> tokens)
    {
        return tokens.GetRange(2, tokens.Count - 2);
    }

    private List<Token> GetTokensAfterKeywordWithoutParens(List<Token> tokens)
    {
        return tokens.GetRange(2, tokens.Count - 3);
    }

    private List<Token> GetTokensAfterTwoKeywordsWithoutParens(List<Token> tokens)
    {
        return tokens.GetRange(3, tokens.Count - 4);
    }
}