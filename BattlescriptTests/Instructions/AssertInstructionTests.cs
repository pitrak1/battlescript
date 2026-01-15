using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class AssertInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void ParsesCondition()
        {
            var input = "assert True";
            var expected = new AssertInstruction(new ConstantInstruction("True"));
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class Interpret
    {
        [Test]
        public void ThrowsErrorIfConditionIsFalse()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("assert False"));
            Assert.That(ex.Type, Is.EqualTo("AssertionError"));
        }
        
        [Test]
        public void DoesNotThrowErrorIfConditionIsTrue()
        {
            Assert.DoesNotThrow(() => Runner.Run("assert True"));
        }
    }
}