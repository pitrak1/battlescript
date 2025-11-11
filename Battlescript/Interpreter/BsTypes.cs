namespace Battlescript;

public static class BsTypes
{
    public enum Types
    {
        Int,
        Float,
        Bool,
        List,
        Exception,
        Dictionary,
        String,
        Numeric,
        SyntaxError,
        AssertionError,
        ValueError,
        TypeError,
        NameError,
    }
    
    public static readonly string[] TypeStrings = [
        "numeric", 
        "int", 
        "float", 
        "bool", 
        "list", 
        "dict", 
        "str", 
        "Exception", 
        "SyntaxError",
        "AssertionError",
        "ValueError",
        "TypeError",
        "NameError",
    ];
    
    public static readonly Dictionary<string, Types> StringsToTypes = new() {
        {"int", Types.Int},
        {"float", Types.Float},
        {"bool", Types.Bool},
        {"list", Types.List},
        {"Exception", Types.Exception},
        {"dict", Types.Dictionary},
        {"str", Types.String},
        {"numeric", Types.Numeric},
        {"SyntaxError", Types.SyntaxError},
        {"AssertionError", Types.AssertionError},
        {"ValueError", Types.ValueError},
        {"TypeError", Types.TypeError},
        {"NameError", Types.NameError},
    };
    
    public static readonly Dictionary<Types, string> TypesToStrings = new() {
        {Types.Int, "int"},
        {Types.Float, "float"},
        {Types.Bool, "bool"},
        {Types.List, "list"},
        {Types.Exception, "Exception"},
        {Types.Dictionary, "dict"},
        {Types.String, "str"},
        {Types.Numeric, "numeric"},
        {Types.SyntaxError, "SyntaxError"},
        {Types.AssertionError, "AssertionError"},
        {Types.ValueError, "ValueError"},
        {Types.TypeError, "TypeError"},
        {Types.NameError, "NameError"},
    };
    
    public static Dictionary<Types, ClassVariable> TypeReferences = [];

    public static Variable True;
    public static Variable False;
    
    public static void PopulateBsTypeReference(CallStack callStack, Closure closure, string builtin)
    {
        var type = StringsToTypes[builtin];
        TypeReferences[type] = closure.GetVariable(callStack, builtin) as ClassVariable;
    }

    public static void PopulateBsTypeConstants(CallStack callStack, Closure closure)
    {
        True = Create(Types.Bool, new NumericVariable(1));
        False = Create(Types.Bool, new NumericVariable(0));
    }
    
    public static bool Is(Types type, Variable variable)
    {
        var builtInClass = TypeReferences[type];
        return variable is ObjectVariable objectVariable && objectVariable.Class.Name == builtInClass.Name;
    }
    
    public static Variable Create(Types type, dynamic value)
    {
        var builtInClass = TypeReferences[type];
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
        } else if (value is string)
        {
            objectVariable.Values["__value"] = new StringVariable(value);
            return objectVariable;
        }
        
        objectVariable.Values["__value"] = value;
        return objectVariable;
    }

    public static Variable CreateException(CallStack callStack, Closure closure, string type, string message)
    {
        var exceptionType = closure.GetVariable(callStack, type);
        if (exceptionType is ClassVariable classVariable)
        {
            var objectVariable = classVariable.CreateObject();
            objectVariable.Values["message"] = Create(Types.String, message);
            return objectVariable;
        }
        else {
            throw new Exception("Invalid exception type");
        }
    }
    
    public static int GetIntValue(Variable variable)
    {
        if (Is(Types.Int, variable) && variable is ObjectVariable objectVariable)
        {
            var valueVariable = objectVariable.Values["__value"];
            return ((NumericVariable)valueVariable).Value;
        }
        else
        {
            throw new Exception("Variable is not an int");
        }
    }
    
    public static double GetFloatValue(Variable variable)
    {
        if (Is(Types.Float, variable) && variable is ObjectVariable objectVariable)
        {
            var valueVariable = objectVariable.Values["__value"];
            return ((NumericVariable)valueVariable).Value;
        }
        else
        {
            throw new Exception("Variable is not a float");
        }
    }
    
    public static bool GetBoolValue(Variable variable)
    {
        if (Is(Types.Bool, variable) && variable is ObjectVariable objectVariable)
        {
            var valueVariable = objectVariable.Values["__value"];
            return ((NumericVariable)valueVariable).Value != 0;
        }
        else
        {
            throw new Exception("Variable is not a bool");
        }
    }

    public static SequenceVariable GetListValue(Variable variable)
    {
        if (Is(Types.List, variable) && variable is ObjectVariable objectVariable)
        {
            var valueVariable = objectVariable.Values["__value"] as SequenceVariable;
            return valueVariable!;
        }
        else
        {
            throw new Exception("Variable is not a list");
        }
    }
    
    public static MappingVariable GetDictValue(Variable variable)
    {
        if (Is(Types.Dictionary, variable) && variable is ObjectVariable objectVariable)
        {
            var valueVariable = objectVariable.Values["__value"] as MappingVariable;
            return valueVariable!;
        }
        else
        {
            throw new Exception("Variable is not a dict");
        }
    }
    
    public static string GetStringValue(Variable variable)
    {
        if (Is(Types.String, variable) && variable is ObjectVariable objectVariable)
        {
            var valueVariable = objectVariable.Values["__value"] as StringVariable;
            return valueVariable!.Value;
        }
        else
        {
            throw new Exception("Variable is not a str");
        }
    }
    
    public static string GetErrorMessage(Variable variable)
    {
        if (IsException(variable) && variable is ObjectVariable objectVariable)
        {
            var messageVariable = objectVariable.Values["message"] as ObjectVariable;
            return GetStringValue(messageVariable);
        }
        else
        {
            throw new Exception("Variable is not an exception");
        }
    }

    public static bool IsException(Variable variable)
    {
        if (variable is ObjectVariable objectVariable)
        {
            return objectVariable.IsInstance(TypeReferences[Types.Exception]);
        }
        else
        {
            return false;
        }
    }
}