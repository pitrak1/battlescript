namespace BattleScript;

public class Tests {
    [SetUp]
    public void Setup() {
    }

    [Test]
    public void Test1() {
        Assert.That(Consts.Booleans[0], Is.EqualTo("true"));
    }
}