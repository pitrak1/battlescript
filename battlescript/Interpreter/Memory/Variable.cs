using System.Diagnostics;
using Newtonsoft.Json;
using BattleScript.Exceptions;
using BattleScript.Instructions;
using BattleScript.Core;
using System.Linq;

namespace BattleScript.Core;

public class Variable
{
    public Consts.VariableTypes? Type { get; set; }
    public dynamic? Value { get; set; }
    public List<Instruction>? Instructions { get; set; }

    public Variable(
        Consts.VariableTypes? type = null,
        dynamic? value = null,
        List<Instruction>? instructions = null
    )
    {
        Type = type;
        Value = value;
        Instructions = instructions;
    }

    public Variable CopyProperties(Variable var)
    {
        Type = var.Type;
        Value = var.Value;
        Instructions = var.Instructions;
        return this;
    }

    public Variable AddVariable(List<string> path, Variable? var = null)
    {
        Debug.Assert(Value is Dictionary<string, Variable>);
        Debug.Assert(path.Count == 1);

        if (var is null)
        {
            var = new Variable();
        }

        Value.Add(path[0], var);
        return var;
    }

    public bool HasVariable(string key)
    {
        return Value!.ContainsKey(key);
    }

    public Variable GetVariable(string key)
    {
        if (Value!.ContainsKey(key))
        {
            return Value[key];
        }
        else
        {
            throw new VariableNotFoundException(key);
        }
    }

    public Variable GetIndex(dynamic key)
    {
        if (Value is List<Variable>)
        {
            int integerKey = (int)key;
            if (integerKey < Value.Count)
            {
                return Value[integerKey];
            }
            else
            {
                throw new ArrayOutOfBoundsException(key);
            }
        }
        else
        {
            if (Value.ContainsKey(key))
            {
                return Value[key];
            }
            else
            {
                var variable = new Variable();
                Value[key] = variable;
                return variable;
            }
        }
    }

    // public Variable GetVariable(string key)
    // {
    //     if (Value.ContainsKey(key))
    //     {
    //         return Value[key];
    //     }
    //     else if (Value.ContainsKey("class") && Type == Consts.VariableTypes.Object)
    //     {
    //         return Value["class"].GetVariable(key);
    //     }
    //     else if (Value.ContainsKey("super") && Type == Consts.VariableTypes.Class)
    //     {
    //         return Value["super"].GetVariable(key);
    //     }
    //     else
    //     {
    //         throw new VariableNotFoundException(key);
    //     }
    // }

    // public Variable GetIndex(dynamic key)
    // {
    //     if (Value is List<Variable>)
    //     {
    //         return handleArrayIndex(key);
    //     }
    //     else
    //     {
    //         return handleObjectIndex(key);
    //     }
    // }

    // private Variable handleArrayIndex(dynamic key)
    // {
    //     Debug.Assert(key is int, "Indexes into an array must be an integer");
    //     if (key < Value!.Count)
    //     {
    //         return Value[key];
    //     }
    //     else
    //     {
    //         throw new ArrayOutOfBoundsException(key);
    //     }
    // }

    // private Variable handleObjectIndex(dynamic key)
    // {
    //     if (Value!.ContainsKey(key))
    //     {
    //         return Value[key];
    //     }
    //     else
    //     {
    //         return createNewVariableWithKey(key);
    //     }
    // }

    // private Variable createNewVariableWithKey(dynamic key)
    // {
    //     var variable = new Variable(Consts.VariableTypes.Literal);
    //     Value![key] = variable;
    //     return variable;
    // }

    // public Variable CreateObject(Variable var)
    // {
    //     Type = Consts.VariableTypes.Object;
    //     Value = new Dictionary<string, Variable>();
    //     List<Variable> chain = GatherClassChain(var);
    //     chain.Reverse();
    //     foreach (Variable scope in chain)
    //     {
    //         MergeScope(scope);
    //     }
    //     Value.Add("class", var);
    //     ClassObject = var;
    //     return this;
    // }

    // public Variable? GetConstructorForClass()
    // {
    //     List<Variable> chain = GatherClassChain(this);
    //     foreach (Variable scope in chain)
    //     {
    //         if (scope.HasVariable("constructor"))
    //         {
    //             return scope.GetVariable("constructor");
    //         }
    //     }

    //     return null;
    // }

    // private List<Variable> GatherClassChain(Variable var)
    // {
    //     List<Variable> chain = new List<Variable>();
    //     Variable? currentScope = var;
    //     while (currentScope is not null)
    //     {
    //         chain.Add(currentScope);
    //         if (currentScope.HasVariable("super"))
    //         {
    //             currentScope = currentScope.Value["super"];
    //         }
    //         else
    //         {
    //             currentScope = null;
    //         }
    //     }
    //     return chain;
    // }

    // private void MergeScope(Variable var)
    // {
    //     foreach (KeyValuePair<string, Variable> pair in var.Value)
    //     {
    //         if (
    //             pair.Value.Type is
    //             Consts.VariableTypes.Array or
    //             Consts.VariableTypes.Dictionary or
    //             Consts.VariableTypes.Object or
    //             Consts.VariableTypes.Literal
    //         )
    //         {
    //             if (HasVariable(pair.Key))
    //             {
    //                 Value[pair.Key] = new Variable().CopyProperties(pair.Value);
    //             }
    //             else
    //             {
    //                 Value.Add(pair.Key, new Variable().CopyProperties(pair.Value));
    //             }
    //         }
    //     }
    // }

    // All of the stuff below this point is to override the == operator
    public override bool Equals(object? obj)
    {
        return this.Equals(obj as Variable);
    }

    public bool Equals(Variable variable)
    {
        if (variable is null)
        {
            return false;
        }

        if (ReferenceEquals(this, variable))
        {
            return true;
        }

        if (GetType() != variable.GetType())
        {
            return false;
        }

        if (Type != variable.Type)
        {
            return false;
        }

        if (Value is Dictionary<dynamic, Variable>)
        {
            if (variable.Value is not Dictionary<dynamic, Variable>)
            {
                return false;
            }

            var dictionaryValue = variable.Value as Dictionary<dynamic, Variable>;
            var varDictionaryValue = variable.Value as Dictionary<dynamic, Variable>;

            foreach (var pair in dictionaryValue)
            {
                if (varDictionaryValue.ContainsKey(pair.Key))
                {
                    if (pair.Value != varDictionaryValue[pair.Key])
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        else if (
            Value is List<Variable> ||
            Value is IEnumerable<KeyValuePair<string, Variable>> ||
            Value is Dictionary<dynamic, Variable>
        )
        {
            if (!Enumerable.SequenceEqual(Value, variable.Value))
            {
                return false;
            }
        }
        else
        {
            if (Value != variable.Value)
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        return (Type, Value?.GetHashCode()).GetHashCode();
    }

    public static bool operator ==(Variable? lhs, Variable? rhs)
    {
        if (lhs is null)
        {
            return rhs is null;
        }
        else if (rhs is null)
        {
            return false;
        }
        else
        {
            return lhs.Equals(rhs);
        }
    }

    public static bool operator !=(Variable? lhs, Variable? rhs)
    {
        return !(lhs == rhs);
    }
}