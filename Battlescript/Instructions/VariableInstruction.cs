namespace Battlescript;

public class VariableInstruction : Instruction 
{
    public string Name { get; set; } 
    public Instruction? Next { get; set; }

    public VariableInstruction(List<Token> tokens)
    {
        Name = tokens[0].Value;
        Next = tokens.Count > 1 ? Parse(tokens.Slice(1, tokens.Count - 1)) : null;
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public VariableInstruction(string name, Instruction? next = null)
    {
        Name = name;
        Next = next;
    }

    public override Variable Interpret(
        Memory memory, 
        Variable? context = null, 
        Variable? objectContext = null)
    {
        var variable = memory.GetVariable(Name);

        var currentObjectContext = variable is ObjectVariable objectVariable ? objectVariable : null; 
        
        // This doesn't work because we're currently getting the variable including indexes from memory in GetVariable
        // but even if we just interpreted Parens here, we would still lose the context of the object we were workign with
        if (Next is not SquareBracketsInstruction && Next is not null)
        {
            return Next.Interpret(memory, variable, currentObjectContext);
        }
        else
        {
            return variable;
        }
    }
}