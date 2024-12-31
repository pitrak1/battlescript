using Battlescript;

namespace BattlescriptTests;

public class LexerUtilitiesTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        LexerUtilities.Print();
        Assert.Pass();
    }
}