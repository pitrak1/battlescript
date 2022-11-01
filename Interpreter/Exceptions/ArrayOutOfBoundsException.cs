namespace BattleScript.Exceptions; 

public class ArrayOutOfBoundsException : Exception {
    public ArrayOutOfBoundsException(int index) : base(String.Format($"Index out of bounds: {index}")) {}
}