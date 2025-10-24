using System.Runtime.InteropServices;
using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public class BuiltInAbsTests
{
    [Test]
    public void HandlesPositiveInts()
    {
        var input = "x = abs(5)";
        var memory = Runner.Run(input);
        var expected = BsTypes.Create(BsTypes.Types.Int, new NumericVariable(5));
        Assertions.AssertVariable(memory, "x", expected);
    }
    
    [Test]
    public void HandlesNegativeInts()
    {
        var input = "x = abs(-5)";
        var memory = Runner.Run(input);
        var expected = BsTypes.Create(BsTypes.Types.Int, new NumericVariable(5));
        Assertions.AssertVariable(memory, "x", expected);
    }
    
    [Test]
    public void HandlesPositiveFloats()
    {
        var input = "x = abs(5.5)";
        var memory = Runner.Run(input);
        var expected = BsTypes.Create(BsTypes.Types.Float, new NumericVariable(5.5));
        Assertions.AssertVariable(memory, "x", expected);
    }
    
    [Test]
    public void HandlesNegativeFloats()
    {
        var input = "x = abs(-5.5)";
        var memory = Runner.Run(input);
        var expected = BsTypes.Create(BsTypes.Types.Float, new NumericVariable(5.5));
        Assertions.AssertVariable(memory, "x", expected);
    }

    [Test]
    public void HandlesClassMethodOverriding()
    {
        var input = """
                    class y:
                        def __abs__(self):
                            return 5
                        
                    z = y()
                    x = abs(z)
                    """;
        var memory = Runner.Run(input);
        var expected = BsTypes.Create(BsTypes.Types.Int, new NumericVariable(5));
        Assertions.AssertVariable(memory, "x", expected);
    }

    [Test]
    public void ThrowsErrorIfWrongNumberOfArguments()
    {
        var input = "x = abs(4, 5)";
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(input));
        Assert.That(ex.Message, Is.EqualTo("abs() takes exactly one argument (2 given)"));
        Assert.That(ex.Type, Is.EqualTo("TypeError"));
    }
    
    [Test]
    public void ThrowsErrorIfWrongArgumentType()
    {
        var input = "x = abs('asdf')";
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(input));
        Assert.That(ex.Message, Is.EqualTo("bad operand type for abs(): 'str'"));
        Assert.That(ex.Type, Is.EqualTo("TypeError"));
    }
}