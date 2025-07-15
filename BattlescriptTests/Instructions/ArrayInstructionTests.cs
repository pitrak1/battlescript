using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class ArrayInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void HandlesMember()
        {
            var expected = new ArrayInstruction(
                [new StringInstruction("asdf")],
                Consts.SquareBrackets
            );
            Assertions.AssertInputProducesParserOutput(".asdf", expected);
        }
        
        [Test]
        public void HandlesMemberWithNext()
        {
            var expected = new ArrayInstruction(
                [new StringInstruction("asdf")],
                Consts.SquareBrackets,
                next: new ArrayInstruction(
                    [new StringInstruction("qwer")],
                    Consts.SquareBrackets)
            );
            Assertions.AssertInputProducesParserOutput(".asdf.qwer", expected);
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
            var expected = new DictionaryVariable(null, new Dictionary<string, Variable>()
            {
                {"asdf", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 3)},
                {"qwer", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 4)},
            });
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
        }
        
        [Test]
        public void HandlesDictionaryWithSingleValue()
        {
            var memory = Runner.Run("x = {'asdf': 3}");
            var expected = new DictionaryVariable(null, new Dictionary<string, Variable>()
            {
                {"asdf", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 3)},
            });
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
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
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 9);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
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
                        new VariableInstruction(
                            "self",
                            new ArrayInstruction([new VariableInstruction("j")], separator: "[")),
                        new VariableInstruction("y"))
                ]);
            var classVariable = new ClassVariable("asdf", new Dictionary<string, Variable>()
            {
                { "i", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 4) },
                { "j", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5) },
                { "asdf", methodVariable }
            });
            var expected = new ObjectVariable(
                new Dictionary<string, Variable>()
                {
                    {"i", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 4) },
                    {"j", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5)}
                },
                classVariable);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
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
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 9);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
        }

        [Test]
        public void HandlesNumericExpression()
        {
            var input = "x = ((4 - 2) * 3) >= (2 * (5 + 5)) or isinstance(5, int)";
            // var input = "x = ((4 - 2) * 3) >= (2 * (5 + 5))";
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "bool", true);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
        }
    }

    [TestFixture]
    public class InterpretSquareBrackets
    {
        [Test]
        public void HandlesListCreation()
        {
            var memory = Runner.Run("x = [9, 8, 7]");
            var expected = new ListVariable([
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 9), 
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 8), 
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 7)]);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
        }
        
        [Test]
        public void HandlesBasicIndex()
        {
            var input = """
                        y = [9, 8, 7]
                        x = y[1]
                        """;
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 8);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
        }
        
        [Test]
        public void HandlesStackedIndices()
        {
            var input = """
                        y = [9, [1, 2, 3], 7]
                        x = y[1][2]
                        """;
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 3);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
        }
        
        [Test]
        public void HandlesRangeIndex()
        {
            var input = """
                        y = [1, 2, 9, 8, 7, 3, 4]
                        x = y[2:5]
                        """;
            var memory = Runner.Run(input);
            var expected = new ListVariable([
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 9), 
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 8), 
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 7)]);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
        }
        
        [Test]
        public void HandlesMembers()
        {
            var input = """
                        y = {'asdf': 3, 'qwer': 4}
                        x = y.asdf
                        """;
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 3);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
        }
        
        [Test]
        public void HandlesIndexingWithExpressions()
        {
            var input = """
                        y = [1, 2, 9, 8, 7, 3, 4]
                        x = y[1 + 2]
                        """;
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 8);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
        }
    }
}