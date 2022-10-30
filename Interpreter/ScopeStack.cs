namespace BattleScript; 

public class ScopeStack {
    private List<ScopeVariable> scopes = new List<ScopeVariable>();

    public ScopeStack() {
        scopes.Add(new ScopeVariable(null, new Dictionary<string, ScopeVariable>()));
    }

    public ScopeVariable Add(List<string> path) {
        return CurrentScope().Add(path);
    }

    public ScopeVariable Get(string key) {
        return CurrentScope().Get(key);
    }

    public ScopeVariable CurrentScope() {
        return scopes[^1];
    }
}