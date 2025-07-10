namespace Battlescript;

public class ConstantInstruction : Instruction, IEquatable<ConstantInstruction>
{
    public string Value { get; set; }

    public ConstantInstruction(List<Token> tokens)
    {
        Value = tokens[0].Value;
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public ConstantInstruction(string value)
    {
        Value = value;
    }

    public override Variable Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        if (Value == "True" || Value == "False")
        {
            var boolClass = memory.BuiltInReferences["bool"];
            var boolObject = boolClass.CreateObject();
            boolObject.Values["__value"] = new NumericVariable(Value == "True" ? 1 : 0);
            return boolObject;
        }
        else
        {
            return new ConstantVariable();
        }
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as ConstantInstruction);
    public bool Equals(ConstantInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;

        if (Value != instruction.Value) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Value, Instructions);
}