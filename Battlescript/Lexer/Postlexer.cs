namespace Battlescript;

public static class Postlexer
{
    public static void Run(List<Token> tokens)
    {
        JoinIsNotAndNotIn(tokens);
    }

    private static void JoinIsNotAndNotIn(List<Token> tokens)
    {
        var i = 0;
        while (i < tokens.Count)
        {
            if (tokens[i].Type == Consts.TokenTypes.Operator && tokens[i].Value == "not")
            {
                if (i > 0 && 
                    tokens[i - 1].Type == Consts.TokenTypes.Operator && 
                    tokens[i - 1].Value == "is")
                {
                    tokens[i - 1].Value = "is not";
                    tokens.RemoveAt(i);
                    continue;
                } else if (i < tokens.Count - 1 &&
                           tokens[i + 1].Type == Consts.TokenTypes.Operator &&
                           tokens[i + 1].Value == "in")
                {
                    tokens[i + 1].Value = "not in";
                    tokens.RemoveAt(i);
                    continue;
                }
            }
            i++;
        }
    }
}