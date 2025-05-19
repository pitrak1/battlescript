using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class SquareBracketsInstructionParse
    {
        [Test]
        public void HandlesArrayDefinition()
        {
            var expected = new SquareBracketsInstruction(
                values:
                [
                    new NumberInstruction(4),
                    new StringInstruction("asdf")
                ]
            );
            ParserAssertions.AssertInputProducesInstruction("[4, 'asdf']", expected);
        }
        
        [Test]
        public void HandlesIndex()
        {
            var expected = new VariableInstruction(
                name: "x",
                next: new SquareBracketsInstruction([new NumberInstruction(4)])
            );
            ParserAssertions.AssertInputProducesInstruction("x[4]", expected);
        }
        
        [Test]
        public void HandlesStackedIndexes()
        {
            var expected = new VariableInstruction(
                name: "x",
                next: new SquareBracketsInstruction(
                    values: [new NumberInstruction(4)],
                    next: new SquareBracketsInstruction([new NumberInstruction(5)])
                )
            );
            ParserAssertions.AssertInputProducesInstruction("x[4][5]", expected);
        }
        
        [Test]
        public void HandlesRangeIndexes()
        {
            var expected = new VariableInstruction(
                name: "x",
                next: new SquareBracketsInstruction(
                    [
                        new KeyValuePairInstruction(
                            new NumberInstruction(4),
                            new NumberInstruction(5)
                        )
                    ]
                )
            );
            ParserAssertions.AssertInputProducesInstruction("x[4:5]", expected);
        }
        
        [Test]
        public void HandlesMembers()
        { 
            var expected = new VariableInstruction(
                name: "asdf",
                next: new SquareBracketsInstruction(
                    [new StringInstruction("asdf")]
                )
            );
            ParserAssertions.AssertInputProducesInstruction("asdf.asdf", expected);
        }

        [Test]
        public void HandlesExpressionIndexing()
        {
            var expected = new VariableInstruction(
                name: "asdf",
                next: new SquareBracketsInstruction(
                    [new OperationInstruction(
                        "+",
                        new NumberInstruction(1),
                        new NumberInstruction(2))]
                )
            );
            ParserAssertions.AssertInputProducesInstruction("asdf[1 + 2]", expected);
        }
    }
}