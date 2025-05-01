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
        
        if (context.Type == Consts.VariableTypes.Function)
        {
            memory.AddScope();
            
            // Transferring arguments to local parameter variables
            for (var i = 0; i < Instructions.Count; i++)
            {
                var variable = memory.GetAndCreateIfNotExists(context.Value[i].Name);
                var value = Instructions[i].Interpret(memory);
                variable.Set(value);
            }
            
            // Actually running hte instructions
            foreach (var inst in context.Instructions)
            {
                inst.Interpret(memory);
            }
            
            // Getting return value
            var returnValue = memory.Get("return");
            
            memory.RemoveScope();
            
            return returnValue ?? new Variable(Consts.VariableTypes.Null, null);
        }
        else
        {
            return context.CreateObject();
        }
    }
}