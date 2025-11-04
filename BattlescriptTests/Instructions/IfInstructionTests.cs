using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class IfInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void ProperlyParsesCondition()
        {
            var expected = new IfInstruction(new ConstantInstruction("True"));
            Assertions.AssertInputProducesParserOutput("if True:", expected);
        }
    }

    [TestFixture]
    public class Interpret
    {
        [Test]
        public void RunsCodeIfConditionIsTrue()
        {
            var (callStack, closure) = Runner.Run("x = 3\nif True:\n\tx = 5");
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Int, 5));
        }
        
        [Test]
        public void DoesNotRunCodeIfConditionIsFalse()
        {
            var (callStack, closure) = Runner.Run("x = 3\nif False:\n\tx = 5");
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Int, 3));
        }
    }
}