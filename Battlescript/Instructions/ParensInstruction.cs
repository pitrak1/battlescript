using System.Diagnostics;

namespace Battlescript;

public class ParensInstruction : Instruction, IEquatable<ParensInstruction>
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
            var objectVariable = objectContext is ObjectVariable ? (ObjectVariable)objectContext : null;
            return functionVariable.RunFunction(memory, Instructions, objectVariable);
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
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as ParensInstruction);
    public bool Equals(ParensInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;

        if (Next != instruction.Next) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Next, Instructions);
}