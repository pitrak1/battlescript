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
        Assertions.AssertVariablesEqual(memory.Scopes[0]["x"], new SequenceVariable());
    }
    
    [Test]
    public void ConstructorSetsValue()
    {
        var memory = Runner.Run("""
                                x = [1, 2, 3]
                                y = x.__value
                                """);
        Assertions.AssertVariablesEqual(memory.Scopes[0]["x"], new SequenceVariable([
            BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 1),
            BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 2),
            BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 3),
        ]));
    }
    
    [Test]
    public void CanGetItem()
    {
        var memory = Runner.Run("""
                                x = [1, 2, 3]
                                y = x[1]
                                """);
        Assertions.AssertVariablesEqual(memory.Scopes[0]["y"], BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 2));
    }

    [Test]
    public void CanSetItem()
    {
        var memory = Runner.Run("""
                                x = [1, 2, 3]
                                x[1] = 4
                                y = x[1]
                                """);
        Assertions.AssertVariablesEqual(memory.Scopes[0]["y"], BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 4));
    }
}