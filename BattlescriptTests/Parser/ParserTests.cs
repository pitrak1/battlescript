using Battlescript;

namespace BattlescriptTests.ParserTests;

[TestFixture]
public class ParserTests
{
    [Test]
    public void HandlesSingleInstruction()
    {
        var expected = new List<Instruction>
        {
            new AssignmentInstruction(
                operation: "=", 
                left: new VariableInstruction("x"),
                right: new NumericInstruction(5)
            )
        };
        Assertions.AssertInputProducesParserOutput("x = 5", expected);
    }

    [Test]
    public void HandlesConditionalInstructionBlocks()
    {
        var expected = new List<Instruction>
        {
            new IfInstruction(
                condition: new OperationInstruction(
                   operation: "<",
                   left: new NumericInstruction(5), 
                   right: new NumericInstruction(6)
                ), 
                instructions: [
                    new AssignmentInstruction(
                        operation: "=",
                        left: new VariableInstruction("x"),
                        right: new NumericInstruction(5)
                    )
                ]
            )
        };
        
        Assertions.AssertInputProducesParserOutput("if 5 < 6:\n\tx = 5", expected);
    }
    
    [Test]
    public void HandlesInstructionsBeforeConditionalInstructionBlock()
    {
        var expected = new List<Instruction>
        {
            new AssignmentInstruction(
                operation: "=",
                left: new VariableInstruction("y"),
                right: new NumericInstruction(7)
            ),
            new IfInstruction(
                condition: new OperationInstruction(
                    operation: "<",
                    left: new NumericInstruction(5), 
                    right: new NumericInstruction(6)
                ), 
                instructions: [
                    new AssignmentInstruction(
                        operation: "=",
                        left: new VariableInstruction("x"),
                        right: new NumericInstruction(5)
                    )
                ]
            )
        };
        
        Assertions.AssertInputProducesParserOutput("y = 7\nif 5 < 6:\n\tx = 5", expected);
    }
    
    [Test]
    public void HandlesInstructionsAfterConditionalInstructionBlock()
    {
        var expected = new List<Instruction>
        {
            new IfInstruction(
                condition: new OperationInstruction(
                    operation: "<",
                    left: new NumericInstruction(5), 
                    right: new NumericInstruction(6)
                ),
                instructions: [
                    new AssignmentInstruction(
                        operation: "=",
                        left: new VariableInstruction("x"),
                        right: new NumericInstruction(5)
                    )
                ]
            ),
            new AssignmentInstruction(
                operation: "=",
                left: new VariableInstruction("y"),
                right: new NumericInstruction(7)
            )
        };
        
        Assertions.AssertInputProducesParserOutput("if 5 < 6:\n\tx = 5\ny = 7", expected);
    }
    
    [Test]
    public void HandlesMultipleLevelReductions()
    {
        var expected = new List<Instruction>
        {
            new IfInstruction(
                condition: new OperationInstruction(
                    operation: "<",
                    left: new NumericInstruction(5), 
                    right: new NumericInstruction(6)
                ),
                instructions: [
                    new IfInstruction(
                        condition: new OperationInstruction(
                            operation: "<",
                            left: new NumericInstruction(5), 
                            right: new NumericInstruction(6)
                        ),
                        instructions: [
                            new AssignmentInstruction(
                                operation: "=",
                                left: new VariableInstruction("x"),
                                right: new NumericInstruction(6)
                            )
                        ]
                    )
                ]
            ),
            new AssignmentInstruction(
                operation: "=",
                left: new VariableInstruction("y"),
                right: new NumericInstruction(7)
            )
        };
        
        Assertions.AssertInputProducesParserOutput("if 5 < 6:\n\tif 5 < 6:\n\t\tx = 6\ny = 7", expected);
    }
}