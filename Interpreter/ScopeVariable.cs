using System.Diagnostics;
using BattleScript.Exceptions;
using Newtonsoft.Json;

namespace BattleScript; 

public class ScopeVariable {
    public Consts.VariableTypes? Type { get; set; }
    public dynamic? Value { get; set; }
    public List<Instruction>? Instructions { get; set; } = new();

    public ScopeVariable(
        Consts.VariableTypes? type = null,
        dynamic? value = null,
        List<Instruction>? instructions = null
    ) {
        Type = type;
        Value = value;
        if (instructions is not null) {
            Instructions = instructions;
        }
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

    public ScopeVariable GetVariable(string key) {
        Debug.Assert(Value is Dictionary<string, ScopeVariable>);
        if (Value.ContainsKey(key)) {
            return Value[key];
        } 
        else if (Value.ContainsKey("class")) {
            return Value["class"].GetVariable(key);
        }
        else {
            throw new VariableNotFoundException(key);
        }
    }

    public ScopeVariable Copy(ScopeVariable var) {
        Type = var.Type;
        Value = var.Value;
        Instructions = var.Instructions;
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

        Value.Add("class", var);
        return this;
    }
}