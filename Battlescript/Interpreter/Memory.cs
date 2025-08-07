using System.Diagnostics;

namespace Battlescript;

public class Memory(List<MemoryScope>? scopes = null)
{
    public List<MemoryScope> Scopes { get; } = scopes ?? [new MemoryScope(null)];

    public enum BsTypes
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
    }
    
    public static readonly string[] BsTypeStrings = [
        "numeric", 
        "int", 
        "float", 
        "bool", 
        "list", 
        "dict", 
        "str", 
        "Exception", 
        "SyntaxError"];
    
    public static readonly Dictionary<string, BsTypes> BsStringsToTypes = new() {
        {"int", BsTypes.Int},
        {"float", BsTypes.Float},
        {"bool", BsTypes.Bool},
        {"list", BsTypes.List},
        {"Exception", BsTypes.Exception},
        {"dict", BsTypes.Dictionary},
        {"str", BsTypes.String},
        {"numeric", BsTypes.Numeric},
        {"SyntaxError", BsTypes.SyntaxError},
    };
    
    public static readonly Dictionary<BsTypes, string> BsTypesToStrings = new() {
        {BsTypes.Int, "int"},
        {BsTypes.Float, "float"},
        {BsTypes.Bool, "bool"},
        {BsTypes.List, "list"},
        {BsTypes.Exception, "Exception"},
        {BsTypes.Dictionary, "dict"},
        {BsTypes.String, "str"},
        {BsTypes.Numeric, "numeric"},
        {BsTypes.SyntaxError, "SyntaxError"},
    };
    
    public Dictionary<BsTypes, ClassVariable> BsTypeReferences = [];
    
    public void PopulateBsTypeReference(string builtin)
    {
        var type = BsStringsToTypes[builtin];
        BsTypeReferences[type] = GetVariable(builtin) as ClassVariable;
    }
    
    public bool Is(BsTypes type, Variable variable)
    {
        var builtInClass = BsTypeReferences[type];
        return variable is ObjectVariable objectVariable && objectVariable.Class.Name == builtInClass.Name;
    }
    
    public Variable Create(BsTypes type, dynamic value)
    {
        var builtInClass = BsTypeReferences[type];
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
    
    public int GetIntValue(Variable variable)
    {
        if (Is(BsTypes.Int, variable) && variable is ObjectVariable objectVariable)
        {
            var valueVariable = objectVariable.Values["__value"];
            return ((NumericVariable)valueVariable).Value;
        }
        else
        {
            throw new Exception("Variable is not an int");
        }
    }
    
    public double GetFloatValue(Variable variable)
    {
        if (Is(BsTypes.Float, variable) && variable is ObjectVariable objectVariable)
        {
            var valueVariable = objectVariable.Values["__value"];
            return ((NumericVariable)valueVariable).Value;
        }
        else
        {
            throw new Exception("Variable is not a float");
        }
    }
    
    public bool GetBoolValue(Variable variable)
    {
        if (Is(BsTypes.Bool, variable) && variable is ObjectVariable objectVariable)
        {
            var valueVariable = objectVariable.Values["__value"];
            return ((NumericVariable)valueVariable).Value != 0;
        }
        else
        {
            throw new Exception("Variable is not a bool");
        }
    }

    public SequenceVariable GetListValue(Variable variable)
    {
        if (Is(BsTypes.List, variable) && variable is ObjectVariable objectVariable)
        {
            var valueVariable = objectVariable.Values["__value"] as SequenceVariable;
            return valueVariable!;
        }
        else
        {
            throw new Exception("Variable is not a list");
        }
    }
    
    public MappingVariable GetDictValue(Variable variable)
    {
        if (Is(BsTypes.Dictionary, variable) && variable is ObjectVariable objectVariable)
        {
            var valueVariable = objectVariable.Values["__value"] as MappingVariable;
            return valueVariable!;
        }
        else
        {
            throw new Exception("Variable is not a dict");
        }
    }
    
    public string GetStringValue(Variable variable)
    {
        if (Is(BsTypes.String, variable) && variable is ObjectVariable objectVariable)
        {
            var valueVariable = objectVariable.Values["__value"] as StringVariable;
            return valueVariable!.Value;
        }
        else
        {
            throw new Exception("Variable is not a str");
        }
    }
    
    public Variable? GetVariable(string name)
    {
        return GetVariable(new VariableInstruction(name));
    }

    public Variable? GetVariable(VariableInstruction variableInstruction)
    {
        for (var i = Scopes.Count - 1; i >= 0; i--)
        {
            if (Scopes[i].Variables.ContainsKey(variableInstruction.Name))
            {
                var foundVariable = Scopes[i].Variables[variableInstruction.Name];
                if (variableInstruction.Next is ArrayInstruction { Separator: "[" } squareBracketsInstruction)
                {
                    return foundVariable.GetItem(this, squareBracketsInstruction);
                }
                else if (variableInstruction.Next is MemberInstruction memberInstruction)
                {
                    return foundVariable.GetMember(this, memberInstruction);
                }
                else
                {
                    return foundVariable;
                }
            }
        }
    
        return null;
    }
    
    public void SetVariable(VariableInstruction variableInstruction, Variable valueVariable)
    {
        // We need to pass in the full instruction here to handle assigning to indexes
        if (GetVariable(variableInstruction.Name) is not null)
        {
            for (var i = Scopes.Count - 1; i >= 0; i--)
            {
                if (Scopes[i].Variables.ContainsKey(variableInstruction.Name))
                {
                    if (variableInstruction.Next is ArrayInstruction { Separator: "[" } squareBracketsInstruction)
                    {
                        Scopes[i].Variables[variableInstruction.Name].SetItem(
                            this, 
                            valueVariable, 
                            squareBracketsInstruction);
                    }
                    else if (variableInstruction.Next is MemberInstruction memberInstruction)
                    {
                        Scopes[i].Variables[variableInstruction.Name].SetMember(
                            this, 
                            valueVariable, 
                            memberInstruction);
                    }
                    else
                    {
                        Scopes[i].Variables[variableInstruction.Name] = valueVariable;
                    }

                    return;
                }
            }
        }
        else
        {
            AddVariableToLastScope(variableInstruction, valueVariable);
        }
    }

    public void AddVariableToLastScope(VariableInstruction variableInstruction, Variable valueVariable)
    {
        Scopes[^1].Add(variableInstruction.Name, valueVariable);
    }
    
    public void AddScope(MemoryScope? scope = null)
    {
        Scopes.Add(scope ?? new MemoryScope(null));
    }

    public MemoryScope RemoveScope()
    {
        var removedScope = Scopes[^1];
        Scopes.RemoveAt(Scopes.Count - 1);
        return removedScope;
    }

    public void RemoveScopes(int count)
    {
        for (var i = 0; i < count; i++)
        {
            RemoveScope();
        }
    }

    public void PrintStacktrace()
    {
        Console.WriteLine("Traceback (most recent call last):");
        for (var i = Scopes.Count - 1; i >= 1; i--)
        {
            Console.WriteLine($"\tFile {Scopes[i].FileName}, line {Scopes[i].Line}, in {Scopes[i].FunctionName ?? "<module>"}");
            Console.WriteLine("\t" + Scopes[i].Expression);
        }
    }
}