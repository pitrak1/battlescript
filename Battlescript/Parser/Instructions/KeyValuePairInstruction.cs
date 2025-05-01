namespace Battlescript;

public class KeyValuePairInstruction : Instruction 
{
    public Instruction Left { get; set; } 
    public Instruction Right { get; set; }

    public KeyValuePairInstruction(List<Token> tokens)
    {
        var colonIndex = ParserUtilities.GetTokenIndex(tokens, [":"]);
        var colonToken = tokens[colonIndex];
        var result = RunLeftAndRightAroundIndex(tokens, colonIndex);
        
        Left = result.Left;
        Right = result.Right;
        Line = colonToken.Line;
        Column = colonToken.Column;
    }

    public KeyValuePairInstruction(Instruction left, Instruction right)
    {
        Left = left;
        Right = right;
    }

    public override Variable Interpret(Memory memory, Variable? context = null)
    {
        throw new Exception("Should never interpret a KVP instruction directly");
    }
}