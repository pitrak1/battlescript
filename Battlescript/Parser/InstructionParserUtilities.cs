namespace Battlescript;

public class InstructionParserUtilities
{
    public static int GetAssignmentIndex(List<Token> tokens)
    {
        for (int i = 0; i < tokens.Count; i++)
        {
            if (tokens[i].Type == Consts.TokenTypes.Assignment)
            {
                return i;
            }
        }

        return -1;
    }

    public static int GetOperatorIndex(List<Token> tokens)
    {
        // Because we want to prioritize operators (and also prioritize earlier operators over later operators
        // of the same priority), we need to loop backwards and look for lower priority operators first.  The way
        // that these instructions are evaluated will be that the more deeply nested expressions will be evaluated
        // first.  Doing it this way means the lowest operator will split the entire expression with the left
        // and right operators containing variables, numbers, or sub-expressions with higher priority.

        int lowestOperatorPriority = -1;
        int lowestOperatorIndex = -1;
        
        for (int i = tokens.Count - 1; i >= 0; i--)
        {
            if (tokens[i].Type == Consts.TokenTypes.Operator)
            {
                // Because we want to find lower priority operators, the Operators const is in mathematical
                // priority descending order, making the higher priority operators have a lower value and
                // the lower priority operators have a higher value
                var priority = Array.FindIndex(Consts.Operators, e => e == tokens[i].Value);

                if (priority != -1 && priority > lowestOperatorPriority)
                {
                    lowestOperatorPriority = priority;
                    lowestOperatorIndex = i;
                }
            }
        }
        
        return lowestOperatorIndex;
    }
}