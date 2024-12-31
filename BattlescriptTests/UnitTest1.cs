using Battlescript;

namespace BattlescriptTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        Class1.Print();
        Assert.Pass();
    }
}