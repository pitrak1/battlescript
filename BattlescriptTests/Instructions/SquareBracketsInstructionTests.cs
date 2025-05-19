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
            var lexer = new Lexer("[4, 'asdf']");
            var lexerResult = lexer.Run();
            
            var expected = new SquareBracketsInstruction(
                values:
                [
                    new NumberInstruction(4),
                    new StringInstruction("asdf")
                ]
            );
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesIndex()
        {
            var lexer = new Lexer("x[4]");
            var lexerResult = lexer.Run();
            
            var expected = new VariableInstruction(
                name: "x",
                next: new SquareBracketsInstruction([new NumberInstruction(4)])
            );
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesStackedIndexes()
        {
            var lexer = new Lexer("x[4][5]");
            var lexerResult = lexer.Run();
            
            var expected = new VariableInstruction(
                name: "x",
                next: new SquareBracketsInstruction(
                    values: [new NumberInstruction(4)],
                    next: new SquareBracketsInstruction([new NumberInstruction(5)])
                )
            );
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesRangeIndexes()
        {
            var lexer = new Lexer("x[4:5]");
            var lexerResult = lexer.Run();
            
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
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesMembers()
        { 
            var lexer = new Lexer("asdf.asdf");
            var lexerResult = lexer.Run();
            
            var expected = new VariableInstruction(
                name: "asdf",
                next: new SquareBracketsInstruction(
                    [new StringInstruction("asdf")]
                )
            );
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }

        [Test]
        public void HandlesExpressionIndexing()
        {
            var lexer = new Lexer("asdf[1 + 2]");
            var lexerResult = lexer.Run();
            
            var expected = new VariableInstruction(
                name: "asdf",
                next: new SquareBracketsInstruction(
                    [new OperationInstruction(
                        "+",
                        new NumberInstruction(1),
                        new NumberInstruction(2))]
                )
            );
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
    }
}