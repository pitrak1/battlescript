using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Battlescript;

public class BuiltInInstruction : Instruction
{
    public string Name { get; set; } 
    public List<Instruction> Parameters { get; set; }
    
    public BuiltInInstruction(List<Token> tokens)
    {
        var endOfArgumentsIndex = InstructionUtilities.GetTokenIndex(tokens, [")"]);
        var argumentTokens = tokens.GetRange(2, endOfArgumentsIndex - 2);
        
        
        Parameters = InstructionUtilities.ParseEntriesBetweenDelimiters(argumentTokens, [","])!;
        
        ParseNext(tokens, endOfArgumentsIndex + 1);
        
        Name = tokens[0].Value;
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }
    
    public BuiltInInstruction(string name, List<Instruction>? parameters = null, Instruction? next = null)
    {
        Name = name;
        Parameters = parameters ?? [];
        Next = next;
    }
    
    public override Variable Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        switch (Name)
        {
            case "super":
                break;
            case "range":
                return RunRangeFunction(memory);
            case "isinstance":
                return RunIsInstanceFunction(memory);
            case "issubclass":
                return RunIsSubclassFunction(memory);
            case "print":
                return RunPrint(memory);
        }
        // TODO
        return new ConstantVariable();
    }

    private Variable RunRangeFunction(Memory memory)
    {
        int startingValue = 0;
        int count = 0;
        int step = 1;

        if (Parameters.Count == 1)
        {
            var countExpression = Parameters[0].Interpret(memory);
            count = BuiltInTypeHelper.GetIntValueFromVariable(memory, countExpression);
        } else if (Parameters.Count == 2)
        {
            var startingValueExpression = Parameters[0].Interpret(memory);
            var countExpression = Parameters[1].Interpret(memory);
            startingValue = BuiltInTypeHelper.GetIntValueFromVariable(memory, startingValueExpression);
            count = BuiltInTypeHelper.GetIntValueFromVariable(memory, countExpression);
        } else if (Parameters.Count == 3)
        {
            var startingValueExpression = Parameters[0].Interpret(memory);
            var countExpression = Parameters[1].Interpret(memory);
            var stepExpression = Parameters[2].Interpret(memory);
            startingValue = BuiltInTypeHelper.GetIntValueFromVariable(memory, startingValueExpression);
            count = BuiltInTypeHelper.GetIntValueFromVariable(memory, countExpression);
            step = BuiltInTypeHelper.GetIntValueFromVariable(memory, stepExpression);
        }
        else
        {
            throw new Exception("Bad arguments, clean this up later");
        }
        
        List<Variable> values = [];

        if (startingValue < count)
        {
            if (step > 0)
            {
                for (var i = startingValue; i < count; i += step)
                {
                    values.Add(BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", i));
                }
            }
            
            return new ListVariable(values);
        }
        else
        {
            if (step < 0)
            {
                for (var i = startingValue; i > count; i += step)
                {
                    values.Add(BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", i));
                }
            }
            return new ListVariable(values);
        }
    }

    private ConstantVariable RunIsInstanceFunction(Memory memory)
    {
        if (Parameters.Count != 2)
        {
            throw new Exception("Bad arguments, clean this up later");
        }
        
        var objectExpression = Parameters[0].Interpret(memory);
        var classExpression = Parameters[1].Interpret(memory);

        if (objectExpression is ObjectVariable objectVariable && classExpression is ClassVariable classVariable)
        {
            return new ConstantVariable(objectVariable.IsInstance(classVariable));
        }
        else
        {
            return new ConstantVariable(false);
        }
    }
    
    private ConstantVariable RunIsSubclassFunction(Memory memory)
    {
        if (Parameters.Count != 2)
        {
            throw new Exception("Bad arguments, clean this up later");
        }
        
        var firstExpression = Parameters[0].Interpret(memory);
        var secondExpression = Parameters[1].Interpret(memory);

        if (firstExpression is ClassVariable firstVariable && secondExpression is ClassVariable secondVariable)
        {
            return new ConstantVariable(firstVariable.IsSubclass(secondVariable));
        }
        else
        {
            throw new Exception("Bad arguments, clean this up later");
        }
    }

    private ConstantVariable RunPrint(Memory memory)
    {
        if (Parameters.Count != 1)
        {
            throw new Exception("Bad arguments, clean this up later");
        }
        
        var firstExpression = Parameters[0].Interpret(memory);
        if (firstExpression is StringVariable stringVariable)
        {
            Console.WriteLine(stringVariable.Value);
            return new ConstantVariable();
        }
        else if (firstExpression is NumericVariable numericVariable)
        {
            Console.WriteLine(numericVariable.Value);
            return new ConstantVariable();
        }
        else if (firstExpression is ObjectVariable objectVariable)
        {
            
            var jsonString = JsonConvert.SerializeObject(
                objectVariable.Values, Formatting.Indented,
                new JsonConverter[] {new StringEnumConverter()});
            Console.WriteLine(jsonString);
            return new ConstantVariable();
        }
        else
        {
            throw new Exception("Bad arguments, clean this up later");
        }
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as BuiltInInstruction);
    public bool Equals(BuiltInInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;
        
        if (!Parameters.SequenceEqual(instruction.Parameters) || Name != instruction.Name || !Next.Equals(instruction.Next)) return false;
        
        return true;
    }
    
    public override int GetHashCode() => HashCode.Combine(Parameters, Name);
}