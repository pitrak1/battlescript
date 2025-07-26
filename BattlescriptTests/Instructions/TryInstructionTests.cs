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
    }
    
    [TestFixture]
    public class Interpret
    {
        [Test]
        public void RunsElseIfTryThrowsException()
        {
            var memory = Runner.Run("""
                                    x = 1
                                    try:
                                        raise 1
                                    else:
                                        x = 'asdf'
                                    """);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.String, "asdf"));
        }
        
        [Test]
        public void RunsExceptIfTryThrowsExceptionThatMatches()
        {
            var memory = Runner.Run("""
                                    x = 1
                                    try:
                                        raise Exception('asdf')
                                    except Exception:
                                        x = 'qwer'
                                    """);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.String, "qwer"));
        }
        
        [Test]
        public void DoesNotRunElseIfTryThrowsExceptionThatMatches()
        {
            var memory = Runner.Run("""
                                    x = 1
                                    try:
                                        raise Exception('asdf')
                                    except Exception:
                                        x = 'qwer'
                                    else:
                                        x = 'asdf'
                                    """);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.String, "qwer"));
        }
        
        [Test]
        public void DoesNotRunExceptIfTryThrowsExceptionThatDoesNotMatch()
        {
            var memory = Runner.Run("""
                                    x = 1
                                    try:
                                        raise Exception('asdf')
                                    except list:
                                        x = 'qwer'
                                    else:
                                        x = 'asdf'
                                    """);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.String, "asdf"));
        }
        
        [Test]
        public void RunsMatchingExceptIfTryThrowsException()
        {
            var memory = Runner.Run("""
                                    x = 1
                                    try:
                                        raise Exception('asdf')
                                    except list:
                                        x = 'qwer'
                                    except Exception:
                                        x = 'zxcv'
                                    else:
                                        x = 'asdf'
                                    """);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.String, "zxcv"));
        }
        
        [Test]
        public void RunsFinallyAfterTryBlockIfTryDoesNotThrowException()
        {
            var memory = Runner.Run("""
                                    x = 1
                                    try:
                                        x = 2
                                    finally:
                                        x = 3
                                    """);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Int, 3));
        }
        
        [Test]
        public void RunsFinallyAfterExceptBlockIfTryThrowsMatchingException()
        {
            var memory = Runner.Run("""
                                    x = 1
                                    y = 1
                                    try:
                                        raise Exception('asdf')
                                    except Exception:
                                        y = 2
                                    finally:
                                        x = 2
                                    """);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Int, 2));
            Assertions.AssertVariable(memory, "y", memory.Create(Memory.BsTypes.Int, 2));
        }
        
        [Test]
        public void RunsFinallyAfterElseBlockIfTryThrowsException()
        {
            var memory = Runner.Run("""
                                    x = 1
                                    y = 1
                                    try:
                                        raise Exception('asdf')
                                    else:
                                        y = 2
                                    finally:
                                        x = 2
                                    """);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Int, 2));
            Assertions.AssertVariable(memory, "y", memory.Create(Memory.BsTypes.Int, 2));
        }
        
        // We'll have to write some additional tests here for running finally block if exceptions are raised in
        // except or else blocks
    }
}