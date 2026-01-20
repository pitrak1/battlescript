using Battlescript.BuiltIn;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Battlescript;

public class BuiltInInstruction : Instruction, IEquatable<BuiltInInstruction>
{
    public string Name { get; set; }
    public List<Instruction> Arguments { get; set; }
    
    public BuiltInInstruction(List<Token> tokens) : base(tokens)
    {
        var tokensAfterBuiltInName = tokens.GetRange(1, tokens.Count - 1);
        var argumentTokens = InstructionUtilities.GetGroupedTokensAtStart(tokensAfterBuiltInName);
        Arguments = InstructionUtilities.ParseEntriesBetweenDelimiters(argumentTokens, [","])!;
        ParseNext(tokens, argumentTokens.Count + 3);
        Name = tokens[0].Value;
    }
    
    public BuiltInInstruction(
        string name, 
        List<Instruction>? arguments = null, 
        Instruction? next = null, 
        int? line = null, 
        string? expression = null) : base(line, expression)
    {
        Name = name;
        Arguments = arguments ?? [];
        Next = next;
    }
    
    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        switch (Name)
        {
            case "abs":
                return BuiltInAbs.Run(callStack, closure, Arguments);
            case "range":
                return BuiltInRange.Run(callStack, closure, Arguments);
            case "isinstance":
                return BuiltInIsInstance.Run(callStack, closure, Arguments);
            case "issubclass":
                return BuiltInIsSubclass.Run(callStack, closure, Arguments);
            case "print":
                BuiltInPrint.Run(callStack, closure, Arguments);
                break;
            case "type":
                return BuiltInType.Run(callStack, closure, Arguments);
            case "len":
                return BuiltInLen.Run(callStack, closure, Arguments);
        }
        // TODO
        return new ConstantVariable();
    }
    
    #region Equality

    public override bool Equals(object? obj) => obj is BuiltInInstruction inst && Equals(inst);

    public bool Equals(BuiltInInstruction? other) =>
        other is not null &&
        Arguments.SequenceEqual(other.Arguments) &&
        Name == other.Name &&
        Equals(Next, other.Next);

    public override int GetHashCode() => HashCode.Combine(Arguments, Name, Next);

    public static bool operator ==(BuiltInInstruction? left, BuiltInInstruction? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(BuiltInInstruction? left, BuiltInInstruction? right) => !(left == right);

    #endregion
}