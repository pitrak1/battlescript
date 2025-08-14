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
            var expected = new AssertInstruction(new ConstantInstruction("True"));
            Assertions.AssertInputProducesParserOutput("assert True", expected);
        }
    }

    [TestFixture]
    public class Interpret
    {
        [Test]
        public void ThrowsErrorIfConditionIsFalse()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("assert False"));
            Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.AssertionError));
        }
        
        [Test]
        public void DoesNotThrowErrorIfConditionIsTrue()
        {
            Assert.DoesNotThrow(() => Runner.Run("assert True"));
        }
    }
}