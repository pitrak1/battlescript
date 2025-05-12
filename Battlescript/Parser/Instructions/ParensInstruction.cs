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

    public override Variable Interpret(
        Memory memory, 
        Variable? context = null, 
        Variable? objectContext = null)
    {
        Debug.Assert(context is not null);

        if (context is FunctionVariable functionVariable)
        {

            var classScopesAdded = 0;
            if (objectContext is ObjectVariable objectVariable)
            {
                var classScopes = objectVariable.ClassVariable.AddClassToMemoryScopes(memory);
                memory.AddScope(objectVariable.Values);
                classScopesAdded += classScopes + 1;
            }
            
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
            var returnValue = memory.GetVariable("return");
            
            memory.RemoveScope();

            for (var i = 0; i < classScopesAdded; i++)
            {
                memory.RemoveScope();
            }
            
            return returnValue ?? new NullVariable();
        }
        else
        {
            if (context is ClassVariable classVariable)
            {
                return classVariable.CreateObject();
            }
            else
            {
                throw new Exception("Can only create an object of a class");
            }
        }
    }
}