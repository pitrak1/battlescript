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
            return functionVariable.RunFunction(callStack, closure, new ArgumentSet(callStack, closure, Values!), this);
        }
        else if (instructionContext is ClassVariable classVariable)
        {
            var objectVariable = classVariable.CreateObject();
            var constructor = objectVariable.GetMember(callStack, closure, new MemberInstruction("__init__"));
            if (constructor is FunctionVariable constructorVariable)
            {
                constructorVariable.RunFunction(callStack, closure, new ArgumentSet(callStack, closure, Values!), this);
            }
            return objectVariable;
        }
        else
        {
            throw new Exception("Parens must follow a function or class");
        }
    }
}