using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class TryInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void ProperlyParsesTry()
        {
            var input = """
                        try:
                            x = 1
                        """;
            var expected = new List<Instruction>() {new TryInstruction([
                new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(1))
            ])};
            Assertions.AssertInputProducesParserOutput(input, expected);
        }

        [Test]
        public void ProperlyParsesTryExcept()
        {
            var input = """
                        try:
                            x = 1
                        except 1:
                            x = 2
                        """;
            var expected = new List<Instruction>() {new TryInstruction([
                new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(1))
            ], new List<ExceptInstruction>()
            {
                new ExceptInstruction(
                    new NumericInstruction(1), 
                    [new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(2))])
            })};
            Assertions.AssertInputProducesParserOutput(input, expected);
        }
        
        [Test]
        public void ProperlyParsesTryExceptWithMultipleExcepts()
        {
            var input = """
                        try:
                            x = 1
                        except 1:
                            x = 2
                        except 2:
                            x = 3
                        """;
            var expected = new List<Instruction>() {new TryInstruction([
                new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(1))
            ], new List<ExceptInstruction>()
            {
                new ExceptInstruction(
                    new NumericInstruction(1), 
                    [new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(2))]),
                new ExceptInstruction(
                    new NumericInstruction(2), 
                    [new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(3))])
            })};
            Assertions.AssertInputProducesParserOutput(input, expected);
        }

        [Test]
        public void ProperlyParsesTryExceptElse()
        {
            var input = """
                        try:
                            x = 1
                        except 1:
                            x = 2
                        except 2:
                            x = 3
                        else:
                            x = 4
                        """;
            var expected = new List<Instruction>() {new TryInstruction([
                new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(1))
            ], new List<ExceptInstruction>()
            {
                new ExceptInstruction(
                    new NumericInstruction(1), 
                    [new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(2))]),
                new ExceptInstruction(
                    new NumericInstruction(2), 
                    [new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(3))])
            }, new ElseInstruction(null, null, new List<Instruction>() { new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(4))}))};
            Assertions.AssertInputProducesParserOutput(input, expected);
        }
        
        [Test]
        public void ProperlyParsesTryExceptElseFinally()
        {
            var input = """
                        try:
                            x = 1
                        except 1:
                            x = 2
                        except 2:
                            x = 3
                        else:
                            x = 4
                        finally:
                            x = 5
                        """;
            var expected = new List<Instruction>() {new TryInstruction(
            [new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(1))], 
            new List<ExceptInstruction>()
            {
                new ExceptInstruction(
                    new NumericInstruction(1), 
                    [new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(2))]),
                new ExceptInstruction(
                    new NumericInstruction(2), 
                    [new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(3))])
            }, 
            new ElseInstruction(null, null, new List<Instruction>() { new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(4))}),
            new FinallyInstruction(new List<Instruction>() { new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(5))}))};
            Assertions.AssertInputProducesParserOutput(input, expected);
        }
        
        [Test]
        public void ProperlyParsesTryExceptWithAs()
        {
            var input = """
                        try:
                            x = 1
                        except 1 as f:
                            x = 2
                        """;
            var expected = new List<Instruction>() {new TryInstruction([
                new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(1))
            ], new List<ExceptInstruction>()
            {
                new ExceptInstruction(
                    new NumericInstruction(1), 
                    [new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(2))],
                    new VariableInstruction("f"))
            })};
            Assertions.AssertInputProducesParserOutput(input, expected);
        }
    }
}