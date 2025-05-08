namespace Battlescript;

public class FunctionInstruction : Instruction 
{
    public string Name { get; set; } 
    public List<Instruction> Parameters { get; set; }

    public FunctionInstruction(List<Token> tokens)
    {
        // def keyword, function name, open parens, close parens, and colon
        if (tokens.Count < 5)
        {
            ThrowErrorForToken("Invalid function definition", tokens[0]);
        }
        
        if (tokens[^1].Value != ":")
        {
            ThrowErrorForToken("Function definition should end with colon", tokens[0]);
        }
        
        if (tokens[1].Type != Consts.TokenTypes.Identifier)
        {
            ThrowErrorForToken("Invalid function name", tokens[1]);
        }

        if (tokens[2].Type != Consts.TokenTypes.Separator || tokens[2].Value != "(")
        {
            ThrowErrorForToken("Expected ( for function definition", tokens[2]);
        }
        
        if (tokens[^2].Type != Consts.TokenTypes.Separator || tokens[^2].Value != ")")
        {
            ThrowErrorForToken("Expected ) for function definition", tokens[^2]);
        }

        var tokensInParens = tokens.GetRange(2, tokens.Count - 2);
        var results = ParseAndRunEntriesWithinSeparator(tokensInParens, [","]);
        Name = tokens[1].Value;
        Parameters = results.Values;
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public FunctionInstruction(string name, List<Instruction>? parameters = null, List<Instruction>? instructions = null)
    {
        Name = name;
        Parameters = parameters ?? [];
        Instructions = instructions ?? [];
    }

    public override Variable Interpret(Memory memory, Variable? context = null)
    {
        var functionValue = new FunctionVariable(Parameters, Instructions);
        memory.AssignToVariable(new VariableInstruction(Name), functionValue);
        return functionValue;
    }
}
