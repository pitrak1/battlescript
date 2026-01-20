using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class ForInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void ParsesBlockVariableAndRange()
        {
            var input = "for x in range(5):";
            var expected = new ForInstruction(
                new VariableInstruction("x"),
                new BuiltInInstruction("range", [new NumericInstruction(5)])
            );
            var result = Runner.Parse(input, false);
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void ParsesBlockVariableAndList()
        {
            var input = "for y in [1, 2, 3]:";
            var expected = new ForInstruction(
                new VariableInstruction("y"),
                new ArrayInstruction(
                    [new NumericInstruction(1), new NumericInstruction(2), new NumericInstruction(3)],
                    ArrayInstruction.BracketTypes.SquareBrackets,
                    ArrayInstruction.DelimiterTypes.Comma
                )
            );
            var result = Runner.Parse(input, false);
            Assert.That(result[0], Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class Interpret
    {
        [Test]
        public void IteratesOverRange()
        {
            var input = """
                        x = 0
                        for i in range(5):
                            x = x + i
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 10); // 0 + 1 + 2 + 3 + 4 = 10
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void IteratesOverList()
        {
            var input = """
                        x = 0
                        for i in [5, 10, 15]:
                            x = x + i
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 30); // 5 + 10 + 15 = 30
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void SetsBlockVariableInEachIteration()
        {
            var input = """
                        x = 0
                        for i in [1, 2, 3]:
                            x = i
                        """;
            var (callStack, closure) = Runner.Run(input);
            // After loop, x should be 3 (last value assigned)
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 3);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }
        
        [Test]
        public void EmptyList()
        {
            var input = """
                        x = 5
                        for i in []:
                            x = x + 1
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 5); // Loop body never executes
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void SingleElementList()
        {
            var input = """
                        x = 0
                        for i in [42]:
                            x = i
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 42);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void SupportsContinueStatement()
        {
            var input = """
                        x = 0
                        for i in range(5):
                            if i == 2:
                                continue
                            x = x + i
                        """;
            var (callStack, closure) = Runner.Run(input);
            // 0 + 1 + 3 + 4 = 8 (skips 2)
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 8);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void SupportsBreakStatement()
        {
            var input = """
                        x = 0
                        for i in range(10):
                            if i == 3:
                                break
                            x = x + i
                        """;
            var (callStack, closure) = Runner.Run(input);
            // 0 + 1 + 2 = 3 (stops at i == 3)
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 3);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void NestedForLoops()
        {
            var input = """
                        x = 0
                        for i in [1, 2]:
                            for j in [10, 20]:
                                x = x + i + j
                        """;
            var (callStack, closure) = Runner.Run(input);
            // (1+10) + (1+20) + (2+10) + (2+20) = 11 + 21 + 12 + 22 = 66
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 66);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }
    }
}
