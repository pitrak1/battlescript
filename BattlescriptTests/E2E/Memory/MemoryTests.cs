using Battlescript;

namespace BattlescriptTests.E2ETests.Memory;

public class MemoryTests
{
    [Test]
    public void TestMemory()
    {
        var input = """
                    class TestClass():
                        def __init__(self):
                            self.test = 10

                    test = TestClass()
                    x = test.test
                    """;
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.Int, 10);
            
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }
}