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
            var Create = new ArrayInstruction(
                [new VariableInstruction("asdf"), new VariableInstruction("qwer")],
                delimiter: Consts.Comma);
            Assertions.AssertInputProducesParserOutput("asdf, qwer", Create);
        }
        
        [Test]
        public void HandlesArrayWithSeparators()
        {
            var Create = new ArrayInstruction(
                [new VariableInstruction("asdf"), new VariableInstruction("qwer")],
                Consts.SquareBrackets,
                Consts.Comma);
            Assertions.AssertInputProducesParserOutput("[asdf, qwer]", Create);
        }
        
        [Test]
        public void HandlesArrayWithSeparatorsAndNext()
        {
            var Create = new ArrayInstruction(
                [new VariableInstruction("asdf"), new VariableInstruction("qwer")],
                Consts.CurlyBraces,
                Consts.Comma,
                new ArrayInstruction([], Consts.Parentheses)
            );
            Assertions.AssertInputProducesParserOutput("{asdf, qwer}()", Create);
        }
        
        [Test]
        public void PrioritizesCommasOverColons()
        {
            var Create = new ArrayInstruction(
                [
                    new ArrayInstruction([new VariableInstruction("asdf"), new NumericInstruction(3)], delimiter: ":"),
                    new ArrayInstruction([new VariableInstruction("qwer"), new NumericInstruction(4)], delimiter: ":"),
                ],
                separator: "{",
                delimiter: ","
            );
            Assertions.AssertInputProducesParserOutput("{asdf: 3, qwer: 4}", Create);
        }
    }

    [TestFixture]
    public class InterpretCurlyBraces
    {
        [Test]
        public void HandlesDictionaryWithMultipleValues()
        {
            var memory = Runner.Run("x = {'asdf': 3, 'qwer': 4}");
            var Create = new DictionaryVariable(null, new Dictionary<string, Variable>()
            {
                {"asdf", BsTypes.Create(memory, BsTypes.Types.Int, 3)},
                {"qwer", BsTypes.Create(memory, BsTypes.Types.Int, 4)},
            });
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], Create);
        }
        
        [Test]
        public void HandlesDictionaryWithSingleValue()
        {
            var memory = Runner.Run("x = {'asdf': 3}");
            var Create = new DictionaryVariable(null, new Dictionary<string, Variable>()
            {
                {"asdf", BsTypes.Create(memory, BsTypes.Types.Int, 3)},
            });
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], Create);
        }
        
        [Test]
        public void HandlesDictionaryWithStringKey()
        {
            var memory = Runner.Run("x = {'asdf': 3}");
            var Create = new DictionaryVariable(null, new Dictionary<string, Variable>()
            {
                {"asdf", BsTypes.Create(memory, BsTypes.Types.Int, 3)},
            });
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], Create);
        }
        
        [Test]
        public void HandlesDictionaryWithIntegerKey()
        {
            var memory = Runner.Run("x = {4: 3}");
            var Create = new DictionaryVariable(new Dictionary<int, Variable>()
            {
                {4, BsTypes.Create(memory, BsTypes.Types.Int, 3)},
            });
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], Create);
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
            var memory = Runner.Run(input);
            var Create = BsTypes.Create(memory, BsTypes.Types.Int, 9);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], Create);
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
            var memory = Runner.Run(input);
            var methodVariable = new FunctionVariable(
                [new VariableInstruction("self"), new VariableInstruction("y")],
                [
                    new AssignmentInstruction(
                        "=",
                        new VariableInstruction("self", new MemberInstruction("j")),
                        new VariableInstruction("y"))
                ]);
            var classVariable = new ClassVariable("asdf", new Dictionary<string, Variable>()
            {
                { "i", BsTypes.Create(memory, BsTypes.Types.Int, 4) },
                { "j", BsTypes.Create(memory, BsTypes.Types.Int, 5) },
                { "asdf", methodVariable }
            });
            var Create = new ObjectVariable(
                new Dictionary<string, Variable>()
                {
                    {"i", BsTypes.Create(memory, BsTypes.Types.Int, 4) },
                    {"j", BsTypes.Create(memory, BsTypes.Types.Int, 5)}
                },
                classVariable);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], Create);
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
            var memory = Runner.Run(input);
            var Create = BsTypes.Create(memory, BsTypes.Types.Int, 9);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], Create);
        }

        [Test]
        public void HandlesNumericExpression()
        {
            var input = "x = ((4 - 2) * 3) >= (2 * (5 + 5)) or isinstance(5, int)";
            // var input = "x = ((4 - 2) * 3) >= (2 * (5 + 5))";
            var memory = Runner.Run(input);
            var Create = BsTypes.Create(memory, BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], Create);
        }
    }

    [TestFixture]
    public class InterpretSquareBrackets
    {
        [Test]
        public void HandlesListCreation()
        {
            var memory = Runner.Run("x = [9, 8, 7]");
            var Create = BsTypes.Create(memory, BsTypes.Types.List, new List<Variable>() {
                BsTypes.Create(memory, BsTypes.Types.Int, 9),
                BsTypes.Create(memory, BsTypes.Types.Int, 8),
                BsTypes.Create(memory, BsTypes.Types.Int, 7)});
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], Create);
        }
        
        [Test]
        public void HandlesBasicIndex()
        {
            var input = """
                        y = [9, 8, 7]
                        x = y[1]
                        """;
            var memory = Runner.Run(input);
            var Create = BsTypes.Create(memory, BsTypes.Types.Int, 8);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], Create);
        }
    }
}