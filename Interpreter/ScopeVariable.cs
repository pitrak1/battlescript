using System.Diagnostics;
using BattleScript.Exceptions;
using Newtonsoft.Json;

namespace BattleScript; 

public class ScopeVariable {
    public Consts.VariableTypes? Type { get; set; }
    public dynamic? Value { get; set; }
    public List<Instruction>? Instructions { get; set; } = new();
    public ScopeVariable? ClassScope { get; set; }

    public ScopeVariable(
        Consts.VariableTypes? type = null,
        dynamic? value = null,
        List<Instruction>? instructions = null,
        ScopeVariable? classScope = null
    ) {
        Type = type;
        Value = value;
        if (instructions is not null) {
            Instructions = instructions;
        }
        ClassScope = classScope;
    }
    
    public ScopeVariable AddVariable(List<string> path, ScopeVariable? var = null) {
        Debug.Assert(Value is Dictionary<string, ScopeVariable>);
        Debug.Assert(path.Count == 1);
        
        if (var is null) {
            var = new ScopeVariable();
        }

        Value.Add(path[0], var);
        return var;
    }

    public bool HasVariable(string key) {
        Debug.Assert(Value is Dictionary<string, ScopeVariable>);
        return Value.ContainsKey(key);
    }

    public ScopeVariable GetVariable(dynamic key) {
        if (Value is List<ScopeVariable>) {
            Debug.Assert(key is int);
            if (key < Value.Count) {
                return Value[key];
            }
            else {
                throw new ArrayOutOfBoundsException(key);
            }
        } else {
            if (Value.ContainsKey(key)) {
                return Value[key];
            } 
            else if (ClassScope is not null) {
                return ClassScope.GetVariable(key);
            }
            else {
                throw new VariableNotFoundException(key);
            }
        }
    }

    public ScopeVariable GetIntIndex(int index) {
        Debug.Assert(Value is List<ScopeVariable>);
        return Value[index];
    }

    public ScopeVariable Copy(ScopeVariable var) {
        Type = var.Type;
        Value = var.Value;
        Instructions = var.Instructions;
        ClassScope = var.ClassScope;
        return this;
    }

    public ScopeVariable CreateObject(ScopeVariable var) {
        Type = Consts.VariableTypes.Object;
        Value = new Dictionary<string, ScopeVariable>();
        foreach (KeyValuePair<string, ScopeVariable> pair in var.Value) {
            if (
                pair.Value.Type is
                Consts.VariableTypes.Array or
                Consts.VariableTypes.Dictionary or
                Consts.VariableTypes.Object or
                Consts.VariableTypes.Value
            ) {
                Value.Add(pair.Key, new ScopeVariable().Copy(pair.Value));
            }
        }
        ClassScope = var;
        return this;
    }
}