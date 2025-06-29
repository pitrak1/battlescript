using System.Diagnostics;

namespace Battlescript;

public class SquareBracketsInstruction : GenericArrayInstruction<Instruction?>
{
    public SquareBracketsInstruction(List<Token> tokens)
    {
        if (tokens[0].Value == ".")
        {
            Values = [new StringInstruction(new List<Token>() { tokens[1] })];
            
            if (tokens.Count > 2)
            {
                Next = InstructionFactory.Create(tokens.GetRange(2, tokens.Count - 2));
            }
        }
        else
        {
            var closingSeparatorIndex = InstructionUtilities.GetTokenIndex(tokens, ["]"]);
            PopulateValues(tokens.GetRange(1, closingSeparatorIndex - 1), ",");

            if (tokens.Count > closingSeparatorIndex + 1)
            {
                Next = InstructionFactory.Create(tokens.GetRange(closingSeparatorIndex + 1, tokens.Count - closingSeparatorIndex - 1));
            }
        }
        
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public SquareBracketsInstruction(List<Instruction?> values, Instruction? next = null)
    {
        Values = values;
        Next = next;
    }

    public SquareBracketsInstruction(Instruction value, Instruction? next = null)
    {
        Values = [value];
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
            if (Values.Count == 1)
            {
                if (Values[0] is StringInstruction stringInstruction &&
                    Consts.ListMethods.Contains(stringInstruction.Value) &&
                    instructionContext is ListVariable listVariable)
                {
                    return listVariable.RunMethod(memory, stringInstruction.Value, Next);
                }
                else
                {
                    var result = instructionContext.GetItem(memory, new SquareBracketsInstruction([Values[0]]));
                    return Next is not null ? Next.Interpret(memory, result, instructionContext as ObjectVariable) : result;
                }
            }
            else
            {
                throw new Exception("Poorly formed index");
            }
        }
        // Dealing with list creation
        else
        {
            var values = new List<Variable>();
            
            foreach (var instructionValue in Values)
            {
                if (instructionValue is not null)
                {
                    values.Add(instructionValue.Interpret(memory));
                }
                else
                {
                    throw new Exception("Poorly formed list initialization");
                }
                
            }

            return new ListVariable(values);
        }
    }
}