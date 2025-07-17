namespace Battlescript;

public class FunctionInstruction : Instruction
{
    public string Name { get; set; } 
    public List<Instruction> Parameters { get; set; }

    public FunctionInstruction(List<Token> tokens)
    {
        var tokensInParens = tokens.GetRange(3, tokens.Count - 5);
        var parameters = InstructionUtilities.ParseEntriesBetweenDelimiters(tokensInParens, [","]);
        CheckThatDefaultArgumentsFollowRequiredArguments();
        
        Name = tokens[1].Value;
        Parameters = parameters!;
        Line = tokens[0].Line;
        Column = tokens[0].Column;

        void CheckThatDefaultArgumentsFollowRequiredArguments()
        {
            var inDefaultArguments = false;
            foreach (var parameter in parameters)
            {
                if (parameter is AssignmentInstruction)
                {
                    inDefaultArguments = true;
                } else if (parameter is VariableInstruction && inDefaultArguments)
                {
                    throw new Exception("Required arguments have to be before default arguments, fix this later");
                }
            }
        }
    }

    public FunctionInstruction(string name, List<Instruction>? parameters = null, List<Instruction>? instructions = null)
    {
        Name = name;
        Parameters = parameters ?? [];
        Instructions = instructions ?? [];
    }

    public override Variable Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var functionValue = new FunctionVariable(Parameters, Instructions);
        memory.SetVariable(new VariableInstruction(Name), functionValue);
        return functionValue;
    }
}
