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
        Assertions.AssertVariablesEqual(memory.Scopes[0]["y"], new SequenceVariable());
    }
    
    [Test]
    public void ConstructorSetsValue()
    {
        var memory = Runner.Run("""
                                x = [1, 2, 3]
                                y = x.__value
                                """);
        Assertions.AssertVariablesEqual(memory.Scopes[0]["y"], new SequenceVariable([
            BsTypes.Create(memory, "int", 1),
            BsTypes.Create(memory, "int", 2),
            BsTypes.Create(memory, "int", 3),
        ]));
    }
    
    [Test]
    public void CanGetItem()
    {
        var memory = Runner.Run("""
                                x = [1, 2, 3]
                                y = x[1]
                                """);
        Assertions.AssertVariablesEqual(memory.Scopes[0]["y"], BsTypes.Create(memory, "int", 2));
    }

    [Test]
    public void CanSetItem()
    {
        var memory = Runner.Run("""
                                x = [1, 2, 3]
                                x[1] = 4
                                y = x[1]
                                """);
        Assertions.AssertVariablesEqual(memory.Scopes[0]["y"], BsTypes.Create(memory, "int", 4));
    }
}