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
        var expected = BsTypes.Create(BsTypes.Types.Int, 10);
            
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
}