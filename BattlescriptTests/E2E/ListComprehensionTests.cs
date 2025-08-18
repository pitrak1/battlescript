using Battlescript;

namespace BattlescriptTests.E2ETests;

[TestFixture]
public class ListComprehensionTests
{
    [Test]
    public void Basic()
    {
        var input = "x = [1, 2, 3]\ny = [i * 2 for i in x]";
        var memory = Runner.Run(input);
        var expected = memory.Create(Memory.BsTypes.List, new SequenceVariable([
            memory.Create(Memory.BsTypes.Int, 2),
            memory.Create(Memory.BsTypes.Int, 4),
            memory.Create(Memory.BsTypes.Int, 6)]));
            
        Assertions.AssertVariable(memory, "y", expected);
    }
}