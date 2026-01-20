using Battlescript;

namespace BattlescriptTests.E2ETests;

[TestFixture]
public class ListComprehensionTests
{
    // [Test]
    // public void Basic()
    // {
    //     var input = "x = [1, 2, 3]\ny = [i * 2 for i in x]";
    //     var callStack = Runner.Run(input);
    //     var expected = callStack.Create(CallStack.BtlTypes.List, new SequenceVariable([
    //         callStack.Create(CallStack.BtlTypes.Int, 2),
    //         callStack.Create(CallStack.BtlTypes.Int, 4),
    //         callStack.Create(CallStack.BtlTypes.Int, 6)]));
    //         
    //     Assertions.AssertVariable(callStack, "y", expected);
    // }
}