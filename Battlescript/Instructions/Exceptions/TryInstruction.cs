using System.Diagnostics;

namespace Battlescript;

public class TryInstruction : Instruction
{
    public List<ExceptInstruction> Excepts { get; set; } = [];
    public ElseInstruction? Else { get; set; }
    public FinallyInstruction? Finally { get; set; }
    public TryInstruction(List<Token> tokens) : base(tokens) {}

    public TryInstruction(
        List<Instruction> instructions,
        List<ExceptInstruction>? excepts = null, 
        ElseInstruction? elseInstruction = null, 
        FinallyInstruction? finallyInstruction = null) : base([])
    {
        Instructions = instructions;
        Excepts = excepts ?? [];
        Else = elseInstruction;
        Finally = finallyInstruction;
    }
    
    public override Variable? Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        try
        {
            foreach (var inst in Instructions)
            {
                inst.Interpret(memory);
            }
            
            if (Finally is not null)
            {
                foreach (var inst in Finally.Instructions)
                {
                    inst.Interpret(memory);
                }
            }
        }
        catch (InternalRaiseException e)
        {
            var isCaught = false;
            foreach (var except in Excepts)
            {
                if (except.ExceptionType.Name == e.Type)
                {
                    isCaught = true;
                    
                    if (except.ExceptionVariable is not null)
                    {
                        var exceptionVariable = memory.CreateException(e.Type, e.Message);
                        memory.AddVariableToLastScope(except.ExceptionVariable, exceptionVariable);
                    }
                    
                    foreach (var inst in except.Instructions)
                    {
                        inst.Interpret(memory);
                    }
                    break;
                }
            }

            if (!isCaught && Else is not null)
            {
                foreach (var inst in Else.Instructions)
                {
                    inst.Interpret(memory);
                }
            }

            if (Finally is not null)
            {
                foreach (var inst in Finally.Instructions)
                {
                    inst.Interpret(memory);
                }
            }

            if (!isCaught && Else is null)
            {
                throw e;
            }
        }
        
        return null;
    }
}