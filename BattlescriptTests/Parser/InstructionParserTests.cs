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
                Consts.InstructionTypes.Operation,
                "+",
                null,
                new Instruction(Consts.InstructionTypes.Number, 5.0),
                new Instruction(Consts.InstructionTypes.Number, 6.0));
            ParserAssertions.AssertInputProducesInstruction("5 + 6", expected);
        }

        [Test]
        public void HandlesUnaryOperators()
        {
            var expected = new Instruction(
                Consts.InstructionTypes.Operation,
                "~",
                null,
                null,
                new Instruction(Consts.InstructionTypes.Number, 6.0));
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
                Consts.InstructionTypes.Assignment,
                "=",
                null,
                new Instruction(Consts.InstructionTypes.Number, 5.0),
                new Instruction(Consts.InstructionTypes.Number, 6.0));
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
                Consts.InstructionTypes.SquareBrackets,
                new List<Instruction>
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
                Consts.InstructionTypes.Variable,
                "x",
                new(
                    Consts.InstructionTypes.SquareBrackets,
                    new List<Instruction>
                    {
                        new(Consts.InstructionTypes.Number, 4)
                    }));
            ParserAssertions.AssertInputProducesInstruction("x[4]", expected);
        }
        
        [Test]
        public void HandlesStackedIndexes()
        {
            var expected = new Instruction(
                Consts.InstructionTypes.Variable,
                "x",
                new(
                    Consts.InstructionTypes.SquareBrackets,
                    new List<Instruction>
                    {
                        new(Consts.InstructionTypes.Number, 4)
                    },
                    new(
                        Consts.InstructionTypes.SquareBrackets,
                        new List<Instruction>
                        {
                            new(Consts.InstructionTypes.Number, 5)
                        })));
            ParserAssertions.AssertInputProducesInstruction("x[4][5]", expected);
        }
        
        [Test]
        public void HandlesRangeIndexes()
        {
            var expected = new Instruction(
                Consts.InstructionTypes.Variable,
                "x",
                new(
                    Consts.InstructionTypes.SquareBrackets,
                    new List<Instruction>
                    {
                        new(
                            Consts.InstructionTypes.KeyValuePair,
                            new List<Instruction>
                            {
                                new (Consts.InstructionTypes.Number, 4),
                                new (Consts.InstructionTypes.Number, 5)
                            })
                    }));
            ParserAssertions.AssertInputProducesInstruction("x[4:5]", expected);
        }

        [Test]
        public void HandlesParens()
        {
            var expected = new Instruction(
                Consts.InstructionTypes.Parens,
                new List<Instruction>
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
                Consts.InstructionTypes.Variable,
                "asdf",
                new(
                    Consts.InstructionTypes.SquareBrackets,
                    new List<Instruction>
                    {
                        new(Consts.InstructionTypes.String, "asdf")
                    }));
            ParserAssertions.AssertInputProducesInstruction("asdf.asdf", expected);
        }
        
        [Test]
        public void HandlesSetDefinition()
        {
            var expected = new Instruction(
                Consts.InstructionTypes.SetDefinition,
                new List<Instruction>
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
                Consts.InstructionTypes.DictionaryDefinition,
                new List<Instruction>
                {
                    new (
                        Consts.InstructionTypes.KeyValuePair, 
                        null, 
                        null, 
                        new (Consts.InstructionTypes.Number, 4), 
                        new (Consts.InstructionTypes.Number, 5)
                    ),
                    new (
                        Consts.InstructionTypes.KeyValuePair, 
                        null, 
                        null, 
                        new (Consts.InstructionTypes.Number, 6), 
                        new (Consts.InstructionTypes.String, "asdf")
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
                Consts.InstructionTypes.Function,
                "func"
            );
            ParserAssertions.AssertInputProducesInstruction("def func():", expected);
        }
        
        [Test]
        public void HandlesFunctionDefinitionWithPositionalArgument()
        {
            var expected = new Instruction(
                Consts.InstructionTypes.Function,
                "func",
                null,
                null,
                null,
                null,
                new List<Instruction>()
                {
                    new (Consts.InstructionTypes.Variable, "asdf"),
                }
            );
            ParserAssertions.AssertInputProducesInstruction("def func(asdf):", expected);
        }
        
        [Test]
        public void HandlesFunctionDefinitionWithMultiplePositionalArguments()
        {
            var expected = new Instruction(
                Consts.InstructionTypes.Function,
                "func",
                null,
                null,
                null,
                null,
                new List<Instruction>()
                {
                    new (Consts.InstructionTypes.Variable, "asdf"),
                    new (Consts.InstructionTypes.Variable, "qwer")
                }
            );
            ParserAssertions.AssertInputProducesInstruction("def func(asdf, qwer):", expected);
        }

        [Test]
        public void HandlesReturn()
        {
            var expected = new Instruction(
                Consts.InstructionTypes.Return, 
                new Instruction(Consts.InstructionTypes.Number, 4));
            ParserAssertions.AssertInputProducesInstruction("return 4", expected);
        }
    }
}