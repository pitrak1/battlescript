using System.Diagnostics;

namespace Battlescript;

public class ParenthesesInstruction : ArrayInstruction
{
    public ParenthesesInstruction(List<Token> tokens) : base([])
    {
        var closingSeparatorIndex = InstructionUtilities.GetTokenIndex(tokens, [")"]);
        var tokensInSeparators = tokens.GetRange(1, closingSeparatorIndex - 1);
        InitializeDelimiter(tokensInSeparators);
        InitializeValues(tokensInSeparators);
        ParseNext(tokens, closingSeparatorIndex + 1);
    }

    public override Variable? Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        if (instructionContext is FunctionVariable functionVariable)
        {
            // var stackUpdates = memory.CurrentStack.AddFrame(this, null, functionVariable.Name);
            var returnValue = functionVariable.RunFunction(memory, new ArgumentSet(memory, Values!, objectContext), this);
            // memory.CurrentStack.PopFrame(stackUpdates);
            return returnValue;
        }
        else if (instructionContext is ClassVariable classVariable)
        {
            var objectVariable = classVariable.CreateObject();
            RunConstructor(memory, objectVariable);
            return objectVariable;
        }
        else
        {
            throw new Exception("Parens must follow a function or class");
        }
    }
    
    private void RunConstructor(Memory memory, ObjectVariable objectVariable)
    {
        var constructor = objectVariable.Class.GetMember(memory, new MemberInstruction("__init__"));
        if (constructor is FunctionVariable constructorVariable)
        {
            // var stackUpdates = memory.CurrentStack.AddFrame(this, null, "__init__");
            constructorVariable.RunFunction(memory, new ArgumentSet(memory, Values!, objectVariable), this);
            // memory.CurrentStack.PopFrame(stackUpdates);
        }
    }
}