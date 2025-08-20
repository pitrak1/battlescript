using Battlescript;

namespace BattlescriptTests.Instructions.BuiltIn;

[TestFixture]
public class BuiltInPrintTests
{
    [Test]
    public void HandlesInternalStrings()
    {
        var memory = Runner.Run("");
        var input = new StringVariable("asdf");
        Assert.That(BuiltInPrint.PrintVariable(memory, input), Is.EqualTo("asdf"));
    }

    [Test]
    public void HandlesStrings()
    {
        var memory = Runner.Run("");
        var input = memory.Create(Memory.BsTypes.String, new StringVariable("asdf"));
        Assert.That(BuiltInPrint.PrintVariable(memory, input), Is.EqualTo("asdf"));
    }
    
    [Test]
    public void HandlesInts()
    {
        var memory = Runner.Run("");
        var input = memory.Create(Memory.BsTypes.Int, new NumericVariable(1));
        Assert.That(BuiltInPrint.PrintVariable(memory, input), Is.EqualTo("1"));
    }
    
    
    [Test]
    public void HandlesFloats()
    {
        var memory = Runner.Run("");
        var input = memory.Create(Memory.BsTypes.Float, new NumericVariable(1.45));
        Assert.That(BuiltInPrint.PrintVariable(memory, input), Is.EqualTo("1.45"));
    }
    
    [Test]
    public void HandlesTrue()
    {
        var memory = Runner.Run("");
        var input = memory.Create(Memory.BsTypes.Bool, new NumericVariable(1));
        Assert.That(BuiltInPrint.PrintVariable(memory, input), Is.EqualTo("True"));
    }

    [Test]
    public void HandlesFalse()
    {
        var memory = Runner.Run("");
        var input = memory.Create(Memory.BsTypes.Bool, new NumericVariable(0));
        Assert.That(BuiltInPrint.PrintVariable(memory, input), Is.EqualTo("False"));
    }
}