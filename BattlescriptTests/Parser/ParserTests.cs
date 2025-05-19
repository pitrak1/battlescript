using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public class ParserTests
{
    [Test]
    public void HandlesSingleInstruction()
    {
        var lexer = new Lexer("x = 5");
        var lexerResult = lexer.Run();
        var parser = new Parser(lexerResult);
        var parserResult = parser.Run();

        var expected = new List<Instruction>
        {
            new AssignmentInstruction(
                operation: "=", 
                left: new VariableInstruction("x"),
                right: new NumberInstruction(5)
            )
        };
        Assert.That(parserResult, Is.EqualTo(expected));
    }

    [Test]
    public void HandlesConditionalInstructionBlocks()
    {
        var lexer = new Lexer("if 5 < 6:\n\tx = 5");
        var lexerResult = lexer.Run();
        var parser = new Parser(lexerResult);
        var parserResult = parser.Run();

        var expected = new List<Instruction>
        {
            new IfInstruction(
                condition: new OperationInstruction(
                   operation: "<",
                   left: new NumberInstruction(5), 
                   right: new NumberInstruction(6)
                ), 
                instructions: [
                    new AssignmentInstruction(
                        operation: "=",
                        left: new VariableInstruction("x"),
                        right: new NumberInstruction(5)
                    )
                ]
            )
        };
        
        Assert.That(parserResult, Is.EqualTo(expected));
    }
    
    [Test]
    public void HandlesInstructionsBeforeConditionalInstructionBlock()
    {
        var lexer = new Lexer("y = 7\nif 5 < 6:\n\tx = 5");
        var lexerResult = lexer.Run();
        var parser = new Parser(lexerResult);
        var parserResult = parser.Run();

        var expected = new List<Instruction>
        {
            new AssignmentInstruction(
                operation: "=",
                left: new VariableInstruction("y"),
                right: new NumberInstruction(7)
            ),
            new IfInstruction(
                condition: new OperationInstruction(
                    operation: "<",
                    left: new NumberInstruction(5), 
                    right: new NumberInstruction(6)
                ), 
                instructions: [
                    new AssignmentInstruction(
                        operation: "=",
                        left: new VariableInstruction("x"),
                        right: new NumberInstruction(5)
                    )
                ]
            )
        };
        
        Assert.That(parserResult, Is.EquivalentTo(expected));
    }
    
    [Test]
    public void HandlesInstructionsAfterConditionalInstructionBlock()
    {
        var lexer = new Lexer("if 5 < 6:\n\tx = 5\ny = 7");
        var lexerResult = lexer.Run();
        var parser = new Parser(lexerResult);
        var parserResult = parser.Run();

        var expected = new List<Instruction>
        {
            new IfInstruction(
                condition: new OperationInstruction(
                    operation: "<",
                    left: new NumberInstruction(5), 
                    right: new NumberInstruction(6)
                ),
                instructions: [
                    new AssignmentInstruction(
                        operation: "=",
                        left: new VariableInstruction("x"),
                        right: new NumberInstruction(5)
                    )
                ]
            ),
            new AssignmentInstruction(
                operation: "=",
                left: new VariableInstruction("y"),
                right: new NumberInstruction(7)
            )
        };
        
        Assert.That(parserResult, Is.EquivalentTo(expected));
    }
    
    [Test]
    public void HandlesMultipleLevelReductions()
    {
        var lexer = new Lexer("if 5 < 6:\n\tif 5 < 6:\n\t\tx = 6\ny = 7");
        var lexerResult = lexer.Run();
        var parser = new Parser(lexerResult);
        var parserResult = parser.Run();

        var expected = new List<Instruction>
        {
            new IfInstruction(
                condition: new OperationInstruction(
                    operation: "<",
                    left: new NumberInstruction(5), 
                    right: new NumberInstruction(6)
                ),
                instructions: [
                    new IfInstruction(
                        condition: new OperationInstruction(
                            operation: "<",
                            left: new NumberInstruction(5), 
                            right: new NumberInstruction(6)
                        ),
                        instructions: [
                            new AssignmentInstruction(
                                operation: "=",
                                left: new VariableInstruction("x"),
                                right: new NumberInstruction(6)
                            )
                        ]
                    )
                ]
            ),
            new AssignmentInstruction(
                operation: "=",
                left: new VariableInstruction("y"),
                right: new NumberInstruction(7)
            )
        };
        
        Assert.That(parserResult, Is.EquivalentTo(expected));
    }
}