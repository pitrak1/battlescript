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
            case "range":
                return RunRangeFunction(memory);
            case "isinstance":
                return RunIsInstanceFunction(memory);
            case "issubclass":
                return RunIsSubclassFunction(memory);
            case "print":
                return RunPrint(memory);
            case "len":
                return RunLen(memory);
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
            count = BsTypes.GetIntValue(memory, countExpression);
        } else if (Parameters.Count == 2)
        {
            var startingValueExpression = Parameters[0].Interpret(memory);
            var countExpression = Parameters[1].Interpret(memory);
            startingValue = BsTypes.GetIntValue(memory, startingValueExpression);
            count = BsTypes.GetIntValue(memory, countExpression);
        } else if (Parameters.Count == 3)
        {
            var startingValueExpression = Parameters[0].Interpret(memory);
            var countExpression = Parameters[1].Interpret(memory);
            var stepExpression = Parameters[2].Interpret(memory);
            startingValue = BsTypes.GetIntValue(memory, startingValueExpression);
            count = BsTypes.GetIntValue(memory, countExpression);
            step = BsTypes.GetIntValue(memory, stepExpression);
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
                    values.Add(BsTypes.Create(memory, "int", i));
                }
            }
            
            return BsTypes.Create(memory, "list", values);
        }
        else
        {
            if (step < 0)
            {
                for (var i = startingValue; i > count; i += step)
                {
                    values.Add(BsTypes.Create(memory, "int", i));
                }
            }
            return BsTypes.Create(memory, "list", values);
        }
    }

    private Variable RunIsInstanceFunction(Memory memory)
    {
        if (Parameters.Count != 2)
        {
            throw new Exception("Bad arguments, clean this up later");
        }
        
        var objectExpression = Parameters[0].Interpret(memory);
        if (Parameters[1] is PrincipleTypeInstruction principleTypeInstruction)
        {
            switch (principleTypeInstruction.Value)
            {
                case "__numeric__":
                    return BsTypes.Create(memory, "bool",
                        objectExpression is NumericVariable);
                case "__sequence__":
                    return BsTypes.Create(memory, "bool", objectExpression is SequenceVariable);
                default:
                    return BsTypes.Create(memory, "bool", false);
            }
        }
        else
        {
            var classExpression = Parameters[1].Interpret(memory);
        
            if (objectExpression is ObjectVariable objectVariable && classExpression is ClassVariable classVariable)
            {
                return BsTypes.Create(memory, "bool", objectVariable.IsInstance(classVariable));
            }
            else
            {
                return BsTypes.Create(memory, "bool", false);
            }
        }
    }
    
    private Variable RunIsSubclassFunction(Memory memory)
    {
        if (Parameters.Count != 2)
        {
            throw new Exception("Bad arguments, clean this up later");
        }
        
        var firstExpression = Parameters[0].Interpret(memory);
        var secondExpression = Parameters[1].Interpret(memory);

        if (firstExpression is ClassVariable firstVariable && secondExpression is ClassVariable secondVariable)
        {
            return BsTypes.Create(memory, "bool", firstVariable.IsSubclass(secondVariable));
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
        else if (firstExpression is SequenceVariable sequenceVariable)
        {
            var jsonString = JsonConvert.SerializeObject(
                sequenceVariable.Values, Formatting.Indented,
                new JsonConverter[] {new StringEnumConverter()});
            Console.WriteLine(jsonString);
            return new ConstantVariable();
        }
        else if (firstExpression is ConstantVariable constantVariable)
        {
            Console.WriteLine(constantVariable.Value);
            return new ConstantVariable();
        }
        else
        {
            throw new Exception("Bad arguments, clean this up later");
        }
    }

    private Variable RunLen(Memory memory)
    {
        if (Parameters.Count != 1)
        {
            throw new Exception("Bad arguments, clean this up later");
        }
        
        var firstExpression = Parameters[0].Interpret(memory);
        if (firstExpression is StringVariable stringVariable)
        {
            return BsTypes.Create(memory, "int", stringVariable.Value.Length);
        }
        else if (firstExpression is SequenceVariable sequenceVariable)
        {
            return BsTypes.Create(memory, "int", sequenceVariable.Values.Count);
        }
        else if (firstExpression is ObjectVariable objectVariable)
        {
            if (BsTypes.Is(memory, "list", objectVariable))
            {
                var value = objectVariable.Values["__value"] as SequenceVariable;
                return BsTypes.Create(memory, "int", value.Values.Count);
            }
            throw new Exception("Bad arguments, clean this up later");
        }
        else
        {
            throw new Exception("Bad arguments, clean this up later");
        }
    }
}