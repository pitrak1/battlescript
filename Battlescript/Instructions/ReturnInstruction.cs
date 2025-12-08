using System.Diagnostics;

namespace Battlescript;

public class ReturnInstruction : Instruction
{
    public Instruction? Value { get; set; }

    public ReturnInstruction(List<Token> tokens) : base(tokens)
    {
        if (tokens.Count > 1)
        {
            Value = InstructionFactory.Create(tokens.GetRange(1, tokens.Count - 1));
        }
    }

    public ReturnInstruction(
        Instruction? value, 
        int? line = null, 
        string? expression = null) : base(line, expression)
    {
        Value = value;
    }

    public override Variable? Interpret(
        CallStack callStack,
        Closure closure,
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var returnValue = Value?.Interpret(callStack, closure);
        throw new InternalReturnException(returnValue);
    }
}