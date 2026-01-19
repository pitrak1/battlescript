using Battlescript.BuiltIn;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Battlescript;

public class BuiltInInstruction : Instruction
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
    
    // All the code below is to override equality
    public override bool Equals(object? obj) => Equals(obj as BuiltInInstruction);
    public bool Equals(BuiltInInstruction? inst)
    {
        if (inst is null) return false;
        if (ReferenceEquals(this, inst)) return true;
        if (GetType() != inst.GetType()) return false;
        
        var argumentsEqual = Arguments.SequenceEqual(inst.Arguments);
        return argumentsEqual && Name == inst.Name && Equals(Next, inst.Next);
    }
    
    public override int GetHashCode() => HashCode.Combine(Arguments, Name, Next);
}