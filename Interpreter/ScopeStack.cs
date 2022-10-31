using BattleScript.Exceptions;

namespace BattleScript; 

public class ScopeStack : ContextStack {
    public ScopeStack() {
        contexts.Add(new ScopeVariable(null, new Dictionary<string, ScopeVariable>()));
    }

    public override void Add(ScopeVariable context) {
        contexts.Add(new ScopeVariable(null, new Dictionary<string, ScopeVariable>()));
    }

    public ScopeVariable AddVariable(List<string> path) {
        return GetCurrentContext().AddVariable(path);
    }

    public ScopeVariable GetVariable(string key) {
        ScopeVariable? result = null;
        for (int i = (contexts.Count - 1); i >= 0; i--) {
            try {
                result = contexts[i].GetVariable(key);
            }
            catch (VariableNotFoundException ex) {
                continue;
            }
        }

        if (result is null) {
            throw new VariableNotFoundException(key);
        }
        else {
            return result;
        }
    }
}