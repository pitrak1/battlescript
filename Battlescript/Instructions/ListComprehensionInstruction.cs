namespace Battlescript;

public class ListComprehensionInstruction : Instruction
{
    // For reference, basic list comprehensions are made of [<expression> for <variable> in <list> if <condition>]
    public ListComprehensionInstruction(List<Token> tokens) : base(tokens)
    {
        var allTokens = GetTokensInsideSquareBrackets(tokens);
        var expressionInstruction = GetExpressionBeforeForKeyword(allTokens);
        
        Instruction lastInstruction = this;
        var remainingTokens = GetTokensAfterAndIncludingForKeyword(allTokens);
        
        // This is effectively starting with the expression and applying each for/if to it by wrapping 
        // the expression instruction in each one
        while (remainingTokens.Count > 0)
        {
            if (remainingTokens[0].Value == "for")
            {
                lastInstruction = HandleForClause(remainingTokens, lastInstruction);
            } 
            else if (remainingTokens[0].Value == "if")
            {
                lastInstruction = HandleIfClause(remainingTokens, lastInstruction);
            }
            else
            {
                throw new Exception("Invalid list comprehension");
            }

            var nextIfOrForIndex = GetNextIfOrForKeywordIndex(remainingTokens);
            if (nextIfOrForIndex == -1)
            {
                remainingTokens = [];
            } else {
                remainingTokens = remainingTokens.GetRange(nextIfOrForIndex, remainingTokens.Count - nextIfOrForIndex);
            }
        }
        
        Instructions.Insert(0, new AssignmentInstruction("=", new VariableInstruction("lstcmp"), new SquareBracketsInstruction([])));
        lastInstruction.Instructions = [new VariableInstruction("lstcmp", new MemberInstruction("append", new ParenthesesInstruction([expressionInstruction])))];
        return;
        
        List<Token> GetTokensInsideSquareBrackets(List<Token> t)
        {
            return t.GetRange(1, t.Count - 2);
        }

        Instruction GetExpressionBeforeForKeyword(List<Token> t)
        {
            var e = t.GetRange(0, InstructionUtilities.GetTokenIndex(t, ["for"]));
            return InstructionFactory.Create(e);
        }
        
        List<Token> GetTokensAfterAndIncludingForKeyword(List<Token> t)
        {
            var i = InstructionUtilities.GetTokenIndex(t, ["for"]);
            return t.GetRange(i, t.Count - i);
        }
    }

    private Instruction HandleForClause(List<Token> tokens, Instruction lastInstruction)
    {
        // If we got here, we can assume the first three tokens are "for <var> in"
        var currentVariable = new VariableInstruction(tokens[1].Value);
        var currentList = GetListInstructionForClause(tokens);
        
        var forInst = new ForInstruction(currentVariable, currentList);
        lastInstruction.Instructions = [forInst];
        return forInst;

        Instruction GetListInstructionForClause(List<Token> t)
        {
            var nextForOrIfIndex = GetNextIfOrForKeywordIndex(t);
            var stopIndex = nextForOrIfIndex == -1 ? t.Count : nextForOrIfIndex;
            var instructionTokens = t.GetRange(3, stopIndex - 3);
            return InstructionFactory.Create(instructionTokens);
        }
    }

    private Instruction HandleIfClause(List<Token> tokens, Instruction lastInstruction)
    {
        var currentCondition = GetConditionInstructionForClause(tokens);
        var ifInst = new IfInstruction(currentCondition);
        lastInstruction.Instructions = [ifInst];
        return ifInst;
        
        Instruction GetConditionInstructionForClause(List<Token> t)
        {
            var nextForOrIfIndex = GetNextIfOrForKeywordIndex(t);
            var stopIndex = nextForOrIfIndex == -1 ? t.Count : nextForOrIfIndex;
            var instructionTokens = t.GetRange(1, stopIndex - 1);
            return InstructionFactory.Create(instructionTokens);
        }
    }

    private int GetNextIfOrForKeywordIndex(List<Token> tokens)
    {
        var index = InstructionUtilities.GetTokenIndex(
            tokens.GetRange(1, tokens.Count - 1),
            ["for", "if"]);
        
        if (index != -1)
        {
            index += 1;
        }

        return index;
    }
    
    public ListComprehensionInstruction(List<Instruction> instructions) : base([])
    {
        Instructions = instructions ?? [];
    }
    
    public override Variable? Interpret(
        CallStack callStack,
        Closure closure,
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        foreach (var inst in Instructions)
        {
            inst.Interpret(callStack, closure, instructionContext, objectContext, lexicalContext);
        }
        
        return closure.GetVariable(callStack, "lstcmp");
    }
}