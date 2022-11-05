using System.Diagnostics;
using BattleScript.Exceptions;
using Newtonsoft.Json;

namespace BattleScript; 

public class ScopeVariable {
    public Consts.VariableTypes? Type { get; set; }
    public dynamic? Value { get; set; }
    public List<Instruction>? Instructions { get; set; } = new();
    
    public ScopeVariable? ClassObject { get; set; }

    public ScopeVariable(
        Consts.VariableTypes? type = null,
        dynamic? value = null,
        List<Instruction>? instructions = null,
        ScopeVariable? classObject = null
    ) {
        Type = type;
        Value = value;
        if (instructions is not null) {
            Instructions = instructions;
        }
        ClassObject = classObject;
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
            else if (Value.ContainsKey("class") && Type == Consts.VariableTypes.Object) {
                return Value["class"].GetVariable(key);
            }
            else if (Value.ContainsKey("super") && Type == Consts.VariableTypes.Class) {
                return Value["super"].GetVariable(key);
            }
            else {
                throw new VariableNotFoundException(key);
            }
        }
    }
    
    public ScopeVariable Copy(ScopeVariable var) {
        Type = var.Type;
        Value = var.Value;
        Instructions = var.Instructions;
        ClassObject = var.ClassObject;
        return this;
    }

    public ScopeVariable CreateObject(ScopeVariable var) {
        Type = Consts.VariableTypes.Object;
        Value = new Dictionary<string, ScopeVariable>();
        List<ScopeVariable> chain = GatherClassChain(var);
        foreach (ScopeVariable scope in chain) {
            MergeScope(scope);
        }
        Value.Add("class", var);
        return this;
    }

    private List<ScopeVariable> GatherClassChain(ScopeVariable var) {
        List<ScopeVariable> chain = new List<ScopeVariable>();
        ScopeVariable? currentScope = var;
        while (currentScope is not null) {
            chain.Add(currentScope);
            if (currentScope.HasVariable("super")) {
                currentScope = currentScope.Value["super"];
            }
            else {
                currentScope = null;
            }
        }
        chain.Reverse();
        return chain;
    }

    private void MergeScope(ScopeVariable var) {
        foreach (KeyValuePair<string, ScopeVariable> pair in var.Value) {
            if (
                pair.Value.Type is
                Consts.VariableTypes.Array or
                Consts.VariableTypes.Dictionary or
                Consts.VariableTypes.Object or
                Consts.VariableTypes.Value
            ) {
                if (HasVariable(pair.Key)) {
                    Value[pair.Key] = new ScopeVariable().Copy(pair.Value);
                }
                else { 
                    Value.Add(pair.Key, new ScopeVariable().Copy(pair.Value));
                }
            }
        }
    }
}