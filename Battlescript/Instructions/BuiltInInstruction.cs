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
        var endOfArgumentsIndex = InstructionUtilities.GetTokenIndex(tokens, [")"]);
        var argumentTokens = tokens.GetRange(2, endOfArgumentsIndex - 2);
        
        
        Arguments = InstructionUtilities.ParseEntriesBetweenDelimiters(argumentTokens, [","])!;
        
        ParseNext(tokens, endOfArgumentsIndex + 1);
        
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
    
    public override Variable? Interpret(
        CallStack callStack,
        Closure closure,
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
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
    
}