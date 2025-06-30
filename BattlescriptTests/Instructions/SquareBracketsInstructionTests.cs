using Battlescript;

namespace BattlescriptTests.InstructionsTests;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class SquareBracketsInstructionParse
    {
        [Test]
        public void HandlesListDefinition()
        {
            var lexer = new Lexer("[4, 'asdf']");
            var lexerResult = lexer.Run();
            
            var expected = new SquareBracketsInstruction([new IntegerInstruction(4), new StringInstruction("asdf")]);
            
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesIndex()
        {
            var lexer = new Lexer("x[4]");
            var lexerResult = lexer.Run();
            
            var expected = new VariableInstruction(
                name: "x",
                next: new SquareBracketsInstruction([new IntegerInstruction(4)])
            );
            
            var result = InstructionFactory.Create(lexerResult);
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesStackedIndexes()
        {
            var lexer = new Lexer("x[4][5]");
            var lexerResult = lexer.Run();
            
            var expected = new VariableInstruction(
                name: "x",
                next: new SquareBracketsInstruction(
                    values: [new IntegerInstruction(4)],
                    next: new SquareBracketsInstruction([new IntegerInstruction(5)])
                )
            );
            
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesRangeIndexes()
        {
            var lexer = new Lexer("x[4:5]");
            var lexerResult = lexer.Run();
            
            var expected = new VariableInstruction(
                name: "x",
                next: new SquareBracketsInstruction([
                        new IntegerInstruction(4),
                        new IntegerInstruction(5)
            ]));
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesMembers()
        { 
            var lexer = new Lexer("asdf.asdf");
            var lexerResult = lexer.Run();
            
            var expected = new VariableInstruction(
                name: "asdf",
                next: new SquareBracketsInstruction([
                    new StringInstruction("asdf")
                ])
            );
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }

        [Test]
        public void HandlesExpressionIndexing()
        {
            var lexer = new Lexer("asdf[1 + 2]");
            var lexerResult = lexer.Run();
            
            var expected = new VariableInstruction(
                name: "asdf",
                next: new SquareBracketsInstruction([
                    new OperationInstruction(
                        "+",
                        new IntegerInstruction(1),
                        new IntegerInstruction(2))
                ])
            );
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }
    }
    
    [TestFixture]
    public class SquareBracketsInstructionInterpret
    {
        [Test]
        public void HandlesListDefinition()
        {
            var memory = Runner.Run("x = [4, 'asdf']");

            var expected = new Dictionary<string, Variable>()
            {
                ["x"] = new ListVariable([new IntegerVariable(4), new StringVariable("asdf")])
            };
            
            Assert.That(memory.Scopes[0], Is.EquivalentTo(expected));
        }
        
        [Test]
        public void HandlesIndex()
        {
            var memory = Runner.Run("x = [0, 1, 7, 3, 4]\ny = x[2]");
            Assert.That(memory.Scopes[0]["y"], Is.EqualTo(new IntegerVariable(7)));
        }
        
        [Test]
        public void HandlesStackedIndexes()
        {
            var lexer = new Lexer("x[4][5]");
            var lexerResult = lexer.Run();
            
            var expected = new VariableInstruction(
                name: "x",
                next: new SquareBracketsInstruction(
                    values: [new IntegerInstruction(4)],
                    next: new SquareBracketsInstruction([new IntegerInstruction(5)])
                )
            );
            
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesRangeIndexes()
        {
            var lexer = new Lexer("x[4:5]");
            var lexerResult = lexer.Run();
            
            var expected = new VariableInstruction(
                name: "x",
                next: new SquareBracketsInstruction([
                            new IntegerInstruction(4),
                            new IntegerInstruction(5)
                        ])
            );
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesMembers()
        { 
            var lexer = new Lexer("asdf.asdf");
            var lexerResult = lexer.Run();
            
            var expected = new VariableInstruction(
                name: "asdf",
                next: new SquareBracketsInstruction([new StringInstruction("asdf")])
            );
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
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
                        new IntegerInstruction(1),
                        new IntegerInstruction(2))]
                )
            );
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }
    }
}