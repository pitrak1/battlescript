using Battlescript;

namespace BattlescriptTests.InterpreterTests;

[TestFixture]
public class StringUtilitiesTests
{
    [TestFixture]
    public class GetVariableAsString
    {
        [Test]
        public void HandlesInternalStrings()
        {
            var (callStack, closure) = Runner.Run("");
            var input = new StringVariable("asdf");
            Assert.That(StringUtilities.GetVariableAsString(callStack, closure, input), Is.EqualTo("asdf"));
        }
    
        [Test]
        public void HandlesStrings()
        {
            var (callStack, closure) = Runner.Run("");
            var input = BsTypes.Create(BsTypes.Types.String, new StringVariable("asdf"));
            Assert.That(StringUtilities.GetVariableAsString(callStack, closure, input), Is.EqualTo("asdf"));
        }
        
        [Test]
        public void HandlesInts()
        {
            var (callStack, closure) = Runner.Run("");
            var input = BsTypes.Create(BsTypes.Types.Int, new NumericVariable(1));
            Assert.That(StringUtilities.GetVariableAsString(callStack, closure, input), Is.EqualTo("1"));
        }
        
        
        [Test]
        public void HandlesFloats()
        {
            var (callStack, closure) = Runner.Run("");
            var input = BsTypes.Create(BsTypes.Types.Float, new NumericVariable(1.45));
            Assert.That(StringUtilities.GetVariableAsString(callStack, closure, input), Is.EqualTo("1.45"));
        }
        
        [Test]
        public void HandlesTrue()
        {
            var (callStack, closure) = Runner.Run("");
            var input = BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(1));
            Assert.That(StringUtilities.GetVariableAsString(callStack, closure, input), Is.EqualTo("True"));
        }
    
        [Test]
        public void HandlesFalse()
        {
            var (callStack, closure) = Runner.Run("");
            var input = BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(0));
            Assert.That(StringUtilities.GetVariableAsString(callStack, closure, input), Is.EqualTo("False"));
        }
    }

    [TestFixture]
    public class GetFormattedStringValue
    {
        [Test]
        public void HandlesSimpleString()
        {
            var (callStack, closure) = Runner.Run("");
            Assert.That(StringUtilities.GetFormattedStringValue(callStack, closure, "asdf"), Is.EqualTo("asdf"));
        }
        
        [Test]
        public void HandlesInsertedVariable()
        {
            var (callStack, closure) = Runner.Run("x = 5");
            Assert.That(StringUtilities.GetFormattedStringValue(callStack, closure, "asdf{x}qwer"), Is.EqualTo("asdf5qwer"));
        }
        
        [Test]
        public void HandlesInsertedVariableAtStart()
        {
            var (callStack, closure) = Runner.Run("x = 5");
            Assert.That(StringUtilities.GetFormattedStringValue(callStack, closure, "{x}qwer"), Is.EqualTo("5qwer"));
        }
        
        [Test]
        public void HandlesInsertedVariableAtEnd()
        {
            var (callStack, closure) = Runner.Run("x = 5");
            Assert.That(StringUtilities.GetFormattedStringValue(callStack, closure, "asdf{x}"), Is.EqualTo("asdf5"));
        }
        
        [Test]
        public void HandlesMultipleInsertedVariables()
        {
            var (callStack, closure) = Runner.Run("x = 5\ny = 10");
            Assert.That(StringUtilities.GetFormattedStringValue(callStack, closure, "asdf{x}x{y}qwer"), Is.EqualTo("asdf5x10qwer"));
        }
        
        [Test]
        public void HandlesInsertedExpressions()
        {
            var (callStack, closure) = Runner.Run("x = 5\ny = 10");
            Assert.That(StringUtilities.GetFormattedStringValue(callStack, closure, "asdf{x + y}qwer"), Is.EqualTo("asdf15qwer"));
        }
    }
}