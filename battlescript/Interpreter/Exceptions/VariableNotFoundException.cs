namespace BattleScript.Exceptions; 

public class VariableNotFoundException : BattleScriptException {
    public VariableNotFoundException(string name) : base(String.Format($"Variable not found: {name}")) {}
}