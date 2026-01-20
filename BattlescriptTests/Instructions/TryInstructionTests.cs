using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class TryInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void ProperlyParsesTry()
        {
            var input = """
                        try:
                            x = 1
                        """;
            var expected = new List<Instruction>() {new TryInstruction([
                new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(1))
            ])};
            var result = Runner.Parse(input);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ProperlyParsesTryExcept()
        {
            var input = """
                        try:
                            x = 1
                        except TypeError:
                            x = 2
                        """;
            var expected = new List<Instruction>() {new TryInstruction([
                new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(1))
            ], new List<ExceptInstruction>()
            {
                new ExceptInstruction(
                    new VariableInstruction("TypeError"),
                    [new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(2))])
            })};
            var result = Runner.Parse(input);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ProperlyParsesTryExceptWithMultipleExcepts()
        {
            var input = """
                        try:
                            x = 1
                        except TypeError:
                            x = 2
                        except AssertionError:
                            x = 3
                        """;
            var expected = new List<Instruction>() {new TryInstruction([
                new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(1))
            ], new List<ExceptInstruction>()
            {
                new ExceptInstruction(
                    new VariableInstruction("TypeError"),
                    [new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(2))]),
                new ExceptInstruction(
                    new VariableInstruction("AssertionError"),
                    [new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(3))])
            })};
            var result = Runner.Parse(input);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ProperlyParsesTryExceptElse()
        {
            var input = """
                        try:
                            x = 1
                        except TypeError:
                            x = 2
                        except AssertionError:
                            x = 3
                        else:
                            x = 4
                        """;
            var expected = new List<Instruction>() {new TryInstruction([
                new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(1))
            ], new List<ExceptInstruction>()
            {
                new ExceptInstruction(
                    new VariableInstruction("TypeError"),
                    [new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(2))]),
                new ExceptInstruction(
                    new VariableInstruction("AssertionError"),
                    [new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(3))])
            }, new ElseInstruction(null, null, new List<Instruction>() { new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(4))}))};
            var result = Runner.Parse(input);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ProperlyParsesTryExceptElseFinally()
        {
            var input = """
                        try:
                            x = 1
                        except TypeError:
                            x = 2
                        except AssertionError:
                            x = 3
                        else:
                            x = 4
                        finally:
                            x = 5
                        """;
            var expected = new List<Instruction>() {new TryInstruction(
            [new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(1))],
            new List<ExceptInstruction>()
            {
                new ExceptInstruction(
                    new VariableInstruction("TypeError"),
                    [new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(2))]),
                new ExceptInstruction(
                    new VariableInstruction("AssertionError"),
                    [new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(3))])
            },
            new ElseInstruction(null, null, new List<Instruction>() { new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(4))}),
            new FinallyInstruction(new List<Instruction>() { new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(5))}))};
            var result = Runner.Parse(input);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ProperlyParsesTryExceptWithAs()
        {
            var input = """
                        try:
                            x = 1
                        except TypeError as f:
                            x = 2
                        """;
            var expected = new List<Instruction>() {new TryInstruction([
                new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(1))
            ], new List<ExceptInstruction>()
            {
                new ExceptInstruction(
                    new VariableInstruction("TypeError"),
                    [new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(2))],
                    new VariableInstruction("f"))
            })};
            var result = Runner.Parse(input);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void SupportsNestedTryExceptInTryBlock()
        {
            var input = """
                        try:
                            try:
                                x = 1
                            except AssertionError:
                                x = 3
                        except TypeError:
                            x = 2
                        """;
            var innerTryInstruction = new TryInstruction([
                new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(1))
            ], new List<ExceptInstruction>()
            {
                new ExceptInstruction(
                    new VariableInstruction("AssertionError"),
                    [new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(3))])
            });
            var expected = new List<Instruction>() {new TryInstruction([
                innerTryInstruction
            ], new List<ExceptInstruction>()
            {
                new ExceptInstruction(
                    new VariableInstruction("TypeError"),
                    [new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(2))])
            })};
            var result = Runner.Parse(input);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
