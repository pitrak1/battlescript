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
            var expected = new ArrayInstruction(
                [new VariableInstruction("asdf"), new VariableInstruction("qwer")],
                Consts.SquareBrackets,
                Consts.Comma);
            Assertions.AssertInputProducesParserOutput("[asdf, qwer]", expected);
        }
        
        [Test]
        public void HandlesArrayWithSeparatorsAndNext()
        {
            var expected = new ArrayInstruction(
                [new VariableInstruction("asdf"), new VariableInstruction("qwer")],
                Consts.CurlyBraces,
                Consts.Comma,
                new ArrayInstruction([], Consts.Parentheses)
            );
            Assertions.AssertInputProducesParserOutput("{asdf, qwer}()", expected);
        }
        
        [Test]
        public void PrioritizesCommasOverColons()
        {
            var expected = new ArrayInstruction(
                [
                    new ArrayInstruction([new VariableInstruction("asdf"), new NumericInstruction(3)], delimiter: ":"),
                    new ArrayInstruction([new VariableInstruction("qwer"), new NumericInstruction(4)], delimiter: ":"),
                ],
                separator: "{",
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
            var memory = Runner.Run("x = {'asdf': 3, 'qwer': 4}");
            var expected = memory.Create(Memory.BsTypes.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
            {
                {"asdf", memory.Create(Memory.BsTypes.Int, 3)},
                {"qwer", memory.Create(Memory.BsTypes.Int, 4)},
            }));
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void HandlesDictionaryWithSingleValue()
        {
            var memory = Runner.Run("x = {'asdf': 3}");
            var expected = memory.Create(Memory.BsTypes.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
            {
                {"asdf", memory.Create(Memory.BsTypes.Int, 3)},
            }));
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void HandlesDictionaryWithStringKey()
        {
            var memory = Runner.Run("x = {'asdf': 3}");
            var expected = memory.Create(Memory.BsTypes.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
            {
                {"asdf", memory.Create(Memory.BsTypes.Int, 3)},
            }));
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void HandlesDictionaryWithIntegerKey()
        {
            var memory = Runner.Run("x = {4: 3}");
            var expected = memory.Create(Memory.BsTypes.Dictionary, new MappingVariable(new Dictionary<int, Variable>()
            {
                {4, memory.Create(Memory.BsTypes.Int, 3)},
            }));
            Assertions.AssertVariable(memory, "x", expected);
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
            var expected = memory.Create(Memory.BsTypes.Int, 9);
            Assertions.AssertVariable(memory, "x", expected);
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
                "asdf",
                new ParameterSet([new VariableInstruction("self"), new VariableInstruction("y")]),
                [
                    new AssignmentInstruction(
                        "=",
                        new VariableInstruction("self", new MemberInstruction("j")),
                        new VariableInstruction("y"))
                ]);
            var classVariable = new ClassVariable("asdf", new Dictionary<string, Variable>()
            {
                { "i", memory.Create(Memory.BsTypes.Int, 4) },
                { "j", memory.Create(Memory.BsTypes.Int, 5) },
                { "asdf", methodVariable }
            });
            var expected = new ObjectVariable(
                new Dictionary<string, Variable>()
                {
                    {"i", memory.Create(Memory.BsTypes.Int, 4) },
                    {"j", memory.Create(Memory.BsTypes.Int, 5)}
                },
                classVariable);
            Assertions.AssertVariable(memory, "x", expected);
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
            var expected = memory.Create(Memory.BsTypes.Int, 9);
            Assertions.AssertVariable(memory, "x", expected);
        }

        [Test]
        public void HandlesNumericExpression()
        {
            var input = "x = ((4 - 2) * 3) >= (2 * (5 + 5)) or isinstance(5, int)";
            // var input = "x = ((4 - 2) * 3) >= (2 * (5 + 5))";
            var memory = Runner.Run(input);
            var expected = memory.Create(Memory.BsTypes.Bool, true);
            Assertions.AssertVariable(memory, "x", expected);
        }
    }

    [TestFixture]
    public class InterpretSquareBrackets
    {
        [Test]
        public void HandlesListCreation()
        {
            var memory = Runner.Run("x = [9, 8, 7]");
            var expected = memory.Create(Memory.BsTypes.List, new List<Variable>() {
                memory.Create(Memory.BsTypes.Int, 9),
                memory.Create(Memory.BsTypes.Int, 8),
                memory.Create(Memory.BsTypes.Int, 7)});
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void HandlesBasicIndex()
        {
            var input = """
                        y = [9, 8, 7]
                        x = y[1]
                        """;
            var memory = Runner.Run(input);
            var expected = memory.Create(Memory.BsTypes.Int, 8);
            Assertions.AssertVariable(memory, "x", expected);
        }
    }
}