namespace BattleScript; 

public class ParserUtilities {
    public static int GetAssignmentOperatorIndex(List<Token> tokens) {
        for (int i = 0; i < tokens.Count; i++) {
            if (tokens[i].Type == Consts.TokenTypes.Assignment) {
                return i;
            }
        }

        return -1;
    }
    
    public static int GetMathematicalOperatorIndex(List<Token> tokens) {
        int lowestOperatorPriority = Consts.Operators.Length;
        int lowestOperatorLocation = tokens.Count;

        List<string> separatorStack = new List<string>();
        
        for (int i = tokens.Count - 1; i >= 0; i--) {
            var currentValue = tokens[i].Value;
            if (Consts.ClosingSeparators.Contains(currentValue)) {
                separatorStack.Add(currentValue);
            } else if (separatorStack.Count > 0 &&
                       separatorStack[^0] == Consts.MatchingSeparatorsMap[currentValue]) {
                separatorStack.RemoveAt(separatorStack.Count - 1);
            } else {
                int currentOperatorPriority = Array.FindIndex(Consts.Operators, element => element == currentValue);
                if (currentOperatorPriority < lowestOperatorPriority) {
                    lowestOperatorPriority = currentOperatorPriority;
                    lowestOperatorLocation = i;
                }
            }
        }

        if (lowestOperatorPriority != Consts.Operators.Length) {
            return lowestOperatorLocation;
        } else {
            return -1;
        }
    }
}