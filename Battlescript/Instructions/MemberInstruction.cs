namespace Battlescript;

public class MemberInstruction : Instruction
{
    public string Value { get; set; }

    public MemberInstruction(List<Token> tokens) : base(tokens)
    {
        Value = tokens[1].Value;
        ParseNext(tokens, 2);
    }

    public MemberInstruction(string value, Instruction? next = null) : base([])
    {
        Value = value;
        Next = next;
    }

    public override Variable? Interpret(
        CallStack callStack,
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        return instructionContext.GetMember(callStack, this, objectContext);
    }
}