using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class ListComprehensionInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void HandlesBasicFor()
        {
            var expected = new ListComprehensionInstruction(
                new OperationInstruction("*", new VariableInstruction("x"), new NumericInstruction(2)),
                [new ForInstruction(new VariableInstruction("x"), new VariableInstruction("y"))]
                );
            Assertions.AssertInputProducesParserOutput("[x * 2 for x in y]", expected);
        }
        
        [Test]
        public void HandlesForWithIf()
        {
            var expected = new ListComprehensionInstruction(
                new OperationInstruction("*", new VariableInstruction("x"), new NumericInstruction(2)),
                [new ForInstruction(
                    new VariableInstruction("x"), 
                    new VariableInstruction("y"), 
                    [new IfInstruction(
                        new OperationInstruction("==", new VariableInstruction("x"), new NumericInstruction(3)))])]
            );
            Assertions.AssertInputProducesParserOutput("[x * 2 for x in y if x == 3]", expected);
        }
        
        [Test]
        public void HandlesMultipleFors()
        {
            var expected = new ListComprehensionInstruction(
                new OperationInstruction("*", new VariableInstruction("x"), new NumericInstruction(2)),
                [new ForInstruction(
                    new VariableInstruction("x"), 
                    new VariableInstruction("y"), 
                    [new ForInstruction(
                        new VariableInstruction("y"), 
                        new VariableInstruction("z"))])]
            );
            Assertions.AssertInputProducesParserOutput("[x * 2 for x in y for y in z]", expected);
        }
        
        [Test]
        public void HandlesMultipleForsAndIfs()
        {
            var innerIf =
                new IfInstruction(new OperationInstruction(
                    "==",
                    new VariableInstruction("y"),
                    new NumericInstruction(3)));
            var innerFor = new ForInstruction(
                new VariableInstruction("y"), 
                new VariableInstruction("z"),
                [innerIf]);
            var outerIf = new IfInstruction(new OperationInstruction(
                "==",
                new VariableInstruction("x"),
                new NumericInstruction(3)), null, [innerFor]);
            var outerFor = new ForInstruction(
                new VariableInstruction("x"),
                new VariableInstruction("y"),
                [outerIf]);
            var expected = new ListComprehensionInstruction(
                new OperationInstruction("*", new VariableInstruction("x"), new NumericInstruction(2)),
                [outerFor]
            );
            Assertions.AssertInputProducesParserOutput("[x * 2 for x in y if x == 3 for y in z if y == 3]", expected);
        }
    }
}