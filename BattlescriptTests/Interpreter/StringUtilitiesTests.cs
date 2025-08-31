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
            var memory = Runner.Run("");
            var input = new StringVariable("asdf");
            Assert.That(StringUtilities.GetVariableAsString(memory, input), Is.EqualTo("asdf"));
        }
    
        [Test]
        public void HandlesStrings()
        {
            var memory = Runner.Run("");
            var input = memory.Create(Memory.BsTypes.String, new StringVariable("asdf"));
            Assert.That(StringUtilities.GetVariableAsString(memory, input), Is.EqualTo("asdf"));
        }
        
        [Test]
        public void HandlesInts()
        {
            var memory = Runner.Run("");
            var input = memory.Create(Memory.BsTypes.Int, new NumericVariable(1));
            Assert.That(StringUtilities.GetVariableAsString(memory, input), Is.EqualTo("1"));
        }
        
        
        [Test]
        public void HandlesFloats()
        {
            var memory = Runner.Run("");
            var input = memory.Create(Memory.BsTypes.Float, new NumericVariable(1.45));
            Assert.That(StringUtilities.GetVariableAsString(memory, input), Is.EqualTo("1.45"));
        }
        
        [Test]
        public void HandlesTrue()
        {
            var memory = Runner.Run("");
            var input = memory.Create(Memory.BsTypes.Bool, new NumericVariable(1));
            Assert.That(StringUtilities.GetVariableAsString(memory, input), Is.EqualTo("True"));
        }
    
        [Test]
        public void HandlesFalse()
        {
            var memory = Runner.Run("");
            var input = memory.Create(Memory.BsTypes.Bool, new NumericVariable(0));
            Assert.That(StringUtilities.GetVariableAsString(memory, input), Is.EqualTo("False"));
        }
    }

    [TestFixture]
    public class GetFormattedStringValue
    {
        [Test]
        public void HandlesSimpleString()
        {
            var memory = Runner.Run("");
            Assert.That(StringUtilities.GetFormattedStringValue(memory, "asdf"), Is.EqualTo("asdf"));
        }
        
        [Test]
        public void HandlesInsertedVariable()
        {
            var memory = Runner.Run("x = 5");
            Assert.That(StringUtilities.GetFormattedStringValue(memory, "asdf{x}qwer"), Is.EqualTo("asdf5qwer"));
        }
        
        [Test]
        public void HandlesInsertedVariableAtStart()
        {
            var memory = Runner.Run("x = 5");
            Assert.That(StringUtilities.GetFormattedStringValue(memory, "{x}qwer"), Is.EqualTo("5qwer"));
        }
        
        [Test]
        public void HandlesInsertedVariableAtEnd()
        {
            var memory = Runner.Run("x = 5");
            Assert.That(StringUtilities.GetFormattedStringValue(memory, "asdf{x}"), Is.EqualTo("asdf5"));
        }
        
        [Test]
        public void HandlesMultipleInsertedVariables()
        {
            var memory = Runner.Run("x = 5\ny = 10");
            Assert.That(StringUtilities.GetFormattedStringValue(memory, "asdf{x}x{y}qwer"), Is.EqualTo("asdf5x10qwer"));
        }
        
        [Test]
        public void HandlesInsertedExpressions()
        {
            var memory = Runner.Run("x = 5\ny = 10");
            Assert.That(StringUtilities.GetFormattedStringValue(memory, "asdf{x + y}qwer"), Is.EqualTo("asdf15qwer"));
        }
    }
}