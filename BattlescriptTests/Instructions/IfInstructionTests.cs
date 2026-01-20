using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class IfInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void ParsesCondition()
        {
            var input = "if True:";
            var expected = new IfInstruction(new ConstantInstruction("True"));
            var result = Runner.Parse(input, false);
            Assert.That(result[0], Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class Interpret
    {
        [Test]
        public void RunsCodeIfConditionIsTrue()
        {
            var input = """
                        x = 3
                        if True:
                            x = 5
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 5);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }
        
        [Test]
        public void DoesNotRunCodeIfConditionIsFalse()
        {
            var input = """
                        x = 3
                        if False:
                            x = 5
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 3);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void RunsElseCodeIfConditionIsFalse()
        {
            var input = """
                        x = 3
                        if False:
                            x = 5
                        else:
                            x = 7
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 7);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void RunsElifCodeIfConditionIsTrue()
        {
            var input = """
                        x = 3
                        if False:
                            x = 5
                        elif True:
                            x = 6
                        else:
                            x = 7
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 6);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }
        
        [Test]
        public void DoesNotRunElifCodeIfConditionIsFalse()
        {
            var input = """
                        x = 3
                        if False:
                            x = 5
                        elif False:
                            x = 6
                        else:
                            x = 7
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 7);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }
        
        [Test]
        public void RunsFirstTrueElifCode()
        {
            var input = """
                        x = 3
                        if False:
                            x = 5
                        elif True:
                            x = 6
                        elif True:
                            x = 7
                        else:
                            x = 8
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 6);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }
    }
}