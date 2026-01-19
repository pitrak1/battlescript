using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Battlescript;

public static class StringUtilities
{
    public static string GetVariableAsString(CallStack callStack, Closure closure, Variable variable)
    {
        if (variable is StringVariable stringVariable)
        {
            return stringVariable.Value;
        }
        else if (variable is ObjectVariable objectVariable)
        {
            var strFunc = objectVariable.GetMember(callStack, closure, new MemberInstruction("__str__"));
            var reprFunc = objectVariable.GetMember(callStack, closure, new MemberInstruction("__repr__"));
            if (strFunc is FunctionVariable funcVariable)
            {
                var strResult = funcVariable.RunFunction(callStack, closure, new ArgumentSet([]));
                return GetVariableAsString(callStack, closure, strResult);
            }
            else if (reprFunc is FunctionVariable reprVariable)
            {
                var reprResult = reprVariable.RunFunction(callStack, closure, new ArgumentSet([]));
                return GetVariableAsString(callStack, closure, reprResult);
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
        else if (variable is NumericVariable numericVariable)
        {
            return numericVariable.Value.ToString();
        }
        else
        {
            throw new Exception("Bad arguments, clean this up later");
        }
    }

    public static string GetFormattedStringValue(CallStack callStack, Closure closure, string value)
    {
        List<string> result = [];
        var start = 0;
        var inBrackets = false;
        for (var i = 0; i < value.Length; i++)
        {
            if (value[i] == '{')
            {
                if (inBrackets)
                {
                    throw new Exception("Bad shit, clean this up later");
                }
                
                inBrackets = true;
                result.Add(value.Substring(start, i - start));
                start = i + 1;
            } else if (value[i] == '}')
            {
                if (!inBrackets)
                {
                    throw new Exception("Bad shit, clean this up later");
                }
                
                inBrackets = false;
                var bracketValue = value.Substring(start, i - start);
                var inst = Runner.Parse(bracketValue);
                var variable = inst.First().Interpret(callStack, closure);
                result.Add(GetVariableAsString(callStack, closure, variable));
                start = i + 1;
            }
        }

        if (start < value.Length)
        {
            result.Add(value[start..]);
        }

        if (inBrackets)
        {
            throw new Exception("Bad shit, clean this up later");
        }

        return String.Join("", result.ToArray());
    }
}