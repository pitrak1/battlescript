namespace Battlescript;

public class ListComprehensionInstruction : Instruction
{
    public ListComprehensionInstruction(List<Token> tokens) : base(tokens)
    {
        var remainingTokens = tokens.GetRange(1, tokens.Count - 2);
        var currentForIndex = InstructionUtilities.GetTokenIndex(remainingTokens, ["for"]);
        
        var expressionTokens = remainingTokens.GetRange(0, currentForIndex);
        var expressionInstruction = InstructionFactory.Create(expressionTokens);
        
        Instruction lastInstruction = this;
        remainingTokens = remainingTokens.GetRange(currentForIndex, remainingTokens.Count - currentForIndex);
        
        while (remainingTokens.Count > 0)
        {
            // We add the + 1 to the index here because we ignore the first token in the search
            int nextIfOrForIndex = InstructionUtilities.GetTokenIndex(
                remainingTokens.GetRange(1, remainingTokens.Count - 1), 
                ["for", "if"]);
            if (nextIfOrForIndex != -1) nextIfOrForIndex += 1;
            
            if (remainingTokens[0].Value == "for")
            {
                // If we got here, we can assume the first three tokens are "for <var> in"
                var currentVar = new VariableInstruction(remainingTokens[1].Value);

                var currentListTokenCount = nextIfOrForIndex == -1 ? remainingTokens.Count - 3 : nextIfOrForIndex - 3;
                var currentListTokens = remainingTokens.GetRange(3, currentListTokenCount);
                var currentList = InstructionFactory.Create(currentListTokens);

                var forInst = new ForInstruction(currentVar, currentList);
                lastInstruction.Instructions = [forInst];
                lastInstruction = forInst;
            } 
            else if (remainingTokens[0].Value == "if")
            {
                var currentConditionTokenCount = nextIfOrForIndex == -1 ? 
                    remainingTokens.Count - 1 : 
                    nextIfOrForIndex - 1;
                var currentConditionTokens = remainingTokens.GetRange(1, currentConditionTokenCount);
                var currentCondition = InstructionFactory.Create(currentConditionTokens);

                var ifInst = new IfInstruction(currentCondition);
                lastInstruction.Instructions = [ifInst];
                lastInstruction = ifInst;
            }
            else
            {
                throw new Exception("Invalid list comprehension");
            }

            if (nextIfOrForIndex == -1)
            {
                remainingTokens = [];
            } else {
                remainingTokens = remainingTokens.GetRange(nextIfOrForIndex, remainingTokens.Count - nextIfOrForIndex);
            }
        }
        
        Instructions.Insert(0, new AssignmentInstruction("=", new VariableInstruction("lstcmp"), new SquareBracketsInstruction([])));
        lastInstruction.Instructions = [new VariableInstruction("lstcmp", new MemberInstruction("append", new ParenthesesInstruction([expressionInstruction])))];
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
        
        return callStack.GetVariable(closure, "lstcmp");
    }
}