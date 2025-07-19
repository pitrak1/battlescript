using System.Diagnostics;

namespace Battlescript;

public class TryInstruction : Instruction
{
    public List<ExceptInstruction> Excepts { get; set; } = [];
    public ElseInstruction? Else { get; set; }
    public FinallyInstruction? Finally { get; set; }
    public TryInstruction(List<Token> tokens)
    {
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public TryInstruction(
        List<Instruction> instructions,
        List<ExceptInstruction>? excepts = null, 
        ElseInstruction? elseInstruction = null, 
        FinallyInstruction? finallyInstruction = null)
    {
        Instructions = instructions;
        Excepts = excepts ?? [];
        Else = elseInstruction;
        Finally = finallyInstruction;
    }
    
    public override Variable Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        try
        {
            foreach (var inst in Instructions)
            {
                memory.AddScope();
                inst.Interpret(memory);
                memory.RemoveScope();
            }
        }
        catch (InternalRaiseException e)
        {
            var isCaught = false;
            foreach (var except in Excepts)
            {
                var exceptionType = except.Value?.Interpret(memory);
                if ((e.Value as ObjectVariable).IsInstance(exceptionType as ClassVariable) && !isCaught)
                {
                    isCaught = true;
                    memory.AddScope();
                    foreach (var inst in except.Instructions)
                    {
                        inst.Interpret(memory);
                    }
                    memory.RemoveScope();
                }
            }

            if (!isCaught && Else is not null)
            {
                memory.AddScope();
                foreach (var inst in Else.Instructions)
                {
                    inst.Interpret(memory);
                }
                memory.RemoveScope();
            }

            if (Finally is not null)
            {
                memory.AddScope();
                foreach (var inst in Finally.Instructions)
                {
                    inst.Interpret(memory);
                }
                memory.RemoveScope();
            }

            if (!isCaught && Else is null)
            {
                throw e;
            }
        }
        
        return new ConstantVariable();
    }
}