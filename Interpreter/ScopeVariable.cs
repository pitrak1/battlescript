using System.Diagnostics;
using BattleScript.Exceptions;
using Newtonsoft.Json;

namespace BattleScript; 

public class ScopeVariable {
    public Consts.VariableTypes? Type { get; set; }
    public dynamic? Value { get; set; }

    public ScopeVariable(
        Consts.VariableTypes? type = null,
        dynamic? value = null
    ) {
        Type = type;
        Value = value;
    }
    
    public ScopeVariable AddVariable(List<string> path) {
        Debug.Assert(Value is Dictionary<string, ScopeVariable>);
        Debug.Assert(path.Count == 1);
        ScopeVariable var = new ScopeVariable();
        Value.Add(path[0], var);
        return var;
    }

    public ScopeVariable GetVariable(string key) {
        Debug.Assert(Value is Dictionary<string, ScopeVariable>);
        if (Value.ContainsKey(key)) {
            return Value[key];
        }
        else {
            throw new VariableNotFoundException(key);
        }
    }

    public void Copy(ScopeVariable var) {
        Type = var.Type;
        Value = var.Value;
    }
}