namespace Battlescript;

public class PrincipleTypeInstruction : Instruction
{
    public string Value { get; set; }
    public List<Instruction> Parameters { get; set; }

    public PrincipleTypeInstruction(List<Token> tokens) : base(tokens)
    {
        if (tokens.Count > 1)
        {
            var endOfArgumentsIndex = InstructionUtilities.GetTokenIndex(tokens, [")"]);
            var argumentTokens = tokens.GetRange(2, endOfArgumentsIndex - 2);
            Parameters = InstructionUtilities.ParseEntriesBetweenDelimiters(argumentTokens, [","])!;
        
            if (tokens.Count > endOfArgumentsIndex + 1)
            {
                throw new ParserUnexpectedTokenException(tokens[endOfArgumentsIndex + 1]);
            }
        }
        
        Value = tokens[0].Value;
    }

    public PrincipleTypeInstruction(string value) : base([])
    {
        Value = value;
    }
    
    public override Variable? Interpret(        
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        switch (Value)
        {
            case "__numeric__":
                if (Parameters.Count == 1 && Parameters[0] is NumericInstruction numInstruction)
                {
                    return new NumericVariable(numInstruction.Value);
                }
                return new NumericVariable(0);
            case "__sequence__":
                return new SequenceVariable();
            case "__mapping__":
                return new MappingVariable();
            case "__string__":
                return new StringVariable();
        }
        return null;
    }
}