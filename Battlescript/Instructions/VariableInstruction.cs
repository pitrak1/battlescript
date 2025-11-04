namespace Battlescript;

public class VariableInstruction : Instruction
{
    public string Name { get; set; } 

    public VariableInstruction(List<Token> tokens) : base(tokens)
    {
        Name = tokens[0].Value;
        ParseNext(tokens, 1);
    }

    public VariableInstruction(string name, Instruction? next = null) : base([])
    {
        Name = name;
        Next = next;
    }

    public override Variable? Interpret(
        CallStack callStack, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var variable = callStack.GetVariable(Name);

        if (Next is null)
        {
            return variable;
        }
        else
        {
            if (variable is ObjectVariable objectVariable)
            {
                return Next.Interpret(callStack, variable, objectVariable);
            }
            
            return Next.Interpret(callStack, variable);
        }
    }
}