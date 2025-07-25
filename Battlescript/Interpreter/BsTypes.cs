namespace Battlescript;

public static class BsTypes
{
    public enum Types
    {
        Numeric,
        Int,
        Float,
        Bool,
        List,
        Exception,
        Dictionary,
        String
    }
    
    public static readonly string[] TypeStrings = ["numeric", "int", "float", "bool", "list", "Exception", "dict", "str"];
    
    private static readonly Dictionary<string, Types> StringsToTypes = new() {
        {"numeric", Types.Numeric},
        {"int", Types.Int},
        {"float", Types.Float},
        {"bool", Types.Bool},
        {"list", Types.List},
        {"Exception", Types.Exception},
        {"dict", Types.Dictionary},
        {"str", Types.String}
    };
    
    private static readonly Dictionary<Types, string> TypesToStrings = new() {
        {Types.Numeric, "numeric"},
        {Types.Int, "int"},
        {Types.Float, "float"},
        {Types.Bool, "bool"},
        {Types.List, "list"},
        {Types.Exception, "Exception"},
        {Types.Dictionary, "dict"},
        {Types.String, "str"}
    };

    public static Types GetTypeFromString(string type)
    {
        return StringsToTypes[type];
    }

    public static string GetStringFromType(Types type)
    {
        return TypesToStrings[type];
    }
    
    public static bool Is(Memory memory, Types type, Variable variable)
    {
        var builtInClass = memory.GetBuiltIn(type);
        return variable is ObjectVariable objectVariable && objectVariable.Class.Name == builtInClass.Name;
    }

    public static Types GetType(Memory memory, Variable variable)
    {
        if (Is(memory, Types.Int, variable))
        {
            return Types.Int;
        } 
        else if (Is(memory, Types.Float, variable))
        {
            return Types.Float;
        }
        else if (Is(memory, Types.Bool, variable))
        {
            return Types.Bool;
        }
        else if (Is(memory, Types.List, variable))
        {
            return Types.List;
        }
        else
        {
            throw new Exception("Variable is not a built in type");
        }
    }
    
    public static Variable Create(Memory memory, Types type, dynamic value)
    {
        var builtInClass = memory.GetBuiltIn(type);
        var objectVariable = builtInClass.CreateObject();

        if (value is int || value is double)
        {
            objectVariable.Values["__value"] = new NumericVariable(value);
            return objectVariable;
        } else if (value is bool)
        {
            objectVariable.Values["__value"] = new NumericVariable(value ? 1 : 0);
            return objectVariable;
        } else if (value is List<Variable>)
        {
            objectVariable.Values["__value"] = new SequenceVariable(value);
            return objectVariable;
        }
        
        objectVariable.Values["__value"] = value;
        return objectVariable;
    }
    
    public static int GetIntValue(Memory memory, Variable variable)
    {
        if (Is(memory, Types.Int, variable) && variable is ObjectVariable objectVariable)
        {
            var valueVariable = objectVariable.Values["__value"];
            return ((NumericVariable)valueVariable).Value;
        }
        else
        {
            throw new Exception("Variable is not an int");
        }
    }
    
    public static double GetFloatValue(Memory memory, Variable variable)
    {
        if (Is(memory, Types.Float, variable) && variable is ObjectVariable objectVariable)
        {
            var valueVariable = objectVariable.Values["__value"];
            return ((NumericVariable)valueVariable).Value;
        }
        else
        {
            throw new Exception("Variable is not a float");
        }
    }
    
    public static bool GetBoolValue(Memory memory, Variable variable)
    {
        if (Is(memory, Types.Bool, variable) && variable is ObjectVariable objectVariable)
        {
            var valueVariable = objectVariable.Values["__value"];
            return ((NumericVariable)valueVariable).Value != 0;
        }
        else
        {
            throw new Exception("Variable is not a bool");
        }
    }
    
    public static SequenceVariable GetListValue(Memory memory, Variable variable)
    {
        if (Is(memory, Types.List, variable) && variable is ObjectVariable objectVariable)
        {
            var valueVariable = objectVariable.Values["__value"] as SequenceVariable;
            return valueVariable!;
        }
        else
        {
            throw new Exception("Variable is not a list");
        }
    }

    public static MappingVariable GetDictValue(Memory memory, Variable variable)
    {
        if (Is(memory, Types.Dictionary, variable) && variable is ObjectVariable objectVariable)
        {
            var valueVariable = objectVariable.Values["__value"] as MappingVariable;
            return valueVariable!;
        }
        else
        {
            throw new Exception("Variable is not a dict");
        }
    }
}