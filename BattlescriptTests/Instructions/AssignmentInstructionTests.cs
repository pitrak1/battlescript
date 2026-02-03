using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class AssignmentInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void Value()
        {
            var input = "x = 6";
            var expected = new AssignmentInstruction(
                operation: "=",
                left: new VariableInstruction("x"),
                right: new NumericInstruction(6)
            );
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void Expression()
        {
            var input = "x = 6 + 5";
            var expected = new AssignmentInstruction(
                operation: "=",
                left: new VariableInstruction("x"),
                right: new OperationInstruction("+", new NumericInstruction(6), new NumericInstruction(5))
            );
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void TupleUnpacking()
        {
            var input = "a, b = (1, 2)";
            var expected = new AssignmentInstruction(
                operation: "=",
                left: new ArrayInstruction(
                    values: [new VariableInstruction("a"), new VariableInstruction("b")],
                    delimiter: ArrayInstruction.DelimiterTypes.Comma),
                right: new ArrayInstruction(
                    values: [new NumericInstruction(1), new NumericInstruction(2)],
                    bracket: ArrayInstruction.BracketTypes.Parentheses,
                    delimiter: ArrayInstruction.DelimiterTypes.Comma)
            );
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void TupleUnpackingWithoutParentheses()
        {
            var input = "a, b = 1, 2";
            var expected = new AssignmentInstruction(
                operation: "=",
                left: new ArrayInstruction(
                    values: [new VariableInstruction("a"), new VariableInstruction("b")],
                    delimiter: ArrayInstruction.DelimiterTypes.Comma),
                right: new ArrayInstruction(
                    values: [new NumericInstruction(1), new NumericInstruction(2)],
                    delimiter: ArrayInstruction.DelimiterTypes.Comma)
            );
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class Interpret
    {
        [Test]
        public void SimpleAssignments()
        {
            var (callStack, closure) = Runner.Run("x = 6");
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 6);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void AssignmentOperators()
        {
            var (callStack, closure) = Runner.Run("x = 6\nx += 2");
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 8);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class TupleUnpacking
    {
        [Test]
        public void BasicTupleUnpacking()
        {
            var input = """
                        a, b = (1, 2)
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "a"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 1)));
            Assert.That(closure.GetVariable(callStack, "b"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 2)));
        }

        [Test]
        public void TupleUnpackingFromVariable()
        {
            var input = """
                        t = (10, 20, 30)
                        a, b, c = t
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "a"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 10)));
            Assert.That(closure.GetVariable(callStack, "b"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 20)));
            Assert.That(closure.GetVariable(callStack, "c"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 30)));
        }

        [Test]
        public void TupleUnpackingFromFunctionReturn()
        {
            var input = """
                        def foo():
                            return (1, 2)
                        a, b = foo()
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "a"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 1)));
            Assert.That(closure.GetVariable(callStack, "b"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 2)));
        }

        [Test]
        public void ThrowsErrorIfTupleLengthsMismatch()
        {
            var input = """
                        a, b = (1, 2, 3)
                        """;
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(input));
            Assert.That(ex.Type, Is.EqualTo("ValueError"));
        }

        [Test]
        public void ThrowsErrorIfAssigningNonTupleToTupleTarget()
        {
            var input = """
                        a, b = 5
                        """;
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(input));
            Assert.That(ex.Type, Is.EqualTo("TypeError"));
        }

        [Test]
        public void SwapVariables()
        {
            var input = """
                        a = 1
                        b = 2
                        a, b = b, a
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "a"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 2)));
            Assert.That(closure.GetVariable(callStack, "b"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 1)));
        }
    }

    [TestFixture]
    public class NestedTupleUnpacking
    {
        [Test]
        public void NestedTupleOnRight()
        {
            // Pattern: a, (b, c) = (1, (2, 3)) - nested tuple on right side of left pattern
            var input = """
                        a, (b, c) = (1, (2, 3))
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "a")), Is.EqualTo(1));
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "b")), Is.EqualTo(2));
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "c")), Is.EqualTo(3));
        }

        [Test]
        public void MultipleNestedTuplesOnRight()
        {
            var input = """
                        a, (b, c), (d, e) = (1, (2, 3), (4, 5))
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "a")), Is.EqualTo(1));
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "b")), Is.EqualTo(2));
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "c")), Is.EqualTo(3));
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "d")), Is.EqualTo(4));
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "e")), Is.EqualTo(5));
        }

        [Test]
        public void DeeplyNestedTuplesOnRight()
        {
            var input = """
                        a, (b, (c, d)) = (1, (2, (3, 4)))
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "a")), Is.EqualTo(1));
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "b")), Is.EqualTo(2));
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "c")), Is.EqualTo(3));
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "d")), Is.EqualTo(4));
        }

        [Test]
        public void NestedTupleFromVariable()
        {
            var input = """
                        t = (10, (20, 30))
                        a, (b, c) = t
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "a")), Is.EqualTo(10));
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "b")), Is.EqualTo(20));
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "c")), Is.EqualTo(30));
        }

        [Test]
        public void NestedTupleFromFunctionReturn()
        {
            var input = """
                        def foo():
                            return (1, (2, 3), (4, 5))
                        a, (b, c), (d, e) = foo()
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "a")), Is.EqualTo(1));
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "b")), Is.EqualTo(2));
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "c")), Is.EqualTo(3));
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "d")), Is.EqualTo(4));
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "e")), Is.EqualTo(5));
        }

        [Test]
        public void ThrowsErrorIfNestedTupleLengthsMismatch()
        {
            var input = """
                        a, (b, c) = (1, (2, 3, 4))
                        """;
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(input));
            Assert.That(ex.Type, Is.EqualTo("ValueError"));
        }

        [Test]
        public void ThrowsErrorIfNestedValueIsNotTuple()
        {
            var input = """
                        a, (b, c) = (1, 2)
                        """;
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(input));
            Assert.That(ex.Type, Is.EqualTo("TypeError"));
        }

        [Test]
        public void MixedTypesInNestedTuple()
        {
            var input = """
                        a, (b, c), d = (1, ("hello", True), 3.14)
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "a")), Is.EqualTo(1));
            Assert.That(BtlTypes.GetStringValue(closure.GetVariable(callStack, "b")), Is.EqualTo("hello"));
            Assert.That(BtlTypes.GetBoolValue(closure.GetVariable(callStack, "c")), Is.EqualTo(true));
            Assert.That(BtlTypes.GetFloatValue(closure.GetVariable(callStack, "d")), Is.EqualTo(3.14));
        }

        [Test]
        public void NestedTupleSwap()
        {
            var input = """
                        a = (1, 2)
                        b = (3, 4)
                        (a, b) = (b, a)
                        """;
            var (callStack, closure) = Runner.Run(input);
            var tupleA = BtlTypes.GetTupleValue(closure.GetVariable(callStack, "a"));
            var tupleB = BtlTypes.GetTupleValue(closure.GetVariable(callStack, "b"));
            Assert.That(BtlTypes.GetIntValue(tupleA.Values[0]), Is.EqualTo(3));
            Assert.That(BtlTypes.GetIntValue(tupleA.Values[1]), Is.EqualTo(4));
            Assert.That(BtlTypes.GetIntValue(tupleB.Values[0]), Is.EqualTo(1));
            Assert.That(BtlTypes.GetIntValue(tupleB.Values[1]), Is.EqualTo(2));
        }

        [Test]
        public void TripleLevelNesting()
        {
            var input = """
                        a, (b, (c, (d, e))) = (1, (2, (3, (4, 5))))
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "a")), Is.EqualTo(1));
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "b")), Is.EqualTo(2));
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "c")), Is.EqualTo(3));
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "d")), Is.EqualTo(4));
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "e")), Is.EqualTo(5));
        }

        [Test]
        public void NestedTupleOnLeftOfComma()
        {
            var input = """
                        (a, b), c = ((1, 2), 3)
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "a")), Is.EqualTo(1));
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "b")), Is.EqualTo(2));
            Assert.That(BtlTypes.GetIntValue(closure.GetVariable(callStack, "c")), Is.EqualTo(3));
        }
    }
}