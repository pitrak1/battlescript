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
    //     var expected = callStack.Create(CallStack.BsTypes.List, new SequenceVariable([
    //         callStack.Create(CallStack.BsTypes.Int, 2),
    //         callStack.Create(CallStack.BsTypes.Int, 4),
    //         callStack.Create(CallStack.BsTypes.Int, 6)]));
    //         
    //     Assertions.AssertVariable(callStack, "y", expected);
    // }
}