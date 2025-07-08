namespace Battlescript;

public class NumericInstruction : Instruction, IEquatable<NumericInstruction>
{
    private dynamic _value;
    public dynamic Value
    {
        get => _value;
        private set
        {
            if (value is not double && value is not int)
            {
                throw new Exception("Wrong type for numeric instruction");
            }
            else
            {
                _value = value;
            }
        }
    }

    public NumericInstruction(List<Token> tokens)
    {
        if (tokens.Count > 1)
        {
            throw new ParserUnexpectedTokenException(tokens[1]);
        }

        if (tokens[0].Value.Contains('.'))
        {
            _value = double.Parse(tokens[0].Value);
        }
        else
        {
            _value = int.Parse(tokens[0].Value);
        }
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public NumericInstruction(dynamic value)
    {
        _value = value;
    }
    
    public override Variable Interpret(        
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        if (Value is double)
        {
            var floatClass = memory.BuiltInReferences["float"];
            var floatObject = floatClass.CreateObject();
            floatObject.Values["__value"] = new NumericVariable(Value);
            return floatObject;
        }
        else
        {
            var intClass = memory.BuiltInReferences["int"];
            var intObject = intClass.CreateObject();
            intObject.Values["__value"] = new NumericVariable(Value);
            return intObject;
        }
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as NumericInstruction);
    public bool Equals(NumericInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;

        if (Value != instruction.Value) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Value, Instructions);
}