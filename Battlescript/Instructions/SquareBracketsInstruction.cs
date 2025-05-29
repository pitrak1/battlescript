using System.Diagnostics;

namespace Battlescript;

public class SquareBracketsInstruction : Instruction, IEquatable<SquareBracketsInstruction>
{
    public List<Instruction> Values { get; set; }
    public Instruction? Next { get; set; }

    public SquareBracketsInstruction(List<Token> tokens, bool isMember = false)
    {
        if (isMember)
        {
            var indexValue = new StringInstruction(new List<Token>() { tokens[1] });
            Instruction? next = null;
            if (tokens.Count > 2)
            {
                next = Parse(tokens.GetRange(2, tokens.Count - 2));
            }

            // It seems like the easiest way to handle using the period for accessing members is to treat it exactly
            // like a square bracket (i.e. x.asdf = x["asdf"]).  This may change later once I know python better :P
            Values = new List<Instruction> { indexValue };
            Next = next;
        }
        else
        {
            var results = ParserUtilities.ParseEntriesWithinSeparator(tokens, [","]);

            Instruction? next = null;
            if (tokens.Count > results.Count)
            {
                next = Parse(tokens.GetRange(results.Count, tokens.Count - results.Count));
            }

            Values = results.Values;
            Next = next;
        }
        
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public SquareBracketsInstruction(List<Instruction> values, Instruction? next = null)
    {
        Values = values;
        Next = next;
    }

    public override Variable Interpret(
        Memory memory, 
        Variable? context = null, 
        Variable? objectContext = null)
    {
        // Dealing with an index
        if (context is not null)
        {
            if (Values.Count > 1) throw new Exception("Too many index values");
            
            var result = context.GetItem(memory, this);
            return Next is not null ? Next.Interpret(memory, result, context) : result;
        }
        // Dealing with list creation
        else
        {
            var values = new List<Variable>();
            foreach (var instructionValue in Values)
            {
                values.Add(instructionValue.Interpret(memory));
            }
            return new ListVariable(values);
        }
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as SquareBracketsInstruction);
    public bool Equals(SquareBracketsInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;

        if (!Values.SequenceEqual(instruction.Values)) return false;
        
        if (Next is not null && !Next.Equals(instruction.Next)) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Values, Next, Instructions);
}