using System.Diagnostics;

namespace Battlescript;

public class ParensInstruction : Instruction
{
    public Instruction? Next { get; set; }

    public ParensInstruction(List<Token> tokens)
    {
        var results = ParseAndRunEntriesWithinSeparator(tokens, [","]);
        var next = CheckAndRunFollowingTokens(tokens, results.Count);

        Instructions = results.Values;
        Next = next;
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public ParensInstruction(List<Instruction> instructions, Instruction? next = null)
    {
        Instructions = instructions;
        Next = next;
    }

    public override Variable Interpret(Memory memory, Variable? context = null)
    {
        Debug.Assert(context is not null);

        if (context is FunctionVariable functionVariable)
        {
            memory.AddScope();
            
            // Transferring arguments to local parameter variables
            for (var i = 0; i < Instructions.Count; i++)
            {
                Debug.Assert(functionVariable.Parameters[i] is VariableInstruction);
                var paramInstruction = (VariableInstruction)functionVariable.Parameters[i];
                var value = Instructions[i].Interpret(memory);
                memory.AssignToVariable(new VariableInstruction(paramInstruction.Name), value);
            }
            
            // Actually running hte instructions
            foreach (var inst in functionVariable.Instructions)
            {
                inst.Interpret(memory);
            }
            
            // Getting return value
            var returnValue = memory.GetVariable(new VariableInstruction("return"));
            
            memory.RemoveScope();
            
            return returnValue ?? new NullVariable();
        }
        else
        {
            // need to update this for the new format
            // return context.CreateObject();
            return new NullVariable();
        }
    }
}