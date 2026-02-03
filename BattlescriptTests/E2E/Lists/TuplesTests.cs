using Battlescript;

namespace BattlescriptTests.E2ETests;

public class TuplesTests
{
    [TestFixture]
    public class PassingTuplesAsArguments
    {
        [Test]
        public void PassTupleToFunction()
        {
            var input = """
                        def foo(t):
                            return t[0] + t[1]
                        x = foo((1, 2))
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(3))));
        }

        [Test]
        public void PassTupleVariableToFunction()
        {
            var input = """
                        def foo(t):
                            return t[0] * t[1]
                        t = (3, 4)
                        x = foo(t)
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(12))));
        }

        [Test]
        public void PassMultipleTuplesToFunction()
        {
            var input = """
                        def combine(a, b):
                            return a[0] + b[0]
                        x = combine((10, 20), (5, 15))
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(15))));
        }

        [Test]
        public void PassNestedTupleToFunction()
        {
            var input = """
                        def foo(t):
                            return t[0][0] + t[1][1]
                        x = foo(((1, 2), (3, 4)))
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(5))));
        }
    }

    [TestFixture]
    public class Indexing
    {
        [Test]
        public void IndexFirstElement()
        {
            var input = """
                        t = (10, 20, 30)
                        x = t[0]
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(10))));
        }

        [Test]
        public void IndexLastElement()
        {
            var input = """
                        t = (10, 20, 30)
                        x = t[2]
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(30))));
        }

        [Test]
        public void NegativeIndex()
        {
            var input = """
                        t = (10, 20, 30)
                        x = t[-1]
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(30))));
        }

        [Test]
        public void IndexNestedTuple()
        {
            var input = """
                        t = ((1, 2), (3, 4))
                        y = t[1]
                        x = t[1][0]
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(3))));
        }

        [Test]
        public void SliceTuple()
        {
            var input = """
                        t = (1, 2, 3, 4, 5)
                        x = t[1:4]
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.List,
                new List<Variable>
                {
                    BtlTypes.Create(BtlTypes.Types.Int, 2),
                    BtlTypes.Create(BtlTypes.Types.Int, 3),
                    BtlTypes.Create(BtlTypes.Types.Int, 4),
                }
            );
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void TupleIsImmutable()
        {
            var input = """
                        t = (1, 2, 3)
                        t[0] = 5
                        """;
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(input));
            Assert.That(ex.Type, Is.EqualTo("TypeError"));
        }

        [Test]
        public void IndexIntoTupleLiteral()
        {
            var input = "x = (1, 2, 3)[1]";
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(2))));
        }
    }

    [TestFixture]
    public class ReturningTuplesFromFunctions
    {
        [Test]
        public void ReturnTupleFromFunction()
        {
            var input = """
                        def foo():
                            return (1, 2, 3)
                        t = foo()
                        x = t[0] + t[1] + t[2]
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(6))));
        }

        [Test]
        public void UnpackReturnedTuple()
        {
            var input = """
                        def foo():
                            return (10, 20)
                        a, b = foo()
                        x = a + b
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(30))));
        }

        [Test]
        public void ReturnNestedTuple()
        {
            var input = """
                        def foo():
                            return ((1, 2), (3, 4))
                        t = foo()
                        x = t[0][0] + t[1][1]
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(5))));
        }

        [Test]
        public void ReturnTupleWithMixedTypes()
        {
            var input = """
                        def foo():
                            return (1, 'hello', 3.14)
                        t = foo()
                        x = t[1]
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.String, "hello")));
        }

        [Test]
        public void IndexIntoReturnedTupleDirectly()
        {
            var input = """
                        def foo():
                            return (5, 10, 15)
                        x = foo()[1]
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(10))));
        }
    }

    [TestFixture]
    public class MethodCallsOnLiterals
    {
        [Test]
        public void IntegerAbsWithParentheses()
        {
            var input = "x = (-5).__abs__()";
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(5))));
        }

        [Test]
        public void PositiveIntegerAbsWithParentheses()
        {
            var input = "x = (5).__abs__()";
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(5))));
        }

        [Test]
        public void FloatAbsWithParentheses()
        {
            var input = "x = (-3.14).__abs__()";
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Float, new NumericVariable(3.14))));
        }

        [Test]
        public void ChainedMethodCallOnLiteral()
        {
            var input = "x = ('hello').__len__()";
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(5))));
        }

        [Test]
        public void MethodOnTupleLiteral()
        {
            var input = "x = (1, 2, 3).__len__()";
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(3))));
        }

        [Test]
        public void SingleElementTupleInParentheses()
        {
            var input = "x = (1,).__len__()";
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(1))));
        }
    }

    [TestFixture]
    public class TupleUnpacking
    {
        [Test]
        public void BasicUnpacking()
        {
            var input = """
                        t = (1, 2, 3)
                        a, b, c = t
                        x = a + b + c
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(6))));
        }

        [Test]
        public void UnpackingTupleInFunctionArguments()
        {
            var input = """
                        def foo(a, b, c):
                            return a + b + c
                        t = (1, 2, 3)
                        x = foo(*t)
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(6))));
        }

        [Test]
        public void UnpackingListInFunctionArguments()
        {
            var input = """
                        def foo(a, b, c):
                            return a + b + c
                        lst = [1, 2, 3]
                        x = foo(*lst)
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(6))));
        }

        [Test]
        public void UnpackingDictAsKwargs()
        {
            var input = """
                        def foo(a, b, c):
                            return a + b + c
                        d = {'a': 1, 'b': 2, 'c': 3}
                        x = foo(**d)
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(6))));
        }

        [Test]
        public void MixedPositionalAndUnpackedArgs()
        {
            var input = """
                        def foo(a, b, c, d):
                            return a + b + c + d
                        t = (2, 3)
                        x = foo(1, *t, 4)
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(10))));
        }

        [Test]
        public void UnpackBothArgsAndKwargs()
        {
            var input = """
                        def foo(a, b, c, d):
                            return a + b + c + d
                        t = (1, 2)
                        d = {'c': 3, 'd': 4}
                        x = foo(*t, **d)
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(10))));
        }
    }

    [TestFixture]
    public class TupleEquality
    {
        [Test]
        public void EqualTuples()
        {
            var input = """
                        a = (1, 2, 3)
                        b = (1, 2, 3)
                        x = a == b
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
        }

        [Test]
        public void UnequalTuples()
        {
            var input = """
                        a = (1, 2, 3)
                        b = (1, 2, 4)
                        x = a == b
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
        }
    }

    [TestFixture]
    public class EmptyTuple
    {
        [Test]
        public void EmptyTupleLength()
        {
            var input = """
                        t = ()
                        x = len(t)
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(0))));
        }

        [Test]
        public void EmptyTupleReturnedFromFunction()
        {
            var input = """
                        def foo():
                            return ()
                        t = foo()
                        x = len(t)
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(0))));
        }
    }
}
