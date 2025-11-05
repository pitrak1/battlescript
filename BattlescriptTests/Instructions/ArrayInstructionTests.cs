using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class ArrayInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void HandlesArrayWithoutSeparators()
        {
            var expected = new ArrayInstruction(
                [new VariableInstruction("asdf"), new VariableInstruction("qwer")],
                delimiter: Consts.Comma);
            Assertions.AssertInputProducesParserOutput("asdf, qwer", expected);
        }
        
        [Test]
        public void HandlesArrayWithSeparators()
        {
            var expected = new SquareBracketsInstruction(
                [new VariableInstruction("asdf"), new VariableInstruction("qwer")],
                Consts.Comma);
            Assertions.AssertInputProducesParserOutput("[asdf, qwer]", expected);
        }
        
        [Test]
        public void HandlesArrayWithSeparatorsAndNext()
        {
            var expected = new CurlyBracesInstruction(
                [new VariableInstruction("asdf"), new VariableInstruction("qwer")],
                Consts.Comma,
                new ParenthesesInstruction([])
            );
            Assertions.AssertInputProducesParserOutput("{asdf, qwer}()", expected);
        }
        
        [Test]
        public void PrioritizesCommasOverColons()
        {
            var expected = new CurlyBracesInstruction(
                [
                    new ArrayInstruction([new VariableInstruction("asdf"), new NumericInstruction(3)], delimiter: ":"),
                    new ArrayInstruction([new VariableInstruction("qwer"), new NumericInstruction(4)], delimiter: ":"),
                ],
                delimiter: ","
            );
            Assertions.AssertInputProducesParserOutput("{asdf: 3, qwer: 4}", expected);
        }
    }

    [TestFixture]
    public class InterpretCurlyBraces
    {
        [Test]
        public void HandlesDictionaryWithMultipleValues()
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
        public void HandlesDictionaryWithSingleValue()
        {
            var (callStack, closure) = Runner.Run("x = {'asdf': 3}");
            var expected = BsTypes.Create(BsTypes.Types.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
            {
                {"asdf", BsTypes.Create(BsTypes.Types.Int, 3)},
            }));
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void HandlesDictionaryWithStringKey()
        {
            var (callStack, closure) = Runner.Run("x = {'asdf': 3}");
            var expected = BsTypes.Create(BsTypes.Types.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
            {
                {"asdf", BsTypes.Create(BsTypes.Types.Int, 3)},
            }));
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void HandlesDictionaryWithIntegerKey()
        {
            var (callStack, closure) = Runner.Run("x = {4: 3}");
            var expected = BsTypes.Create(BsTypes.Types.Dictionary, new MappingVariable(new Dictionary<int, Variable>()
            {
                {4, BsTypes.Create(BsTypes.Types.Int, 3)},
            }));
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    }
    
    [TestFixture]
    public class InterpretParentheses
    {
        [Test]
        public void HandlesFunctionCalls()
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
        public void HandlesClassInstantiation()
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
        public void HandlesNumericExpression()
        {
            var input = "x = ((4 - 2) * 3) >= (2 * (5 + 5)) or isinstance(5, int)";
            // var input = "x = ((4 - 2) * 3) >= (2 * (5 + 5))";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    }

    [TestFixture]
    public class InterpretSquareBrackets
    {
        [Test]
        public void HandlesListCreation()
        {
            var (callStack, closure) = Runner.Run("x = [9, 8, 7]");
            var expected = BsTypes.Create(BsTypes.Types.List, new List<Variable>() {
                BsTypes.Create(BsTypes.Types.Int, 9),
                BsTypes.Create(BsTypes.Types.Int, 8),
                BsTypes.Create(BsTypes.Types.Int, 7)});
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void HandlesBasicIndex()
        {
            var input = """
                        y = [9, 8, 7]
                        x = y[1]
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 8);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    }
}
