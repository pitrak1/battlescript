using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Battlescript;

public static class BuiltInPrint
{
    public static void Run(Memory memory, List<Instruction> arguments)
    {
        if (arguments.Count != 1)
        {
            throw new Exception("Bad arguments, clean this up later");
        }
        
        var firstExpression = arguments[0].Interpret(memory);
        Console.WriteLine(PrintVariable(memory, firstExpression));
    }

    public static string PrintVariable(Memory memory, Variable variable)
    {
        if (variable is StringVariable stringVariable)
        {
            return stringVariable.Value;
        }
        else if (variable is ObjectVariable objectVariable)
        {
            var strFunc = objectVariable.GetMember(memory, new MemberInstruction("__str__"));
            var reprFunc = objectVariable.GetMember(memory, new MemberInstruction("__repr__"));
            if (strFunc is FunctionVariable funcVariable)
            {
                var strResult = funcVariable.RunFunction(memory, new List<Instruction>(), objectVariable);
                return PrintVariable(memory, strResult);
            }
            else if (reprFunc is FunctionVariable reprVariable)
            {
                var reprResult = reprVariable.RunFunction(memory, new List<Instruction>(), objectVariable);
                return PrintVariable(memory, reprResult);
            }
            else
            {
                var jsonString = JsonConvert.SerializeObject(
                    objectVariable.Values, Formatting.Indented,
                    new JsonConverter[] {new StringEnumConverter()});
                return jsonString;
            }
        }
        else if (variable is SequenceVariable sequenceVariable)
        {
            var jsonString = JsonConvert.SerializeObject(
                sequenceVariable.Values, Formatting.Indented,
                new JsonConverter[] {new StringEnumConverter()});
            return jsonString;
        }
        else if (variable is ConstantVariable constantVariable)
        {
            return constantVariable.Value.ToString();
        }
        else
        {
            throw new Exception("Bad arguments, clean this up later");
        }
    }
}