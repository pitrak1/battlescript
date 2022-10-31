namespace BattleScript; 

public class ContextStack {
    public List<ScopeVariable> contexts { get; set; }

    public ContextStack() {
        contexts = new List<ScopeVariable>();
    }

    public ScopeVariable GetCurrentContext() {
        return contexts[^1];
    }

    public void Add(ScopeVariable context) {
        contexts.Add(context);
    }

    public void Pop() {
        contexts.RemoveAt(contexts.Count - 1);
    }

    public bool Empty() {
        return contexts.Count == 0;
    }
}