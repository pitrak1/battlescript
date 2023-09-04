namespace BattleScript.InterpreterNS;

public class ContextStack
{
    public List<ScopeVariable> contexts { get; set; }

    public ContextStack()
    {
        contexts = new List<ScopeVariable>();
    }

    public ScopeVariable GetCurrentContext()
    {
        return contexts[^1];
    }

    public void SetCurrentContext(ScopeVariable context)
    {
        contexts[^1] = context;
    }

    public void Add(ScopeVariable context)
    {
        contexts.Add(context);
    }

    public ScopeVariable Pop()
    {
        ScopeVariable removed = contexts[^1];
        contexts.RemoveAt(contexts.Count - 1);
        return removed;
    }

    public bool IsEmpty()
    {
        return contexts.Count == 0;
    }
}