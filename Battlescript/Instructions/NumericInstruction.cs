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
    }

    public NumericInstruction(dynamic value) : base([])
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
            var floatClass = memory.GetBuiltIn(BsTypes.Types.Float);
            var floatObject = floatClass.CreateObject();
            floatObject.Values["__value"] = new NumericVariable(Value);
            return floatObject;
        }
        else
        {
            var intClass = memory.GetBuiltIn(BsTypes.Types.Int);
            var intObject = intClass.CreateObject();
            intObject.Values["__value"] = new NumericVariable(Value);
            return intObject;
        }
    }
}