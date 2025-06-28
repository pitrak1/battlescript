using System.Diagnostics;

namespace Battlescript;

public class SquareBracketsInstruction : Instruction, IEquatable<SquareBracketsInstruction>
{
    public Instruction? Value { get; set; }
    public Instruction? Next { get; set; }

    public SquareBracketsInstruction(List<Token> tokens)
    {
        if (tokens[0].Value == ".")
        {
            var indexValue = new StringInstruction(new List<Token>() { tokens[1] });
            Instruction? next = null;
            if (tokens.Count > 2)
            {
                next = Parse(tokens.GetRange(2, tokens.Count - 2));
            }

            // It seems like the easiest way to handle using the period for accessing members is to treat it exactly
            // like a square bracket (i.e. x.asdf = x["asdf"]).  This may change later once I know python better :P
            Value = indexValue;
            Next = next;
        }
        else
        {
            var closingSeparator = Consts.MatchingSeparatorsMap[tokens[0].Value];
            var closingSeparatorIndex = ParserUtilities.GetTokenIndex(tokens, [closingSeparator]);
            var tokensWithinSeparator = tokens.GetRange(1, closingSeparatorIndex - 1);

            Instruction? next = null;
            if (tokens.Count > closingSeparatorIndex + 1)
            {
                next = Parse(tokens.GetRange(closingSeparatorIndex + 1, tokens.Count - closingSeparatorIndex - 1));
            }

            Value = Instruction.Parse(tokensWithinSeparator);
            Next = next;
        }
        
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public SquareBracketsInstruction(Instruction value, Instruction? next = null)
    {
        Value = value;
        Next = next;
    }

    public override Variable Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        // Dealing with an index
        if (instructionContext is not null)
        {
            if (Value is StringInstruction stringInstruction &&
                Consts.ListMethods.Contains(stringInstruction.Value) &&
                instructionContext is ListVariable listVariable)
            {
                return listVariable.RunMethod(memory, stringInstruction.Value, Next);
            }
            else
            {
                var result = instructionContext.GetItem(memory, this);
                return Next is not null ? Next.Interpret(memory, result, instructionContext as ObjectVariable) : result;
            }
        }
        // Dealing with list creation
        else
        {
            var values = new List<Variable>();
            if (Value is ArrayInstruction arrayInstruction)
            {
                foreach (var instructionValue in arrayInstruction.Values)
                {
                    values.Add(instructionValue.Interpret(memory));
                }
            }
            else if (Value is not null)
            {
                values.Add(Value.Interpret(memory));
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

        if (!Value.Equals(instruction.Value)) return false;
        
        if (Next is not null && !Next.Equals(instruction.Next)) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Value, Next, Instructions);
}