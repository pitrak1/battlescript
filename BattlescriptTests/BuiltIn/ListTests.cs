using Battlescript;

namespace BattlescriptTests.BuiltIn;

[TestFixture]
public class ListTests
{
    [Test]
    public void ValueIsSequenceVariable()
    {
        var memory = Runner.Run("""
                                x = []
                                y = x.__value
                                """);
        Assertions.AssertVariable(memory, "y", new SequenceVariable());
    }
    
    [Test]
    public void ConstructorSetsValue()
    {
        var memory = Runner.Run("""
                                x = [1, 2, 3]
                                y = x.__value
                                """);
        Assertions.AssertVariable(memory, "y", new SequenceVariable([
            BsTypes.Create(memory, BsTypes.Types.Int, 1),
            BsTypes.Create(memory, BsTypes.Types.Int, 2),
            BsTypes.Create(memory, BsTypes.Types.Int, 3),
        ]));
    }
    
    [Test]
    public void CanGetItem()
    {
        var memory = Runner.Run("""
                                x = [1, 2, 3]
                                y = x[1]
                                """);
        Assertions.AssertVariable(memory, "y", BsTypes.Create(memory, BsTypes.Types.Int, 2));
    }

    [Test]
    public void CanSetItem()
    {
        var memory = Runner.Run("""
                                x = [1, 2, 3]
                                x[1] = 4
                                y = x[1]
                                """);
        Assertions.AssertVariable(memory, "y", BsTypes.Create(memory, BsTypes.Types.Int, 4));
    }
}