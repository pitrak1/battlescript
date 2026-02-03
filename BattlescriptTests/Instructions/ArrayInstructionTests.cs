using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class ArrayInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void WithoutBrackets()
        {
            var input = "asdf, qwer";
            var expected = new ArrayInstruction(
                values: [new VariableInstruction("asdf"), new VariableInstruction("qwer")],
                delimiter: ArrayInstruction.DelimiterTypes.Comma);
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }
        
        [Test]
        public void WithoutDelimiters()
        {
            var input = "[asdf]";
            var expected = new ArrayInstruction(
                values: [new VariableInstruction("asdf")],
                bracket: ArrayInstruction.BracketTypes.SquareBrackets);
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }
        
        [Test]
        public void WithBracketsDelimitersAndNext()
        {
            var input = "[asdf, qwer]()";
            var expected = new ArrayInstruction(
                values: [new VariableInstruction("asdf"), new VariableInstruction("qwer")],
                bracket: ArrayInstruction.BracketTypes.SquareBrackets,
                delimiter: ArrayInstruction.DelimiterTypes.Comma,
                next: new ArrayInstruction([], ArrayInstruction.BracketTypes.Parentheses)
            );
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }
        
        [Test]
        public void PrioritizesCommasOverColons()
        {
            var input = "{asdf: 3, qwer: 4}";
            var expected = new ArrayInstruction(
                values: [
                    new ArrayInstruction([new VariableInstruction("asdf"), new NumericInstruction(3)], delimiter: ArrayInstruction.DelimiterTypes.Colon),
                    new ArrayInstruction([new VariableInstruction("qwer"), new NumericInstruction(4)], delimiter: ArrayInstruction.DelimiterTypes.Colon),
                ],
                bracket: ArrayInstruction.BracketTypes.CurlyBraces,
                delimiter: ArrayInstruction.DelimiterTypes.Comma
            );
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class InterpretCurlyBraces
    {
        [Test]
        public void MultipleValues()
        {
            var (callStack, closure) = Runner.Run("x = {'asdf': 3, 'qwer': 4}");
            var expected = BtlTypes.Create(BtlTypes.Types.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
            {
                {"asdf", BtlTypes.Create(BtlTypes.Types.Int, 3)},
                {"qwer", BtlTypes.Create(BtlTypes.Types.Int, 4)},
            }));
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }
        
        [Test]
        public void SingleValue()
        {
            var (callStack, closure) = Runner.Run("x = {'asdf': 3}");
            var expected = BtlTypes.Create(BtlTypes.Types.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
            {
                {"asdf", BtlTypes.Create(BtlTypes.Types.Int, 3)},
            }));
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }
        
        [Test]
        public void StringKey()
        {
            var (callStack, closure) = Runner.Run("x = {'asdf': 3}");
            var expected = BtlTypes.Create(BtlTypes.Types.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
            {
                {"asdf", BtlTypes.Create(BtlTypes.Types.Int, 3)},
            }));
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }
        
        [Test]
        public void IntegerKey()
        {
            var (callStack, closure) = Runner.Run("x = {4: 3}");
            var expected = BtlTypes.Create(BtlTypes.Types.Dictionary, new MappingVariable(new Dictionary<int, Variable>()
            {
                {4, BtlTypes.Create(BtlTypes.Types.Int, 3)},
            }));
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void MixedKeys()
        {
            var (callStack, closure) = Runner.Run("x = {'asdf': 3, 4: 5}");
            var expected = BtlTypes.Create(BtlTypes.Types.Dictionary, new MappingVariable(new Dictionary<int, Variable>()
            {
                {4, BtlTypes.Create(BtlTypes.Types.Int, 5)},
            }, new Dictionary<string, Variable>()
            {
                {"asdf", BtlTypes.Create(BtlTypes.Types.Int, 3)},
            }));
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }
    }
    
    [TestFixture]
    public class InterpretParentheses
    {
        [Test]
        public void FunctionCalls()
        {
            
            var input = """
                        def asdf(y, z):
                            return y + z

                        x = asdf(4, 5)
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 9);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }
        
        [Test]
        public void ClassInstantiation()
        {
            var input = """
                        class asdf:
                            i = 4
                            j = 5
                        
                            def asdf(self, y):
                                self.j = y

                        x = asdf()
                        """;
            var (callStack, closure) = Runner.Run(input);
            var methodVariable = new FunctionVariable(
                "asdf",
                closure,
                new ParameterSet([new VariableInstruction("self"), new VariableInstruction("y")]),
                [
                    new AssignmentInstruction(
                        "=",
                        new VariableInstruction("self", new MemberInstruction("j")),
                        new VariableInstruction("y"))
                ]);
            var classVariable = new ClassVariable("asdf", new Dictionary<string, Variable>()
            {
                { "i", BtlTypes.Create(BtlTypes.Types.Int, 4) },
                { "j", BtlTypes.Create(BtlTypes.Types.Int, 5) },
                { "asdf", methodVariable }
            }, closure);
            var expected = new ObjectVariable(
                new Dictionary<string, Variable>()
                {
                    {"i", BtlTypes.Create(BtlTypes.Types.Int, 4) },
                    {"j", BtlTypes.Create(BtlTypes.Types.Int, 5)}
                },
                classVariable);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }
        
        [Test]
        public void CallsConstructorIfDefined()
        {
            var input = """
                        class asdf:
                            i = 4
                        
                            def __init__(self, y):
                                self.i = y

                        y = asdf(9)
                        x = y.i
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 9);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void NumericExpression()
        {
            var input = "x = ((4 - 2) * 3) >= (2 * (5 + 5))";
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Bool, false);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class InterpretSquareBrackets
    {
        [Test]
        public void ListCreation()
        {
            var (callStack, closure) = Runner.Run("x = [9, 8, 7]");
            var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 9),
                BtlTypes.Create(BtlTypes.Types.Int, 8),
                BtlTypes.Create(BtlTypes.Types.Int, 7)});
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void BasicIndex()
        {
            var input = """
                        y = [9, 8, 7]
                        x = y[1]
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 8);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void RangeIndex()
        {
            var input = """
                        y = [9, 8, 7, 6, 5]
                        x = y[1:3]
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.List, new SequenceVariable([
                BtlTypes.Create(BtlTypes.Types.Int, 8),
                BtlTypes.Create(BtlTypes.Types.Int, 7)
            ]));
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class ParseTuples
    {
        [Test]
        public void ParsesEmptyTuple()
        {
            var input = "()";
            var expected = new ArrayInstruction(
                values: [],
                bracket: ArrayInstruction.BracketTypes.Parentheses);
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void ParsesSingleElementTupleWithTrailingComma()
        {
            var input = "(1,)";
            var expected = new ArrayInstruction(
                values: [new NumericInstruction(1)],
                bracket: ArrayInstruction.BracketTypes.Parentheses,
                delimiter: ArrayInstruction.DelimiterTypes.Comma);
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void ParsesMultipleElementTuple()
        {
            var input = "(1, 2, 3)";
            var expected = new ArrayInstruction(
                values: [new NumericInstruction(1), new NumericInstruction(2), new NumericInstruction(3)],
                bracket: ArrayInstruction.BracketTypes.Parentheses,
                delimiter: ArrayInstruction.DelimiterTypes.Comma);
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void ParsesNestedTuple()
        {
            var input = "((1, 2), (3, 4))";
            var expected = new ArrayInstruction(
                values: [
                    new ArrayInstruction(
                        values: [new NumericInstruction(1), new NumericInstruction(2)],
                        bracket: ArrayInstruction.BracketTypes.Parentheses,
                        delimiter: ArrayInstruction.DelimiterTypes.Comma),
                    new ArrayInstruction(
                        values: [new NumericInstruction(3), new NumericInstruction(4)],
                        bracket: ArrayInstruction.BracketTypes.Parentheses,
                        delimiter: ArrayInstruction.DelimiterTypes.Comma)
                ],
                bracket: ArrayInstruction.BracketTypes.Parentheses,
                delimiter: ArrayInstruction.DelimiterTypes.Comma);
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void ParsesTupleWithMixedTypes()
        {
            var input = "(1, 'hello', 3.14)";
            var expected = new ArrayInstruction(
                values: [new NumericInstruction(1), new StringInstruction("hello"), new NumericInstruction(3.14)],
                bracket: ArrayInstruction.BracketTypes.Parentheses,
                delimiter: ArrayInstruction.DelimiterTypes.Comma);
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void ParsesTupleAsArgument()
        {
            var input = "foo((1, 2))";
            var expected = new VariableInstruction(
                "foo",
                new ArrayInstruction(
                    values: [
                        new ArrayInstruction(
                            values: [new NumericInstruction(1), new NumericInstruction(2)],
                            bracket: ArrayInstruction.BracketTypes.Parentheses,
                            delimiter: ArrayInstruction.DelimiterTypes.Comma)
                    ],
                    bracket: ArrayInstruction.BracketTypes.Parentheses));
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void ParsesTupleWithIndexAccess()
        {
            var input = "(1, 2, 3)[0]";
            var expected = new ArrayInstruction(
                values: [new NumericInstruction(1), new NumericInstruction(2), new NumericInstruction(3)],
                bracket: ArrayInstruction.BracketTypes.Parentheses,
                delimiter: ArrayInstruction.DelimiterTypes.Comma,
                next: new ArrayInstruction(
                    values: [new NumericInstruction(0)],
                    bracket: ArrayInstruction.BracketTypes.SquareBrackets));
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void ParsesParenthesizedIntegerWithMethodCall()
        {
            var input = "(5).__abs__()";
            var expected = new ArrayInstruction(
                values: [new NumericInstruction(5)],
                bracket: ArrayInstruction.BracketTypes.Parentheses,
                next: new MemberInstruction(
                    "__abs__",
                    new ArrayInstruction([], ArrayInstruction.BracketTypes.Parentheses)));
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void ParsesParenthesizedNegativeIntegerWithMethodCall()
        {
            var input = "(-5).__abs__()";
            var expected = new ArrayInstruction(
                values: [new OperationInstruction("-", null, new NumericInstruction(5))],
                bracket: ArrayInstruction.BracketTypes.Parentheses,
                next: new MemberInstruction(
                    "__abs__",
                    new ArrayInstruction([], ArrayInstruction.BracketTypes.Parentheses)));
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void ParsesTupleWithMethodCall()
        {
            var input = "(1, 2, 3).__len__()";
            var expected = new ArrayInstruction(
                values: [new NumericInstruction(1), new NumericInstruction(2), new NumericInstruction(3)],
                bracket: ArrayInstruction.BracketTypes.Parentheses,
                delimiter: ArrayInstruction.DelimiterTypes.Comma,
                next: new MemberInstruction(
                    "__len__",
                    new ArrayInstruction([], ArrayInstruction.BracketTypes.Parentheses)));
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }
    }
}
