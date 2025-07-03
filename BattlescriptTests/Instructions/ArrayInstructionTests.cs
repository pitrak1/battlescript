using Battlescript;

namespace BattlescriptTests.InstructionsTests;

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
                separator: "["
            );
            Assertions.AssertInputProducesParserOutput(".asdf", expected);
        }
        
        [Test]
        public void HandlesMemberWithNext()
        {
            var expected = new ArrayInstruction(
                [new StringInstruction("asdf")],
                new ArrayInstruction(
                    [new StringInstruction("qwer")],
                    separator: "["
                ),
                separator: "["
            );
            Assertions.AssertInputProducesParserOutput(".asdf.qwer", expected);
        }

        [Test]
        public void HandlesArrayWithSeparators()
        {
            var expected = new ArrayInstruction(
                [new VariableInstruction("asdf"), new VariableInstruction("qwer")],
                separator: "[",
                delimiter: ","
            );
            Assertions.AssertInputProducesParserOutput("[asdf, qwer]", expected);
        }
        
        [Test]
        public void HandlesArrayWithSeparatorsAndNext()
        {
            var expected = new ArrayInstruction(
                [new VariableInstruction("asdf"), new VariableInstruction("qwer")],
                new ArrayInstruction([], null, separator: "("),
                separator: "{",
                delimiter: ","
            );
            Assertions.AssertInputProducesParserOutput("{asdf, qwer}()", expected);
        }
        
        [Test]
        public void PrioritizesCommasOverColons()
        {
            var expected = new ArrayInstruction(
                [
                    new ArrayInstruction([new VariableInstruction("asdf"), new IntegerInstruction(3)], delimiter: ":"),
                    new ArrayInstruction([new VariableInstruction("qwer"), new IntegerInstruction(4)], delimiter: ":"),
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
            var expected = new DictionaryVariable(new Dictionary<Variable, Variable>()
            {
                {new StringVariable("asdf"), new IntegerVariable(3)},
                {new StringVariable("qwer"), new IntegerVariable(4)},
            });
            Assertions.AssertInputProducesValueInMemory("x = {'asdf': 3, 'qwer': 4}", "x", expected);
        }
        
        [Test]
        public void HandlesDictionaryWithSingleValue()
        {
            var expected = new DictionaryVariable(new Dictionary<Variable, Variable>()
            {
                {new StringVariable("asdf"), new IntegerVariable(3)},
            });
            Assertions.AssertInputProducesValueInMemory("x = {'asdf': 3}", "x", expected);
        }
    }
    
    [TestFixture]
    public class InterpretParentheses
    {
        [Test]
        public void HandlesFunctionCalls()
        {
            var expected = new IntegerVariable(9);
            var input = @"
def asdf(y, z):
    return y + z

x = asdf(4, 5)";
            Assertions.AssertInputProducesValueInMemory(input, "x", expected);
        }
        
        [Test]
        public void HandlesClassInstantiation()
        {
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
            var classVariable = new ClassVariable(new Dictionary<string, Variable>()
            {
                { "i", new IntegerVariable(4) },
                { "j", new IntegerVariable(5) },
                { "asdf", methodVariable }
            });
            var expected = new ObjectVariable(
                new Dictionary<string, Variable>()
                {
                    {"i", new IntegerVariable(4)},
                    {"j", new IntegerVariable(5)}
                },
                classVariable);
            var input = @"
class asdf:
    i = 4
    j = 5

    def asdf(self, y):
        self.j = y

x = asdf()";
            Assertions.AssertInputProducesValueInMemory(input, "x", expected);
        }
        
        [Test]
        public void CallsConstructorIfDefined()
        {
            var expected = new IntegerVariable(9);
            var input = @"
class asdf:
    i = 4

    def __init__(self, y):
        self.i = y

y = asdf(9)
x = y.i";
            Assertions.AssertInputProducesValueInMemory(input, "x", expected);
        }
    }

    [TestFixture]
    public class InterpretSquareBrackets
    {
        [Test]
        public void HandlesListCreation()
        {
            var expected = new ListVariable([new IntegerVariable(9), new IntegerVariable(8), new IntegerVariable(7)]);
            Assertions.AssertInputProducesValueInMemory("x = [9, 8, 7]", "x", expected);
        }
        
        [Test]
        public void HandlesBasicIndex()
        {
            var expected = new IntegerVariable(8);
            var input = @"
y = [9, 8, 7]
x = y[1]";
            Assertions.AssertInputProducesValueInMemory(input, "x", expected);
        }
        
        [Test]
        public void HandlesStackedIndices()
        {
            var expected = new IntegerVariable(3);
            var input = @"
y = [9, [1, 2, 3], 7]
x = y[1][2]";
            Assertions.AssertInputProducesValueInMemory(input, "x", expected);
        }
        
        [Test]
        public void HandlesRangeIndex()
        {
            var expected = new ListVariable([new IntegerVariable(9), new IntegerVariable(8), new IntegerVariable(7)]);
            var input = @"
y = [1, 2, 9, 8, 7, 3, 4]
x = y[2:5]";
            Assertions.AssertInputProducesValueInMemory(input, "x", expected);
        }
        
        [Test]
        public void HandlesMembers()
        {
            var expected = new IntegerVariable(3);
            var input = @"
y = {'asdf': 3, 'qwer': 4}
x = y.asdf";
            Assertions.AssertInputProducesValueInMemory(input, "x", expected);
        }
        
        [Test]
        public void HandlesIndexingWithExpressions()
        {
            var expected = new IntegerVariable(8);
            var input = @"
y = [1, 2, 9, 8, 7, 3, 4]
x = y[1 + 2]";
            Assertions.AssertInputProducesValueInMemory(input, "x", expected);
        }
    }
}