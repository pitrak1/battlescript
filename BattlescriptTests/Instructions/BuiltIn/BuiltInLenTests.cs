using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public class BuiltInLenTests
{
    [Test]
    public void EmptyString()
    {
        var memory = Runner.Run("""
                                x = len("")
                                """);
        var expected = memory.Create(Memory.BsTypes.Int, 0);
        Assertions.AssertVariable(memory, "x", expected);
    }
    
    [Test]
    public void NonEmptyString()
    {
        var memory = Runner.Run("""
                                x = len("asdf")
                                """);
        var expected = memory.Create(Memory.BsTypes.Int, 4);
        Assertions.AssertVariable(memory, "x", expected);
    }
}