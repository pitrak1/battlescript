using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public class BuiltInIsInstanceTests
{
    [Test]
    public void ReturnsTrueIfObjectIsDirectInstanceOfClass()
    {
        var memory = Runner.Run("""
                                class asdf:
                                    i = 5
                                    
                                x = asdf()
                                y = isinstance(x, asdf)
                                """);
        var expected = memory.Create(Memory.BsTypes.Bool, true);
        Assertions.AssertVariable(memory, "y", expected);
    }
    
    [Test]
    public void ReturnsTrueIfObjectIsInheritedInstanceOfClass()
    {
        var memory = Runner.Run("""
                                class asdf:
                                    i = 5
                                    
                                class qwer(asdf):
                                    j = 6
                                    
                                x = qwer()
                                y = isinstance(x, asdf)
                                """);
        var expected = memory.Create(Memory.BsTypes.Bool, true);
        Assertions.AssertVariable(memory, "y", expected);
    }

    [Test]
    public void ReturnsFalseIfObjectIsNotInstanceOfClass()
    {
        var memory = Runner.Run("""
                                class asdf:
                                    i = 5
                                    
                                class qwer:
                                    j = 6
                                    
                                x = qwer()
                                y = isinstance(x, asdf)
                                """);
        var expected = memory.Create(Memory.BsTypes.Bool, false);
        Assertions.AssertVariable(memory, "y", expected);
    }
}