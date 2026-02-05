using Battlescript;

namespace BattlescriptTests.E2ETests;

[TestFixture]
public static class ShortCircuitTests
{
    [TestFixture]
    public class AndOperator
    {
        [Test]
        public void ShortCircuitsOnFirstFalseValue()
        {
            var input = """
                        x = 0
                        def side_effect():
                            nonlocal x
                            x = 1
                            return True

                        result = False and side_effect()
                        """;
            var (callStack, closure) = Runner.Run(input);

            // x should still be 0 because side_effect() was never called
            var expectedX = BtlTypes.Create(BtlTypes.Types.Int, 0);
            var expectedResult = BtlTypes.Create(BtlTypes.Types.Bool, false);

            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expectedX));
            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expectedResult));
        }

        [Test]
        public void EvaluatesSecondOperandWhenFirstIsTrue()
        {
            var input = """
                        x = 0
                        def side_effect():
                            nonlocal x
                            x = 1
                            return False

                        result = True and side_effect()
                        """;
            var (callStack, closure) = Runner.Run(input);

            // x should be 1 because side_effect() was called
            var expectedX = BtlTypes.Create(BtlTypes.Types.Int, 1);
            var expectedResult = BtlTypes.Create(BtlTypes.Types.Bool, false);

            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expectedX));
            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expectedResult));
        }

        [Test]
        public void ReturnsFirstFalsyValue()
        {
            var input = "result = 0 and 5";
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 0);

            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
        }

        [Test]
        public void ReturnsLastValueWhenAllTruthy()
        {
            var input = "result = 5 and 10";
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 10);

            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
        }

        [Test]
        public void WorksWithMultipleOperands()
        {
            var input = """
                        counter = 0
                        def increment():
                            nonlocal counter
                            counter = counter + 1
                            return True

                        result = True and increment() and False and increment()
                        """;
            var (callStack, closure) = Runner.Run(input);

            // counter should be 1, not 2, because the second increment() is never called
            var expectedCounter = BtlTypes.Create(BtlTypes.Types.Int, 1);
            var expectedResult = BtlTypes.Create(BtlTypes.Types.Bool, false);

            Assert.That(closure.GetVariable(callStack, "counter"), Is.EqualTo(expectedCounter));
            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expectedResult));
        }

        [Test]
        public void WorksWithNonBooleanFalsyValues()
        {
            var input = """
                        x = 0
                        def side_effect():
                            nonlocal x
                            x = 1
                            return True

                        result = 0 and side_effect()
                        """;
            var (callStack, closure) = Runner.Run(input);

            // x should still be 0 because side_effect() was never called (0 is falsy)
            var expectedX = BtlTypes.Create(BtlTypes.Types.Int, 0);
            var expectedResult = BtlTypes.Create(BtlTypes.Types.Int, 0);

            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expectedX));
            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expectedResult));
        }

        [Test]
        public void WorksWithEmptyString()
        {
            var input = """
                        x = 0
                        def side_effect():
                            nonlocal x
                            x = 1
                            return True

                        result = "" and side_effect()
                        """;
            var (callStack, closure) = Runner.Run(input);

            // x should still be 0 because side_effect() was never called ("" is falsy)
            var expectedX = BtlTypes.Create(BtlTypes.Types.Int, 0);
            var expectedResult = BtlTypes.Create(BtlTypes.Types.String, "");

            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expectedX));
            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expectedResult));
        }

        [Test]
        public void WorksWithEmptyList()
        {
            var input = """
                        x = 0
                        def side_effect():
                            nonlocal x
                            x = 1
                            return True

                        result = [] and side_effect()
                        """;
            var (callStack, closure) = Runner.Run(input);

            // x should still be 0 because side_effect() was never called ([] is falsy)
            var expectedX = BtlTypes.Create(BtlTypes.Types.Int, 0);
            var expectedResult = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>());

            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expectedX));
            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expectedResult));
        }
    }

    [TestFixture]
    public class OrOperator
    {
        [Test]
        public void ShortCircuitsOnFirstTrueValue()
        {
            var input = """
                        x = 0
                        def side_effect():
                            nonlocal x
                            x = 1
                            return False

                        result = True or side_effect()
                        """;
            var (callStack, closure) = Runner.Run(input);

            // x should still be 0 because side_effect() was never called
            var expectedX = BtlTypes.Create(BtlTypes.Types.Int, 0);
            var expectedResult = BtlTypes.Create(BtlTypes.Types.Bool, true);

            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expectedX));
            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expectedResult));
        }

        [Test]
        public void EvaluatesSecondOperandWhenFirstIsFalse()
        {
            var input = """
                        x = 0
                        def side_effect():
                            nonlocal x
                            x = 1
                            return True

                        result = False or side_effect()
                        """;
            var (callStack, closure) = Runner.Run(input);

            // x should be 1 because side_effect() was called
            var expectedX = BtlTypes.Create(BtlTypes.Types.Int, 1);
            var expectedResult = BtlTypes.Create(BtlTypes.Types.Bool, true);

            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expectedX));
            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expectedResult));
        }

        [Test]
        public void ReturnsFirstTruthyValue()
        {
            var input = "result = 5 or 10";
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 5);

            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
        }

        [Test]
        public void ReturnsLastValueWhenAllFalsy()
        {
            var input = "result = 0 or False";
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Bool, false);

            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
        }

        [Test]
        public void WorksWithMultipleOperands()
        {
            var input = """
                        counter = 0
                        def increment():
                            nonlocal counter
                            counter = counter + 1
                            return False

                        result = False or increment() or True or increment()
                        """;
            var (callStack, closure) = Runner.Run(input);

            // counter should be 1, not 2, because the second increment() is never called
            var expectedCounter = BtlTypes.Create(BtlTypes.Types.Int, 1);
            var expectedResult = BtlTypes.Create(BtlTypes.Types.Bool, true);

            Assert.That(closure.GetVariable(callStack, "counter"), Is.EqualTo(expectedCounter));
            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expectedResult));
        }

        [Test]
        public void WorksWithNonBooleanTruthyValues()
        {
            var input = """
                        x = 0
                        def side_effect():
                            nonlocal x
                            x = 1
                            return False

                        result = 5 or side_effect()
                        """;
            var (callStack, closure) = Runner.Run(input);

            // x should still be 0 because side_effect() was never called (5 is truthy)
            var expectedX = BtlTypes.Create(BtlTypes.Types.Int, 0);
            var expectedResult = BtlTypes.Create(BtlTypes.Types.Int, 5);

            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expectedX));
            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expectedResult));
        }

        [Test]
        public void WorksWithNonEmptyString()
        {
            var input = """
                        x = 0
                        def side_effect():
                            nonlocal x
                            x = 1
                            return False

                        result = "hello" or side_effect()
                        """;
            var (callStack, closure) = Runner.Run(input);

            // x should still be 0 because side_effect() was never called ("hello" is truthy)
            var expectedX = BtlTypes.Create(BtlTypes.Types.Int, 0);
            var expectedResult = BtlTypes.Create(BtlTypes.Types.String, "hello");

            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expectedX));
            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expectedResult));
        }
    }

    [TestFixture]
    public class CombinedOperators
    {
        [Test]
        public void WorksWithAndThenOr()
        {
            var input = """
                        x = 0
                        def increment():
                            nonlocal x
                            x = x + 1
                            return x

                        result = False and increment() or increment()
                        """;
            var (callStack, closure) = Runner.Run(input);

            // x should be 1 (only the second increment() is called)
            var expectedX = BtlTypes.Create(BtlTypes.Types.Int, 1);
            var expectedResult = BtlTypes.Create(BtlTypes.Types.Int, 1);

            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expectedX));
            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expectedResult));
        }

        [Test]
        public void WorksWithOrThenAnd()
        {
            var input = """
                        x = 0
                        def increment():
                            nonlocal x
                            x = x + 1
                            return x

                        result = True or increment() and increment()
                        """;
            var (callStack, closure) = Runner.Run(input);

            // x should be 0 (neither increment() is called due to short circuit)
            var expectedX = BtlTypes.Create(BtlTypes.Types.Int, 0);
            var expectedResult = BtlTypes.Create(BtlTypes.Types.Bool, true);

            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expectedX));
            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expectedResult));
        }

        [Test]
        public void WorksWithComplexExpression()
        {
            var input = """
                        counter = 0
                        def make_func(val):
                            def inner():
                                nonlocal counter
                                counter = counter + 1
                                return val
                            return inner

                        result = (False and make_func(1)()) or (True and make_func(2)()) or make_func(3)()
                        """;
            var (callStack, closure) = Runner.Run(input);

            // counter should be 1 (only make_func(2)() is called)
            var expectedCounter = BtlTypes.Create(BtlTypes.Types.Int, 1);
            var expectedResult = BtlTypes.Create(BtlTypes.Types.Int, 2);

            Assert.That(closure.GetVariable(callStack, "counter"), Is.EqualTo(expectedCounter));
            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expectedResult));
        }
    }

    [TestFixture]
    public class PracticalUseCases
    {
        [Test]
        public void WorksForDefaultValuePattern()
        {
            var input = """
                        name = ""
                        display = name or "Anonymous"
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.String, "Anonymous");

            Assert.That(closure.GetVariable(callStack, "display"), Is.EqualTo(expected));
        }

        [Test]
        public void WorksForGuardPattern()
        {
            var input = """
                        data = None
                        result = data and data.value
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = new NoneVariable();

            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
        }

        [Test]
        public void WorksForChainedChecks()
        {
            var input = """
                        x = 5
                        result = x > 0 and x < 10 and x != 7
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Bool, true);

            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
        }
    }
}
