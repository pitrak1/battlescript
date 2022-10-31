namespace BattleScript.Exceptions; 

public class VariableNotFoundException : Exception {
    public VariableNotFoundException(string name) : base(String.Format($"Variable not found: {name}")) {}
}