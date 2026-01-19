namespace Battlescript;

public class NumericInstruction : Instruction
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

    public NumericInstruction(List<Token> tokens) : base(tokens)
    {
        if (tokens.Count > 1)
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "invalid syntax");
        }

        if (tokens[0].Value.Contains('.'))
        {
            _value = double.Parse(tokens[0].Value);
        }
        else
        {
            _value = int.Parse(tokens[0].Value);
        }
    }

    public NumericInstruction(dynamic value, int? line = null, string? expression = null) : base(line, expression)
    {
        _value = value;
    }
    
    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        if (Value is double)
        {
            return BsTypes.Create(BsTypes.Types.Float, Value);
        }
        else
        {
            return BsTypes.Create(BsTypes.Types.Int, Value);
        }
    }
    
    // All the code below is to override equality
    public override bool Equals(object? obj) => Equals(obj as NumericInstruction);
    public bool Equals(NumericInstruction? inst)
    {
        if (inst is null) return false;
        if (ReferenceEquals(this, inst)) return true;
        if (GetType() != inst.GetType()) return false;
        
        return Value.Equals(inst.Value);
    }
    
    public override int GetHashCode() => Value.GetHashCode() * 70;
}