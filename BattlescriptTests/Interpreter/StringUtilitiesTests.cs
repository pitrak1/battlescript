using Battlescript;

namespace BattlescriptTests.InterpreterTests;

[TestFixture]
public class StringUtilitiesTests
{
    [TestFixture]
    public class GetVariableAsString
    {
        [Test]
        public void InternalStrings()
        {
            var (callStack, closure) = Runner.Run("");
            var input = new StringVariable("asdf");
            Assert.That(StringUtilities.GetVariableAsString(callStack, closure, input), Is.EqualTo("asdf"));
        }
    
        [Test]
        public void Strings()
        {
            var (callStack, closure) = Runner.Run("");
            var input = BtlTypes.Create(BtlTypes.Types.String, new StringVariable("asdf"));
            Assert.That(StringUtilities.GetVariableAsString(callStack, closure, input), Is.EqualTo("asdf"));
        }
        
        [Test]
        public void Ints()
        {
            var (callStack, closure) = Runner.Run("");
            var input = BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(1));
            Assert.That(StringUtilities.GetVariableAsString(callStack, closure, input), Is.EqualTo("1"));
        }
        
        
        [Test]
        public void Floats()
        {
            var (callStack, closure) = Runner.Run("");
            var input = BtlTypes.Create(BtlTypes.Types.Float, new NumericVariable(1.45));
            Assert.That(StringUtilities.GetVariableAsString(callStack, closure, input), Is.EqualTo("1.45"));
        }
        
        [Test]
        public void True()
        {
            var (callStack, closure) = Runner.Run("");
            var input = BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(1));
            Assert.That(StringUtilities.GetVariableAsString(callStack, closure, input), Is.EqualTo("True"));
        }
    
        [Test]
        public void False()
        {
            var (callStack, closure) = Runner.Run("");
            var input = BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(0));
            Assert.That(StringUtilities.GetVariableAsString(callStack, closure, input), Is.EqualTo("False"));
        }
    }

    [TestFixture]
    public class GetFormattedStringValue
    {
        [Test]
        public void SimpleString()
        {
            var (callStack, closure) = Runner.Run("");
            Assert.That(StringUtilities.GetFormattedStringValue(callStack, closure, "asdf"), Is.EqualTo("asdf"));
        }
        
        [Test]
        public void InsertedVariable()
        {
            var (callStack, closure) = Runner.Run("x = 5");
            Assert.That(StringUtilities.GetFormattedStringValue(callStack, closure, "asdf{x}qwer"), Is.EqualTo("asdf5qwer"));
        }
        
        [Test]
        public void InsertedVariableAtStart()
        {
            var (callStack, closure) = Runner.Run("x = 5");
            Assert.That(StringUtilities.GetFormattedStringValue(callStack, closure, "{x}qwer"), Is.EqualTo("5qwer"));
        }
        
        [Test]
        public void InsertedVariableAtEnd()
        {
            var (callStack, closure) = Runner.Run("x = 5");
            Assert.That(StringUtilities.GetFormattedStringValue(callStack, closure, "asdf{x}"), Is.EqualTo("asdf5"));
        }
        
        [Test]
        public void MultipleInsertedVariables()
        {
            var (callStack, closure) = Runner.Run("x = 5\ny = 10");
            Assert.That(StringUtilities.GetFormattedStringValue(callStack, closure, "asdf{x}x{y}qwer"), Is.EqualTo("asdf5x10qwer"));
        }
        
        [Test]
        public void InsertedExpressions()
        {
            var (callStack, closure) = Runner.Run("x = 5\ny = 10");
            Assert.That(StringUtilities.GetFormattedStringValue(callStack, closure, "asdf{x + y}qwer"), Is.EqualTo("asdf15qwer"));
        }
    }
}