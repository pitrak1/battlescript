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
            var expected = BsTypes.Create(BsTypes.Types.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
            {
                {"asdf", BsTypes.Create(BsTypes.Types.Int, 3)},
                {"qwer", BsTypes.Create(BsTypes.Types.Int, 4)},
            }));
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void SingleValue()
        {
            var (callStack, closure) = Runner.Run("x = {'asdf': 3}");
            var expected = BsTypes.Create(BsTypes.Types.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
            {
                {"asdf", BsTypes.Create(BsTypes.Types.Int, 3)},
            }));
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void StringKey()
        {
            var (callStack, closure) = Runner.Run("x = {'asdf': 3}");
            var expected = BsTypes.Create(BsTypes.Types.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
            {
                {"asdf", BsTypes.Create(BsTypes.Types.Int, 3)},
            }));
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void IntegerKey()
        {
            var (callStack, closure) = Runner.Run("x = {4: 3}");
            var expected = BsTypes.Create(BsTypes.Types.Dictionary, new MappingVariable(new Dictionary<int, Variable>()
            {
                {4, BsTypes.Create(BsTypes.Types.Int, 3)},
            }));
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }

        [Test]
        public void MixedKeys()
        {
            var (callStack, closure) = Runner.Run("x = {'asdf': 3, 4: 5}");
            var expected = BsTypes.Create(BsTypes.Types.Dictionary, new MappingVariable(new Dictionary<int, Variable>()
            {
                {4, BsTypes.Create(BsTypes.Types.Int, 5)},
            }, new Dictionary<string, Variable>()
            {
                {"asdf", BsTypes.Create(BsTypes.Types.Int, 3)},
            }));
            Assertions.AssertVariable(callStack, closure, "x", expected);
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
            var expected = BsTypes.Create(BsTypes.Types.Int, 9);
            Assertions.AssertVariable(callStack, closure, "x", expected);
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
                { "i", BsTypes.Create(BsTypes.Types.Int, 4) },
                { "j", BsTypes.Create(BsTypes.Types.Int, 5) },
                { "asdf", methodVariable }
            }, closure);
            var expected = new ObjectVariable(
                new Dictionary<string, Variable>()
                {
                    {"i", BsTypes.Create(BsTypes.Types.Int, 4) },
                    {"j", BsTypes.Create(BsTypes.Types.Int, 5)}
                },
                classVariable);
            Assertions.AssertVariable(callStack, closure, "x", expected);
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
            var expected = BsTypes.Create(BsTypes.Types.Int, 9);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }

        [Test]
        public void NumericExpression()
        {
            var input = "x = ((4 - 2) * 3) >= (2 * (5 + 5))";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    }

    [TestFixture]
    public class InterpretSquareBrackets
    {
        [Test]
        public void ListCreation()
        {
            var (callStack, closure) = Runner.Run("x = [9, 8, 7]");
            var expected = BsTypes.Create(BsTypes.Types.List, new List<Variable>() {
                BsTypes.Create(BsTypes.Types.Int, 9),
                BsTypes.Create(BsTypes.Types.Int, 8),
                BsTypes.Create(BsTypes.Types.Int, 7)});
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void BasicIndex()
        {
            var input = """
                        y = [9, 8, 7]
                        x = y[1]
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 8);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void RangeIndex()
        {
            var input = """
                        y = [9, 8, 7, 6, 5]
                        x = y[1:3]
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.List, new SequenceVariable([
                BsTypes.Create(BsTypes.Types.Int, 8),
                BsTypes.Create(BsTypes.Types.Int, 7)
            ]));
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    }
}
