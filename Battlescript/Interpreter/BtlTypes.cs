namespace Battlescript;

public static class BtlTypes
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
        AttributeError,
    }

    // Load order matters for built-in initialization
    private static readonly (string Name, Types Type)[] TypeMappings = [
        ("numeric", Types.Numeric),
        ("int", Types.Int),
        ("float", Types.Float),
        ("bool", Types.Bool),
        ("list", Types.List),
        ("dict", Types.Dictionary),
        ("str", Types.String),
        ("Exception", Types.Exception),
        ("SyntaxError", Types.SyntaxError),
        ("AssertionError", Types.AssertionError),
        ("ValueError", Types.ValueError),
        ("TypeError", Types.TypeError),
        ("NameError", Types.NameError),
        ("AttributeError", Types.AttributeError),
    ];

    public static readonly string[] TypeStrings = TypeMappings.Select(t => t.Name).ToArray();

    public static readonly Dictionary<string, Types> StringsToTypes =
        TypeMappings.ToDictionary(t => t.Name, t => t.Type);

    public static readonly Dictionary<Types, string> TypesToStrings =
        TypeMappings.ToDictionary(t => t.Type, t => t.Name);

    private static readonly Dictionary<Types, ClassVariable> TypeReferences = [];

    public static Variable True;
    public static Variable False;
    public static Variable None;
    
    public static void PopulateBtlTypeReference(CallStack callStack, Closure closure, string builtin)
    {
        var type = StringsToTypes[builtin];
        TypeReferences[type] = closure.GetVariable(callStack, builtin) as ClassVariable;
    }

    public static void PopulateBtlTypeConstants(CallStack callStack, Closure closure)
    {
        True = Create(Types.Bool, new NumericVariable(1));
        False = Create(Types.Bool, new NumericVariable(0));
        None = new NoneVariable();
    }

    public static bool IsValueType(ClassVariable classVariable)
    {
        return classVariable == TypeReferences[Types.Int]
               || classVariable == TypeReferences[Types.Float]
               || classVariable == TypeReferences[Types.String]
               || classVariable == TypeReferences[Types.Bool];
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

        objectVariable.Values["__btl_value"] = value switch
        {
            int i => new NumericVariable(i),
            double d => new NumericVariable(d),
            bool b => new NumericVariable(b ? 1 : 0),
            List<Variable> list => new SequenceVariable(list),
            string s => new StringVariable(s),
            _ => value
        };

        return objectVariable;
    }

    public static Variable CreateException(CallStack callStack, Closure closure, string type, string message)
    {
        if (closure.GetVariable(callStack, type) is not ClassVariable classVariable)
            throw new Exception("Invalid exception type");

        var objectVariable = classVariable.CreateObject();
        objectVariable.Values["message"] = Create(Types.String, message);
        return objectVariable;
    }

    public static int GetIntValue(Variable variable) =>
        GetInnerValue<NumericVariable>(Types.Int, variable).Value;

    public static double GetFloatValue(Variable variable) =>
        GetInnerValue<NumericVariable>(Types.Float, variable).Value;

    public static bool GetBoolValue(Variable variable) =>
        GetInnerValue<NumericVariable>(Types.Bool, variable).Value != 0;

    public static SequenceVariable GetListValue(Variable variable) =>
        GetInnerValue<SequenceVariable>(Types.List, variable);

    public static MappingVariable GetDictValue(Variable variable) =>
        GetInnerValue<MappingVariable>(Types.Dictionary, variable);

    public static string GetStringValue(Variable variable) =>
        GetInnerValue<StringVariable>(Types.String, variable).Value;

    public static string GetErrorMessage(Variable variable)
    {
        if (!IsException(variable) || variable is not ObjectVariable objectVariable)
            throw new Exception("Variable is not an exception");

        return GetStringValue((ObjectVariable)objectVariable.Values["message"]);
    }

    public static bool IsException(Variable variable) =>
        variable is ObjectVariable obj && obj.IsInstance(TypeReferences[Types.Exception]);

    private static T GetInnerValue<T>(Types type, Variable variable) where T : Variable
    {
        if (!Is(type, variable) || variable is not ObjectVariable objectVariable)
            throw new Exception($"Variable is not a {TypesToStrings[type]}");

        return (T)objectVariable.Values["__btl_value"];
    }
}