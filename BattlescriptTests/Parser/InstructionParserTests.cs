using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class InstructionParserTests
{
    [TestFixture]
    public class Literals
    {
        [Test]
        public void HandlesNumbers()
        {
            var expected = new Instruction(Consts.InstructionTypes.Number, 5.0);
            ParserAssertions.AssertInputProducesInstruction("5", expected);
        }
        
        [Test]
        public void HandlesStrings()
        {
            var expected = new Instruction(Consts.InstructionTypes.String, "asdf");
            ParserAssertions.AssertInputProducesInstruction("'asdf'", expected);
        }
        
        [Test]
        public void HandlesBooleans()
        {
            var expected = new Instruction(Consts.InstructionTypes.Boolean, false);
            ParserAssertions.AssertInputProducesInstruction("False", expected);
        }
    }

    [TestFixture]
    public class Operations
    {
        [Test]
        public void HandlesOperations()
        {
            var expected = new Instruction(
                type: Consts.InstructionTypes.Operation,
                operation: "+",
                left: new Instruction(Consts.InstructionTypes.Number, 5.0),
                right: new Instruction(Consts.InstructionTypes.Number, 6.0));
            ParserAssertions.AssertInputProducesInstruction("5 + 6", expected);
        }

        [Test]
        public void HandlesUnaryOperators()
        {
            var expected = new Instruction(
                type: Consts.InstructionTypes.Operation,
                operation: "~",
                right: new Instruction(Consts.InstructionTypes.Number, 6.0));
            ParserAssertions.AssertInputProducesInstruction("~6", expected);
        }
    }
    
    [TestFixture]
    public class Assignments
    {
        [Test]
        public void HandlesAssignments()
        {
            // This is nonsensical, but is an easy example for this test *shrug*
            var expected = new Instruction(
                type: Consts.InstructionTypes.Assignment,
                operation: "=",
                left: new Instruction(Consts.InstructionTypes.Number, 5.0),
                right: new Instruction(Consts.InstructionTypes.Number, 6.0));
            ParserAssertions.AssertInputProducesInstruction("5 = 6", expected);
        }
    }
    
    [TestFixture]
    public class Separators
    {
        [Test]
        public void HandlesArrayDefinition()
        {
            var expected = new Instruction(
                type: Consts.InstructionTypes.SquareBrackets,
                valueList: new List<Instruction>
                {
                    new(Consts.InstructionTypes.Number, 4),
                    new(Consts.InstructionTypes.String, "asdf")
                });
            ParserAssertions.AssertInputProducesInstruction("[4, 'asdf']", expected);
        }
        
        [Test]
        public void HandlesIndex()
        {
            var expected = new Instruction(
                type: Consts.InstructionTypes.Variable,
                name: "x",
                next: new(
                    type: Consts.InstructionTypes.SquareBrackets,
                    valueList: new List<Instruction>
                    {
                        new(Consts.InstructionTypes.Number, 4)
                    }));
            ParserAssertions.AssertInputProducesInstruction("x[4]", expected);
        }
        
        [Test]
        public void HandlesStackedIndexes()
        {
            var expected = new Instruction(
                type: Consts.InstructionTypes.Variable,
                name: "x",
                next: new(
                    type: Consts.InstructionTypes.SquareBrackets,
                    valueList: new List<Instruction>
                    {
                        new(Consts.InstructionTypes.Number, 4)
                    },
                    next: new(
                        type: Consts.InstructionTypes.SquareBrackets,
                        valueList: new List<Instruction>
                        {
                            new(Consts.InstructionTypes.Number, 5)
                        })));
            ParserAssertions.AssertInputProducesInstruction("x[4][5]", expected);
        }
        
        [Test]
        public void HandlesRangeIndexes()
        {
            var expected = new Instruction(
                type: Consts.InstructionTypes.Variable,
                name: "x",
                next: new(
                    type: Consts.InstructionTypes.SquareBrackets,
                    valueList: new List<Instruction>
                    {
                        new(
                            type: Consts.InstructionTypes.KeyValuePair,
                            left: new (Consts.InstructionTypes.Number, 4),
                            right: new (Consts.InstructionTypes.Number, 5))
                    }));
            ParserAssertions.AssertInputProducesInstruction("x[4:5]", expected);
        }

        [Test]
        public void HandlesParens()
        {
            var expected = new Instruction(
                type: Consts.InstructionTypes.Parens,
                valueList: new List<Instruction>
                {
                    new(Consts.InstructionTypes.Number, 4),
                    new(Consts.InstructionTypes.String, "asdf")
                });
            ParserAssertions.AssertInputProducesInstruction("(4, 'asdf')", expected);
        }

        [Test]
        public void HandlesMembers()
        { 
            var expected = new Instruction(
                type: Consts.InstructionTypes.Variable,
                name: "asdf",
                next: new(
                    type: Consts.InstructionTypes.SquareBrackets,
                    valueList: new List<Instruction>
                    {
                        new(Consts.InstructionTypes.String, "asdf")
                    }));
            ParserAssertions.AssertInputProducesInstruction("asdf.asdf", expected);
        }
        
        [Test]
        public void HandlesSetDefinition()
        {
            var expected = new Instruction(
                type: Consts.InstructionTypes.SetDefinition,
                valueList: new List<Instruction>
                {
                    new (Consts.InstructionTypes.Number, 4),
                    new(Consts.InstructionTypes.String, "asdf")
                });
            ParserAssertions.AssertInputProducesInstruction("{4, 'asdf'}", expected);
        }
        
        [Test]
        public void HandlesDictionaryDefinition()
        {
            var expected = new Instruction(
                type: Consts.InstructionTypes.DictionaryDefinition,
                valueList: new List<Instruction>
                {
                    new (
                        type: Consts.InstructionTypes.KeyValuePair,
                        left: new (Consts.InstructionTypes.Number, 4), 
                        right: new (Consts.InstructionTypes.Number, 5)
                    ),
                    new (
                        Consts.InstructionTypes.KeyValuePair, 
                        left: new (Consts.InstructionTypes.Number, 6), 
                        right: new (Consts.InstructionTypes.String, "asdf")
                    )
                });
            ParserAssertions.AssertInputProducesInstruction("{4: 5, 6: 'asdf'}", expected);
        }
    }

    [TestFixture]
    public class Functions
    {
        [Test]
        public void HandlesBasicFunctionDefinition()
        {
            var expected = new Instruction(
                type: Consts.InstructionTypes.Function,
                name: "func"
            );
            ParserAssertions.AssertInputProducesInstruction("def func():", expected);
        }
        
        [Test]
        public void HandlesFunctionDefinitionWithPositionalArgument()
        {
            var expected = new Instruction(
                type: Consts.InstructionTypes.Function,
                name: "func",
                valueList: new List<Instruction>()
                {
                    new (type: Consts.InstructionTypes.Variable, name: "asdf"),
                }
            );
            ParserAssertions.AssertInputProducesInstruction("def func(asdf):", expected);
        }
        
        [Test]
        public void HandlesFunctionDefinitionWithMultiplePositionalArguments()
        {
            var expected = new Instruction(
                type: Consts.InstructionTypes.Function,
                name: "func",
                valueList: new List<Instruction>()
                {
                    new (type: Consts.InstructionTypes.Variable, name: "asdf"),
                    new (type: Consts.InstructionTypes.Variable, name: "qwer")
                }
            );
            ParserAssertions.AssertInputProducesInstruction("def func(asdf, qwer):", expected);
        }

        [Test]
        public void HandlesReturn()
        {
            var expected = new Instruction(
                type: Consts.InstructionTypes.Return, 
                value: new Instruction(Consts.InstructionTypes.Number, 4));
            ParserAssertions.AssertInputProducesInstruction("return 4", expected);
        }
    }
}