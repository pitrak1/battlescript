using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public class BuiltInIsSubclassTests
{
    [Test]
    public void ReturnsTrueIfFirstClassIsEqualToSecondClass()
    {
        var memory = Runner.Run("""
                                class asdf:
                                    i = 5
                                    
                                y = issubclass(asdf, asdf)
                                """);
        var expected = memory.Create(Memory.BsTypes.Bool, true);
        Assertions.AssertVariable(memory, "y", expected);
    }
    
    [Test]
    public void ReturnsTrueIfFirstClassInheritsFromSecondClass()
    {
        var memory = Runner.Run("""
                                class asdf:
                                    i = 5
                                    
                                class qwer(asdf):
                                    j = 6
                                    
                                y = issubclass(qwer, asdf)
                                """);
        var expected = memory.Create(Memory.BsTypes.Bool, true);
        Assertions.AssertVariable(memory, "y", expected);
    }
    
    [Test]
    public void ReturnsFalseIfFirstClassDoesNotInheritFromSecondClass()
    {
        var memory = Runner.Run("""
                                class asdf:
                                    i = 5
                                    
                                class qwer(asdf):
                                    j = 6
                                    
                                y = issubclass(asdf, qwer)
                                """);
        var expected = memory.Create(Memory.BsTypes.Bool, false);
        Assertions.AssertVariable(memory, "y", expected);
    }
}