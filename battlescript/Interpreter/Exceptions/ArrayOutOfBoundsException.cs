namespace BattleScript.Exceptions; 

public class ArrayOutOfBoundsException : BattleScriptException {
    public ArrayOutOfBoundsException(int index) : base(String.Format($"Index out of bounds: {index}")) {}
}