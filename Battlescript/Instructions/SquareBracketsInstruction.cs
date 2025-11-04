using System.Diagnostics;

namespace Battlescript;

public class SquareBracketsInstruction : ArrayInstruction
{
    public SquareBracketsInstruction(List<Token> tokens) : base([])
    {
        var closingSeparatorIndex = InstructionUtilities.GetTokenIndex(tokens, ["]"]);
        var tokensInSeparators = tokens.GetRange(1, closingSeparatorIndex - 1);
        InitializeDelimiter(tokensInSeparators);
        InitializeValues(tokensInSeparators);
        ParseNext(tokens, closingSeparatorIndex + 1);
    }

    public override Variable? Interpret(
        CallStack callStack, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        if (instructionContext is not null)
        {
            return instructionContext.GetItem(callStack, this, objectContext);
        }
        else
        {
            return InterpretListCreation(callStack);
        }
    }
    
    private Variable InterpretListCreation(CallStack callStack)
    {
        var values = new List<Variable>();
        
        foreach (var instructionValue in Values)
        {
            if (instructionValue is not null)
            {
                values.Add(instructionValue.Interpret(callStack));
            }
            else
            {
                throw new Exception("Poorly formed list initialization");
            }
            
        }

        return BsTypes.Create(BsTypes.Types.List, values);
    }
}