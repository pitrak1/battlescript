using System.Diagnostics;

namespace Battlescript;

public class ParenthesesInstruction : ArrayInstruction
{
    public ParenthesesInstruction(List<Token> tokens) : base([])
    {
        var closingSeparatorIndex = InstructionUtilities.GetTokenIndex(tokens, [")"]);
        if (tokens.Count > 2)
        {
            var tokensInSeparators = tokens.GetRange(1, closingSeparatorIndex - 1);
            InitializeDelimiter(tokensInSeparators);
            InitializeValues(tokensInSeparators);
            ParseNext(tokens, closingSeparatorIndex + 1);
        }
        Initialize(tokens);
    }
    
    public ParenthesesInstruction(
        List<Instruction?> values, 
        string? delimiter = null,
        Instruction? next = null, 
        int? line = null, 
        string? expression = null) : base(values, delimiter, next, line, expression)
    {
    }

    public override Variable? Interpret(
        CallStack callStack,
        Closure closure,
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        if (instructionContext is FunctionVariable functionVariable)
        {
            // var stackUpdates = callStack.CurrentStack.AddFrame(this, null, functionVariable.Name);
            var returnValue = functionVariable.RunFunction(callStack, closure, new ArgumentSet(callStack, closure, Values!, objectContext), this);
            // callStack.CurrentStack.PopFrame(stackUpdates);
            return returnValue;
        }
        else if (instructionContext is ClassVariable classVariable)
        {
            var objectVariable = classVariable.CreateObject();
            RunConstructor(callStack, closure, objectVariable);
            return objectVariable;
        }
        else
        {
            throw new Exception("Parens must follow a function or class");
        }
    }
    
    private void RunConstructor(CallStack callStack, Closure closure, ObjectVariable objectVariable)
    {
        var constructor = objectVariable.Class.GetMember(callStack, closure, new MemberInstruction("__init__"));
        if (constructor is FunctionVariable constructorVariable)
        {
            // var stackUpdates = callStack.CurrentStack.AddFrame(this, null, "__init__");
            constructorVariable.RunFunction(callStack, closure, new ArgumentSet(callStack, closure, Values!, objectVariable), this);
            // callStack.CurrentStack.PopFrame(stackUpdates);
        }
    }
}