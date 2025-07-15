namespace Battlescript;

public class VariableInstruction : Instruction
{
    public string Name { get; set; } 

    public VariableInstruction(List<Token> tokens)
    {
        Name = tokens[0].Value;
        ParseNext(tokens, 1);
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
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var variable = memory.GetVariable(Name);

        if (Next is null)
        {
            return variable;
        }
        else
        {
            if (variable is ObjectVariable objectVariable)
            {
                return Next.Interpret(memory, variable, objectVariable);
            }
            
            return Next.Interpret(memory, variable);
        }
    }
}