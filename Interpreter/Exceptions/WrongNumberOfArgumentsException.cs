namespace BattleScript.Exceptions; 

public class WrongNumberOfArgumentsException : Exception {
    public WrongNumberOfArgumentsException(int expected, int actual) : base(String.Format($"Wrong number of arguments: expected {expected}, actual {actual}")) {}
}