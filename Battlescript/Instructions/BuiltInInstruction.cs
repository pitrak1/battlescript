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
    
    public BuiltInInstruction(string name, List<Instruction>? arguments = null, Instruction? next = null) : base([])
    {
        Name = name;
        Arguments = arguments ?? [];
        Next = next;
    }
    
    public override Variable? Interpret(
        CallStack callStack, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        switch (Name)
        {
            case "abs":
                return BuiltInAbs.Run(callStack, Arguments);
            case "range":
                return BuiltInRange.Run(callStack, Arguments);
            case "isinstance":
                return BuiltInIsInstance.Run(callStack, Arguments);
            case "issubclass":
                return BuiltInIsSubclass.Run(callStack, Arguments);
            case "print":
                BuiltInPrint.Run(callStack, Arguments);
                break;
            case "type":
                return BuiltInType.Run(callStack, Arguments);
            case "len":
                return BuiltInLen.Run(callStack, Arguments);
        }
        // TODO
        return new ConstantVariable();
    }
    
}