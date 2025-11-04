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
        Closure closure,
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var variable = callStack.GetVariable(closure, Name);

        if (Next is null)
        {
            return variable;
        }
        else
        {
            if (variable is ObjectVariable objectVariable)
            {
                return Next.Interpret(callStack, closure, variable, objectVariable);
            }
            
            return Next.Interpret(callStack, closure, variable);
        }
    }
}