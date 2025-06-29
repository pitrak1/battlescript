namespace Battlescript;

public class GenericArrayInstruction<T> : Instruction, IEquatable<GenericArrayInstruction<T>>
{
    protected List<T> _values = [];

    public List<T> Values
    {
        get => _values; 
        set => _values = value;
    }
    
    public Instruction? Next { get; set; }

    public void PopulateValues(List<Token> tokens, string separator)
    {
        var separatorIndex = InstructionUtilities.GetTokenIndex(tokens, [separator]);
        
        if (separatorIndex != -1)
        {
            var results = InstructionUtilities.ParseEntriesBetweenSeparatingCharacters(tokens, [separator]);

            // I'm sure there's a better way of casting this, but I'm tired of wrestling with it
            var values = new List<T>();
            foreach (var result in results)
            {
                if (result is T value)
                { 
                    values.Add(value);
                }
                else
                {
                    throw new Exception("Invalid input to GenericArrayInstruction");
                }
            }
            
            Values = values;
            Line = tokens[separatorIndex].Line;
            Column = tokens[separatorIndex].Column;
        }
        else
        {
            var value = InstructionFactory.Create(tokens);
            
            if (value is T typedValue)
            {
                Values = [typedValue];
                Line = tokens[0].Line;
                Column = tokens[0].Column;
            } else if (value is null)
            {
                Values = [];
            }
            else
            {
                throw new Exception("Invalid input to GenericArrayInstruction");
            }
        }
    }
    
    public void PopulateValues(List<T> values)
    {
        Values = values;
    }

    public override Variable Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var values = new List<Variable?>();
        foreach (var instruction in Instructions)
        {
            values.Add(instruction.Interpret(memory, instructionContext, objectContext, lexicalContext));
        }
        return new ListVariable(values);
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as GenericArrayInstruction<T>);
    public bool Equals(GenericArrayInstruction<T>? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;
        
        if (Next is not null && !Next.Equals(instruction.Next)) return false;
        if (!Values.SequenceEqual(instruction.Values)) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Values);
}